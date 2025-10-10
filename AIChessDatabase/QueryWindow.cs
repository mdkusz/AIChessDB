using AIChessDatabase.Controls;
using AIChessDatabase.Data;
using AIChessDatabase.Dialogs;
using AIChessDatabase.Interfaces;
using AIChessDatabase.PGNParser;
using AIChessDatabase.Query;
using BaseClassesAndInterfaces.Interfaces;
using BaseClassesAndInterfaces.SQL;
using BaseClassesAndInterfaces.Tools;
using BaseClassesAndInterfaces.UserInterface;
using DesktopControls.Controls;
using GlobalCommonEntities.DependencyInjection;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIElements;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase
{
    /// <summary>
    /// Form to query the chess database.
    /// </summary>
    public partial class QueryWindow : Form, IChessDBWindow, IUIRemoteControlElement, IAIChessPlayer, IProgressMonitor, IDependencyProvider
    {
        private string _fileName = "Query" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".chdbq";
        private IObjectRepository _repository = null;
        private IAppServiceProvider _provider = null;
        private ChessDBQuery _query = new ChessDBQuery();
        private DataTable _results = null;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;

        public QueryWindow()
        {
            InitializeComponent();
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
            cPlayer.AddComments = true;
        }
        /// <summary>
        /// Total matches in the database.
        /// </summary>
        public ulong TotalMatches { get; private set; }
        /// <summary>
        /// Total results for last query.
        /// </summary>
        public ulong TotalResults { get { return (ulong)(_results?.Rows.Count ?? 0); } }
        /// <summary>
        /// IChessDBWindow: Window identifier, used to uniquely identify the window in the application.
        /// </summary>
        [Browsable(false)]
        public string WINDOW_ID
        {
            get
            {
                return ID_QUERYWINDOW + _fileName;
            }
        }
        /// <summary>
        /// IChessDBWindow: Database connection index, used to identify the connection in multi-connection scenarios.
        /// </summary>
        public int ConnectionIndex
        {
            get
            {
                return cPlayer.ConnectionIndex;
            }
            set
            {
                cPlayer.ConnectionIndex = value;
                if (_query != null)
                {
                    _query.ConnectionIndex = value;
                }
            }
        }
        /// <summary>
        /// IChessDBWindow: Application services provider.
        /// </summary>
        [Browsable(false)]
        public IAppServiceProvider Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                _provider = value;
            }
        }
        /// <summary>
        /// IChessDBWindow: Database repository for data operations.
        /// </summary>
        [Browsable(false)]
        public IObjectRepository Repository
        {
            get
            {
                return _repository ?? Provider?.SharedRepository;
            }
            set
            {
                _repository = value;
                if (_repository != null)
                {
                    ConnectionIndex = _repository.GetFreeConnection();
                    if (ConnectionIndex < 0)
                    {
                        throw new Exception(ERR_NOAVAILABLECONNECTIONS);
                    }
                    foreach (string rep in _provider.OtherRepositories(_repository.ConnectionName))
                    {
                        cbDatabases.Items.Add(rep);
                    }
                    Text = string.Format(TTL_QUERY, _repository.ServerName + " (" + _repository.ConnectionName + ")");
                }
            }
        }
        /// <summary>
        /// IUIRemoteControlElement, IAIChessPlayer: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement, IAIChessPlayer: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return FNAME_QueryWindow; } }
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
        /// IProgressMonitor: Sets the total step number of the operation.
        /// </summary>
        /// <param name="steps">
        /// Number of steps to complete the operation.
        /// </param>
        public void SetTotalSteps(int steps)
        {
            BeginInvoke(new Action(() =>
            {
                pbExport.Step = 1;
                pbExport.Maximum = steps;
                pbExport.Value = 0;
            }));
        }
        /// <summary>
        /// IProgressMonitor: Restets the progress monitor to its initial state.
        /// </summary>
        /// <param name="sender">
        /// Object that is resetting the progress monitor, typically the operation initiator.
        /// </param>
        public void Reset(object sender)
        {
            EndInvoke(BeginInvoke(new Action(() =>
            {
                pbExport.Value = 0;
                if (sender is PGNFormatter)
                {
                    PGNFormatter pgnf = sender as PGNFormatter;
                    EnableButtons(false);
                    dgMatches.Enabled = false;
                    UseWaitCursor = true;
                    DialogResult dr = MessageBox.Show(MSG_EXPORTCOMMENTS, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    pgnf.ExportComments = dr == DialogResult.Yes;
                    pgnf.ConnectionIndex = ConnectionIndex;
                    pgnf.Repository = Repository;
                }
                else if (sender is MatchFormatter)
                {
                    if (cbDatabases.SelectedItem == null)
                    {
                        throw new Exception(ERR_NODATABASESELECTED);
                    }
                    MatchFormatter mf = sender as MatchFormatter;
                    MatchDataImportManager mdim = mf.ImportManager as MatchDataImportManager;
                    EnableButtons(false);
                    dgMatches.Enabled = false;
                    UseWaitCursor = true;
                    DialogResult dr = MessageBox.Show(MSG_INSERTDUPLICATES, TTL_DUPLICATES, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    mf.ConnectionIndex = ConnectionIndex;
                    mdim.ExportedCount = 0;
                    mdim.Duplicates = dr != DialogResult.No;
                    mdim.Repository = Repository;
                    mdim.ExportRepository = _provider.GetRepository(cbDatabases.SelectedItem.ToString());
                }
            })));
        }
        /// <summary>
        /// IProgressMonitor: Advances the progress monitor.
        /// </summary>
        /// <param name="c">
        /// Number of steps to advance the progress monitor.
        /// </param>
        public void Step(int c = 1)
        {
            BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < c; i++)
                {
                    pbExport.PerformStep();
                }
            }));
        }
        /// <summary>
        /// IProgressMonitor: Process finished event.
        /// </summary>
        /// <param name="sender">
        /// Finishing object, typically the operation initiator.
        /// </param>
        public void Stop(object sender)
        {
            BeginInvoke(new Action(() =>
            {
                pbExport.Value = 0;
                if (sender is PGNFormatter)
                {
                    EnableButtons(true);
                    dgMatches.Enabled = true;
                    UseWaitCursor = false;
                    Repository.Connector.CloseConnection(ConnectionIndex);
                    MessageBox.Show(MSG_MATCHESSAVED);
                }
                else if (sender is MatchFormatter)
                {
                    EnableButtons(true);
                    dgMatches.Enabled = true;
                    UseWaitCursor = false;
                    MatchDataImportManager mdim = (sender as MatchFormatter).ImportManager as MatchDataImportManager;
                    Repository.Connector.CloseConnection(ConnectionIndex);
                    mdim.ExportRepository?.Connector.CloseConnection();
                    MessageBox.Show(string.Format(MSG_RESULTEXPORTED, mdim.ExportedCount));
                }
            }));
        }
        /// <summary>
        /// IAIChessPlayer: Match is editable. New moves can be added, and existing moves can be canceled.
        /// </summary>
        public bool Editable { get { return false; } }
        /// <summary>
        /// IAIChessPlayer: Json object with match information.
        /// </summary>
        public Match MatchInfo { get { return cPlayer.CurrentMatch; } }
        /// <summary>
        /// IAIChessPlayer: List of moves in the match. Each move is a Json object.
        /// </summary>
        public List<MatchMove> Moves { get { return new List<MatchMove>(cPlayer.CurrentMatch.Moves); } }
        /// <summary>
        /// IAIChessPlayer: Current move in the match.
        /// </summary>
        public MatchMove CurrentMove { get { return cPlayer.CurrentMove; } }
        /// <summary>
        /// IAIChessPlayer: Current board position.
        /// </summary>
        public string CurrentBoard { get { return cPlayer.CurrentBoard; } }
        /// <summary>
        /// IAIChessPlayer: Advance to the next move in the match.
        /// </summary>
        /// <param name="comment">
        /// Show a comment on the move being made.
        /// </param>
        /// <returns>
        /// A list with three moves: the previous move, the current move, and the next move.
        /// </returns>
        public List<MatchMove> NextMove(string comment) { return cPlayer.EnqueueMove(comment); }
        /// <summary>
        /// IAIChessPlayer: Forward to the previous move in the match.
        /// </summary>
        public MatchMove PrevMove { get { return cPlayer.MoveBack(); } }
        /// <summary>
        /// IAIChessPlayer: Add a move to the match.
        /// </summary>
        /// <param name="move">
        /// Textual representation of the move in Algebraic Notation (AN).
        /// </param>
        /// <param name="comment">
        /// Optional move comment
        /// </param>
        public string AddMove(string move, string comment = null)
        {
            return ERR_NOTEDITABLEMATCH;
        }
        /// <summary>
        /// IAIChessPlayer: Add a whole match in PGN format.
        /// </summary>
        /// <param name="pgn">
        /// Match in PGN format.
        /// </param>
        /// <returns>
        /// Message indicating success or failure.
        /// </returns>
        public string AddMatch(string pgn)
        {
            try
            {
                PGNFile pf = new PGNFile();
                int cnt = pf.ParseString(pgn);
                if (cnt == 0)
                {
                    throw new Exception(ERR_NOMATCHADDED);
                }
                pf.ConvertAllMatches(Repository);
                cPlayer.CurrentMatch = pf.GetMatch(0, Repository);
                return cnt == 1 ? MSG_MATCHADDED : string.Format(MSG_MATCHESADDED, cnt);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// IAIChessPlayer: Cancel the last move made in the match.
        /// </summary>
        public void CancelLastMove()
        {
            throw new Exception(ERR_NOTEDITABLEMATCH);
        }
        /// <summary>
        /// IDependencyProvider: Central repository of embedded resources
        /// </summary>
        public ResourcesRepository AllResources
        {
            get
            {
                return ((Main)Provider).AllResources;
            }
            set { }
        }
        /// <summary>
        /// IDependencyProvider: Check if a given class or interface is supported
        /// </summary>
        /// <param type="services">
        /// Type names separated by semicolon
        /// </param>
        /// <returns>
        /// True if can instantiante objets of at least one of the given types
        /// </returns>
        public bool QueryClassOrInterface(string services)
        {
            return ((Main)Provider).QueryClassOrInterface(services);
        }
        /// <summary>
        /// IDependencyProvider: Get all the instances of all the rquested types or interfaces 
        /// </summary>
        /// <param type="services">
        /// Semicolon separated type type list
        /// </param>
        /// <param type="filter">
        /// Object containing an application-defined filter to select elements
        /// </param>
        /// <returns>
        /// Enumeration of objects implementing the types as IUIIdentifier objects
        /// </returns>
        public IEnumerable<IUIIdentifier> GetObjects(string services, object filter = null)
        {
            foreach (IUIIdentifier obj in ((Main)Provider).GetObjects(services, filter))
            {
                if (obj is PGNFormatter)
                {
                    PGNFormatter pgnf = obj as PGNFormatter;
                    pgnf.ExportComments = false;
                    pgnf.ConnectionIndex = ConnectionIndex;
                    pgnf.Repository = Repository;
                    pgnf.ProgressMonitor = this;
                    yield return pgnf;
                }
                else if (obj is MatchFormatter)
                {
                    MatchFormatter mf = obj as MatchFormatter;
                    mf.ConnectionIndex = ConnectionIndex;
                    mf.ImportManager.ConfigurationProvider = MdiParent as Main;
                    mf.ProgressMonitor = this;
                    yield return mf;
                }
                else
                {
                    yield return obj;
                }
            }
        }
        /// <summary>
        /// Change the current view in the matches grid.
        /// </summary>
        /// <param name="viewname">
        /// View name
        /// </param>
        /// <returns>
        /// Query object used to populate the matches grid.
        /// </returns>
        /// <exception cref="Exception">
        /// Unknown view name
        /// </exception>
        public ISQLUIQuery ChangeView(string viewname)
        {
            switch (viewname.ToLower())
            {
                case "vw_match_keywords":
                    bFilterByTag.PerformClick();
                    break;
                case "vw_match_positions":
                    bFilterByPos.PerformClick();
                    break;
                case "vw_match_moves":
                    bFilterByMov.PerformClick();
                    break;
                default:
                    throw new Exception(string.Format(ERR_UNKNOWNVIEWNAME, viewname));
            }
            return dgMatches.Grid.Query;
        }
        /// <summary>
        /// Get current query in the matches grid.
        /// </summary>
        /// <returns>
        /// ISQLUIQuery object used to populate the matches grid.
        /// </returns>
        public ISQLUIQuery GetCurrentQuery()
        {
            return dgMatches.Grid.Query;
        }
        /// <summary>
        /// Add a new filter to the current query in the matches grid.
        /// </summary>
        /// <param name="filter">
        /// Filter expression to add to the current query.
        /// </param>
        public void AddFilter(UIFilterExpression filter)
        {
            if (!dgMatches.Grid.Query.ContainsFilter(filter))
            {
                if (dgMatches.FOManager != null)
                {
                    ((IFilterManager)dgMatches.FOManager).AddFilter(filter);
                }
                else
                {
                    dgMatches.Grid.Query.AddFilter(filter);
                }
            }
        }
        /// <summary>
        /// Remove filters from the current query in the matches grid.
        /// </summary>
        /// <param name="filterindexes">
        /// Filter indexes to remove from the current query. Empty or null to remove all.
        /// </param>
        public void RemoveFilters(List<int> filterindexes)
        {
            if ((filterindexes == null) || (filterindexes.Count == 0))
            {
                if (dgMatches.FOManager != null)
                {
                    ((IFilterManager)dgMatches.FOManager).ClearAllFilters();
                }
                dgMatches.Grid.Query.ClearAll(true, false, false);
            }
            else
            {
                filterindexes.Sort();
                filterindexes.Reverse();
                List<UIFilterExpression> filters = null;
                while (filterindexes.Count > 0)
                {
                    int ix = filterindexes[0];
                    filterindexes.RemoveAt(0);
                    if (dgMatches.FOManager != null)
                    {
                        if (filters == null)
                        {
                            filters = new List<UIFilterExpression>(((IFilterManager)dgMatches.FOManager).GetAllFilters());
                        }
                        // Notify changes only on the last filter removal
                        ((IFilterManager)dgMatches.FOManager).RemoveFilter(filters[ix], filterindexes.Count == 0);
                    }
                    else
                    {
                        dgMatches.Grid.Query.UserWhereFilters.RemoveAt(ix);
                    }
                }
            }
        }
        /// <summary>
        /// Set the order by clause of the current query in the matches grid.
        /// </summary>
        /// <param name="fields">
        /// List of field names to build the order by clause. Empty or null to remove all user ordering.
        /// </param>
        public void SetOrderBy(List<string> fields)
        {
            if (dgMatches.FOManager as IOrderManager != null)
            {
                ((IOrderManager)dgMatches.FOManager).ClearAllOrder();
            }
            dgMatches.Grid.Query.ClearAll(false, false, true);
            if (fields?.Count > 0)
            {
                List<UIOrderByExpression> orderby = new List<UIOrderByExpression>();
                while (fields.Count > 0)
                {
                    string oexpr = fields[0];
                    fields.RemoveAt(0);
                    SQLElement sexpr = ((ISQLElementBuilder)dgMatches.Grid.Query).BuildExpression(ref oexpr);
                    if (dgMatches.FOManager as IOrderManager != null)
                    {
                        ((IOrderManager)dgMatches.FOManager).AddOrderElement(sexpr, fields.Count == 0);
                    }
                    else
                    {
                        UIOrderByExpression obe = new UIOrderByExpression();
                        obe.SetElement(sexpr);
                        dgMatches.Grid.Query.AddOrder(obe);
                    }
                }
            }
        }
        /// <summary>
        /// Launch the query to refresh the data in the matches grid.
        /// </summary>
        public void RefreshQueryData()
        {
            dgMatches.Grid.RefreshData();
        }
        /// <summary>
        /// Enable or dissable toolbar buttons depending on the current selection in the matches grid.
        /// </summary>
        /// <param name="enable"></param>
        private void EnableButtons(bool enable)
        {
            if (enable)
            {
                bFilterByTag.Enabled = true;
                bFilterByPos.Enabled = true;
                bFilterByMov.Enabled = true;
                bShow.Enabled = dgMatches.Grid.SelectedRows.Count == 1;
                bEditMatch.Enabled = dgMatches.Grid.SelectedRows.Count == 1;
                bPlay.Enabled = dgMatches.Grid.SelectedRows.Count == 1;
                bDelMatch.Enabled = dgMatches.Grid.SelectedRows.Count > 0;
                bTagSel.Enabled = dgMatches.Grid.SelectedRows.Count > 0;
                bDelete.Enabled = dgMatches.Grid.SelectedRows.Count > 0;
            }
            else
            {
                bFilterByTag.Enabled = false;
                bFilterByPos.Enabled = false;
                bFilterByMov.Enabled = false;
                bShow.Enabled = false;
                bEditMatch.Enabled = false;
                bPlay.Enabled = false;
                bDelMatch.Enabled = false;
                bTagSel.Enabled = false;
                bDelete.Enabled = false;
            }
        }
        /// <summary>
        /// Set the total count of matches in the database.
        /// </summary>
        /// <param name="match">
        /// Match to get the count from.
        /// </param>
        private async Task MatchCount(Match match)
        {
            try
            {
                TotalMatches = await match.GetCount(null, ConnectionIndex);
                lMatchCount.Text = string.Format(lMatchCount_Description, TotalMatches);
                lMatchCount.AccessibleDescription = lMatchCount.Text;
            }
            catch { }
        }
        private async void bShow_Click(object sender, EventArgs e)
        {
            try
            {
                EnableButtons(false);
                UseWaitCursor = true;
                if (_query != null)
                {
                    ulong m = Convert.ToUInt64(dgMatches.Grid.SelectedRows[0].Cells[0].Value);
                    Match match = Repository.CreateObject(typeof(Match)) as Match;
                    await match.FastLoad(m, ConnectionIndex);
                    cPlayer.CurrentMatch = match;
                    if ((_query.Query.QueryLinks != null) && (_query.Query.QueryLinks.Count > 0))
                    {
                        // Find the moves that meet the filter criteria
                        ISQLUIQuery mquery = _query.Query.QueryLinks[0].DetailQuery;
                        mquery.Parameters[0].DefaultValue = m;
                        mquery.UserWhereFilters?.Clear();
                        mquery.UserWhereFilters = new List<QueryFilterContainer>(_query.Query.UserWhereFilters);
                        DataTable dtm = await Repository.Connector.ExecuteTableAsync(mquery, null, null, ConnectionIndex);
                        if (dtm.Rows.Count < Convert.ToInt32(dgMatches.Grid.SelectedRows[0].Cells[6].Value))
                        {
                            // Mark the moves that meet the filter criteria with a special color
                            mDisplay.SetMoves(dtm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                EnableButtons(true);
                UseWaitCursor = false;
            }
        }

        private void QueryWindow_Load(object sender, EventArgs e)
        {
            Icon = Icon.Clone() as Icon;
            dgMatches.Grid.BeforeRefreshing += new EventHandler(dgMatches_RefreshingQuery);
            dgMatches.FOManager = new BasicFilterAndOrderManager(FOManagerUIType.List);
        }
        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                dgMatches.Enabled = false;
                EnableButtons(false);
                foreach (DataGridViewRow row in dgMatches.Grid.SelectedRows)
                {
                    dgMatches.Grid.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dgMatches.Enabled = true;
                EnableButtons(true);
            }
        }

        private void dgMatches_SelectionChanged(object sender, EventArgs e)
        {
            EnableButtons(true);
        }
        private void dgMatches_RefreshingQuery(object sender, EventArgs e)
        {
            EnableButtons(false);
        }
        private void dgMatches_QueryChanged(object sender, EventArgs e)
        {
            try
            {
                _results = dgMatches.Grid.DataSource as DataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                EnableButtons(true);
            }
        }
        private async void bDelMatch_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(MSG_DELMATCHES, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                EnableButtons(false);
                dgMatches.Enabled = false;
                UseWaitCursor = true;
                Match match = null;
                try
                {
                    using (OverlayScope ov = new OverlayScope(this, string.Format(MSG_DELETINGMATCHES, dgMatches.Grid.SelectedRows.Count)))
                    {
                        Repository.Connector.OpenConnection(ConnectionIndex);
                        for (int ir = dgMatches.Grid.SelectedRows.Count - 1; ir >= 0; ir--)
                        {
                            Repository.Connector.BeginTransaction(ConnectionIndex);
                            ulong m = Convert.ToUInt64(dgMatches.Grid.SelectedRows[ir].Cells[0].Value);
                            match = Repository.CreateObject(typeof(Match)) as Match;
                            await match.FastLoad(m, ConnectionIndex);
                            await match.Delete(ConnectionIndex);
                            Repository.Connector.Commit(ConnectionIndex);
                            dgMatches.Grid.Rows.RemoveAt(dgMatches.Grid.SelectedRows[ir].Index);
                            ov.Message = string.Format(MSG_DELETINGMATCHES, ir);
                        }
                    }
                    MessageBox.Show(MSG_MATCHESDEL);
                }
                catch (Exception ex)
                {
                    Repository.Connector.Rollback(ConnectionIndex);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    EnableButtons(true);
                    if (match != null)
                    {
                        await MatchCount(match);
                    }
                    dgMatches.Enabled = true;
                    UseWaitCursor = false;
                    Repository.Connector.CloseConnection(ConnectionIndex);
                }
            }
        }
        private async void bEditMatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (_query != null)
                {
                    ulong m = Convert.ToUInt64(dgMatches.Grid.SelectedRows[0].Cells[0].Value);
                    Match match = Repository.CreateObject(typeof(Match)) as Match;
                    await match.FastLoad(m, ConnectionIndex);
                    _provider.EditMatch(match);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableButtons(true);
        }
        private async void bPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (_query != null)
                {
                    ulong m = Convert.ToUInt64(dgMatches.Grid.SelectedRows[0].Cells[0].Value);
                    Match match = Repository.CreateObject(typeof(Match)) as Match;
                    await match.FastLoad(m, ConnectionIndex);
                    _provider.PlayMatch(match);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void bTagSel_Click(object sender, EventArgs e)
        {
            try
            {
                DlgLabelMatch dlg = new DlgLabelMatch();
                Repository.Connector.OpenConnection(ConnectionIndex);
                await dlg.SetRepository(Repository);
                dlg.ConnectionIndex = ConnectionIndex;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Keyword k = dlg.Selection;
                    string value = dlg.Value;
                    if (k != null)
                    {
                        Repository.Connector.BeginTransaction(ConnectionIndex);
                        if (k.IdKeyword == 0)
                        {
                            await k.Insert(ConnectionIndex);
                        }
                        for (int ix = 0; ix < dgMatches.Grid.SelectedRows.Count; ix++)
                        {
                            ulong m = Convert.ToUInt64(dgMatches.Grid.SelectedRows[ix].Cells[0].Value);
                            MatchKeyword mk = Repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                            mk.IdKeyword = k.IdKeyword;
                            mk.Name = k.Name;
                            mk.Value = value;
                            mk.IdMatch = m;
                            await mk.Insert(ConnectionIndex);
                        }
                        Repository.Connector.Commit(ConnectionIndex);
                        MessageBox.Show(MSG_LABELMATCH);
                    }
                }
            }
            catch (Exception ex)
            {
                Repository.Connector.Rollback(ConnectionIndex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Repository.Connector.CloseConnection(ConnectionIndex);
            }
        }
        private async void bFilterByTag_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripButton b = sender as ToolStripButton;
                if (b != null)
                {
                    bFilterByTag.Checked = false;
                    bFilterByPos.Checked = false;
                    bFilterByMov.Checked = false;
                    b.Checked = true;
                    _query.ConnectionIndex = ConnectionIndex;
                    _query.Repository = Repository;
                    Main app = MdiParent as Main;
                    switch (b.Name)
                    {
                        case nameof(bFilterByTag):
                            if (_query.QueryType != CAT_KEYWORDS)
                            {
                                ColumnManagerUI colmgr = new ColumnManagerUI();
                                ISQLUIQuery query = await _query.BuildKeywordsQuery(colmgr);
                                dgMatches.SetQueryConfiguration(app.DatabaseManager, this, query);
                                dgMatches.ColumnManager = colmgr;
                            }
                            break;
                        case nameof(bFilterByPos):
                            if (_query.QueryType != CAT_POSITION)
                            {
                                ColumnManagerUI colmgr = new ColumnManagerUI();
                                ISQLUIQuery query = _query.BuildPositionQuery(colmgr);
                                dgMatches.SetQueryConfiguration(app.DatabaseManager, this, query);
                                dgMatches.ColumnManager = colmgr;
                            }
                            break;
                        case nameof(bFilterByMov):
                            if (_query.QueryType != CAT_MOVE)
                            {
                                ColumnManagerUI colmgr = new ColumnManagerUI();
                                ISQLUIQuery query = _query.BuildMoveQuery(colmgr);
                                dgMatches.SetQueryConfiguration(app.DatabaseManager, this, query);
                                dgMatches.ColumnManager = colmgr;
                            }
                            break;
                    }
                    EnableButtons(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void QueryWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Repository?.ReleaseConnection(ConnectionIndex);
        }

        private async void QueryWindow_Shown(object sender, EventArgs e)
        {
            using (OverlayScope ov = new OverlayScope(this, MSG_CountingMatches))
            {
                Match m = _repository.CreateObject(typeof(Match)) as Match;
                await MatchCount(m);
            }
        }
    }
}
