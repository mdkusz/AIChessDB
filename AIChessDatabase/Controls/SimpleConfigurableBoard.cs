using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Control to edit a fixed chess board configuration.
    /// </summary>
    public partial class SimpleConfigurableBoard : UserControl
    {
        /// <summary>
        /// Auxiliary class to represent a square configuration on the chess board.
        /// </summary>
        private class Square
        {
            public Square()
            {
            }
            /// <summary>
            /// Piece on the square, represented by a character:
            /// </summary>
            /// <remarks>
            /// 0: are for empty squares:
            /// b or B are for black and white bishops respectively,
            /// r or R are for black and white rooks respectively,
            /// n or N are for black and white knights respectively,
            /// q or Q are for black and white queens respectively,
            /// k or K are for black and white kings respectively,
            /// p or P are for black and white pawns respectively.
            /// </remarks>
            public char Piece { get; set; }
            /// <summary>
            /// Square color, true for light squares, false for dark squares.
            /// </summary>
            public bool Color { get; set; }
            /// <summary>
            /// Piece image.
            /// </summary>
            public Bitmap Image
            {
                get
                {
                    Graphics gr = null;
                    try
                    {
                        Bitmap src = ChessPiecesArray;
                        Bitmap bmpp = new Bitmap(60, 60);
                        gr = Graphics.FromImage(bmpp);
                        gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 60, 60));
                        switch (Piece)
                        {
                            case 'b':
                                gr.DrawImage(src, 0, 0, new Rectangle(240, 0, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'k':
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 0, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'n':
                                gr.DrawImage(src, 0, 0, new Rectangle(180, 0, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'p':
                                gr.DrawImage(src, 0, 0, new Rectangle(300, 0, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'q':
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 0, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'r':
                                gr.DrawImage(src, 0, 0, new Rectangle(120, 0, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'B':
                                gr.DrawImage(src, 0, 0, new Rectangle(240, 60, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'K':
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 60, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'N':
                                gr.DrawImage(src, 0, 0, new Rectangle(180, 60, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'P':
                                gr.DrawImage(src, 0, 0, new Rectangle(300, 60, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'Q':
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 60, 60, 60), GraphicsUnit.Pixel);
                                break;
                            case 'R':
                                gr.DrawImage(src, 0, 0, new Rectangle(120, 60, 60, 60), GraphicsUnit.Pixel);
                                break;
                            default:
                                return null;
                        }
                        return bmpp;
                    }
                    finally
                    {
                        if (gr != null)
                        {
                            gr.Dispose();
                        }
                    }
                }
            }
            /// <summary>
            /// Piece tiny image.
            /// </summary>
            public Bitmap TinyImage
            {
                get
                {
                    Graphics gr = null;
                    try
                    {
                        Bitmap src = ChessPiecesArraySmall;
                        Bitmap bmpp = new Bitmap(20, 20);
                        gr = Graphics.FromImage(bmpp);
                        gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                        switch (Piece)
                        {
                            case 'b':
                                gr.DrawImage(src, 0, 0, new Rectangle(80, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'k':
                                gr.DrawImage(src, 0, 0, new Rectangle(20, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'n':
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'p':
                                gr.DrawImage(src, 0, 0, new Rectangle(100, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'q':
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'r':
                                gr.DrawImage(src, 0, 0, new Rectangle(40, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'B':
                                gr.DrawImage(src, 0, 0, new Rectangle(80, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'K':
                                gr.DrawImage(src, 0, 0, new Rectangle(20, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'N':
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'P':
                                gr.DrawImage(src, 0, 0, new Rectangle(100, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'Q':
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case 'R':
                                gr.DrawImage(src, 0, 0, new Rectangle(40, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            default:
                                return null;
                        }
                        return bmpp;
                    }
                    finally
                    {
                        if (gr != null)
                        {
                            gr.Dispose();
                        }
                    }
                }
            }
        }
        private Square[,] _position = new Square[8, 8];
        private Point _xyMouse = new Point(-1, -1);
        private string _iDrag = null;

        public SimpleConfigurableBoard()
        {
            InitializeComponent();
            FromTo = new Point(-1, -1);
            InitializeBoard();
            DrawBoard();
        }
        public SimpleConfigurableBoard(string board)
        {
            InitializeComponent();
            for (int ix = 0; ix < Math.Min(64, board.Length); ix++)
            {
                _position[ix % 8, ix / 8] = new Square()
                {
                    Piece = board[ix],
                    Color = (((ix % 8) & 1) ^ ((ix / 8) & 1)) == 0
                };
            }
        }
        /// <summary>
        /// Board piece disposition in custom format.
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
        public string Board
        {
            get
            {
                string board = "";
                for (int f = 0; f < 8; f++)
                {
                    for (int r = 0; r < 8; r++)
                    {
                        board += _position[r, f].Piece;
                    }
                }
                return board;
            }
            set
            {
                if (value != null)
                {
                    for (int ix = 0; ix < Math.Min(64, value.Length); ix++)
                    {
                        _position[ix % 8, ix / 8] = new Square()
                        {
                            Piece = value[ix],
                            Color = (((ix % 8) & 1) ^ ((ix / 8) & 1)) == 0
                        };
                    }
                }
                DrawBoard();
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
        /// Tiny board image.
        /// </summary>
        public Bitmap TinyBoard
        {
            get
            {
                return DrawTinyBoard();
            }
        }
        /// <summary>
        /// Clear board from all pieces and reset to initial state.
        /// </summary>
        public void Clear()
        {
            InitializeBoard();
            DrawBoard();
        }
        /// <summary>
        /// Set all squares to empty.
        /// </summary>
        private void InitializeBoard()
        {
            for (int f = 0; f < 8; f++)
            {
                for (int r = 0; r < 8; r++)
                {
                    _position[r, f] = new Square()
                    {
                        Color = ((r & 1) ^ (f & 1)) == 0,
                        Piece = '0'
                    };
                }
            }
        }
        /// <summary>
        /// Draw a bitmap with the board configuration.
        /// </summary>
        private void DrawBoard()
        {
            Bitmap board = new Bitmap(520, 520);
            Graphics gr = Graphics.FromImage(board);
            Font f = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            try
            {
                gr.FillRectangle(Brushes.DarkGray, 0, 0, board.Width, board.Height);
                gr.FillRectangle(Brushes.Black, 18, 18, board.Width - 36, board.Height - 36);
                for (int file = 0; file < 8; file++)
                {
                    string leg = char.ConvertFromUtf32(97 + file);
                    SizeF sz = gr.MeasureString(leg, f);
                    gr.DrawString(leg, f, Brushes.Yellow, 20 + (file * 60) + (60 - sz.Width) / 2, (18 - sz.Height) / 2);
                    gr.DrawString(leg, f, Brushes.Yellow, 20 + (file * 60) + (60 - sz.Width) / 2, 502 + (18 - sz.Height) / 2);
                    leg = (8 - file).ToString();
                    sz = gr.MeasureString(leg, f);
                    gr.DrawString(leg, f, Brushes.Yellow, (18 - sz.Width) / 2, 20 + (file * 60) + (60 - sz.Height) / 2);
                    gr.DrawString(leg, f, Brushes.Yellow, 502 + (18 - sz.Width) / 2, 20 + (file * 60) + (60 - sz.Height) / 2);
                    for (int rank = 0; rank < 8; rank++)
                    {
                        if (_position[rank, file].Color)
                        {
                            gr.FillRectangle(Brushes.LightGray, 20 + rank * 60, 20 + file * 60, 60, 60);
                        }
                        else
                        {
                            gr.FillRectangle(Brushes.Gray, 20 + rank * 60, 20 + file * 60, 60, 60);
                        }
                        if (FromTo.X == rank + file * 8)
                        {
                            using (Brush bm = new SolidBrush(Color.FromArgb(100, Color.Yellow)))
                            {
                                gr.FillRectangle(bm, 20 + rank * 60, 20 + file * 60, 60, 60);
                            }
                        }
                        if (FromTo.Y == rank + file * 8)
                        {
                            using (Brush bm = new SolidBrush(Color.FromArgb(100, Color.Lime)))
                            {
                                gr.FillRectangle(bm, 20 + rank * 60, 20 + file * 60, 60, 60);
                            }
                        }
                        if (_position[rank, file].Piece != '0')
                        {
                            Bitmap bmp = _position[rank, file].Image;
                            if (bmp != null)
                            {
                                gr.DrawImage(bmp, new Rectangle(20 + rank * 60, 20 + file * 60, 60, 60));
                            }
                        }
                    }
                }
                pbBoard.Image = board.Clone() as Bitmap;
                Refresh();
            }
            finally
            {
                f.Dispose();
                gr.Dispose();
                board.Dispose();
            }
        }
        /// <summary>
        /// Draw a reduced bitmap from the board position.
        /// </summary>
        /// <returns></returns>
        private Bitmap DrawTinyBoard()
        {
            Bitmap board = new Bitmap(162, 162);
            Graphics gr = Graphics.FromImage(board);
            try
            {
                gr.FillRectangle(Brushes.Black, 0, 0, board.Width, board.Height);
                for (int file = 0; file < 8; file++)
                {
                    for (int rank = 0; rank < 8; rank++)
                    {
                        if (_position[rank, file].Color)
                        {
                            gr.FillRectangle(Brushes.LightGray, 1 + rank * 20, 1 + file * 20, 20, 20);
                        }
                        else
                        {
                            gr.FillRectangle(Brushes.Gray, 1 + rank * 20, 1 + file * 20, 20, 20);
                        }
                        if (_position[rank, file].Piece != '0')
                        {
                            Bitmap bmp = _position[rank, file].TinyImage;
                            if (bmp != null)
                            {
                                gr.DrawImage(bmp, new Rectangle(1 + rank * 20, 1 + file * 20, 20, 20));
                            }
                        }
                    }
                }
                return board;
            }
            finally
            {
                gr.Dispose();
            }
        }

        private void pbBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (new Rectangle(20, 20, 480, 480).Contains(e.Location))
                {
                    _xyMouse = new Point(20 + 60 * ((e.Location.X - 20) / 60), 20 + 60 * ((e.Location.Y - 20) / 60));
                    int r = (e.X - 20) / 60;
                    int f = (e.Y - 20) / 60;
                    if (!string.IsNullOrEmpty(_iDrag))
                    {
                        string pz = _iDrag;
                        if ((pz[0] == 'k') || (pz[0] == 'K'))
                        {
                            for (int fk = 0; fk < 8; fk++)
                            {
                                for (int rk = 0; rk < 8; rk++)
                                {
                                    if (_position[r, f].Piece == pz[0])
                                    {
                                        _position[r, f].Piece = '0';
                                    }
                                }
                            }
                        }
                        _position[r, f].Piece = pz[0];
                    }
                    else
                    {
                        _position[r, f].Piece = '0';
                    }
                    DrawBoard();
                }
            }
        }

        private void pbBoard_DoubleClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_iDrag))
            {
                _iDrag = null;
                Cursor = Cursors.Default;
            }
        }

        private void pbBoard_MouseLeave(object sender, EventArgs e)
        {
            _iDrag = null;
            Cursor = Cursors.Default;
        }

        private void pTools_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (new Rectangle((pTools.Width - 360) / 2, 0, 360, 180).Contains(e.Location))
                {
                    int x = (e.Location.X - (pTools.Width - 360) / 2) / 60;
                    int y = e.Location.Y / 60;
                    switch (x + 6 * y)
                    {
                        case 0:
                            _iDrag = "q";
                            break;
                        case 1:
                            _iDrag = "k";
                            break;
                        case 2:
                            _iDrag = "r";
                            break;
                        case 3:
                            _iDrag = "n";
                            break;
                        case 4:
                            _iDrag = "b";
                            break;
                        case 5:
                            _iDrag = "p";
                            break;
                        case 6:
                            _iDrag = "Q";
                            break;
                        case 7:
                            _iDrag = "K";
                            break;
                        case 8:
                            _iDrag = "R";
                            break;
                        case 9:
                            _iDrag = "N";
                            break;
                        case 10:
                            _iDrag = "B";
                            break;
                        case 11:
                            _iDrag = "P";
                            break;
                        default:
                            _iDrag = null;
                            break;
                    }
                }
            }
        }

        private void pTools_MouseMove(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(_iDrag))
            {
                switch (_iDrag)
                {
                    case "p":
                        Cursor = new Cursor(new MemoryStream(bpawn));
                        break;
                    case "P":
                        Cursor = new Cursor(new MemoryStream(wpawn));
                        break;
                    case "b":
                        Cursor = new Cursor(new MemoryStream(bbishop));
                        break;
                    case "B":
                        Cursor = new Cursor(new MemoryStream(wbishop));
                        break;
                    case "n":
                        Cursor = new Cursor(new MemoryStream(bknight));
                        break;
                    case "N":
                        Cursor = new Cursor(new MemoryStream(wknight));
                        break;
                    case "r":
                        Cursor = new Cursor(new MemoryStream(brook));
                        break;
                    case "R":
                        Cursor = new Cursor(new MemoryStream(wrook));
                        break;
                    case "q":
                        Cursor = new Cursor(new MemoryStream(bqueen));
                        break;
                    case "Q":
                        Cursor = new Cursor(new MemoryStream(wqueen));
                        break;
                    case "k":
                        Cursor = new Cursor(new MemoryStream(bking));
                        break;
                    case "K":
                        Cursor = new Cursor(new MemoryStream(wking));
                        break;
                }
            }
        }
    }
}
