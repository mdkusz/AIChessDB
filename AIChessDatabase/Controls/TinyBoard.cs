using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Control to show a small chess board with pieces and moves.
    /// </summary>
    /// <seealso cref="Board"/>
    public partial class TinyBoard : UserControl
    {
        private string _position = INITIAL_BOARD;
        private bool _color = true;
        private bool _side = true;

        public TinyBoard()
        {
            InitializeComponent();
            FromTo = new Point(-1, -1);
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
        /// Put white pieces up (false) or down (true).
        /// </summary>
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
            }
        }
        /// <summary>
        /// Current move text in algebraic notation.
        /// </summary>
        public string ANText
        {
            get
            {
                return lMove.Text;
            }
            set
            {
                lMove.Text = value;
            }
        }
        /// <summary>
        /// Player color: true for white, false for black.
        /// </summary>
        public bool Player
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                lMove.BackColor = _color ? Color.White : Color.Black;
                lMove.ForeColor = _color ? Color.Black : Color.White;
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
        /// Draw the chess board with the current position and highlight the squares from and to if they are set.
        /// </summary>
        /// <param name="pos">
        /// 64 char string representing the board position.
        /// </param>
        /// <returns>
        /// Bitmap with the drawn board.
        /// </returns>
        private Bitmap DrawBoardImage(string pos)
        {
            Bitmap pset = ChessPiecesArraySmall;
            Bitmap board = new Bitmap(164, 164);
            try
            {
                using (Graphics gr = Graphics.FromImage(board))
                {
                    gr.FillRectangle(Brushes.Black, 0, 0, board.Width, board.Height);
                    Rectangle rfrom = Rectangle.Empty;
                    Rectangle rto = Rectangle.Empty;
                    for (int rowt = 0; rowt < 8; rowt++)
                    {
                        int row = _side ? rowt : (7 - rowt);
                        int rowm = _side ? (7 - rowt) : rowt;
                        bool black = (rowt & 1) != 0;
                        for (int colt = 0; colt < 8; colt++)
                        {
                            int col = _side ? colt : (7 - colt);
                            if (black)
                            {
                                gr.FillRectangle(Brushes.Gray, 2 + col * 20, 2 + row * 20, 20, 20);
                            }
                            else
                            {
                                gr.FillRectangle(Brushes.LightGray, 2 + col * 20, 2 + row * 20, 20, 20);
                            }
                            if (FromTo.X == (colt + rowt * 8))
                            {
                                rfrom = new Rectangle(2 + col * 20, 2 + rowm * 20, 20, 20);
                            }
                            if (FromTo.Y == (colt + rowt * 8))
                            {
                                rto = new Rectangle(2 + col * 20, 2 + rowm * 20, 20, 20);
                            }
                            black = !black;
                            Rectangle dest = new Rectangle(2 + col * 20, 2 + row * 20, 20, 20);
                            switch (pos[colt + (8 * rowt)])
                            {
                                case 'p':
                                    gr.DrawImage(pset, dest, new Rectangle(100, 0, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'P':
                                    gr.DrawImage(pset, dest, new Rectangle(100, 20, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'r':
                                    gr.DrawImage(pset, dest, new Rectangle(40, 0, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'R':
                                    gr.DrawImage(pset, dest, new Rectangle(40, 20, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'b':
                                    gr.DrawImage(pset, dest, new Rectangle(80, 0, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'B':
                                    gr.DrawImage(pset, dest, new Rectangle(80, 20, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'n':
                                    gr.DrawImage(pset, dest, new Rectangle(60, 0, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'N':
                                    gr.DrawImage(pset, dest, new Rectangle(60, 20, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'q':
                                    gr.DrawImage(pset, dest, new Rectangle(0, 0, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'Q':
                                    gr.DrawImage(pset, dest, new Rectangle(0, 20, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'k':
                                    gr.DrawImage(pset, dest, new Rectangle(20, 0, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case 'K':
                                    gr.DrawImage(pset, dest, new Rectangle(20, 20, 20, 20), GraphicsUnit.Pixel);
                                    break;
                                case '0':
                                    break;
                                default:
                                    throw new Exception(string.Format(ERR_INVALIDBOARDCHAR, pos[colt + (8 * rowt)].ToString()));
                            }
                        }
                    }
                    if (rfrom.Width > 0)
                    {
                        using (Pen pm = new Pen(Color.Lime, 2f))
                        {
                            gr.DrawRectangle(pm, rfrom);
                        }
                    }
                    if (rto.Width > 0)
                    {
                        using (Pen pm = new Pen(Color.Lime, 2f))
                        {
                            gr.DrawRectangle(pm, rto);
                        }
                    }
                    return board;
                }
            }
            finally
            {
                pset.Dispose();
            }
        }
    }
}
