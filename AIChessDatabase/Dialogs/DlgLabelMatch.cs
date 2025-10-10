using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using AIChessDatabase.Query;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    /// <summary>
    /// Dialog box to add keywords to label a match.
    /// </summary>
    public partial class DlgLabelMatch : Form, IUIRemoteControlElement, IChessDBWindow
    {
        private IObjectRepository _repository = null;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;

        public DlgLabelMatch()
        {
            InitializeComponent();
            Text = TTL_LABELMATCHES;
            label2.Text = LAB_VALUE;
            label1.Text = LAB_KEYWORD;
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
        }
        /// <summary>
        /// IChessDBWindow: Window identifier, used to uniquely identify the window in the application.
        /// </summary>
        public string WINDOW_ID { get { return UID; } }
        /// <summary>
        /// IChessDBWindow: Application services provider.
        /// </summary>

        public IAppServiceProvider Provider { get; set; }
        /// <summary>
        /// IChessDBWindow: Connection index to the database.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// IChessDBWindow: Database and object repository to use for this dialog.
        /// </summary>
        public IObjectRepository Repository
        {
            get
            {
                return _repository;
            }
            set { }
        }
        /// <summary>
        /// Current selected keyword.
        /// </summary>
        public Keyword Selection
        {
            get
            {
                return cbKeywords.SelectedItem as Keyword;
            }
        }
        /// <summary>
        /// Current selected value for the keyword.
        /// </summary>
        public string Value
        {
            get
            {
                return txtKeyValue.Text;
            }
        }
        /// <summary>
        /// IUIRemoteControlElement: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return Text; } }
        /// <summary>
        /// IUIRemoteControlElement: Get first-level UI elements.
        /// </summary>
        /// <returns>
        /// List of first-level UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElements()
        {
            return _collector.GetUIElements(null);
        }
        /// <summary>
        /// IUIRemoteControlElement: Get all-level UI elements.
        /// </summary>
        /// <returns>
        /// List of all UI elements. Can be empty or null if there are no elements.
        /// </returns>
        public List<UIRelevantElement> GetAllUIElements()
        {
            return _collector.GetAllUIElements(null);
        }
        /// <summary>
        /// IUIRemoteControlElement: Get the child UI elements of that on a specific path.
        /// </summary>
        /// <param name="path">
        /// Path to the parent UI element.
        /// </param>
        /// <returns>
        /// List of child UI elements. Can be empty or null if there are no child elements.
        /// </returns>
        public List<UIRelevantElement> GetUIElementChildren(string path)
        {
            return _collector.GetUIElementChildren(null, path);
        }
        /// <summary>
        /// IUIRemoteControlElement: Highlight a UI element for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to highlight.
        /// </param>
        /// <param name="seconds">
        /// Duration in seconds to highlight the element.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        public void HighlightUIElement(string path, int seconds, string mode)
        {
            _interactor.HighLight(path, seconds, mode);
        }
        /// <summary>
        /// IUIRemoteControlElement: Show a tooltip for a UI element with a comment for a specified number of seconds.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to comment.
        /// </param>
        /// <param name="title">
        /// The title of the notification balloon.
        /// </param>
        /// <param name="comment">
        /// Comment to display in the tooltip.
        /// </param>
        /// <param name="mode">
        /// Implementation-dependant mode of highlighting.
        /// </param>
        /// <param name="seconds">
        /// Seconds to display the tooltip.
        /// </param>
        public void CommentUIElement(string path, string title, string comment, string mode, int seconds)
        {
            _interactor.ShowBalloon(path, title, comment, mode, seconds);
        }
        /// <summary>
        /// IUIRemoteControlElement: Invoke an action on a UI element at the specified path.
        /// </summary>
        /// <param name="path">
        /// Path to the UI element to invoke the action on.
        /// </param>
        /// <param name="action">
        /// Action name to invoke on the UI element, such as "click", "double-click", etc.
        /// </param>
        /// <returns>
        /// String value or null
        /// </returns>
        /// <remarks>
        /// The format for action is: action[:param]
        /// </remarks>
        public object InvokeElementAction(string path, string action)
        {
            return _interactor.Invoke(path, action);
        }
        /// <summary>
        /// Set the repository to use for this dialog.
        /// </summary>
        /// <param name="rep">
        /// Database and object repository to use for this dialog.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Exception thrown when no free connections are available in the repository.
        /// </exception>
        public async Task SetRepository(IObjectRepository rep)
        {
            _repository = rep;
            if (_repository != null)
            {
                ConnectionIndex = _repository?.GetFreeConnection() ?? -1;
                if (ConnectionIndex == -1)
                {
                    throw new InvalidOperationException(ERR_NOAVAILABLECONNECTIONS);
                }
                Keyword k = _repository.CreateObject(typeof(Keyword)) as Keyword;
                ISQLUIQuery query = k.ObjectQuery(ConnectionIndex);
                ISQLElementProvider esql = query.Parser.Provider;
                SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "keyword_type"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { TYPE_MATCHKEYWORD }));
                UIFilterExpression filter = new UIFilterExpression();
                filter.SetElement(expr);
                DataFilter df = new DataFilter()
                {
                    WFilter = filter,
                    Query = query
                };
                cbKeywords.BeginUpdate();
                foreach (Keyword kw in await k.GetAllAsync(df, ConnectionIndex))
                {
                    cbKeywords.Items.Add(kw);
                }
                cbKeywords.EndUpdate();
            }
            else
            {
                cbKeywords.Items.Clear();
            }
        }
        private async void bNewKey_Click(object sender, EventArgs e)
        {
            try
            {
                Keyword k = _repository.CreateObject(typeof(Keyword)) as Keyword;
                ISQLUIQuery query = k.ObjectQuery(ConnectionIndex);
                ISQLElementProvider esql = query.Parser.Provider;
                SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "keyword_type"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { TYPE_MATCHKEYWORD }));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "and" }));
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "keyword"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { txtNewKey.Text }));
                UIFilterExpression filter = new UIFilterExpression();
                filter.SetElement(expr);
                DataFilter df = new DataFilter()
                {
                    WFilter = filter,
                    Query = query
                };
                Keyword nwk = null;
                foreach (Keyword kw in await k.GetAllAsync(df, ConnectionIndex))
                {
                    nwk = kw;
                    break;
                }
                if (nwk == null)
                {
                    nwk = k;
                    nwk.Name = txtNewKey.Text;
                    nwk.KeywordType = TYPE_MATCHKEYWORD;
                }
                if (!cbKeywords.Items.Contains(nwk))
                {
                    cbKeywords.Items.Add(nwk);
                }
                cbKeywords.SelectedItem = nwk;
                txtNewKey.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtNewKey_TextChanged(object sender, EventArgs e)
        {
            bNewKey.Enabled = !string.IsNullOrEmpty(txtNewKey.Text);
        }

        private void cbKeywords_SelectedIndexChanged(object sender, EventArgs e)
        {
            bOK.Enabled = cbKeywords.SelectedItem != null;
        }

        private void DlgLabelMatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            _repository?.ReleaseConnection(ConnectionIndex);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (Modal)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                Close();
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            if (Modal)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                Close();
            }
        }
    }
}
