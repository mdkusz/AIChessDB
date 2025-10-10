using AIChessDatabase.Data;
using AIChessDatabase.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Ches board with a toolbar to control the match moves.
    /// </summary>
    public partial class Board : UserControl
    {
        private string _position = INITIAL_BOARD;
        private bool _color = true;
        private bool _side = true;
        private ToolTip _bubbleMessage = null;

        public Board()
        {
            InitializeComponent();
            FromTo = new Point(-1, -1);
        }
        /// <summary>
        /// Event fired when reseting the match.
        /// </summary>
        public event EventHandler ResetMatch = null;
        /// <summary>
        /// Event fired when the user clicks the "Back" button to move back in the match.
        /// </summary>
        public event EventHandler MoveBack = null;
        /// <summary>
        /// Event fired when the user clicks the "Next" button to move forward in the match.
        /// </summary>
        public event EventHandler MoveForward = null;
        /// <summary>
        /// Event fired when the board pieces disposition changes, for example when the user sets a new initial position.
        /// </summary>
        public event EventHandler BoardChanged = null;
        /// <summary>
        /// Event fired when the side view changes, for example when the user clicks the "Rotate View" button.
        /// </summary>
        public event EventHandler SideViewChanged = null;
        /// <summary>
        /// Current board position as string.
        /// </summary>
        /// <remarks>
        /// This is a 64 character string representing the board position.
        /// 0: are for empty squares:
        /// b or B are for black and white bishops respectively,
        /// r or R are for black and white rooks respectively,
        /// n or N are for black and white knights respectively,
        /// q or Q are for black and white queens respectively,
        /// k or K are for black and white kings respectively,
        /// p or P are for black and white pawns respectively.
        /// </remarks>
        public string BoardPosition
        {
            get
            {
                return _position;
            }
            set
            {
                DrawBoard(value);
                _position = value;
            }
        }
        /// <summary>
        /// Show or hide the "Add Comments" button.
        /// </summary>
        public bool AddComments
        {
            get
            {
                return bAddComments.Visible && bAddComments.Checked;
            }
            set
            {
                bAddComments.Visible = value;
                if (!value)
                {
                    bAddComments.Checked = value;
                }
            }
        }
        /// <summary>
        /// Squares from and to for the current move.
        /// </summary>
        /// <remarks>
        /// Squares arepresented by an integer from 1 to 64, where 1 is a1 and 64 is h8.
        /// X is the square from which the piece is moved, and Y is the square to which the piece is moved.
        /// </remarks>
        [Browsable(false)]
        public Point FromTo { get; set; }
        /// <summary>
        /// Move events for the current move.
        /// </summary>
        /// <remarks>
        /// This is a bitmask representing the events that occurred during the move.
        /// 1 ('EVENT_CHECK'): This event represents a check.
        /// 2 ('EVENT_CHECK_MATE'): This event represents a checkmate.
        /// 4 ('EVENT_MULTIPLE_CHECK'): This event represents a multiple check.
        /// 8 ('EVENT_DISCOVERED_CHECK'): This event represents a discovered check.
        /// 16 ('EVENT_DRAW_OFFER'): This event represents a draw offer.
        /// 32 ('EVENT_PAWN_PROMOTED'): This event represents a pawn promotion.
        /// 64 ('EVENT_PAWN_PASSANT'): This event represents an en passant pawn capture.
        /// 128 ('EVENT_CAPTURE'): This event represents a capture.
        /// 256 ('EVENT_CASTLEK'): This event represents a kingside castling.
        /// 512 ('EVENT_CASTLEQ'): This event represents a queenside castling.
        /// 1024 ('EVENT_PAWN1'): This event represents a pawn involved in a move (the piece that moves, captures, or gives check).
        /// 2048 ('EVENT_WBISHOP1'): This event represents a white-square bishop involved in a move (the piece that moves, captures, or gives check).
        /// 4096 ('EVENT_KNIGHT1'): This event represents a knight involved in a move (the piece that moves, captures, or gives check).
        /// 8192 ('EVENT_ROOK1'): This event represents a rook involved in a move (the piece that moves, captures, or gives check).
        /// 16384 ('EVENT_QUEEN1'): This event represents a queen involved in a move (the piece that moves, captures, or gives check).
        /// 32768 ('EVENT_KING1'): This event represents a king involved in a move (the piece that moves or captures).
        /// 65536 ('EVENT_PAWN2'): This event represents a pawn involved in a move (the piece that is captured).
        /// 131072 ('EVENT_WBISHOP2'): This event represents a white-square bishop involved in a move (the piece that is captured).
        /// 262144 ('EVENT_KNIGHT2'): This event represents a knight involved in a move (the piece that is captured).
        /// 524288 ('EVENT_ROOK2'): This event represents a rook involved in a move (the piece that is captured).
        /// 1048576 ('EVENT_QUEEN2'): This event represents a queen involved in a move (the piece that is captured).
        /// 2097152 ('EVENT_MOVE'): This event represents a movement.
        /// 4194304 ('EVENT_BBISHOP1'): This event represents a black-square bishop involved in a move (the piece that moves, captures, or gives check).
        /// 8388608 ('EVENT_BBISHOP2'): This event represents a black-square bishop involved in a move (the piece that is captured).
        /// 16777216 ('EVENT_BISHOP3'): This event represents a generic bishop (e.g., the piece a pawn promotes to).
        /// 33554432 ('EVENT_KNIGHT3'): This event represents a generic knight (e.g., the piece a pawn promotes to).
        /// 134217728 ('EVENT_QUEEN3'): This event represents a generic queen (e.g., the piece a pawn promotes to).
        /// </remarks>      
        /// <example>
        /// 2113538 = EVENT_CHECK_MATE + EVENT_QUEEN1 + EVENT_MOVE (The queen moves and gives checkmate).
        /// 2098176 = EVENT_PAWN1 + EVENT_MOVE(a pawn moves).
        /// 4259968 = EVENT_CAPTURE + EVENT_PAWN2 + EVENT_BBISHOP1(a bishop captures a pawn).
        /// 136315937 = EVENT_CHECK + EVENT_PAWN_PROMOTED + EVENT_PAWN1 + EVENT_MOVE + EVENT_QUEEN3(a pawn moves, promotes to a queen, and gives check).
        /// 136315938 = EVENT_CHECK_MATE + EVENT_PAWN_PROMOTED + EVENT_PAWN1 + EVENT_MOVE + EVENT_QUEEN3(a pawn moves, promotes to queen, and gives checkmate).
        /// </example>
        [Browsable(false)]
        public ulong MoveEvents { get; set; }
        /// <summary>
        /// Put white pieces up (false) or down (true).
        /// </summary>
        [Browsable(false)]
        public bool SideView
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
                DrawBoard(_position);
                SideViewChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Allow user to change piece disposition manually.
        /// </summary>
        public bool AllowSetPosition
        {
            get
            {
                return bInitialPos.Visible;
            }
            set
            {
                bInitialPos.Visible = value;
            }
        }
        /// <summary>
        /// Reset the board to the initial position.
        /// </summary>
        /// <param name="color">
        /// Player turn color, true for white and false for black.
        /// </param>
        public void Reset(bool color)
        {
            Reset(INITIAL_BOARD, color);
            bNext.Enabled = false;
        }
        /// <summary>
        /// Hide the tooltip if visible.
        /// </summary>
        public void HideToolTip()
        {
            if (_bubbleMessage != null)
            {
                try
                {
                    _bubbleMessage.Hide(this);
                    _bubbleMessage.Dispose();
                    _bubbleMessage = null;
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Show a tooltip with a ply comnent over a square.
        /// </summary>
        /// <param name="square">
        /// Number from 1 to 64 representing the square, where 1 is a1 and 64 is h8.
        /// </param>
        public void ShowToolTip(byte square, string comment, string title)
        {
            HideToolTip();
            int file = square / 8;
            file = _side ? (7 - file) : file;
            int rank = square % 8;
            Rectangle rc = new Rectangle(20 + rank * 60, 20 + file * 60, 60, 60);
            int offset = _color ? (_side ? 10 : -10) : (_side ? -10 : 10);
            _bubbleMessage = new ToolTip()
            {
                ToolTipTitle = title,
                ToolTipIcon = ToolTipIcon.Info,
                IsBalloon = true,
                ShowAlways = true
            };
            string spmessage = comment;
            int hl = 1;
            if (spmessage.Length > 32)
            {
                spmessage = spmessage.Replace("\r", "").Replace("\n", " ").Replace("  ", " ");
                List<string> lsm = new List<string>();
                while (!string.IsNullOrEmpty(spmessage))
                {
                    if (spmessage.Length <= 32)
                    {
                        lsm.Add(spmessage);
                        spmessage = null;
                    }
                    else
                    {
                        int ix = spmessage.IndexOf(' ', 32);
                        if (ix < 0)
                        {
                            lsm.Add(spmessage);
                            spmessage = null;
                        }
                        else
                        {
                            lsm.Add(spmessage.Substring(0, ix));
                            spmessage = spmessage.Substring(ix + 1);
                        }
                    }
                }
                hl = lsm.Count;
                spmessage = string.Join("\r\n", lsm);
            }
            Point pc = new Point(rc.Left + rc.Width / 2, rc.Top + (rc.Height / 2) - (35 + hl * 15));
            Point pt = PointToScreen(new Point(pc.X, pc.Y + offset));
            Point oldPos = Cursor.Position;
            Cursor.Position = pt;
            Application.DoEvents();
            _bubbleMessage.Show(spmessage, this, pc, 8000);
            Cursor.Position = oldPos;
        }
        /// <summary>
        /// Resets the game state to the specified board position and player turn.
        /// </summary>
        /// <param name="board">
        /// A string representing the initial board position to reset to.
        /// </param>
        /// <param name="color">
        /// A boolean value indicating the player's color
        /// </param>
        public void Reset(string board, bool color)
        {
            FromTo = new Point(-1, -1);
            MoveEvents = 0;
            txtMove.Text = "";
            _color = color;
            _position = board;
            DrawBoard(_position);
            bBack.Enabled = false;
            bNext.Enabled = true;
            HideToolTip();
        }
        /// <summary>
        /// Set a player move on the board.
        /// </summary>
        /// <param name="move">
        /// AN textual representation of the move, such as "e2-e4" or "Nf3".
        /// </param>
        /// <param name="color">
        /// Player color
        /// </param>
        /// <param name="board">
        /// Board position as a string, where each character represents a square on the board.
        /// </param>
        /// <param name="events">
        /// Move events as a bitmask, indicating the events that occurred during the move.
        /// </param>
        public void PlayerMove(string move, bool color, string board, ulong events)
        {
            txtMove.Text = move;
            MoveEvents = events;
            _color = color;
            if (string.IsNullOrEmpty(board))
            {
                bNext.Enabled = false;
            }
            else
            {
                BoardPosition = board;
            }
            bBack.Enabled = true;
            txtMove.BackColor = Color.White;
            txtMove.ForeColor = Color.DarkOliveGreen;
            if ((events & (uint)MatchEvent.Event.Capture) == (uint)MatchEvent.Event.Capture)
            {
                txtMove.ForeColor = Color.Black;
            }
            if ((events & (uint)MatchEvent.Event.Check) == (uint)MatchEvent.Event.Check)
            {
                txtMove.ForeColor = Color.Red;
            }
        }
        /// <summary>
        /// Convert a board position string to a bitmap image of the board.
        /// </summary>
        /// <param name="board">
        /// String representing the board position.
        /// </param>
        /// <returns>
        /// Bitmap with the drawn board.
        /// </returns>
        public Bitmap BoardFromString(string board)
        {
            return DrawBoardImage(board);
        }
        /// <summary>
        /// Draw the chess board with the current position and highlight the squares from and to if they are set.
        /// </summary>
        /// <param name="pos">
        /// 64 char string representing the board position.
        /// </param>
        private void DrawBoard(string pos)
        {
            Bitmap board = DrawBoardImage(pos);
            try
            {
                pbBoard.Image = board.Clone() as Bitmap;
                Refresh();
            }
            finally
            {
                board.Dispose();
            }
        }
        /// <summary>
        /// Draw the board image based on the current position string.
        /// </summary>
        /// <param name="pos">
        /// 64 char string representing the board position.
        /// </param>
        /// <returns>
        /// Bitmap with the drawn board.
        /// </returns>
        private Bitmap DrawBoardImage(string pos)
        {
            Bitmap pset = ChessPiecesArray;
            Bitmap board = new Bitmap(520, 520);
            try
            {
                using (Graphics gr = Graphics.FromImage(board))
                {
                    using (Font f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel))
                    {
                        gr.FillRectangle(Brushes.DarkGray, 0, 0, board.Width, board.Height);
                        if (_color)
                        {
                            if (_side)
                            {
                                gr.FillRectangle(Brushes.White, 0, 502, board.Width, 18);
                            }
                            else
                            {
                                gr.FillRectangle(Brushes.White, 0, 0, board.Width, 18);
                            }
                        }
                        else
                        {
                            if (_side)
                            {
                                gr.FillRectangle(Brushes.Black, 0, 0, board.Width, 18);
                            }
                            else
                            {
                                gr.FillRectangle(Brushes.Black, 0, 502, board.Width, 18);
                            }
                        }
                        gr.FillRectangle(Brushes.Black, 18, 18, board.Width - 36, board.Height - 36);
                        Rectangle rfrom = Rectangle.Empty;
                        Rectangle rto = Rectangle.Empty;
                        for (int rowt = 0; rowt < 8; rowt++)
                        {
                            int row = _side ? rowt : (7 - rowt);
                            int rowm = _side ? (7 - rowt) : rowt;
                            bool black = (rowt & 1) != 0;
                            string leg = char.ConvertFromUtf32(97 + rowt);
                            SizeF sz = gr.MeasureString(leg, f);
                            if (_color)
                            {
                                if (_side)
                                {
                                    gr.DrawString(leg, f, Brushes.Yellow, 20 + (row * 60) + (60 - sz.Width) / 2, (18 - sz.Height) / 2);
                                    gr.DrawString(leg, f, Brushes.Black, 20 + (row * 60) + (60 - sz.Width) / 2, 502 + (18 - sz.Height) / 2);
                                }
                                else
                                {
                                    gr.DrawString(leg, f, Brushes.Black, 20 + (row * 60) + (60 - sz.Width) / 2, (18 - sz.Height) / 2);
                                    gr.DrawString(leg, f, Brushes.Yellow, 20 + (row * 60) + (60 - sz.Width) / 2, 502 + (18 - sz.Height) / 2);
                                }
                            }
                            else
                            {
                                gr.DrawString(leg, f, Brushes.Yellow, 20 + (row * 60) + (60 - sz.Width) / 2, (18 - sz.Height) / 2);
                                gr.DrawString(leg, f, Brushes.Yellow, 20 + (row * 60) + (60 - sz.Width) / 2, 502 + (18 - sz.Height) / 2);
                            }
                            leg = (8 - rowt).ToString();
                            sz = gr.MeasureString(leg, f);
                            gr.DrawString(leg, f, Brushes.Yellow, (18 - sz.Width) / 2, 20 + (row * 60) + (60 - sz.Height) / 2);
                            gr.DrawString(leg, f, Brushes.Yellow, 502 + (18 - sz.Width) / 2, 20 + (row * 60) + (60 - sz.Height) / 2);
                            for (int colt = 0; colt < 8; colt++)
                            {
                                int col = _side ? colt : (7 - colt);
                                if (black)
                                {
                                    gr.FillRectangle(Brushes.Gray, 20 + col * 60, 20 + row * 60, 60, 60);
                                }
                                else
                                {
                                    gr.FillRectangle(Brushes.LightGray, 20 + col * 60, 20 + row * 60, 60, 60);
                                }
                                if (FromTo.X == (colt + rowt * 8))
                                {
                                    rfrom = new Rectangle(20 + col * 60, 20 + rowm * 60, 60, 60);
                                }
                                if (FromTo.Y == (colt + rowt * 8))
                                {
                                    rto = new Rectangle(20 + col * 60, 20 + rowm * 60, 60, 60);
                                }
                                black = !black;
                                Rectangle dest = new Rectangle(20 + col * 60, 20 + row * 60, 60, 60);
                                switch (pos[colt + (8 * rowt)])
                                {
                                    case 'p':
                                        gr.DrawImage(pset, dest, new Rectangle(300, 0, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'P':
                                        gr.DrawImage(pset, dest, new Rectangle(300, 60, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'r':
                                        gr.DrawImage(pset, dest, new Rectangle(120, 0, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'R':
                                        gr.DrawImage(pset, dest, new Rectangle(120, 60, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'b':
                                        gr.DrawImage(pset, dest, new Rectangle(240, 0, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'B':
                                        gr.DrawImage(pset, dest, new Rectangle(240, 60, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'n':
                                        gr.DrawImage(pset, dest, new Rectangle(180, 0, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'N':
                                        gr.DrawImage(pset, dest, new Rectangle(180, 60, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'q':
                                        gr.DrawImage(pset, dest, new Rectangle(0, 0, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'Q':
                                        gr.DrawImage(pset, dest, new Rectangle(0, 60, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'k':
                                        gr.DrawImage(pset, dest, new Rectangle(60, 0, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case 'K':
                                        gr.DrawImage(pset, dest, new Rectangle(60, 60, 60, 60), GraphicsUnit.Pixel);
                                        break;
                                    case '0':
                                        break;
                                    default:
                                        throw new Exception(string.Format(ERR_INVALIDBOARDCHAR, pos[colt + (8 * rowt)].ToString()));
                                }
                            }
                        }
                        if (bMarkMoves.Checked)
                        {
                            Color cpen = Color.Lime;
                            if (rfrom.Width > 0)
                            {
                                using (Pen pm = new Pen(cpen, 2f))
                                {
                                    gr.DrawRectangle(pm, rfrom);
                                }
                            }
                            if (rto.Width > 0)
                            {
                                if ((MoveEvents & (uint)MatchEvent.Event.Capture) == (uint)MatchEvent.Event.Capture)
                                {
                                    cpen = Color.Yellow;
                                }
                                if ((MoveEvents & (uint)MatchEvent.Event.Check) == (uint)MatchEvent.Event.Check)
                                {
                                    cpen = Color.Red;
                                }
                                if ((MoveEvents & (uint)MatchEvent.Event.Checkmate) == (uint)MatchEvent.Event.Checkmate)
                                {
                                    cpen = Color.Black;
                                }
                                using (Pen pm = new Pen(cpen, 2f))
                                {
                                    gr.DrawRectangle(pm, rto);
                                }
                            }
                        }
                        return board;
                    }
                }
            }
            finally
            {
                pset.Dispose();
            }
        }

        private void Board_Load(object sender, EventArgs e)
        {
            Width = 520;
            Height = 545;
            DrawBoard(_position);
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            ResetMatch?.Invoke(this, EventArgs.Empty);
            bNext.Enabled = true;
        }

        private void bBack_Click(object sender, EventArgs e)
        {
            MoveBack?.Invoke(this, EventArgs.Empty);
            bNext.Enabled = true;
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            MoveForward?.Invoke(this, EventArgs.Empty);
        }

        private void bRotateView_Click(object sender, EventArgs e)
        {
            try
            {
                SideView = !SideView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bInitialPos_Click(object sender, EventArgs e)
        {
            try
            {
                DlgInitialPosition dlg = new DlgInitialPosition();
                dlg.Board = BoardPosition;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    BoardPosition = dlg.Board;
                    BoardChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
