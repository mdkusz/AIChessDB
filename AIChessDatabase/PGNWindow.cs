using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using AIChessDatabase.PGNParser;
using BaseClassesAndInterfaces.Interfaces;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase
{
    /// <summary>
    /// Form to import and view PGN files.
    /// </summary>
    public partial class PGNWindow : Form, IChessDBWindow, IUIRemoteControlElement, IAIChessPlayer
    {
        internal class ldResult
        {
            public DataTable dt { get; set; }
            public PGNFile pgnfile { get; set; }
        }
        private PGNFile _pgnFile = null;
        private string _bmffilename = null;
        private string _pgnfilename = null;
        private string _errorsFile = null;
        private List<Match> _matches = null;
        private IObjectRepository _repository = null;
        private IAppServiceProvider _provider = null;
        private DataTable _dtgrid = new DataTable();
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;

        public PGNWindow()
        {
            InitializeComponent();
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
        }
        /// <summary>
        /// IChessDBWindow: Connection index to the database repository.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// IChessDBWindow: Window identifier for the application.
        /// </summary>
        [Browsable(false)]
        public string WINDOW_ID
        {
            get
            {
                return ID_PGNWINDOW;
            }
        }
        /// <summary>
        /// IChessDBWindow: Database repository
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
                if (value != null)
                {
                    ConnectionIndex = _repository?.GetFreeConnection() ?? -1;
                    if (ConnectionIndex == -1)
                    {
                        throw new InvalidOperationException(ERR_NOAVAILABLECONNECTIONS);
                    }
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
        /// IUIRemoteControlElement, IAIChessPlayer: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return FNAME_PGNWindow; } }
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
        /// IAIChessPlayer: Match is editable. New moves can be added, and existing moves can be canceled.
        /// </summary>
        public bool Editable { get { return false; } }
        /// <summary>
        /// IAIChessPlayer: Json object with match information.
        /// </summary>
        public Match MatchInfo { get { return chessPlayer.CurrentMatch; } }
        /// <summary>
        /// IAIChessPlayer: List of moves in the match. Each move is a Json object.
        /// </summary>
        public List<MatchMove> Moves { get { return new List<MatchMove>(chessPlayer.CurrentMatch.Moves); } }
        /// <summary>
        /// IAIChessPlayer: Current move in the match.
        /// </summary>
        public MatchMove CurrentMove { get { return chessPlayer.CurrentMove; } }
        /// <summary>
        /// IAIChessPlayer: Current board position.
        /// </summary>
        public string CurrentBoard { get { return chessPlayer.CurrentBoard; } }
        /// <summary>
        /// IAIChessPlayer: Advance to the next move in the match.
        /// </summary>
        /// <param name="comment">
        /// Show a comment on the move being made.
        /// </param>
        /// <returns>
        /// A list with three moves: the previous move, the current move, and the next move.
        /// </returns>
        public List<MatchMove> NextMove(string comment)
        {
            return chessPlayer.EnqueueMove(comment);
        }
        /// <summary>
        /// IAIChessPlayer: Forward to the previous move in the match.
        /// </summary>
        public MatchMove PrevMove { get { return chessPlayer.MoveBack(); } }
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
                DataTable dt = bsData.DataSource as DataTable;
                bsData.SuspendBinding();
                int crows = dt.Rows.Count;
                for (int m = 0; m < pf.MatchCount; m++)
                {
                    PGNMatch match = pf.GetPGNMatch(m);
                    DataRow row = dt.NewRow();
                    row["id"] = m + crows;
                    row["selected"] = 0;
                    row["match"] = match.GetAttribute("Event");
                    row["date"] = match.GetAttribute("Date");
                    row["white"] = match.GetAttribute("White");
                    row["black"] = match.GetAttribute("Black");
                    row["result"] = match.Result;
                    row["moves"] = match.MoveCount;
                    row["fullmoves"] = match.FullMoveCount;
                    dt.Rows.Add(row);
                }
                bsData.ResumeBinding();
                chessPlayer.CurrentMatch = pf.GetMatch(0, Repository);
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
        /// PGNFile: object representing the PGN file to be displayed and imported.
        /// Get the PGN file object or set it to a new PGN file.
        /// Setting the PGN file will clear the current matches and populate the data grid with the matches from the PGN file.
        /// </summary>
        [Browsable(false)]
        public PGNFile PGN
        {
            get
            {
                return _pgnFile;
            }
            set
            {
                _matches = null;
                _bmffilename = null;
                _pgnFile = value;
                Text = string.Format(TTL_PGNWINDOW, _pgnFile.FileName, Repository.ServerName + " (" + Repository.ConnectionName + ")");
            }
        }
        [Browsable(false)]
        public string PGNFilename
        {
            get
            {
                return _pgnfilename;
            }
            set
            {
                _pgnfilename = value;
                _pgnFile = null;
                _matches = null;
                Text = string.Format(TTL_PGNWINDOW, _pgnfilename, Repository.ServerName + " (" + Repository.ConnectionName + ")");
            }
        }
        [Browsable(false)]
        public string BMFFilename
        {
            get
            {
                return _bmffilename;
            }
            set
            {
                _bmffilename = value;
                _pgnFile = null;
                _matches = null;
                Text = string.Format(TTL_PGNWINDOW, _bmffilename, Repository.ServerName + " (" + Repository.ConnectionName + ")");
            }
        }
        /// <summary>
        /// Set the match count label and progress bar based on the selected matches.
        /// </summary>
        private void SetMatchCount()
        {
            int nsel = 0;
            foreach (DataGridViewRow row in dgMatches.Rows)
            {
                if ((int)row.Cells["sel"].Value == 1)
                {
                    nsel++;
                }
            }
            lMatchCount.Text = string.Format(LAB_MATCHSEL, new object[] { dgMatches.Rows.Count, nsel });
            pgInsert.Maximum = nsel;
        }
        /// <summary>
        /// Get the match object from the data grid by its ID.
        /// </summary>
        /// <param name="id">
        /// Match ID to retrieve.
        /// </param>
        /// <returns>
        /// Match object corresponding to the ID.
        /// </returns>
        private Match GetMatch(int id)
        {
            if (_pgnFile != null)
            {
                PGNMatch pgnmatch = _pgnFile.GetPGNMatch(id);
                return pgnmatch.ConvertToMatch(Repository);
            }
            else
            {
                return _matches[id];
            }
        }
        /// <summary>
        /// Create a DataTable schema for the matches data grid.
        /// </summary>
        /// <returns>
        /// DataTable with the schema for the matches.
        /// </returns>
        private DataTable CreateSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("selected", typeof(int));
            dt.Columns.Add("match", typeof(string));
            dt.Columns.Add("date", typeof(string));
            dt.Columns.Add("white", typeof(string));
            dt.Columns.Add("black", typeof(string));
            dt.Columns.Add("result", typeof(string));
            dt.Columns.Add("moves", typeof(int));
            dt.Columns.Add("fullmoves", typeof(int));
            return dt;
        }
        private void bView_Click(object sender, EventArgs e)
        {
            if (dgMatches.SelectedRows.Count == 1)
            {
                try
                {
                    int m = Convert.ToInt32(dgMatches.SelectedRows[0].Cells["id"].Value);
                    Match match = GetMatch(m);
                    chessPlayer.CurrentMatch = match;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void bCheck_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgMatches.SelectedRows)
                {
                    row.Cells["sel"].Value = 1;
                }
                SetMatchCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bUncheck_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgMatches.SelectedRows)
                {
                    row.Cells["sel"].Value = 0;
                }
                SetMatchCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bInsert_Click(object sender, EventArgs e)
        {
            try
            {
                bInsert.Enabled = false;
                bSelAll.Enabled = false;
                bUncheck.Enabled = false;
                bCheck.Enabled = false;
                dgMatches.Enabled = false;
                UseWaitCursor = true;
                DateTime dtini = DateTime.Now;
                DialogResult dr = MessageBox.Show(MSG_INSERTDUPLICATES, TTL_DUPLICATES, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                if (dr != DialogResult.Cancel)
                {
                    int inserted = 0;
                    int wrong = 0;
                    int processed = 0;
                    int duplicates = 0;
                    if (_pgnFile != null)
                    {
                        _pgnFile.ConvertAllMatches(Repository);
                    }
                    ISQLUIQuery loopquery = null;
                    Repository.Connector.OpenConnection(ConnectionIndex);
                    foreach (DataRow row in _dtgrid.Rows)
                    {
                        if (Convert.ToInt32(row["selected"]) != 0)
                        {
                            Match match = null;
                            try
                            {
                                processed++;
                                int m = Convert.ToInt32(row["id"]);
                                match = _pgnFile != null ? _pgnFile.GetMatch(m, Repository) : Repository.CreateObject(_matches[m]) as Match;
                                Repository.Connector.BeginTransaction(ConnectionIndex);
                                if (dr == DialogResult.No)
                                {
                                    if (await match.Duplicated(ConnectionIndex, loopquery))
                                    {
                                        duplicates++;
                                        Repository.Connector.Rollback(ConnectionIndex);
                                    }
                                    else
                                    {
                                        loopquery = await match.LoopInsert(ConnectionIndex, loopquery);
                                        row["selected"] = 0;
                                        Repository.Connector.Commit(ConnectionIndex);
                                        inserted++;
                                    }
                                }
                                else
                                {
                                    loopquery = await match.LoopInsert(ConnectionIndex, loopquery);
                                    row["selected"] = 0;
                                    Repository.Connector.Commit(ConnectionIndex);
                                    inserted++;
                                }
                            }
                            catch (Exception ex)
                            {
                                wrong++;
                                Repository.Connector.Rollback(ConnectionIndex);
                                if (!string.IsNullOrEmpty(_errorsFile))
                                {
                                    string pgn = "";
                                    try
                                    {
                                        pgn = match.GetPGN(true);
                                    }
                                    catch
                                    {
                                        pgn = $"{row["id"]} match";
                                    }
                                    try { File.AppendAllText(_errorsFile, string.Join("\r\n", Path.GetFileName(_pgnfilename ?? _bmffilename ?? ""), ex.Message, pgn, "\r\n\r\n")); } catch { }
                                }
                            }
                            BeginInvoke((Action)(() =>
                            {
                                pgInsert.Value = processed;
                                lMatchCount.Text = string.Format(LAB_MATCHINT, new object[] { dgMatches.Rows.Count, inserted, wrong, DateTime.Now.Subtract(dtini).ToString(@"hh\:mm\:ss") });
                            }));
                            Application.DoEvents();
                        }
                    }
                    MessageBox.Show(string.Format(MSG_MATCHESINSERTED, new object[] { processed, inserted, duplicates }));
                    if (wrong == 0)
                    {
                        if (MessageBox.Show(MSG_DELETEFILE, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            try
                            {
                                File.Delete(PGN.FileName);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    pgInsert.Value = 0;
                    bsData.DataSource = _dtgrid;
                    SetMatchCount();
                }
            }
            catch (Exception ex)
            {
                Repository.Connector.Rollback(ConnectionIndex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                bInsert.Enabled = true;
                bSelAll.Enabled = true;
                bUncheck.Enabled = true;
                bCheck.Enabled = true;
                dgMatches.Enabled = true;
                UseWaitCursor = false;
                Repository.Connector.CloseConnection(ConnectionIndex);
            }
        }

        private void bSelAll_Click(object sender, EventArgs e)
        {
            try
            {
                bsData.DataSource = null;
                for (int m = 0; m < _dtgrid.Rows.Count; m++)
                {
                    DataRow row = _dtgrid.Rows[m];
                    row["selected"] = 1;
                }
                bsData.DataSource = _dtgrid;
                SetMatchCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PGNWindow_Load(object sender, EventArgs e)
        {
            Icon = Icon.Clone() as Icon;
        }

        private void PGNWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Repository?.ReleaseConnection(ConnectionIndex);
        }

        private async void PGNWindow_Shown(object sender, EventArgs e)
        {
            try
            {
                _errorsFile = "";
                if (!string.IsNullOrEmpty(_pgnfilename) && File.Exists(_pgnfilename))
                {
                    sfDlg.InitialDirectory = Path.GetFullPath(ConfigurationManager.AppSettings[SETTING_LogPathBase]);
                    sfDlg.Filter = FIL_TXT;
                    sfDlg.Title = TTL_PGNERROR;
                    if (sfDlg.ShowDialog() == DialogResult.OK)
                    {
                        _errorsFile = sfDlg.FileName;
                    }
                }
                using (new OverlayScope(this, MSG_LoadingPGN))
                {
                    ldResult result = await Task.Run(() =>
                    {
                        ldResult lres = new ldResult();
                        DataTable dt = CreateSchema();
                        if (!string.IsNullOrEmpty(_pgnfilename) && File.Exists(_pgnfilename))
                        {
                            ParallelPGNFile pgnf = new ParallelPGNFile();
                            if (!string.IsNullOrEmpty(_errorsFile))
                            {
                                pgnf.Parse(_pgnfilename, _errorsFile);
                            }
                            else
                            {
                                pgnf.Parse(_pgnfilename);
                            }
                            lres.pgnfile = pgnf.PGNFile;
                        }
                        if (lres.pgnfile != null)
                        {
                            for (int m = 0; m < lres.pgnfile.MatchCount; m++)
                            {
                                PGNMatch match = lres.pgnfile.GetPGNMatch(m);
                                DataRow row = dt.NewRow();
                                row["id"] = m;
                                row["selected"] = 0;
                                row["match"] = match.GetAttribute("Event");
                                row["date"] = match.GetAttribute("Date");
                                row["white"] = match.GetAttribute("White");
                                row["black"] = match.GetAttribute("Black");
                                row["result"] = match.Result;
                                row["moves"] = match.MoveCount;
                                row["fullmoves"] = match.FullMoveCount;
                                dt.Rows.Add(row);
                            }
                        }
                        else if (!string.IsNullOrEmpty(_bmffilename) && File.Exists(_bmffilename))
                        {
                            using (FileStream fs = new FileStream(_bmffilename, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                _matches = bf.Deserialize(fs) as List<Match>;
                            }
                            for (int m = 0; m < _matches.Count; m++)
                            {
                                _matches[m] = Repository.CreateObject(_matches[m]) as Match;
                                Match match = _matches[m];
                                DataRow row = dt.NewRow();
                                row["id"] = m;
                                row["selected"] = 0;
                                row["match"] = match.Description;
                                row["date"] = match.Date;
                                row["white"] = match.White;
                                row["black"] = match.Black;
                                row["result"] = match.ResultText;
                                row["moves"] = match.MoveCount;
                                row["fullmoves"] = match.FullMoveCount;
                                dt.Rows.Add(row);
                            }
                        }
                        lres.dt = dt;
                        return lres;
                    });
                    _dtgrid = result.dt;
                    _pgnFile = result.pgnfile;
                    bsData.DataSource = _dtgrid;
                    SetMatchCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                pgInsert.Value = 0;
            }
        }
    }
    /// <summary>
    /// Progress data for background operations in the PGN import process.
    /// </summary>
    public class BGData
    {
        public BGData()
        {
        }
        /// <summary>
        /// Dialog result for the background operation.
        /// </summary>
        public DialogResult Dr { get; set; }
        /// <summary>
        /// Count of matches processed, inserted, and duplicates found during the import operation.
        /// </summary>
        public int Processed { get; set; }
        /// <summary>
        /// Count of matches inserted into the database.
        /// </summary>
        public int Inserted { get; set; }
        /// <summary>
        /// Count of duplicate matches found during the import operation.
        /// </summary>
        public int Duplicates { get; set; }
    }
}
