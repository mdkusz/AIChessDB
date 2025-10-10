using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using DesktopControls.Controls;
using GlobalCommonEntities.Interfaces;
using GlobalCommonEntities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Dialogs
{
    /// <summary>
    /// Dialog box to copy matches from one database to another.
    /// </summary>
    public partial class DlgBulkCopyDB : Form, IUIRemoteControlElement
    {
        private IAppServiceProvider _provider = null;
        private IObjectRepository _repository;
        private bool _copy = false;
        private RelevantControlCollector _collector = null;
        private ControlInteractor _interactor = null;

        public DlgBulkCopyDB()
        {
            InitializeComponent();
            label1.Text = LAB_MATCHCOUNT;
            label2.Text = LAB_DESTDB;
            label3.Text = LAB_CHUNKSIZE;
            _collector = new RelevantControlCollector() { BaseInstance = this };
            _interactor = new ControlInteractor() { ElementCollector = _collector };
        }
        /// <summary>
        /// Connection index to the source database.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// Connection index to the destination database.
        /// </summary>
        public int ConnectionDestIndex { get; set; }
        /// <summary>
        /// Application Service Provider to provide the repositories and other services.
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
        /// Database object repository
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
                cbDestinationDB.Items.Clear();
                if (value != null)
                {
                    ConnectionIndex = _repository?.GetFreeConnection() ?? -1;
                    if (ConnectionIndex == -1)
                    {
                        throw new InvalidOperationException(ERR_NOAVAILABLECONNECTIONS);
                    }
                    foreach (string odb in _provider.OtherRepositories(_repository.ConnectionName))
                    {
                        cbDestinationDB.Items.Add(odb);
                    }
                    cbDestinationDB.Items.Add(TXT_DESTFILES);
                    Text = string.Format(TTL_BULKCOPY, _repository.ServerName + " (" + _repository.ConnectionName + ")");
                }
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
        private void cbDestinationDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int chs = 0;
            bExport.Enabled = int.TryParse(txtSize.Text, out chs) && (cbDestinationDB.SelectedItem != null) && !_copy;
        }

        private async void bExport_Click(object sender, EventArgs e)
        {
            _copy = true;
            bExport.Enabled = false;
            UseWaitCursor = true;
            bool file = false;
            string filename = null;
            try
            {
                if (cbDestinationDB.SelectedItem.ToString() == TXT_DESTFILES)
                {
                    file = true;
                    if (file)
                    {
                        sfDlg.Filter = FIL_MATCHES;
                        if (sfDlg.ShowDialog() == DialogResult.OK)
                        {
                            filename = Path.Combine(Path.GetDirectoryName(sfDlg.FileName), Path.GetFileNameWithoutExtension(sfDlg.FileName) + "{0}" + Path.GetExtension(sfDlg.FileName));
                        }
                        else
                        {
                            MessageBox.Show(MSG_OPCANCEL);
                            return;
                        }
                    }
                }
                Match m = Repository.CreateObject(typeof(Match)) as Match;
                ulong nm = await m.GetCount();
                int sz = int.Parse(txtSize.Text);
                if ((ulong)Math.Ceiling((double)nm / sz) > int.MaxValue)
                {
                    sz = (int)(nm / int.MaxValue);
                }
                txtSize.Text = sz.ToString();
                pbCopy.Maximum = (int)Math.Ceiling((double)nm / sz);
                pbCopy.Value = 0;
                int ix = 0;
                IObjectRepository rdest = null;
                if (!file)
                {
                    rdest = _provider.GetRepository(cbDestinationDB.SelectedItem.ToString());
                }
                while (nm > 0)
                {
                    int cnt = file ? await m.BulkCopy(ix, sz, string.Format(filename, ix), ConnectionDestIndex)
                        : await m.BulkCopy(ix, sz, cbDuplicates.Checked, rdest, ConnectionIndex, ConnectionDestIndex);
                    nm -= (ulong)cnt;
                    ix++;
                    if (ix < pbCopy.Maximum)
                    {
                        BeginInvoke((Action)(() => { pbCopy.Value = ix; }));
                        Application.DoEvents();
                    }
                }
                MessageBox.Show(MSG_DBEXPORTED);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _copy = false;
                bExport.Enabled = true;
                UseWaitCursor = false;
            }
        }

        private void DlgBulkCopyDB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_copy)
            {
                e.Cancel = true;
                MessageBox.Show(MSG_DBOPERATION);
            }
        }

        private async void DlgBulkCopyDB_Shown(object sender, EventArgs e)
        {
            try
            {
                Match m = Repository.CreateObject(typeof(Match)) as Match;
                label1.Text = string.Format(LAB_MATCHCOUNT, await m.GetCount());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void DlgBulkCopyDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            Repository?.ReleaseConnection(ConnectionIndex);
        }
    }
}
