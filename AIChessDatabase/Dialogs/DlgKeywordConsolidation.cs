using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using AIChessDatabase.Query;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.UserInterface;
using DesktopControls.Controls;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    public partial class DlgKeywordConsolidation : Form, IUIRemoteControlElement, IChessDBWindow
    {
        private IObjectRepository _repository = null;
        private ObjectWrapper<string> _currentValue = null;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;
        private bool _noclose = false;

        public DlgKeywordConsolidation()
        {
            InitializeComponent();
            label1.Text = LAB_KEYWORD;
            label2.Text = LAB_VALUES;
            label3.Text = LAB_FILTER;
            label4.Text = LAB_FILTER;
            label5.Text = LAB_EQUIVALENCES;
            label6.Text = LAB_CANONICAL;
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
        /// IChessDBWindow: Gets the repository used to access and manage objects in the data store.
        /// </summary>
        public IObjectRepository Repository
        {
            get
            {
                return _repository;
            }
            set
            {
                _repository = value;
                ConnectionIndex = _repository?.GetFreeConnection() ?? -1;
                if (ConnectionIndex == -1)
                {
                    throw new InvalidOperationException(ERR_NOAVAILABLECONNECTIONS);
                }
                Text = string.Format(TTL_CONSOLIDATE, _repository.ServerName + " (" + _repository.ConnectionName + ")");
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
        /// Asynchronous loading of keywords from the repository.
        /// </summary>
        /// <returns></returns>
        private async Task LoadData()
        {
            cbKeyword.Items.Clear();
            lbValues.Items.Clear();
            lbEquivalences.Items.Clear();
            if (_repository != null)
            {
                Keyword k = _repository.CreateObject(typeof(Keyword)) as Keyword;
                ISQLUIQuery query = k.ObjectQuery(ConnectionIndex);
                ISQLElementProvider esql = query.Parser.Provider;
                DataFilter filter = new DataFilter()
                {
                    Query = query
                };
                filter.WFilter = new UIFilterExpression();
                SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "keyword_type"));
                expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { TYPE_MATCHKEYWORD }));
                filter.WFilter.SetElement(expr);
                cbKeyword.BeginUpdate();
                foreach (Keyword kw in await k.GetAllAsync(filter, ConnectionIndex))
                {
                    cbKeyword.Items.Add(kw);
                }
                cbKeyword.EndUpdate();
                cbKeyword.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Asynchronously loads values and equivalences for the selected keyword.
        /// </summary>
        /// <param name="lb">
        /// Flags to indicate which list to load: 1 for values, 2 for equivalences, or 3 for both.
        /// </param>
        private async Task LoadValues(int lb)
        {
            MatchKeyword mk = _repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
            ISQLUIQuery query = mk.ValuesQuery(ConnectionIndex);
            ISQLElementProvider esql = query.Parser.Provider;
            DataFilter filter = new DataFilter();
            filter.Query = query;
            filter.WFilter = new UIFilterExpression();
            SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
            expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "keyword"));
            expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
            expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { cbKeyword.SelectedItem.ToString() }));
            filter.WFilter.SetElement(expr);
            DataTable dt = await mk.Query(filter);
            if (dt == null)
            {
                MessageBox.Show(MSG_NODATARETURNED);
                return;
            }
            lbValues.BeginUpdate();
            lbEquivalences.BeginUpdate();
            foreach (DataRow row in dt.Rows)
            {
                ObjectWrapper<string> data = new ObjectWrapper<string>(row["keyword"].ToString(), row["kw_value"].ToString(), null);
                if ((lb & 1) != 0)
                {
                    if (!string.IsNullOrEmpty(txtFilter.Text))
                    {
                        Regex rex = new Regex(txtFilter.Text, RegexOptions.IgnoreCase);
                        if (rex.IsMatch(data.FriendlyName))
                        {
                            lbValues.Items.Add(data);
                        }
                    }
                    else
                    {
                        lbValues.Items.Add(data);
                    }
                }
                if ((lb & 2) != 0)
                {
                    if (_currentValue != null)
                    {
                        if (data.FriendlyName != _currentValue.FriendlyName)
                        {
                            if (!string.IsNullOrEmpty(txtFilterEq.Text))
                            {
                                Regex rex = new Regex(txtFilterEq.Text, RegexOptions.IgnoreCase);
                                if (rex.IsMatch(data.FriendlyName))
                                {
                                    lbEquivalences.Items.Add(data);
                                }
                            }
                            else
                            {
                                lbEquivalences.Items.Add(data);
                            }
                        }
                    }
                }
            }
            lbValues.EndUpdate();
            lbEquivalences.EndUpdate();
        }

        private async void cbKeyword_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Keyword k = cbKeyword.SelectedItem as Keyword;
                lbValues.Items.Clear();
                lbEquivalences.Items.Clear();
                if (k != null)
                {
                    await LoadValues(3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Keyword k = cbKeyword.SelectedItem as Keyword;
                lbValues.Items.Clear();
                if (k != null)
                {
                    await LoadValues(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bRefreshEq_Click(object sender, EventArgs e)
        {
            try
            {
                Keyword k = cbKeyword.SelectedItem as Keyword;
                lbEquivalences.Items.Clear();
                if (k != null)
                {
                    await LoadValues(2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void lbValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObjectWrapper<string> data = lbValues.SelectedItem as ObjectWrapper<string>;
                _currentValue = null;
                txtCanonical.Text = "";
                bConsolidate.Enabled = false;
                if (data != null)
                {
                    txtCanonical.Text = data.FriendlyName;
                    _currentValue = data;
                    lbEquivalences.Items.Clear();
                    await LoadValues(2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFilter.Text))
                {
                    Regex rex = new Regex(txtFilter.Text, RegexOptions.IgnoreCase);
                    for (int ix = lbValues.Items.Count - 1; ix >= 0; ix--)
                    {
                        ObjectWrapper<string> data = lbValues.Items[ix] as ObjectWrapper<string>;
                        if (!rex.IsMatch(data.FriendlyName))
                        {
                            lbValues.Items.RemoveAt(ix);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bFilterEq_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFilterEq.Text))
                {
                    Regex rex = new Regex(txtFilterEq.Text, RegexOptions.IgnoreCase);
                    for (int ix = lbEquivalences.Items.Count - 1; ix >= 0; ix--)
                    {
                        ObjectWrapper<string> data = lbEquivalences.Items[ix] as ObjectWrapper<string>;
                        if (!rex.IsMatch(data.FriendlyName))
                        {
                            lbEquivalences.Items.RemoveAt(ix);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bConsolidate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format(MSG_CONSOLIDATE, txtCanonical.Text), CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bConsolidate.Enabled = false;
                    bClose.Enabled = false;
                    _noclose = true;
                    ObjectWrapper<Keyword> k = cbKeyword.SelectedItem as ObjectWrapper<Keyword>;
                    if (k != null)
                    {
                        List<string> keys = new List<string>() { _currentValue.FriendlyName };
                        for (int i = 0; i < lbEquivalences.SelectedItems.Count; i++)
                        {
                            ObjectWrapper<string> data = lbEquivalences.SelectedItems[i] as ObjectWrapper<string>;
                            keys.Add(data.FriendlyName);
                        }
                        MatchKeyword mk = _repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                        _repository.Connector.OpenConnection(ConnectionIndex);
                        _repository.Connector.BeginTransaction(ConnectionIndex);
                        await mk.Consolidate(txtCanonical.Text, keys, k.TypedImplementation.IdKeyword, ConnectionIndex);
                        _repository.Connector.Commit(ConnectionIndex);
                        lbValues.Items.Clear();
                        lbEquivalences.Items.Clear();
                        await LoadValues(3);
                    }
                }
                catch (Exception ex)
                {
                    _repository.Connector.Rollback(ConnectionIndex);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    _repository.Connector.CloseConnection(ConnectionIndex);
                    _noclose = false;
                    bClose.Enabled = true;
                }
            }
        }

        private void lbEquivalences_SelectedIndexChanged(object sender, EventArgs e)
        {
            bConsolidate.Enabled = (lbEquivalences.SelectedIndices.Count > 0)
                && (_currentValue != null)
                && !string.IsNullOrEmpty(txtCanonical.Text);
        }

        private void DlgKeywordConsolidation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_noclose)
            {
                e.Cancel = _noclose;
                MessageBox.Show(MSG_DBOPERATION);
            }
        }

        private void bClose_Click(object sender, EventArgs e)
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

        private void DlgKeywordConsolidation_FormClosed(object sender, FormClosedEventArgs e)
        {
            _repository?.ReleaseConnection(ConnectionIndex);
        }

        private async void DlgKeywordConsolidation_Shown(object sender, EventArgs e)
        {
            using (new OverlayScope(this, MSG_LoadingData))
            {
                await LoadData();
            }
        }
    }
}
