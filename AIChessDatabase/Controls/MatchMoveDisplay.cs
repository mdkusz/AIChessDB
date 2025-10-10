using AIChessDatabase.Chess;
using AIChessDatabase.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Control to display the moves of a chess match.
    /// </summary>
    public partial class MatchMoveDisplay : UserControl
    {
        private const string fontName = "Arial";
        private List<MoveData> _moves = null;
        private List<int> _resultMoves = new List<int>();
        private int _hmoves = 0;
        private int _vmoves = 0;
        private int _szelement = 0;
        private int _maxwidth = 0;
        private int _szsep = 0;
        private List<ComparableRectangle> _movePositions = null;
        private Bitmap _bmpMatch = null;
        private Match _match = null;
        private int _movenumber = 0;
        private bool _color = true;
        private ChessPlayer _player = null;
        private ChessBoard _board = null;
        // Last move color and number when adding moves
        private bool _moveColor = true;
        private int _nMove = 0;

        public MatchMoveDisplay()
        {
            InitializeComponent();
        }
        /// <summary>
        /// User added move using the toolbar textbox.
        /// </summary>
        public event EventHandler NewMoveAdded = null;
        /// <summary>
        /// Event triggered when the current move changes.
        /// </summary>
        public event EventHandler MoveChanged = null;
        /// <summary>
        /// Event triggered when the last match move is reached.
        /// </summary>
        public event EventHandler MatchFinished = null;
        /// <summary>
        /// Board position control to use for the moves.
        /// </summary>
        public ChessPlayer Player
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
            }
        }
        /// <summary>
        /// Allow user to add moves to the match.
        /// </summary>
        public bool AllowAddMoves
        {
            get
            {
                return sepAddMove.Visible;
            }
            set
            {
                sepAddMove.Visible = value;
                txtAddMove.Visible = value;
                bAddMove.Visible = value;
                bDelMove.Visible = value;
            }
        }
        /// <summary>
        /// Populates _resultMoves list with the unique move order numbers from the provided DataTable
        /// (these are the moves that match the current filter and will be rendered in violet), and refreshes
        /// the match display if a match is already set.
        /// </summary>
        /// <param name="moves">
        /// DataTable containing the moves to be processed.
        /// </param>
        public void SetMoves(DataTable moves)
        {
            _resultMoves = new List<int>();
            foreach (DataRow row in moves.Rows)
            {
                for (int c = 0; c < row.ItemArray.Length; c++)
                {
                    int n = Convert.ToInt32(row[c]);
                    if (!_resultMoves.Contains(n))
                    {
                        _resultMoves.Add(n);
                    }
                }
            }
            if (_match != null)
            {
                SetMatch(_match);
            }
        }
        /// <summary>
        /// Renders the match moves based on the provided Match object.
        /// </summary>
        /// <param name="match">
        /// Match object containing the moves to be displayed.
        /// </param>
        public void SetMatch(Match match)
        {
            if (match == null)
            {
                return;
            }
            _match = match;
            if (AllowAddMoves)
            {
                AllowAddMoves = _match.IdMatch == 0;
                bDelMove.Enabled = AllowAddMoves && _match.MoveCount > 0;
            }
            _moves = new List<MoveData>();
            int movenum = 1;
            _movenumber = 0;
            bool color = true;
            MatchMove imove = match.GetMove(1, color);
            if ((imove == null) && (_match.MoveCount > 0))
            {
                _moves.Add(new MoveData()
                {
                    MoveText = "...",
                    Color = true,
                    Events = 0,
                    MoveNumber = 1,
                    Result = false
                });
                color = !color;
                imove = match.GetMove(1, color);
            }
            if (_match.MoveCount > 0)
            {
                MatchMove lmove = match.GetMove(match.MoveCount);
                _nMove = lmove.MoveNumber;
                _moveColor = lmove.Player == 0;
            }
            _maxwidth = 0;
            Font fbold = new Font(fontName, 20f, FontStyle.Bold, GraphicsUnit.Pixel);
            Bitmap bmp = new Bitmap(8, 8);
            Graphics gr = Graphics.FromImage(bmp);
            _szsep = (int)Math.Ceiling(gr.MeasureString(" ", fbold).Width);
            try
            {
                while (imove != null)
                {

                    string mtext = imove.ANText;
                    SizeF sz = gr.MeasureString(mtext, fbold);
                    if (_maxwidth < (int)Math.Ceiling(sz.Width))
                    {
                        _maxwidth = (int)Math.Ceiling(sz.Width);
                    }
                    _moves.Add(new MoveData()
                    {
                        MoveText = mtext,
                        Color = color,
                        Events = imove.Events,
                        MoveNumber = movenum,
                        Result = _resultMoves.Contains(imove.Order)
                    });
                    color = !color;
                    if (color)
                    {
                        movenum++;
                    }
                    imove = match.GetMove(movenum, color);
                }
            }
            finally
            {
                gr.Dispose();
                bmp.Dispose();
                fbold.Dispose();
            }
            _szelement = 40 + _szsep + (2 * _maxwidth);
            if (bTopDown.Checked)
            {
                _vmoves = Math.Max(1, ((Height - 20) / 20) - 1);
                _hmoves = (int)Math.Ceiling((double)_moves.Count / (2 * _vmoves));
            }
            else
            {
                _hmoves = Math.Max(1, (Width - 20) / _szelement);
                _vmoves = (int)Math.Ceiling((double)_moves.Count / (2 * _hmoves));
            }
            _bmpMatch = new Bitmap(20 + _hmoves * _szelement, 10 + 20 * _vmoves);
            ReasignIndexes();
            DrawMoves();
            pbMatch.Left = Math.Max(0, (Width - pbMatch.Width) / 2);
        }
        /// <summary>
        /// Go to a specific position in the match moves.
        /// </summary>
        /// <param name="pos">
        /// Position order
        /// </param>
        /// <param name="color">
        /// Player color (true for white, false for black).
        /// </param>
        public void SetPosition(int pos, bool color)
        {
            _movenumber = pos;
            _color = color;
            DrawMoves();
            MoveChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Add a new move to the match.
        /// </summary>
        /// <param name="movetext">
        /// AN move text to be added to the match.
        /// </param>
        /// <param name="comment">
        /// Optional move comment
        /// </param>
        /// <exception cref="Exception">
        /// Bad move format or if the match is not editable.
        /// </exception>
        public void AddNewMove(string movetext, string comment)
        {
            if (_match != null)
            {
                if (_match.MoveCount == 0)
                {
                    _board = new ChessBoard(_player.CurrentBoard);
                    if (movetext == "...")
                    {
                        if (_moveColor)
                        {
                            IgnoreFirstWhiteMove();
                            _moveColor = false;
                            txtAddMove.Clear();
                            txtAddMove.Focus();
                            if (_player != null)
                            {
                                _player.AllowSetPosition = false;
                            }
                            return;
                        }
                        else
                        {
                            throw new Exception(ERR_BADMOVE);
                        }
                    }
                    _nMove = 1;
                }
                if (movetext == _match.ResultText)
                {
                    MatchFinished?.Invoke(this, EventArgs.Empty);
                    if (_player != null)
                    {
                        _player.SetEnd();
                        _player.AllowSetPosition = false;
                    }
                    AllowAddMoves = false;
                }
                else
                {
                    MatchMove mm = _match.Repository.CreateObject(typeof(MatchMove)) as MatchMove;
                    if (_match.MoveCount == 0)
                    {
                        mm.InitialPosition = _match.InitialPosition;
                        mm.MoveNumber = _nMove;
                    }
                    else
                    {
                        MatchMove mprev = _match.GetMove(_match.MoveCount);
                        mm.InitialPosition = mprev.FinalPosition;
                        mm.MoveNumber = mprev.MoveNumber;
                        if (mprev.Player == 1)
                        {
                            mm.MoveNumber++;
                        }
                    }
                    mm.Player = (byte)(_moveColor ? 0 : 1);
                    mm.ANText = movetext;
                    _board.Move(_match.Description, _nMove, movetext, _moveColor);
                    mm.Events = _board.Events;
                    mm.From = _board.From;
                    mm.To = _board.To;
                    mm.FinalPosition = _match.Repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                    mm.Order = _match.MoveCount + 1;
                    mm.FinalPosition.Order = mm.Order;
                    mm.FinalPosition.Board = _match.Repository.CreateObject(typeof(Position)) as Position;
                    mm.FinalPosition.Board.Board = _board.Board;
                    mm.FinalPosition.Events = mm.Events;
                    if (!string.IsNullOrEmpty(comment))
                    {
                        _player.board.ShowToolTip(mm.To, comment, mm.ANText);
                        if (_player.AddComments)
                        {
                            MoveComment mc = _match.Repository.CreateObject(typeof(MoveComment)) as MoveComment;
                            mc.Comment = comment;
                            mm.AddChild(mc, _player.ConnectionIndex);
                        }
                    }
                    _match.AddChild(mm, _player.ConnectionIndex);
                    SetMatch(_match);
                    if (_player != null)
                    {
                        _player.SetPosition(_nMove, _moveColor);
                        _player.AllowSetPosition = false;
                    }
                    SetPosition(_nMove, _moveColor);
                    _moveColor = !_moveColor;
                    if (_moveColor)
                    {
                        _nMove++;
                    }
                    txtAddMove.Clear();
                    txtAddMove.Focus();
                    bDelMove.Enabled = true;
                }
            }
        }
        /// <summary>
        /// Remove the last move from the match.
        /// </summary>
        public void DeleteLastMove()
        {
            if (_match != null)
            {
                if (_match.MoveCount == 0)
                {
                    _moves.Clear();
                    _moveColor = true;
                    _nMove = 0;
                    bDelMove.Enabled = false;
                    if (_player != null)
                    {
                        _player.CurrentMatch = _match;
                        _player.AllowSetPosition = _match.IdMatch == 0;
                    }
                    else
                    {
                        SetMatch(_match);
                    }
                }
                else
                {
                    MatchMove mm = _match.GetMove(_match.MoveCount);
                    _match.RemoveChild(mm, _player.ConnectionIndex);
                    if (_match.MoveCount == 0)
                    {
                        _moves.Clear();
                        _moveColor = true;
                        _nMove = 0;
                        bDelMove.Enabled = false;
                        if (_player != null)
                        {
                            _player.CurrentMatch = _match;
                            _player.AllowSetPosition = _match.IdMatch == 0;
                        }
                        else
                        {
                            SetMatch(_match);
                        }
                    }
                    else
                    {
                        _moveColor = mm.Player == 0;
                        _nMove = mm.MoveNumber;
                        if (!_moveColor)
                        {
                            _nMove--;
                        }
                        bool cmove = _moveColor;
                        int nmove = _nMove;
                        if (_player != null)
                        {
                            _player.SetPosition(nmove, cmove);
                        }
                        SetMatch(_match);
                        SetPosition(nmove, cmove);
                        _moveColor = cmove;
                        _nMove = nmove;
                    }
                }
                _board = new ChessBoard(_player.CurrentBoard);
            }
        }
        /// <summary>
        /// Ignore the first white move in the match.
        /// </summary>
        private void IgnoreFirstWhiteMove()
        {
            _maxwidth = 0;
            Font fbold = new Font(fontName, 20f, FontStyle.Bold, GraphicsUnit.Pixel);
            Bitmap bmp = new Bitmap(8, 8);
            Graphics gr = Graphics.FromImage(bmp);
            _szsep = (int)Math.Ceiling(gr.MeasureString(" ", fbold).Width);
            try
            {
                string mtext = "...";
                SizeF sz = gr.MeasureString(mtext, fbold);
                if (_maxwidth < (int)Math.Ceiling(sz.Width))
                {
                    _maxwidth = (int)Math.Ceiling(sz.Width);
                }
                _moves.Add(new MoveData()
                {
                    MoveText = mtext,
                    Color = _moveColor,
                    Events = 0,
                    MoveNumber = 1,
                    Result = false
                });
            }
            finally
            {
                gr.Dispose();
                bmp.Dispose();
                fbold.Dispose();
            }
            _szelement = 40 + _szsep + (2 * _maxwidth);
            if (bTopDown.Checked)
            {
                _vmoves = Math.Max(1, ((Height - 20) / 20) - 1);
                _hmoves = (int)Math.Ceiling((double)_moves.Count / (2 * _vmoves));
            }
            else
            {
                _hmoves = Math.Max(1, (Width - 20) / _szelement);
                _vmoves = (int)Math.Ceiling((double)_moves.Count / (2 * _hmoves));
            }
            _bmpMatch = new Bitmap(20 + _hmoves * _szelement, 10 + 20 * _vmoves);
            ReasignIndexes();
            DrawMoves();
            pbMatch.Left = Math.Max(0, (Width - pbMatch.Width) / 2);
        }
        /// <summary>
        /// Rebuild the list of move positions based on the current match and moves.
        /// </summary>
        private void ReasignIndexes()
        {
            if (_match == null)
            {
                return;
            }
            bool color;
            int dataindex = 0;
            _movePositions = new List<ComparableRectangle>();
            if (bTopDown.Checked)
            {
                for (int x = 0; x < _hmoves; x++)
                {
                    for (int y = 0; y < _vmoves; y++)
                    {
                        if (dataindex < _moves.Count)
                        {
                            color = _moves[dataindex].Color;
                            _movePositions.Add(new ComparableRectangle(color ? new Rectangle(50 + (_szelement * x), 10 + 20 * y, _maxwidth, 20) :
                                new Rectangle(50 + _szsep + _maxwidth + (_szelement * x), 10 + 20 * y, _maxwidth, 20))
                            {
                                MoveDataIndex = dataindex
                            });
                            dataindex++;
                            if (dataindex < _moves.Count)
                            {
                                color = _moves[dataindex].Color;
                                _movePositions.Add(new ComparableRectangle(color ? new Rectangle(50 + (_szelement * x), 10 + 20 * y, _maxwidth, 20) :
                                    new Rectangle(50 + _szsep + _maxwidth + (_szelement * x), 10 + 20 * y, _maxwidth, 20))
                                {
                                    MoveDataIndex = dataindex
                                });
                                dataindex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int y = 0; y < _vmoves; y++)
                {
                    for (int x = 0; x < _hmoves; x++)
                    {
                        if (dataindex < _moves.Count)
                        {
                            color = _moves[dataindex].Color;
                            _movePositions.Add(new ComparableRectangle(color ? new Rectangle(50 + (_szelement * x), 10 + 20 * y, _maxwidth, 20) :
                                new Rectangle(50 + _szsep + _maxwidth + (_szelement * x), 10 + 20 * y, _maxwidth, 20))
                            {
                                MoveDataIndex = dataindex
                            });
                            dataindex++;
                            if (dataindex < _moves.Count)
                            {
                                color = _moves[dataindex].Color;
                                _movePositions.Add(new ComparableRectangle(color ? new Rectangle(50 + (_szelement * x), 10 + 20 * y, _maxwidth, 20) :
                                    new Rectangle(50 + _szsep + _maxwidth + (_szelement * x), 10 + 20 * y, _maxwidth, 20))
                                {
                                    MoveDataIndex = dataindex
                                });
                                dataindex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws the moves on the bitmap and updates the PictureBox control to display them.
        /// </summary>
        private void DrawMoves()
        {
            if (_bmpMatch != null)
            {
                Font fbold = new Font(fontName, 20f, FontStyle.Bold, GraphicsUnit.Pixel);
                Font fnormal = new Font(fontName, 20f, GraphicsUnit.Pixel);
                Graphics gr = Graphics.FromImage(_bmpMatch);
                gr.FillRectangle(Brushes.White, new Rectangle(0, 0, _bmpMatch.Width, _bmpMatch.Height));
                try
                {
                    foreach (ComparableRectangle rect in _movePositions)
                    {
                        MoveData md = _moves[rect.MoveDataIndex];
                        if (md.Color)
                        {
                            SizeF sznum = gr.MeasureString(md.MoveNumber.ToString() + ". ", fnormal);
                            gr.DrawString(md.MoveNumber.ToString() + ". ", fnormal, Brushes.Black, rect.Rect.X - sznum.Width, rect.Rect.Y);
                        }
                        Brush br = Brushes.DarkOliveGreen;
                        if (md.Result)
                        {
                            br = Brushes.BlueViolet;
                        }
                        else
                        {
                            if ((md.Events & (uint)MatchEvent.Event.Capture) == (uint)MatchEvent.Event.Capture)
                            {
                                br = Brushes.Gray;
                            }
                            if ((md.Events & (uint)MatchEvent.Event.AllChecks) != 0)
                            {
                                br = Brushes.DarkRed;
                            }
                            if ((md.Events & (uint)MatchEvent.Event.Checkmate) == (uint)MatchEvent.Event.Checkmate)
                            {
                                br = Brushes.Red;
                            }
                        }
                        gr.DrawString(md.MoveText, ((_movenumber == md.MoveNumber) && (_color == md.Color)) ? fbold : fnormal, br, rect.Rect.X, rect.Rect.Y);
                    }
                    pbMatch.Image = _bmpMatch.Clone() as Bitmap;
                }
                finally
                {
                    fbold.Dispose();
                    fnormal.Dispose();
                    gr.Dispose();
                }
            }
        }

        private void MatchMoveDisplay_SizeChanged(object sender, EventArgs e)
        {
            SetMatch(_match);
        }

        private void bTopDown_Click(object sender, EventArgs e)
        {
            if (!bTopDown.Checked)
            {
                bTopDown.Checked = true;
                bLeftRight.Checked = false;
                _vmoves = Math.Max(1, ((Height - 20) / 20) - 1);
                _hmoves = (int)Math.Ceiling((double)_moves.Count / (2 * _vmoves));
                _bmpMatch = new Bitmap(20 + _hmoves * _szelement, 10 + 20 * _vmoves);
                ReasignIndexes();
                DrawMoves();
                pbMatch.Left = Math.Max(0, (Width - pbMatch.Width) / 2);
            }
        }

        private void bLeftRight_Click(object sender, EventArgs e)
        {
            if (!bLeftRight.Checked)
            {
                bTopDown.Checked = false;
                bLeftRight.Checked = true;
                _hmoves = Math.Max(1, (Width - 20) / _szelement);
                _vmoves = (int)Math.Ceiling((double)_moves.Count / (2 * _hmoves));
                _bmpMatch = new Bitmap(20 + _hmoves * _szelement, 10 + 20 * _vmoves);
                ReasignIndexes();
                DrawMoves();
                pbMatch.Left = Math.Max(0, (Width - pbMatch.Width) / 2);
            }
        }

        private void pbMatch_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                int pmov = _movePositions.IndexOf(new ComparableRectangle(new Rectangle(e.Location, new Size(1, 1))));
                if (pmov >= 0)
                {
                    SetPosition(_moves[_movePositions[pmov].MoveDataIndex].MoveNumber, _moves[_movePositions[pmov].MoveDataIndex].Color);
                    if (_player != null)
                    {
                        _player.SetPosition(_moves[_movePositions[pmov].MoveDataIndex].MoveNumber, _moves[_movePositions[pmov].MoveDataIndex].Color);
                    }
                }
            }
        }

        private void txtAddMove_TextChanged(object sender, EventArgs e)
        {
            bAddMove.Enabled = !string.IsNullOrEmpty(txtAddMove.Text);
        }

        private async void bAddMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_match != null)
                {
                    if (_match.MoveCount == 0)
                    {
                        _board = new ChessBoard(_player.CurrentBoard);
                        if (txtAddMove.Text == "...")
                        {
                            if (_moveColor)
                            {
                                IgnoreFirstWhiteMove();
                                _moveColor = false;
                                txtAddMove.Clear();
                                txtAddMove.Focus();
                                if (_player != null)
                                {
                                    _player.AllowSetPosition = false;
                                }
                                NewMoveAdded?.Invoke(this, EventArgs.Empty);
                                return;
                            }
                            else
                            {
                                throw new Exception(ERR_BADMOVE);
                            }
                        }
                        _nMove = 1;
                    }
                    if (txtAddMove.Text == _match.ResultText)
                    {
                        if (MessageBox.Show(MSG_TERMINATEMATCH, CAP_QUESTION, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            MatchFinished?.Invoke(this, EventArgs.Empty);
                            if (_player != null)
                            {
                                _player.SetEnd();
                                _player.AllowSetPosition = false;
                            }
                            AllowAddMoves = false;
                        }
                        else
                        {
                            txtAddMove.Clear();
                            txtAddMove.Focus();
                        }
                    }
                    else
                    {
                        MatchMove mm = _match.Repository.CreateObject(typeof(MatchMove)) as MatchMove;
                        if (_match.MoveCount == 0)
                        {
                            mm.InitialPosition = _match.InitialPosition;
                            mm.MoveNumber = _nMove;
                        }
                        else
                        {
                            MatchMove mprev = _match.GetMove(_match.MoveCount);
                            mm.InitialPosition = mprev.FinalPosition;
                            mm.MoveNumber = mprev.MoveNumber;
                            if (mprev.Player == 1)
                            {
                                mm.MoveNumber++;
                            }
                        }
                        mm.Player = (byte)(_moveColor ? 0 : 1);
                        mm.ANText = txtAddMove.Text;
                        _board.Move(_match.Description, _nMove, txtAddMove.Text, _moveColor);
                        mm.Events = _board.Events;
                        mm.From = _board.From;
                        mm.To = _board.To;
                        mm.FinalPosition = _match.Repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                        mm.Order = _match.MoveCount + 1;
                        mm.FinalPosition.Order = mm.Order;
                        mm.FinalPosition.Board = _match.Repository.CreateObject(typeof(Position)) as Position;
                        mm.FinalPosition.Board.Board = _board.Board;
                        mm.FinalPosition.Events = mm.Events;
                        await _match.AddChildAsync(mm, _player.ConnectionIndex);
                        SetMatch(_match);
                        if (_player != null)
                        {
                            _player.SetPosition(_nMove, _moveColor);
                            _player.AllowSetPosition = false;
                        }
                        SetPosition(_nMove, _moveColor);
                        _moveColor = !_moveColor;
                        if (_moveColor)
                        {
                            _nMove++;
                        }
                        txtAddMove.Clear();
                        txtAddMove.Focus();
                        bDelMove.Enabled = true;
                        if ((mm.Events & (uint)MatchEvent.Event.Checkmate) == (uint)MatchEvent.Event.Checkmate)
                        {
                            MatchFinished?.Invoke(this, EventArgs.Empty);
                            if (_player != null)
                            {
                                _player.SetEnd();
                                _player.AllowSetPosition = false;
                            }
                            AllowAddMoves = false;
                        }
                        else
                        {
                            NewMoveAdded?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtAddMove.SelectAll();
                txtAddMove.Focus();
            }
        }

        private void txtAddMove_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                bAddMove_Click(sender, e);
            }
        }

        private async void bDelMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_match != null)
                {
                    if (_match.MoveCount == 0)
                    {
                        _moves.Clear();
                        _moveColor = true;
                        _nMove = 0;
                        bDelMove.Enabled = false;
                        if (_player != null)
                        {
                            _player.CurrentMatch = _match;
                            _player.AllowSetPosition = _match.IdMatch == 0;
                        }
                        else
                        {
                            SetMatch(_match);
                        }
                    }
                    else
                    {
                        MatchMove mm = _match.GetMove(_match.MoveCount);
                        await _match.RemoveChildAsync(mm, _player.ConnectionIndex);
                        if (_match.MoveCount == 0)
                        {
                            _moves.Clear();
                            _moveColor = true;
                            _nMove = 0;
                            bDelMove.Enabled = false;
                            if (_player != null)
                            {
                                _player.CurrentMatch = _match;
                                _player.AllowSetPosition = _match.IdMatch == 0;
                            }
                            else
                            {
                                SetMatch(_match);
                            }
                        }
                        else
                        {
                            _moveColor = mm.Player == 0;
                            _nMove = mm.MoveNumber;
                            if (!_moveColor)
                            {
                                _nMove--;
                            }
                            bool cmove = _moveColor;
                            int nmove = _nMove;
                            if (_player != null)
                            {
                                _player.SetPosition(nmove, cmove);
                            }
                            SetMatch(_match);
                            SetPosition(nmove, cmove);
                            _moveColor = cmove;
                            _nMove = nmove;
                        }
                    }
                    _board = new ChessBoard(_player.CurrentBoard);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
