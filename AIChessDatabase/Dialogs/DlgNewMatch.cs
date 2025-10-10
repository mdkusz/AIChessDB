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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    /// <summary>
    /// Dialog box to enter general information about a new match.
    /// </summary>
    public partial class DlgNewMatch : Form, IUIRemoteControlElement
    {
        private IObjectRepository _repository = null;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;

        public DlgNewMatch()
        {
            InitializeComponent();
            txtDate.Text = DateTime.Now.ToShortDateString();
            label7.Text = LAB_BLACK;
            label6.Text = LAB_RESULT;
            label5.Text = LAB_DATE;
            label4.Text = LAB_WHITE;
            label3.Text = LAB_DESCRIPTION;
            Text = TTL_NEWMATCH;
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
        }
        /// <summary>
        /// Connection index to the database.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// Objetct repository to use for database operations.
        /// </summary>
        [Browsable(false)]
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
            }
        }
        /// <summary>
        /// Current match being created by this dialog.
        /// </summary>
        public Match Match { get; private set; }
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
        private void txtWhite_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = !(string.IsNullOrEmpty(txtBlack.Text) || string.IsNullOrEmpty(txtWhite.Text) || (cbResult.SelectedItem == null));
        }
        private async void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                Match = Repository.CreateObject(typeof(Match)) as Match;
                Match.Description = txtMatch.Text;
                MatchKeyword mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_EVENT;
                mk.Value = txtMatch.Text;
                await Match.AddChildAsync(mk);
                Match.Date = txtDate.Text;
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_DATE;
                mk.Value = txtDate.Text;
                await Match.AddChildAsync(mk);
                Match.White = txtWhite.Text;
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_WHITE;
                mk.Value = txtWhite.Text;
                await Match.AddChildAsync(mk);
                Match.Black = txtBlack.Text;
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_BLACK;
                mk.Value = txtBlack.Text;
                await Match.AddChildAsync(mk);
                Match.ResultText = cbResult.SelectedItem as string;
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_RESULT;
                mk.Value = cbResult.SelectedItem as string;
                await Match.AddChildAsync(mk);
                Match.InitialPosition = Repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                Match.InitialPosition.Order = 0;
                Match.InitialPosition.Board = Repository.CreateObject(typeof(Position)) as Position;
                Match.InitialPosition.Board.Board = INITIAL_BOARD;
                if (Modal)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Match = Repository.CreateObject(typeof(Match)) as Match;
                Match.Description = txtMatch.Text;
                MatchKeyword mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_EVENT;
                mk.Value = "?";
                await Match.AddChildAsync(mk);
                Match.Date = "?";
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_DATE;
                mk.Value = "?";
                await Match.AddChildAsync(mk);
                Match.White = "?";
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_WHITE;
                mk.Value = "?";
                await Match.AddChildAsync(mk);
                Match.Black = "?";
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_BLACK;
                mk.Value = "?";
                await Match.AddChildAsync(mk);
                Match.ResultText = cbResult.SelectedItem as string;
                mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = KEY_RESULT;
                mk.Value = "*";
                await Match.AddChildAsync(mk);
                Match.InitialPosition = Repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                Match.InitialPosition.Order = 0;
                Match.InitialPosition.Board = Repository.CreateObject(typeof(Position)) as Position;
                Match.InitialPosition.Board.Board = INITIAL_BOARD;
            }
            catch
            {
                Match = null;
            }
            if (Modal)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                Close();
            }
        }

        private async void DlgNewMatch_Shown(object sender, EventArgs e)
        {
            try
            {
                if (_repository != null)
                {
                    MatchKeyword mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                    ISQLUIQuery query = mk.ObjectQuery(ConnectionIndex);
                    ISQLElementProvider esql = _repository.Provider.GetObjects(nameof(ISQLElementProvider)).First().Implementation() as ISQLElementProvider;
                    SQLExpression expr = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                    expr.Elements.Add(query.QueryColumns.Find(q => q.Name == "keyword"));
                    expr.Elements.Add(esql.SQLElement(typeof(LogicOperator), new object[] { "=" }));
                    expr.Elements.Add(esql.SQLElement(typeof(LiteralString), new object[] { KEY_WHITE }));
                    UIFilterExpression filter = new UIFilterExpression();
                    filter.SetElement(expr);
                    SQLExpression edis = esql.SQLElement(typeof(SQLExpression)) as SQLExpression;
                    edis.Elements.Add(esql.SQLElement(typeof(SQLLiteral), new[] { "distinct" }));
                    DataFilter df = new DataFilter()
                    {
                        WFilter = filter,
                        SetQ = edis
                    };
                    BeginInvoke(new Action(() =>
                    {
                        _interactor.ShowBalloon(string.Join(".", UID, txtWhite.Name),
                            CAP_LOADING,
                            MSG_LOADINGWHITEK,
                            fHighlight.HighlightMode.Tooltip.ToString(),
                            5);
                    }));
                    Application.DoEvents();
                    txtWhite.AutoCompleteCustomSource.Clear();
                    txtWhite.AutoCompleteCustomSource.AddRange((await mk.GetAllValues(df, ConnectionIndex)).ToArray());
                    ((LiteralString)expr.Elements[2]).Value = KEY_BLACK;
                    BeginInvoke(new Action(() =>
                    {
                        _interactor.ShowBalloon(string.Join(".", UID, txtBlack.Name),
                            CAP_LOADING,
                            MSG_LOADINGWHITEK,
                            fHighlight.HighlightMode.Tooltip.ToString(),
                            5);
                    }));
                    Application.DoEvents();
                    txtBlack.AutoCompleteCustomSource.Clear();
                    txtBlack.AutoCompleteCustomSource.AddRange((await mk.GetAllValues(df, ConnectionIndex)).ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DlgNewMatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            _repository?.ReleaseConnection(ConnectionIndex);
        }
    }
}
