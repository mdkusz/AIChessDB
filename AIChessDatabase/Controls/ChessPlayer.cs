using AIChessDatabase.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Chess board viewr. Shows match positions, comments and keywords. It has controls to navigate through the match moves.
    /// </summary>
    public partial class ChessPlayer : UserControl
    {
        private Match _match = null;
        private MatchMoveDisplay _display = null;
        private int _currentMove = 1;
        private bool _player = true;
        private string _moveText = "";
        private Queue<string> _moveQueue = new Queue<string>();

        public ChessPlayer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Event handler to notify when the current move changes.
        /// </summary>
        public event EventHandler MoveChanged = null;
        /// <summary>
        /// Connection index to use for database operations.
        /// </summary>
        public int ConnectionIndex { get; set; }
        /// <summary>
        /// Current match to display. It can be null, in which case the board is reset.
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
                _match = value;
                if (_match != null)
                {
                    MatchMove imove = _match.GetMove(1, true);
                    _player = (imove != null) || (_match.MoveCount == 0);
                    board.Reset(_match.InitialPosition.Board.Board, _player);
                    _currentMove = 1;
                    _moveText = _player ? "1." : "1... ";
                    if (_display != null)
                    {
                        _display.SetMatch(_match);
                    }
                    SetComments(imove);
                    SetPosInfo(_match.InitialPosition);
                    SetMatchInfo();
                    if (AllowSetPosition)
                    {
                        AllowSetPosition = _match.IdMatch == 0;
                    }
                }
                else
                {
                    board.Reset(true);
                    _player = true;
                }
            }
        }
        /// <summary>
        /// Current move being displayed. It can be null if there is no current move.
        /// </summary>
        [Browsable(false)]
        public MatchMove CurrentMove
        {
            get
            {
                return _match.GetMove(_currentMove, _player);
            }
        }
        /// <summary>
        /// Next move to be played in the match. It can be null if there are no more moves.
        /// </summary>
        [Browsable(false)]
        public MatchMove NextMove
        {
            get
            {
                return _match.GetMove(_player ? _currentMove : (_currentMove + 1), !_player);
            }
        }
        /// <summary>
        /// Previous move that has been played in the match. It can be null if the current move is the first one.
        /// </summary>
        [Browsable(false)]
        public MatchMove PrevMove
        {
            get
            {
                return _match.GetMove(_player ? _currentMove - 1 : _currentMove, !_player);
            }
        }
        /// <summary>
        /// Current board position in the chess player. It is the position of the board after the last move.
        /// </summary>
        [Browsable(false)]
        public string CurrentBoard
        {
            get
            {
                return board.BoardPosition;
            }
        }
        /// <summary>
        /// Allow user to set the position on the board. If true, the user can set the position on the board.
        /// </summary>
        public bool AllowSetPosition
        {
            get
            {
                return board.AllowSetPosition;
            }
            set
            {
                board.AllowSetPosition = value;
            }
        }
        /// <summary>
        /// Allow add assistants comments to moves. If true, comments can be added to moves.
        /// </summary>
        [Browsable(false)]
        public bool AddComments
        {
            get
            {
                return board.AddComments;
            }
            set
            {
                board.AddComments = value;
            }
        }
        /// <summary>
        /// Match move display control. It is used to display the moves in a more visual way.
        /// </summary>
        public MatchMoveDisplay Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                if ((_match != null) && (_display != null))
                {
                    _display.SetMatch(_match);
                }
            }
        }
        /// <summary>
        /// Position Viewr control. This control is usually the parent of this board. It allows users to select matches bay positions, and opens this board at user request.
        /// </summary>
        public PositionViewer PosViewer { get; set; }
        /// <summary>
        /// Show or hide controls to add new comments. If true, the controls are shown.
        /// </summary>
        public bool NewComment
        {
            get
            {
                return bNew.Visible;
            }
            set
            {
                bNew.Visible = value;
                bOK.Visible = value;
                bCancel.Visible = value;
                ChessPlayer_SizeChanged(null, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Show or hide the keywords list. If true, the keywords list is shown.
        /// </summary>
        public bool Keywords
        {
            get
            {
                return lvKeys.Visible;
            }
            set
            {
                lvKeys.Visible = value;
                if (value && (_match != null))
                {
                    SetMatchInfo();
                }
            }
        }
        /// <summary>
        /// Enqueue a move to be made. If the timer is not enabled, it makes the move immediately and enables the timer. If the timer is enabled, it adds the move to the queue.
        /// </summary>
        /// <param name="comment">
        /// Comment to show on the move being made.
        /// </param>
        /// <returns>
        /// List with three moves: Previous move, move made or to be made, and next move. It can be null if there are no more moves.
        /// </returns>
        public List<MatchMove> EnqueueMove(string comment)
        {
            try
            {
                tmQueue.Enabled = false;
                List<MatchMove> moves = new List<MatchMove>();
                _moveQueue.Enqueue(comment);
                int N = _moveQueue.Count;
                MatchMove moved = _match.GetMove(_currentMove + (N / 2) + ((!_player && (N % 2 == 1)) ? 1 : 0),
                    _player ^ (N % 2 == 1));
                MatchMove prev = null;
                MatchMove next = null;
                if (moved == null)
                {
                    prev = _match.Repository.CreateObject(typeof(MatchMove)) as MatchMove;
                    next = prev;
                    moved = prev;
                }
                else
                {
                    prev = _match.GetMove(moved.MoveNumber - (moved.Player == 0 ? 1 : 0), moved.Player == 1)
                        ?? _match.Repository.CreateObject(typeof(MatchMove)) as MatchMove;
                    next = _match.GetMove(moved.MoveNumber + (moved.Player == 1 ? 1 : 0), moved.Player == 1)
                        ?? _match.Repository.CreateObject(typeof(MatchMove)) as MatchMove;
                }
                moves.Add(prev);
                moves.Add(moved);
                moves.Add(next);
                return moves;
            }
            finally
            {
                tmQueue.Enabled = true;
            }
        }
        /// <summary>
        /// Reset move queue and disable the timer.
        /// </summary>
        public void ResetQueue()
        {
            tmQueue.Enabled = false;
            _moveQueue.Clear();
        }
        /// <summary>
        /// Move forward to the next move in the match. If there are no more moves, it sets the board to the end position and displays the result text.
        /// </summary>
        /// <param name="comment">
        /// Show a comment on the move being made.
        /// </param>
        /// <returns>
        /// Next move made. It can be null if there are no more moves.
        /// </returns>
        public MatchMove MoveForward(string comment = null)
        {
            MatchMove imove = _match.GetMove(_currentMove, _player);
            if (imove != null)
            {
                if (AddComments && !string.IsNullOrEmpty(comment))
                {
                    MoveComment mc = _match.Repository.CreateObject(typeof(MoveComment)) as MoveComment;
                    mc.Comment = comment;
                    imove.AddChild(mc, ConnectionIndex);
                }
                if (_display != null)
                {
                    _display.SetPosition(_currentMove, _player);
                }
                board.FromTo = new Point(imove.From, imove.To);
                board.PlayerMove(_moveText + imove.ANText, !_player, imove.FinalPosition.Board.Board, imove.Events);
                if (!_player)
                {
                    _currentMove++;
                    _moveText = _currentMove.ToString() + ".";
                }
                else
                {
                    _moveText += imove.ANText + " ";
                }
                _player = !_player;
                if (PosViewer != null)
                {
                    PosViewer.Side = board.SideView;
                    PosViewer.BoardPosition = board.BoardPosition;
                    PosViewer.Color = _player;
                }
            }
            else
            {
                board.PlayerMove(_match.ResultText, _player, "", 0);
                if (_display != null)
                {
                    _display.SetPosition(0, _player);
                }
                if (PosViewer != null)
                {
                    PosViewer.Side = board.SideView;
                    PosViewer.BoardPosition = null;
                    PosViewer.Color = _player;
                }
            }
            SetComments(imove);
            if (lvKeys.Visible && (imove != null))
            {
                SetPosInfo(imove.FinalPosition);
            }
            if (!string.IsNullOrEmpty(comment) && (imove != null))
            {
                board.ShowToolTip(imove.To, comment, imove.ANText);
            }
            if (_moveQueue.Count > 0)
            {
                tmQueue.Enabled = true;
            }
            return imove;
        }
        public MatchMove MoveBack()
        {
            ResetQueue();
            if (!_player)
            {
                if (_currentMove == 1)
                {
                    CurrentMatch = _match;
                    return _match.GetMove(1, true);
                }
                else
                {
                    MatchMove imove = _match.GetMove(_currentMove - 1, true);
                    _moveText = _currentMove.ToString() + "." + imove.ANText + " ";
                    imove = _match.GetMove(_currentMove - 1, false);
                    _moveText += imove.ANText;
                    _player = true;
                    board.FromTo = new Point(imove.From, imove.To);
                    board.PlayerMove(_moveText, _player, imove.FinalPosition.Board.Board, imove.Events);
                    SetComments(imove);
                    if (lvKeys.Visible)
                    {
                        SetPosInfo(imove.FinalPosition);
                    }
                    if (_display != null)
                    {
                        _display.SetPosition(_currentMove - 1, false);
                    }
                    if (PosViewer != null)
                    {
                        PosViewer.Side = board.SideView;
                        PosViewer.BoardPosition = board.BoardPosition;
                        PosViewer.Color = _player;
                    }
                    return imove;
                }
            }
            else
            {
                _currentMove--;
                MatchMove imove = _match.GetMove(_currentMove, true);
                if (imove == null)
                {
                    CurrentMatch = _match;
                    return null;
                }
                else
                {
                    _moveText = _currentMove.ToString() + "." + imove.ANText + " ";
                    _player = false;
                    board.FromTo = new Point(imove.From, imove.To);
                    board.PlayerMove(_moveText, _player, imove.FinalPosition.Board.Board, imove.Events);
                    SetComments(imove);
                    if (lvKeys.Visible)
                    {
                        SetPosInfo(imove.FinalPosition);
                    }
                    if (_display != null)
                    {
                        _display.SetPosition(_currentMove, true);
                    }
                    if (PosViewer != null)
                    {
                        PosViewer.Side = board.SideView;
                        PosViewer.BoardPosition = board.BoardPosition;
                        PosViewer.Color = _player;
                    }
                    return imove;
                }
            }
        }
        /// <summary>
        /// Set the position on the board to a specific move. The move is identified by its position in the match. If the position is not valid, it does nothing.
        /// </summary>
        /// <param name="pos">
        /// Move position in the match. It is 1-based, so the first move is 1.
        /// </param>
        /// <param name="color">
        /// Player color to set the position for. If true, it is white's turn, if false, it is the black player.
        /// </param>
        public void SetPosition(int pos, bool color)
        {
            MatchMove imove = _match.GetMove(pos, color);
            if (imove != null)
            {
                _currentMove = pos;
                _player = color;
                string text = "";
                if (!_player)
                {
                    MatchMove wmove = _match.GetMove(pos, true);
                    if (wmove == null)
                    {
                        text = _currentMove.ToString() + "... " + imove.ANText;
                    }
                    else
                    {
                        text = _currentMove.ToString() + "." + wmove.ANText + " " + imove.ANText;
                    }
                    _currentMove++;
                    _player = true;
                    _moveText = _currentMove.ToString() + ".";
                }
                else
                {
                    text = _currentMove.ToString() + "." + imove.ANText;
                    _player = false;
                    _moveText = text + " ";
                }
                board.FromTo = new Point(imove.From, imove.To);
                board.PlayerMove(text, _player, imove.FinalPosition.Board.Board, imove.Events);
                if (PosViewer != null)
                {
                    PosViewer.Side = board.SideView;
                    PosViewer.BoardPosition = board.BoardPosition;
                    PosViewer.Color = _player;
                }
            }
            else if ((pos == 1) && color)
            {
                _currentMove = 1;
                _player = false;
                _moveText = "1... ";
                board.FromTo = new Point(-1, -1);
                board.PlayerMove(_moveText, _player, _match.InitialPosition.Board.Board, 0);
                if (PosViewer != null)
                {
                    PosViewer.Side = board.SideView;
                    PosViewer.BoardPosition = board.BoardPosition;
                    PosViewer.Color = _player;
                }
            }
            SetComments(imove);
            if (lvKeys.Visible && (imove != null))
            {
                SetPosInfo(imove.FinalPosition);
            }
        }
        /// <summary>
        /// Mark the end of the game. It sets the board to the end position and displays the result text.
        /// </summary>
        public void SetEnd()
        {
            board.FromTo = new Point(-1, -1);
            board.PlayerMove(_match.ResultText, _player, "", 0);
            if (_display != null)
            {
                _display.SetPosition(0, _player);
            }
            if (PosViewer != null)
            {
                PosViewer.Side = board.SideView;
                PosViewer.BoardPosition = null;
                PosViewer.Color = _player;
            }
        }
        /// <summary>
        /// Set the comments for the current move. It retrieves the comments from the match and displays them in the comments text box.
        /// </summary>
        public void ResetComments()
        {
            SetComments(_match.GetMove(_currentMove, _player));
        }
        /// <summary>
        /// Manage controls to show / edit comments for a specific move.
        /// </summary>
        /// <param name="move"></param>
        private void SetComments(MatchMove move)
        {
            txtComments.Clear();
            txtComments.ReadOnly = true;
            EnableButtons(true);
            if (move != null)
            {
                List<string> coms = new List<string>();
                foreach (MoveComment com in move.Comments)
                {
                    coms.Add(com.ToString());
                }
                txtComments.Lines = coms.ToArray();
            }
            MoveChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Removes all keywords from the specified group in the list view.
        /// </summary>
        /// <param name="group">
        /// The name of a group of keywords.
        /// </param>
        private void EmptyGroup(string group)
        {
            while (lvKeys.Groups[group].Items.Count > 0)
            {
                lvKeys.Items.Remove(lvKeys.Groups[group].Items[0]);
            }
        }
        /// <summary>
        /// Update statistics for a specific position in the match. Statistics are shown in the keywords List View.
        /// </summary>
        /// <param name="pos"></param>
        private void SetPosInfo(MatchPosition pos)
        {
            EmptyGroup("lgPositionsSts");
            foreach (PositionStatistic s in pos.Board.Statistics)
            {
                ListViewItem item = new ListViewItem(new string[] { PositionStatistic.Translate(s.Name), s.Value.ToString() }, lvKeys.Groups["lgPositionsSts"]);
                lvKeys.Items.Add(item);
            }
        }
        /// <summary>
        /// Update the keywords and statistics for the match. It shows the keywords and statistics in the keywords List View.
        /// </summary>
        private void SetMatchInfo()
        {
            EmptyGroup("lgKeywords");
            foreach (MatchKeyword k in _match.Keywords)
            {
                ListViewItem item = new ListViewItem(new string[] { k.Name, k.Value }, lvKeys.Groups["lgKeywords"]);
                lvKeys.Items.Add(item);
            }
            EmptyGroup("lgStatistics");
            foreach (MatchStatistic s in _match.Statistics)
            {
                ListViewItem item = new ListViewItem(new string[] { MatchStatistic.Translate(s.Name), s.Value.ToString() }, lvKeys.Groups["lgStatistics"]);
                lvKeys.Items.Add(item);
            }
        }
        /// <summary>
        /// Enable or disable the buttons to add new comments and save or cancel them.
        /// </summary>
        /// <param name="enable">
        /// Button state.
        /// </param>
        private void EnableButtons(bool enable)
        {
            bNew.Enabled = enable;
            bOK.Enabled = !enable;
            bCancel.Enabled = !enable;
        }
        /// <summary>
        /// Adjust the size of the controls when the chess player control is resized. It centers the board and adjusts the comments and keywords list view.
        /// </summary>
        private void ChessPlayer_SizeChanged(object sender, EventArgs e)
        {
            board.Left = (ClientSize.Width - board.Width) / 2;
            if (lvKeys.Visible)
            {
                txtComments.Width = 3 * (ClientSize.Width - 48) / 5;
                lvKeys.Width = 2 * (ClientSize.Width - 48) / 5;
                txtComments.Left = 16;
                lvKeys.Left = ClientSize.Width - (lvKeys.Width + 16);
                lvKeys.Height = ClientSize.Height - lvKeys.Top - 16;
            }
            else
            {
                txtComments.Width = ClientSize.Width - 32;
                txtComments.Left = 16;
            }
            if (bNew.Visible)
            {
                txtComments.Height = ClientSize.Height - txtComments.Top - 16 - (bNew.Height + 16);
                bNew.Top = txtComments.Bottom + 16;
                bCancel.Top = txtComments.Bottom + 16;
                bOK.Top = txtComments.Bottom + 16;
                bNew.Left = txtComments.Left;
                bCancel.Left = txtComments.Right - bCancel.Width;
                bOK.Left = bCancel.Left - bOK.Width - 10;

            }
            else
            {
                txtComments.Height = ClientSize.Height - txtComments.Top - 16;
            }
        }

        private void board_ResetMatch(object sender, EventArgs e)
        {
            ResetQueue();
            CurrentMatch = _match;
        }

        private void board_MoveForward(object sender, EventArgs e)
        {
            ResetQueue();
            MoveForward();
        }

        private void board_MoveBack(object sender, EventArgs e)
        {
            ResetQueue();
            MoveBack();
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            try
            {
                txtComments.ReadOnly = false;
                txtComments.Clear();
                EnableButtons(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SetComments(_match.GetMove(_currentMove, _player));
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetComments(_match.GetMove(_currentMove, _player));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bOK_Click(object sender, EventArgs e)
        {
            try
            {
                MatchMove imove = _match.GetMove(_currentMove, _player);
                try
                {
                    if (!string.IsNullOrEmpty(txtComments.Text))
                    {
                        MoveComment com = imove.Repository.CreateObject(typeof(MoveComment)) as MoveComment;
                        com.Comment = txtComments.Text;
                        imove.Repository.Connector.OpenConnection(ConnectionIndex);
                        imove.Repository.Connector.BeginTransaction(ConnectionIndex);
                        await imove.AddChildAsync(com, ConnectionIndex);
                        await com.Insert(ConnectionIndex);
                        imove.Repository.Connector.Commit(ConnectionIndex);
                    }
                    SetComments(_match.GetMove(_currentMove, _player));
                }
                catch (Exception ex)
                {
                    imove.Repository.Connector.Rollback(ConnectionIndex);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    imove.Repository.Connector.CloseConnection(ConnectionIndex);
                }
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }

        private void board_BoardChanged(object sender, EventArgs e)
        {
            if (_match != null)
            {
                _match.InitialPosition.Board.Board = board.BoardPosition;
            }
        }

        private void board_SideViewChanged(object sender, EventArgs e)
        {
            if (PosViewer != null)
            {
                PosViewer.Side = board.SideView;
                PosViewer.BoardPosition = board.BoardPosition;
                PosViewer.Color = _player;
            }
        }

        private void ChessPlayer_Load(object sender, EventArgs e)
        {
            if (AllowSetPosition && (_match != null))
            {
                AllowSetPosition = _match.IdMatch == 0;
            }
        }

        private void tmQueue_Tick(object sender, EventArgs e)
        {
            try
            {
                tmQueue.Enabled = false;
                if (_moveQueue.Count > 0)
                {
                    MoveForward(_moveQueue.Dequeue());
                }
            }
            catch { }
        }
    }
}
