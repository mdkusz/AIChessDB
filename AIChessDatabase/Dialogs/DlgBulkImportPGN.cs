using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using AIChessDatabase.PGNParser;
using BaseClassesAndInterfaces.Interfaces;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    /// <summary>
    /// Dialog to bulk import PGN files into the database.
    /// </summary>
    public partial class DlgBulkImportPGN : Form, IUIRemoteControlElement, IChessDBWindow
    {
        private List<string> _files = null;
        private bool _operation = false;
        private IObjectRepository _repository = null;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;

        public DlgBulkImportPGN()
        {
            InitializeComponent();
            lFiles.Text = LAB_FILES;
            lMatches.Text = LAB_MATCHES;
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
        /// IChessDBWindow: Data repository to use for the import operation.
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
                Text = string.Format(TTL_BULKPGNIMPORT, _repository.ServerName + " (" + _repository.ConnectionName + ")");

            }
        }
        /// <summary>
        /// IUIRemoteControlElement: Unique identifier for the UI container.
        /// </summary>
        public string UID { get; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// IUIRemoteControlElement: Form friendly name.
        /// </summary>
        public string FriendlyName { get { return FNAME_BulkIMportPGN; } }
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
        private void bFiles_Click(object sender, EventArgs e)
        {
            try
            {
                ofDlg.Filter = FIL_PGN;
                sfDlg.InitialDirectory = Path.GetFullPath(ConfigurationManager.AppSettings[SETTING_PGNdir]);
                if (ofDlg.ShowDialog() == DialogResult.OK)
                {
                    _files = new List<string>(ofDlg.FileNames);
                    bImport.Enabled = _files.Count > 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bImport_Click(object sender, EventArgs e)
        {
            try
            {
                lFileCount.Text = "";
                lMatchCount.Text = "";
                int inserted = 0;
                int processed = 0;
                int wrong = 0;
                int duplicates = 0;
                bFiles.Enabled = false;
                bImport.Enabled = false;
                _operation = true;
                pbFiles.Value = 0;
                pbFiles.Maximum = _files.Count;
                sfDlg.InitialDirectory = Path.GetFullPath(ConfigurationManager.AppSettings[SETTING_LogPathBase]);
                sfDlg.Filter = FIL_TXT;
                sfDlg.Title = TTL_PGNERROR;
                string errors = "";
                if (sfDlg.ShowDialog(this) == DialogResult.OK)
                {
                    errors = sfDlg.FileName;
                }
                int files = 0;
                UseWaitCursor = true;
                Repository.Connector.OpenConnection(ConnectionIndex);
                ISQLUIQuery loopquery = null;
                DateTime initf = DateTime.Now;
                foreach (string file in _files)
                {
                    PGNFile pgnf = null;
                    List<Match> matches = null;
                    int proclocal = 0;
                    pbMatches.Value = 0;
                    if (string.Compare(Path.GetExtension(file), ".pgn", true) == 0)
                    {
                        ParallelPGNFile ppf = new ParallelPGNFile();
                        try
                        {
                            if (string.IsNullOrEmpty(errors))
                            {
                                ppf.Parse(file);
                            }
                            else
                            {
                                ppf.Parse(file, errors);
                            }
                            pgnf = ppf.PGNFile;
                            pgnf.ConvertAllMatches(Repository);
                            pbMatches.Maximum = pgnf.MatchCount;
                        }
                        catch (Exception ex)
                        {
                            if (!string.IsNullOrEmpty(errors))
                            {
                                File.AppendAllText(errors, Path.GetFileName(file) + ": " + ex.Message + "\n");
                            }
                            files++;
                            BeginInvoke((Action)(() =>
                            {
                                pbFiles.Value = files;
                                lFileCount.Text = string.Format(LAB_BULKFILESPROC, files, _files.Count, DateTime.Now.Subtract(initf).ToString(@"hh\:mm\:ss"));
                            }));
                            Application.DoEvents();
                            continue;
                        }
                    }
                    else
                    {
                        FileStream fs = null;
                        try
                        {
                            fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                            BinaryFormatter bf = new BinaryFormatter();
                            matches = bf.Deserialize(fs) as List<Match>;
                            pbMatches.Maximum = matches.Count;
                        }
                        finally
                        {
                            if (fs != null)
                            {
                                fs.Close();
                            }
                        }
                    }
                    DateTime initm = DateTime.Now;
                    wrong = 0;
                    for (int m = 0; m < pbMatches.Maximum; m++)
                    {
                        processed++;
                        proclocal++;
                        Match match = null;
                        try
                        {
                            Repository.Connector.BeginTransaction(ConnectionIndex);
                            match = pgnf != null ? pgnf.GetMatch(m, Repository) : Repository.CreateObject(matches[m]) as Match;
                            if (!cbDuplicates.Checked)
                            {
                                if (await match.Duplicated(ConnectionIndex, loopquery))
                                {
                                    duplicates++;
                                    Repository.Connector.Rollback(ConnectionIndex);
                                }
                                else
                                {
                                    loopquery = await match.LoopInsert(ConnectionIndex, loopquery);
                                    Repository.Connector.Commit(ConnectionIndex);
                                    inserted++;
                                }
                            }
                            else
                            {
                                loopquery = await match.LoopInsert(ConnectionIndex, loopquery);
                                Repository.Connector.Commit(ConnectionIndex);
                                inserted++;
                            }
                        }
                        catch (Exception ex)
                        {
                            wrong++;
                            Repository.Connector.Rollback(ConnectionIndex);
                            if (!string.IsNullOrEmpty(errors))
                            {
                                string pgn = "";
                                try
                                {
                                    pgn = match.GetPGN(true);
                                }
                                catch
                                {
                                    pgn = $"{m} match";
                                }
                                try { File.AppendAllText(errors, string.Join("\r\n", Path.GetFileName(file), ex.Message, pgn, "\r\n\r\n")); } catch { }
                            }
                        }
                        BeginInvoke((Action)(() =>
                        {
                            pbMatches.Value = proclocal;
                            lMatchCount.Text = string.Format(LAB_BULKMATCHESINT, proclocal, pbMatches.Maximum, DateTime.Now.Subtract(initm).ToString(@"hh\:mm\:ss"), wrong);
                        }));
                        Application.DoEvents();
                    }
                    if (cbDelete.Checked && (wrong == 0))
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            try
                            {
                                File.Delete(file);
                                break;
                            }
                            catch (IOException)
                            {
                                Thread.Sleep(500);
                            }
                        }
                    }
                    files++;
                    BeginInvoke((Action)(() =>
                    {
                        pbFiles.Value = files;
                        lFileCount.Text = string.Format(LAB_BULKFILESPROC, files, _files.Count, DateTime.Now.Subtract(initf).ToString(@"hh\:mm\:ss"));
                    }));
                    Application.DoEvents();
                }
                MessageBox.Show(string.Format(MSG_MATCHESINSERTED2, new object[] { processed, inserted, duplicates }));
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message + "\n" + ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                UseWaitCursor = false;
                _operation = false;
                _files.Clear();
                bFiles.Enabled = true;
                Repository.Connector.CloseConnection(ConnectionIndex);
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void DlgBulkImportPGN_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_operation && (MessageBox.Show(MSG_DBOPERATIONQ, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No))
            {
                e.Cancel = true;
            }
        }

        private void DlgBulkImportPGN_FormClosed(object sender, FormClosedEventArgs e)
        {
            _repository?.ReleaseConnection(ConnectionIndex);
        }
    }
}
