using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Panel to edit match information, comments and keywords.
    /// </summary>
    public partial class EditMatchPanel : UserControl
    {
        private IObjectRepository _repository = null;
        private Match _match = null;
        private Match _match0 = null;
        private MatchMove _cmove = null;
        private bool _save = true;
        private List<ObjectBase> _changes = new List<ObjectBase>();
        private List<ObjectBase> _deleted = new List<ObjectBase>();
        private List<ObjectBase> _new = new List<ObjectBase>();

        public EditMatchPanel()
        {
            InitializeComponent();
            lComments.Text = LAB_COMMENTS;
            label7.Text = LAB_BLACK;
            label6.Text = LAB_RESULT;
            label5.Text = LAB_DATE;
            label4.Text = LAB_WHITE;
            label3.Text = LAB_MATCH;
            label2.Text = LAB_VALUE;
            label1.Text = LAB_KEYWORD;
        }
        /// <summary>
        /// Event raised when comments are changed in the comments editor.
        /// </summary>
        public event EventHandler CommentsChanged = null;
        /// <summary>
        /// Connection index to use for database operations.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// Current editing match object.
        /// </summary>
        [Browsable(false)]
        public Match CurrentMatch
        {
            get
            {
                return _match;
            }
            set
            {
                _changes.Clear();
                _deleted.Clear();
                _new.Clear();
                cbKeywords.Items.Clear();
                lbComments.Items.Clear();
                txtKeyValue.Clear();
                _match = value;
                if (_match != null)
                {
                    _repository = _match.Repository;
                    if (_match.Key[0] == 0)
                    {
                        _match0 = _repository.CreateObject(_match) as Match;
                    }
                    txtBlack.Text = _match.Black;
                    txtDate.Text = _match.Date;
                    txtMatch.Text = _match.Description;
                    cbResult.SelectedItem = _match.ResultText;
                    txtWhite.Text = _match.White;
                    foreach (MatchKeyword mk in _match.Keywords)
                    {
                        cbKeywords.Items.Add(mk);
                    }
                }
                else
                {
                    txtBlack.Clear();
                    txtDate.Clear();
                    txtMatch.Clear();
                    cbResult.SelectedItem = "*";
                    txtWhite.Clear();
                    _repository = null;
                }
                bUpdate.Enabled = false;
                bCancelChg.Enabled = false;
            }
        }
        /// <summary>
        /// Current result of the match as selected in the combo box.
        /// </summary>
        public string CurrentResult
        {
            get
            {
                return cbResult.SelectedItem as string;
            }
        }
        /// <summary>
        /// Allow save button to be enabled or disabled.
        /// </summary>
        public bool AllowSave
        {
            get
            {
                return _save && (cbResult.SelectedItem != null);
            }
            set
            {
                _save = value;
                bUpdate.Enabled = _save;
            }
        }
        /// <summary>
        /// Sets the current move and updates the associated comments.
        /// </summary>
        public void SetMove(MatchMove mov)
        {
            try
            {
                _cmove = mov;
                if (mov != null)
                {
                    lbComments.Items.Clear();
                    txtComment.Clear();
                    foreach (MoveComment com in mov.Comments)
                    {
                        lbComments.Items.Add(com);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtMatch_TextChanged(object sender, EventArgs e)
        {
            _match.Description = txtMatch.Text;
            MatchKeyword mk = _match.GetKeyword(KEY_EVENT);
            if (mk != null)
            {
                mk.Value = txtMatch.Text;
                if (!_changes.Contains(mk))
                {
                    _changes.Add(mk);
                }
            }
            bUpdate.Enabled = AllowSave;
            bCancelChg.Enabled = true;
        }

        private void txtComment_TextChanged(object sender, EventArgs e)
        {
            bAddCom.Enabled = !string.IsNullOrEmpty(txtComment.Text);
            bChangeCom.Enabled = !string.IsNullOrEmpty(txtComment.Text);
        }

        private void txtNewKey_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewKey.Text))
            {
                foreach (MatchKeyword mk in cbKeywords.Items)
                {
                    if (string.Compare(mk.Name, txtNewKey.Text, true) == 0)
                    {
                        bNewKey.Enabled = false;
                        return;
                    }
                }
                bNewKey.Enabled = true;
            }
            else
            {
                bNewKey.Enabled = false;
            }
        }

        private void txtKeyValue_TextChanged(object sender, EventArgs e)
        {
            bChangeValue.Enabled = !string.IsNullOrEmpty(txtKeyValue.Text);
        }

        private void cbKeywords_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MatchKeyword mk = cbKeywords.SelectedItem as MatchKeyword;
                if (mk != null)
                {
                    bDelete.Enabled = true;
                    txtKeyValue.Text = mk.Value;
                }
                else
                {
                    bDelete.Enabled = false;
                    txtKeyValue.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lbComments_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MoveComment com = lbComments.SelectedItem as MoveComment;
                if (com != null)
                {
                    bDeleteCom.Enabled = true;
                    txtComment.Text = com.ToString();
                }
                else
                {
                    bDeleteCom.Enabled = false;
                    txtComment.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bChangeValue_Click(object sender, EventArgs e)
        {
            try
            {
                MatchKeyword mk = cbKeywords.SelectedItem as MatchKeyword;
                if (mk != null)
                {
                    mk.Value = txtKeyValue.Text;
                    if (!_new.Contains(mk))
                    {
                        if (!_changes.Contains(mk))
                        {
                            _changes.Add(mk);
                        }
                    }
                    bUpdate.Enabled = AllowSave;
                    bCancelChg.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                MatchKeyword mk = cbKeywords.SelectedItem as MatchKeyword;
                if (mk != null)
                {
                    await _match.RemoveChildAsync(mk, ConnectionIndex);
                    if (!_deleted.Contains(mk))
                    {
                        if (_new.Contains(mk))
                        {
                            _new.Remove(mk);
                        }
                        else
                        {
                            _deleted.Add(mk);
                            _changes.Remove(mk);
                        }
                    }
                    txtKeyValue.Clear();
                    cbKeywords.Items.Remove(mk);
                    bUpdate.Enabled = AllowSave;
                    bCancelChg.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bNewKey_Click(object sender, EventArgs e)
        {
            try
            {
                MatchKeyword mk = _repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                mk.Name = txtNewKey.Text;
                mk.Value = "";
                await _match.AddChildAsync(mk, ConnectionIndex);
                cbKeywords.Items.Add(mk);
                cbKeywords.SelectedItem = mk;
                _new.Add(mk);
                txtNewKey.Clear();
                bUpdate.Enabled = AllowSave;
                bCancelChg.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bAddCom_Click(object sender, EventArgs e)
        {
            try
            {
                MoveComment com = _repository.CreateObject(typeof(MoveComment)) as MoveComment;
                com.Comment = txtComment.Text;
                MatchMove mm = _cmove;
                await mm.AddChildAsync(com, ConnectionIndex);
                lbComments.Items.Add(com);
                _new.Add(com);
                txtComment.Clear();
                bUpdate.Enabled = AllowSave;
                bCancelChg.Enabled = true;
                CommentsChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bChangeCom_Click(object sender, EventArgs e)
        {
            try
            {
                MoveComment com = lbComments.SelectedItem as MoveComment;
                if (com != null)
                {
                    com.Comment = txtComment.Text;
                    if (!_new.Contains(com))
                    {
                        if (!_changes.Contains(com))
                        {
                            _changes.Add(com);
                        }
                    }
                    lbComments.Items[lbComments.SelectedIndex] = com;
                    txtComment.Clear();
                    CommentsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bDeleteCom_Click(object sender, EventArgs e)
        {
            try
            {
                MoveComment mc = lbComments.SelectedItem as MoveComment;
                if (mc != null)
                {
                    await _cmove.RemoveChildAsync(mc, ConnectionIndex);
                    if (!_deleted.Contains(mc))
                    {
                        if (_new.Contains(mc))
                        {
                            _new.Remove(mc);
                        }
                        else
                        {
                            _deleted.Add(mc);
                            _changes.Remove(mc);
                        }
                    }
                    txtComment.Clear();
                    lbComments.Items.Remove(mc);
                    bUpdate.Enabled = AllowSave;
                    bCancelChg.Enabled = true;
                    CommentsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(MSG_COMMITCHG, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    _repository.Connector.OpenConnection(ConnectionIndex);
                    _repository.Connector.BeginTransaction(ConnectionIndex);
                    if (_match.Key[0] == 0)
                    {
                        if (await _match.Duplicated(ConnectionIndex))
                        {
                            if (MessageBox.Show(MSG_MATCHDUPLICATED, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.No)
                            {
                                await _match.Insert(ConnectionIndex);
                                _repository.Connector.Commit(ConnectionIndex);
                            }
                        }
                        else
                        {
                            await _match.Insert(ConnectionIndex);
                            _repository.Connector.Commit(ConnectionIndex);
                        }
                    }
                    else
                    {
                        foreach (ObjectBase obj in _new)
                        {
                            await obj.Insert(ConnectionIndex);
                        }
                        foreach (ObjectBase obj in _deleted)
                        {
                            await obj.Delete(ConnectionIndex);
                        }
                        foreach (ObjectBase obj in _changes)
                        {
                            await obj.Update(ConnectionIndex);
                        }
                        await _match.Update(ConnectionIndex);
                        _repository.Connector.Commit(ConnectionIndex);
                    }
                    CurrentMatch = _match;
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
            }
        }

        private async void bCancelChg_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(MSG_CANCELCHG, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    if (_match.Key[0] == 0)
                    {
                        CurrentMatch = _match0;
                    }
                    else
                    {
                        Match nm = _repository.CreateObject(typeof(Match)) as Match;
                        await nm.FastLoad(_match.Key[0], ConnectionIndex);
                        CurrentMatch = nm;
                    }
                    CommentsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtWhite_TextChanged(object sender, EventArgs e)
        {
            _match.White = txtWhite.Text;
            MatchKeyword mk = _match.GetKeyword(KEY_WHITE);
            if (mk != null)
            {
                mk.Value = txtWhite.Text;
                if (!_changes.Contains(mk))
                {
                    _changes.Add(mk);
                }
            }
            bUpdate.Enabled = AllowSave;
            bCancelChg.Enabled = true;
        }

        private void txtBlack_TextChanged(object sender, EventArgs e)
        {
            _match.Black = txtBlack.Text;
            MatchKeyword mk = _match.GetKeyword(KEY_BLACK);
            if (mk != null)
            {
                mk.Value = txtBlack.Text;
                if (!_changes.Contains(mk))
                {
                    _changes.Add(mk);
                }
            }
            bUpdate.Enabled = AllowSave;
            bCancelChg.Enabled = true;
        }

        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            _match.Date = txtDate.Text;
            MatchKeyword mk = _match.GetKeyword(KEY_DATE);
            if (mk != null)
            {
                mk.Value = txtDate.Text;
                if (!_changes.Contains(mk))
                {
                    _changes.Add(mk);
                }
            }
            bUpdate.Enabled = AllowSave;
            bCancelChg.Enabled = true;
        }

        private void cbResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbResult.SelectedItem != null) && (_match != null))
            {
                _match.ResultText = cbResult.SelectedItem as string;
                MatchKeyword mk = _match.GetKeyword(KEY_RESULT);
                if (mk != null)
                {
                    mk.Value = cbResult.SelectedItem as string;
                    if (!_changes.Contains(mk))
                    {
                        _changes.Add(mk);
                    }
                }
                bUpdate.Enabled = AllowSave;
                bCancelChg.Enabled = true;
            }
        }
    }
}
