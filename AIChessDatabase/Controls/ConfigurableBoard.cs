using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIElements;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Control to set up a chess board with configurable pieces and colors.
    /// </summary>
    /// <remarks>
    /// You can put more than one piece on a square to set multiple possible options, and the pieces can be black, white or no-matter color.
    /// You can mark a square as specifically empty.
    /// You can set up only a rectangle of the board, with a structure of pieces and squares that can be located anywhere on the board.
    /// </remarks
    public partial class ConfigurableBoard : UserControl
    {
        private const char cUndefinedKing = 'w';
        private const char cUndefinedQueen = 'v';
        private const char cUndefinedBishop = 'a';
        private const char cUndefinedKnight = 'c';
        private const char cUndefinedRook = 't';
        private const char cUndefinedPawn = 's';

        /// <summary>
        /// Flags to set multiple possible pices on a square.
        /// </summary>
        [Flags]
        [Serializable]
        private enum PiecesSet
        {
            NoMatter = 0, WhitePawn = 1, BlackPawn = 2, Pawn = 3, WhiteRook = 4, BlackRook = 8, Rook = 12, WhiteKnight = 16, BlackKnight = 32, Knight = 48,
            WhiteBishop = 64, BlackBishop = 128, Bishop = 192, WhiteQueen = 256, BlackQueen = 512, Queen = 768, WhiteKing = 1024, BlackKing = 2048, King = 3072,
            Empty = 4096
        }
        /// <summary>
        /// Flags to set square colors.
        /// </summary>
        [Flags]
        private enum SquareColor
        {
            Dark = 1, Light = 2, Indifferent = 3,
        }
        /// <summary>
        /// Auxiliary class to represent a square on the chess board.
        /// </summary>
        private class Square
        {
            private bool _colorChanged = false;
            public Square()
            {
            }
            /// <summary>
            /// Event fired when user changes the square properties, like color or pieces.
            /// </summary>
            public event EventHandler SquareChanged = null;
            /// <summary>
            /// Flag to indicate if the color of the square has changed since last check.
            /// </summary>
            public bool ColorChanged
            {
                get
                {
                    bool ch = _colorChanged;
                    _colorChanged = false;
                    return ch;
                }
            }
            /// <summary>
            /// Flags to set the pieces on the square.
            /// </summary>
            public PiecesSet Pieces { get; set; }
            /// <summary>
            /// Square marked as selected by the user.
            /// </summary>
            public bool Selected { get; set; }
            /// <summary>
            /// Square color: dark, light or indifferent.
            /// </summary>
            public SquareColor Color { get; set; }
            /// <summary>
            /// Count of different pieces on the square.
            /// </summary>
            public int Count
            {
                get
                {
                    int n = 0;
                    int pz = (int)Pieces;
                    while (pz > 0)
                    {
                        if ((pz & 3) == 3)
                        {
                            n++;
                        }
                        else if (((pz & 1) != 0) || ((pz & 2) != 0))
                        {
                            n++;
                        }
                        pz >>= 2;
                    }
                    return n;
                }
            }
            /// <summary>
            /// Regular expression representation of the pieces on the square.
            /// </summary>
            public string Expression
            {
                get
                {
                    string expr = "";
                    if (Count > 0)
                    {
                        if ((Pieces & PiecesSet.BlackBishop) != 0)
                        {
                            expr += "b";
                        }
                        if ((Pieces & PiecesSet.WhiteBishop) != 0)
                        {
                            expr += "B";
                        }
                        if ((Pieces & PiecesSet.BlackKnight) != 0)
                        {
                            expr += "n";
                        }
                        if ((Pieces & PiecesSet.WhiteKnight) != 0)
                        {
                            expr += "N";
                        }
                        if ((Pieces & PiecesSet.BlackPawn) != 0)
                        {
                            expr += "p";
                        }
                        if ((Pieces & PiecesSet.WhitePawn) != 0)
                        {
                            expr += "P";
                        }
                        if ((Pieces & PiecesSet.BlackQueen) != 0)
                        {
                            expr += "q";
                        }
                        if ((Pieces & PiecesSet.WhiteQueen) != 0)
                        {
                            expr += "Q";
                        }
                        if ((Pieces & PiecesSet.BlackRook) != 0)
                        {
                            expr += "r";
                        }
                        if ((Pieces & PiecesSet.WhiteRook) != 0)
                        {
                            expr += "R";
                        }
                        if ((Pieces & PiecesSet.BlackKing) != 0)
                        {
                            expr += "k";
                        }
                        if ((Pieces & PiecesSet.WhiteKing) != 0)
                        {
                            expr += "K";
                        }
                        if ((Pieces & PiecesSet.Empty) != 0)
                        {
                            expr += "0";
                        }
                        expr = expr.Length > 1 ? "[" + expr + "]" : expr;
                    }
                    else
                    {
                        expr = ".";
                    }
                    return expr;
                }
                set
                {
                    Pieces = PiecesSet.NoMatter;
                    if (value != ".")
                    {
                        Pieces = Pieces | GetPieces(value);
                    }
                }
            }
            /// <summary>
            /// Bitmap representing the square with the pieces on it.
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
                        switch (Pieces)
                        {
                            case PiecesSet.Bishop:
                                gr.DrawImage(src, 0, 0, new Rectangle(80, 40, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.BlackBishop:
                                gr.DrawImage(src, 0, 0, new Rectangle(80, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.BlackKing:
                                gr.DrawImage(src, 0, 0, new Rectangle(20, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.BlackKnight:
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.BlackPawn:
                                gr.DrawImage(src, 0, 0, new Rectangle(100, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.BlackQueen:
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.BlackRook:
                                gr.DrawImage(src, 0, 0, new Rectangle(40, 0, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.King:
                                gr.DrawImage(src, 0, 0, new Rectangle(20, 40, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.Knight:
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 40, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.Pawn:
                                gr.DrawImage(src, 0, 0, new Rectangle(100, 40, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.Queen:
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 40, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.Rook:
                                gr.DrawImage(src, 0, 0, new Rectangle(40, 40, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.WhiteBishop:
                                gr.DrawImage(src, 0, 0, new Rectangle(80, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.WhiteKing:
                                gr.DrawImage(src, 0, 0, new Rectangle(20, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.WhiteKnight:
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.WhitePawn:
                                gr.DrawImage(src, 0, 0, new Rectangle(100, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.WhiteQueen:
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.WhiteRook:
                                gr.DrawImage(src, 0, 0, new Rectangle(40, 20, 20, 20), GraphicsUnit.Pixel);
                                break;
                            case PiecesSet.Empty:
                                using (Pen p = new Pen(Brushes.Black, 2f))
                                {
                                    gr.DrawEllipse(p, new Rectangle(6, 4, 8, 12));
                                    gr.DrawLine(p, 6, 4, 14, 16);
                                }
                                break;
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
            /// Piece images to draw on the square.
            /// </summary>
            public IEnumerable<Bitmap> Images
            {
                get
                {
                    Graphics gr = null;
                    try
                    {
                        int cnt = Count;
                        if (cnt == 1)
                        {
                            Bitmap src = ChessPiecesArray;
                            Bitmap bmpp = new Bitmap(60, 60);
                            gr = Graphics.FromImage(bmpp);
                            gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 60, 60));
                            switch (Pieces)
                            {
                                case PiecesSet.Bishop:
                                    gr.DrawImage(src, 0, 0, new Rectangle(240, 120, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.BlackBishop:
                                    gr.DrawImage(src, 0, 0, new Rectangle(240, 0, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.BlackKing:
                                    gr.DrawImage(src, 0, 0, new Rectangle(60, 0, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.BlackKnight:
                                    gr.DrawImage(src, 0, 0, new Rectangle(180, 0, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.BlackPawn:
                                    gr.DrawImage(src, 0, 0, new Rectangle(300, 0, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.BlackQueen:
                                    gr.DrawImage(src, 0, 0, new Rectangle(0, 0, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.BlackRook:
                                    gr.DrawImage(src, 0, 0, new Rectangle(120, 0, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.King:
                                    gr.DrawImage(src, 0, 0, new Rectangle(60, 120, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.Knight:
                                    gr.DrawImage(src, 0, 0, new Rectangle(180, 120, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.Pawn:
                                    gr.DrawImage(src, 0, 0, new Rectangle(300, 120, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.Queen:
                                    gr.DrawImage(src, 0, 0, new Rectangle(0, 120, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.Rook:
                                    gr.DrawImage(src, 0, 0, new Rectangle(120, 120, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.WhiteBishop:
                                    gr.DrawImage(src, 0, 0, new Rectangle(240, 60, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.WhiteKing:
                                    gr.DrawImage(src, 0, 0, new Rectangle(60, 60, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.WhiteKnight:
                                    gr.DrawImage(src, 0, 0, new Rectangle(180, 60, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.WhitePawn:
                                    gr.DrawImage(src, 0, 0, new Rectangle(300, 60, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.WhiteQueen:
                                    gr.DrawImage(src, 0, 0, new Rectangle(0, 60, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.WhiteRook:
                                    gr.DrawImage(src, 0, 0, new Rectangle(120, 60, 60, 60), GraphicsUnit.Pixel);
                                    break;
                                case PiecesSet.Empty:
                                    using (Pen p = new Pen(Brushes.Black, 2f))
                                    {
                                        gr.DrawEllipse(p, new Rectangle(18, 13, 25, 35));
                                        gr.DrawLine(p, 18, 13, 43, 48);
                                    }
                                    break;
                            }
                            yield return bmpp;
                        }
                        else if (cnt > 1)
                        {
                            Bitmap src = ChessPiecesArraySmall;
                            if ((Pieces & PiecesSet.Empty) != 0)
                            {
                                Bitmap bmpp = new Bitmap(20, 20);
                                gr = Graphics.FromImage(bmpp);
                                gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                gr.DrawEllipse(Pens.Black, new Rectangle(6, 4, 8, 11));
                                gr.DrawLine(Pens.Black, 6, 4, 14, 15);
                                yield return bmpp;
                            }
                            if ((Pieces & PiecesSet.Bishop) == PiecesSet.Bishop)
                            {
                                Bitmap bmpp = new Bitmap(20, 20);
                                gr = Graphics.FromImage(bmpp);
                                gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                gr.DrawImage(src, 0, 0, new Rectangle(80, 40, 20, 20), GraphicsUnit.Pixel);
                                yield return bmpp;
                            }
                            else
                            {
                                if ((Pieces & PiecesSet.BlackBishop) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(80, 0, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                                if ((Pieces & PiecesSet.WhiteBishop) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(80, 20, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                            }
                            if ((Pieces & PiecesSet.Knight) == PiecesSet.Knight)
                            {
                                Bitmap bmpp = new Bitmap(20, 20);
                                gr = Graphics.FromImage(bmpp);
                                gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                gr.DrawImage(src, 0, 0, new Rectangle(60, 40, 20, 20), GraphicsUnit.Pixel);
                                yield return bmpp;
                            }
                            else
                            {
                                if ((Pieces & PiecesSet.BlackKnight) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(60, 0, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                                if ((Pieces & PiecesSet.WhiteKnight) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(60, 20, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                            }
                            if ((Pieces & PiecesSet.Pawn) == PiecesSet.Pawn)
                            {
                                Bitmap bmpp = new Bitmap(20, 20);
                                gr = Graphics.FromImage(bmpp);
                                gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                gr.DrawImage(src, 0, 0, new Rectangle(100, 40, 20, 20), GraphicsUnit.Pixel);
                                yield return bmpp;
                            }
                            else
                            {
                                if ((Pieces & PiecesSet.BlackPawn) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(100, 0, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                                if ((Pieces & PiecesSet.WhitePawn) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(100, 20, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                            }
                            if ((Pieces & PiecesSet.Queen) == PiecesSet.Queen)
                            {
                                Bitmap bmpp = new Bitmap(20, 20);
                                gr = Graphics.FromImage(bmpp);
                                gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                gr.DrawImage(src, 0, 0, new Rectangle(0, 40, 20, 20), GraphicsUnit.Pixel);
                                yield return bmpp;
                            }
                            else
                            {
                                if ((Pieces & PiecesSet.BlackQueen) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(0, 0, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                                if ((Pieces & PiecesSet.WhiteQueen) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(0, 20, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                            }
                            if ((Pieces & PiecesSet.Rook) == PiecesSet.Rook)
                            {
                                Bitmap bmpp = new Bitmap(20, 20);
                                gr = Graphics.FromImage(bmpp);
                                gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                gr.DrawImage(src, 0, 0, new Rectangle(40, 40, 20, 20), GraphicsUnit.Pixel);
                                yield return bmpp;
                            }
                            else
                            {
                                if ((Pieces & PiecesSet.BlackRook) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(40, 0, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                                if ((Pieces & PiecesSet.WhiteRook) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(40, 20, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                            }
                            if ((Pieces & PiecesSet.King) == PiecesSet.King)
                            {
                                Bitmap bmpp = new Bitmap(20, 20);
                                gr = Graphics.FromImage(bmpp);
                                gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                gr.DrawImage(src, 0, 0, new Rectangle(20, 40, 20, 20), GraphicsUnit.Pixel);
                                yield return bmpp;
                            }
                            else
                            {
                                if ((Pieces & PiecesSet.BlackKing) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(20, 0, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                                if ((Pieces & PiecesSet.WhiteKing) != 0)
                                {
                                    Bitmap bmpp = new Bitmap(20, 20);
                                    gr = Graphics.FromImage(bmpp);
                                    gr.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 20, 20));
                                    gr.DrawImage(src, 0, 0, new Rectangle(20, 20, 20, 20), GraphicsUnit.Pixel);
                                    yield return bmpp;
                                }
                            }
                        }
                        yield break;
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
            /// Get a PiecesSet from a string representation of pieces.
            /// </summary>
            /// <param name="value">
            /// String representing pieces on the square, like "pBq" for black pawn, white bishop and black queen.
            /// </param>
            /// <returns>
            /// PieceSet flags representing the pieces on the square.
            /// </returns>
            /// <seealso cref="Expression"/>
            public static PiecesSet GetPieces(string value)
            {
                PiecesSet pz = PiecesSet.NoMatter;
                if (value.Contains("p"))
                {
                    pz = pz | PiecesSet.BlackPawn;
                }
                if (value.Contains("P"))
                {
                    pz = pz | PiecesSet.WhitePawn;
                }
                if (value.Contains("b"))
                {
                    pz = pz | PiecesSet.BlackBishop;
                }
                if (value.Contains("B"))
                {
                    pz = pz | PiecesSet.WhiteBishop;
                }
                if (value.Contains("n"))
                {
                    pz = pz | PiecesSet.BlackKnight;
                }
                if (value.Contains("N"))
                {
                    pz = pz | PiecesSet.WhiteKnight;
                }
                if (value.Contains("r"))
                {
                    pz = pz | PiecesSet.BlackRook;
                }
                if (value.Contains("R"))
                {
                    pz = pz | PiecesSet.WhiteRook;
                }
                if (value.Contains("q"))
                {
                    pz = pz | PiecesSet.BlackQueen;
                }
                if (value.Contains("Q"))
                {
                    pz = pz | PiecesSet.WhiteQueen;
                }
                if (value.Contains("k"))
                {
                    pz = pz | PiecesSet.BlackKing;
                }
                if (value.Contains("K"))
                {
                    pz = pz | PiecesSet.WhiteKing;
                }
                if (value.Contains("0"))
                {
                    pz = pz | PiecesSet.Empty;
                }
                return pz;
            }
            /// <summary>
            /// Show a context menu for the square to change its properties.
            /// </summary>
            /// <param name="menu">
            /// Context menu strip to add options.
            /// </param>
            /// <param name="colors">
            /// Add color options to menu.
            /// </param>
            /// <param name="ctl">
            /// Control to show the menu on.
            /// </param>
            /// <param name="pos">
            /// Position to show the menu at.
            /// </param>
            /// <param name="ClearAll">
            /// Clear event handler to clear the square.
            /// </param>
            /// <param name="EmptyAll">
            /// Event handler to empty the square.
            /// </param>
            public void ShowMenu(ContextMenuStrip menu, bool colors, Control ctl, Point pos, EventHandler ClearAll, EventHandler EmptyAll)
            {
                menu.Items.Clear();
                if (colors)
                {
                    menu.Items.Add(new ToolStripMenuItem(OPT_NOCOLOR, null, new EventHandler(IndifferentColor))
                    {
                        AccessibleRole = AccessibleRole.MenuItem,
                        AccessibleName = OPT_NOCOLOR,
                        AccessibleDescription = OPT_NOCOLOR_DESCRIPTION,
                    });
                    menu.Items.Add(new ToolStripMenuItem(OPT_COLORBLACK, null, new EventHandler(BlackColor))
                    {
                        AccessibleRole = AccessibleRole.MenuItem,
                        AccessibleName = OPT_COLORBLACK,
                        AccessibleDescription = OPT_COLORBLACK_DESCRIPTION,
                    });
                    menu.Items.Add(new ToolStripMenuItem(OPT_COLORWHITE, null, new EventHandler(WhiteColor))
                    {
                        AccessibleRole = AccessibleRole.MenuItem,
                        AccessibleName = OPT_COLORWHITE,
                        AccessibleDescription = OPT_COLORWHITE_DESCRIPTION,
                    });
                    menu.Items.Add(new ToolStripSeparator());
                }
                menu.Items.Add(new ToolStripMenuItem(OPT_CLEAR, null, new EventHandler(Clear))
                {
                    AccessibleRole = AccessibleRole.MenuItem,
                    AccessibleName = OPT_CLEAR,
                    AccessibleDescription = OPT_CLEAR_DESCRIPTION,
                });
                menu.Items.Add(new ToolStripMenuItem(OPT_CLEARALL, null, ClearAll)
                {
                    AccessibleRole = AccessibleRole.MenuItem,
                    AccessibleName = OPT_CLEARALL,
                    AccessibleDescription = OPT_CLEARALL_DESCRIPTION,
                });
                menu.Items.Add(new ToolStripMenuItem(OPT_EMPTY, null, new EventHandler(Empty))
                {
                    AccessibleRole = AccessibleRole.MenuItem,
                    AccessibleName = OPT_EMPTY,
                    AccessibleDescription = OPT_EMPTY_DESCRIPTION,
                });
                menu.Items.Add(new ToolStripMenuItem(OPT_EMPTYALL, null, EmptyAll)
                {
                    AccessibleRole = AccessibleRole.MenuItem,
                    AccessibleName = OPT_EMPTYALL,
                    AccessibleDescription = OPT_EMPTYALL_DESCRIPTION,
                });
                menu.Show(ctl, pos);
            }
            private void IndifferentColor(object sender, EventArgs e)
            {
                _colorChanged = Color != SquareColor.Indifferent;
                Color = SquareColor.Indifferent;
                SquareChanged?.Invoke(this, EventArgs.Empty);
            }
            private void WhiteColor(object sender, EventArgs e)
            {
                _colorChanged = Color != SquareColor.Light;
                Color = SquareColor.Light;
                SquareChanged?.Invoke(this, EventArgs.Empty);
            }
            private void BlackColor(object sender, EventArgs e)
            {
                _colorChanged = Color != SquareColor.Dark;
                Color = SquareColor.Dark;
                SquareChanged?.Invoke(this, EventArgs.Empty);
            }
            private void Clear(object sender, EventArgs e)
            {
                Pieces = PiecesSet.NoMatter;
                SquareChanged?.Invoke(this, EventArgs.Empty);
            }
            private void Empty(object sender, EventArgs e)
            {
                Pieces = Pieces | PiecesSet.Empty;
                SquareChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private Square[,] _position = new Square[8, 8];
        private Point _xyMouse = new Point(-1, -1);
        private Point _xySel = new Point(-1, -1);
        private string _iDrag = null;
        private Rectangle _rSelection = Rectangle.Empty;

        public ConfigurableBoard()
        {
            InitializeComponent();
            InitializeBoard();
            DrawBoard();
        }
        /// <summary>
        /// Rectangle representing the current selection on the board.
        /// </summary>
        public Rectangle Selection
        {
            get
            {
                return _rSelection;
            }
            set
            {
                _rSelection = value;
                if (_rSelection == new Rectangle(0, 0, 8, 8))
                {
                    SetSelection(SquareColor.Light);
                }
                else
                {
                    SetSelection(_position[_rSelection.X, _rSelection.Y].Color);
                }
                DrawBoard();
            }
        }
        /// <summary>
        /// Chess board is represented as a regular expression.
        /// </summary>
        public bool Regex
        {
            get
            {
                string expr = Expression;
                return expr.Contains("{") || expr.Contains("[") || expr.Contains(".") || expr.Contains("(") || expr.Contains("^") || (expr.Length != 64);
            }
        }
        /// <summary>
        /// Regular expression representing the current board state.
        /// </summary>
        public string Expression
        {
            get
            {
                string expr = "";
                if (Selection.Width > 0)
                {
                    for (int f = 0; f < Selection.Height; f++)
                    {
                        if ((f != 0) && (Selection.Width < 8))
                        {
                            expr += ".{" + (8 - Selection.Width).ToString() + "}";
                        }
                        for (int r = 0; r < Selection.Width; r++)
                        {
                            expr += _position[r + Selection.X, f + Selection.Y].Expression;
                        }
                    }
                    if ((_position[Selection.X, Selection.Y].Color != SquareColor.Indifferent)
                        && (Selection.Width * Selection.Height < 64))
                    {
                        string sep = "";
                        string opexpr = "";
                        for (int f = 0; f <= 8 - Selection.Height; f++)
                        {
                            for (int r = 0; r <= 8 - Selection.Width; r++)
                            {
                                if (((_position[Selection.X, Selection.Y].Color == SquareColor.Light) && (((f ^ r) & 1) == 0)) ||
                                    ((_position[Selection.X, Selection.Y].Color == SquareColor.Dark) && (((f ^ r) & 1) == 1)))
                                {
                                    if ((f + r) != 0)
                                    {
                                        opexpr += sep + "(^.{" + (r + (f * 8)).ToString() + "}" + expr + ")";
                                    }
                                    else
                                    {
                                        opexpr += sep + "(^" + expr + ")";
                                    }
                                    sep = "|";
                                }
                            }
                        }
                        expr = opexpr;
                    }
                }
                return expr;
            }
            set
            {
                InitializeBoard();
                if (!string.IsNullOrEmpty(value))
                {
                    SetBoardExpression(value);
                }
                DrawBoard();
            }
        }
        /// <summary>
        /// Clear the board and reset it to the initial state.
        /// </summary>
        public void Clear()
        {
            InitializeBoard();
            Selection = new Rectangle(0, 0, 8, 8);
            DrawBoard();
        }
        /// <summary>
        /// Get a small image of the board with pieces on it.
        /// </summary>
        /// <param name="board">
        /// String representing the board state as a regular expression or in this application custom format.
        /// </param>
        /// <returns>
        /// Bitmap image of the board with pieces.
        /// </returns>
        /// <remarks>
        /// The custom format is a 64 character string representing the board position.
        /// 0: are for empty squares:
        /// b or B are for black and white bishops respectively,
        /// r or R are for black and white rooks respectively,
        /// n or N are for black and white knights respectively,
        /// q or Q are for black and white queens respectively,
        /// k or K are for black and white kings respectively,
        /// p or P are for black and white pawns respectively.
        /// In this board, alsa:
        /// w for undefined color king
        /// v for undefined color queen
        /// a for undefined color bishop
        /// c for undefined color knight
        /// t for undefined color rook
        /// s for undefined color pawn
        /// </remarks>
        public Bitmap GetTinyBoard(string board)
        {
            InitializeBoard();
            SetBoardExpression(board);
            return DrawTinyBoard();
        }
        /// <summary>
        /// Set the board state from a string representation.
        /// </summary>
        /// <param name="expr">
        /// String expression representing the board state.
        /// </param>
        private void SetBoardExpression(string expr)
        {
            SquareColor color = SquareColor.Indifferent;
            if (expr[0] == '(')
            {
                if (expr[2] == '.')
                {
                    if (expr[3] != '{')
                    {
                        color = SquareColor.Light;
                        expr = expr.Substring(2);
                    }
                    else
                    {
                        color = SquareColor.Dark;
                        int pos = expr.IndexOf('}');
                        expr = expr.Substring(pos + 1);
                    }
                }
                else
                {
                    color = SquareColor.Light;
                    expr = expr.Substring(2);
                }
                int end = expr.IndexOf(')');
                expr = expr.Substring(0, end);
            }
            SetSelection(color);
            int x = Selection.X;
            int y = Selection.Y;
            while (!string.IsNullOrEmpty(expr))
            {
                switch (expr[0])
                {
                    case '[':
                        expr = expr.Substring(1);
                        while (expr[0] != ']')
                        {
                            _position[x, y].Pieces |= GetPiece(expr[0]);
                            expr = expr.Substring(1);
                        }
                        x++;
                        if (x == 8)
                        {
                            x = Selection.X;
                            y++;
                        }
                        break;
                    case '.':
                        int n = 1;
                        if ((expr.Length > 1) && (expr[1] == '{'))
                        {
                            expr = expr.Substring(2);
                            n = int.Parse(expr.Substring(0, expr.IndexOf('}')));
                            expr = expr.Substring(expr.IndexOf('}'));
                        }
                        while (n > 0)
                        {
                            x++;
                            if (x == 8)
                            {
                                x = 0;
                                y++;
                            }
                            n--;
                        }
                        break;
                    default:
                        _position[x, y].Pieces |= GetPiece(expr[0]);
                        x++;
                        if (x == 8)
                        {
                            x = Selection.X;
                            y++;
                        }
                        break;
                }
                if (expr.Length > 1)
                {
                    expr = expr.Substring(1);
                }
                else
                {
                    expr = "";
                }
            }
        }
        /// <summary>
        /// Initialize the chess board with default values.
        /// </summary>
        private void InitializeBoard()
        {
            for (int f = 0; f < 8; f++)
            {
                for (int r = 0; r < 8; r++)
                {
                    _position[r, f] = new Square()
                    {
                        Color = SquareColor.Indifferent,
                        Pieces = PiecesSet.NoMatter,
                        Selected = _rSelection.Contains(new Point(r, f))
                    };
                    _position[r, f].SquareChanged += new EventHandler(SquareChanged);
                }
            }
        }
        /// <summary>
        /// Clear the current selection on the board.
        /// </summary>
        private void ClearSelection()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int f = 0; f < 8; f++)
                {
                    _position[r, f].Selected = false;
                }
            }
        }
        /// <summary>
        /// Set the square colors in the selection rectangle using the specified color as a reference for the upper left corner.
        /// </summary>
        /// <param name="color">
        /// Upper left corner color to use for the selection.
        /// </param>
        private void SetSelection(SquareColor color)
        {
            if (_rSelection == new Rectangle(0, 0, 8, 8))
            {
                color = SquareColor.Light;
            }
            for (int r = Selection.X; r < Selection.Right; r++)
            {
                for (int f = Selection.Y; f < Selection.Bottom; f++)
                {
                    switch (color)
                    {
                        case SquareColor.Indifferent:
                            _position[r, f].Color = color;
                            break;
                        case SquareColor.Dark:
                            if ((((f - Selection.Y) ^ (r - Selection.X)) & 1) == 0)
                            {
                                _position[r, f].Color = color;
                            }
                            else
                            {
                                _position[r, f].Color = SquareColor.Light;
                            }
                            break;
                        case SquareColor.Light:
                            if ((((f - Selection.Y) ^ (r - Selection.X)) & 1) == 0)
                            {
                                _position[r, f].Color = color;
                            }
                            else
                            {
                                _position[r, f].Color = SquareColor.Dark;
                            }
                            break;
                    }
                    _position[r, f].Selected = true;
                }
            }
        }
        /// <summary>
        /// Get a PiecesSet flag from a character representing a piece.
        /// </summary>
        /// <param name="p">
        /// Piece character.
        /// </param>
        /// <returns>
        /// PiecesSet flag representing the piece.
        /// </returns>
        private PiecesSet GetPiece(char p)
        {
            switch (p)
            {
                case 'p':
                    return PiecesSet.BlackPawn;
                case 'P':
                    return PiecesSet.WhitePawn;
                case 'b':
                    return PiecesSet.BlackBishop;
                case 'B':
                    return PiecesSet.WhiteBishop;
                case 'n':
                    return PiecesSet.BlackKnight;
                case 'N':
                    return PiecesSet.WhiteKnight;
                case 'r':
                    return PiecesSet.BlackRook;
                case 'R':
                    return PiecesSet.WhiteRook;
                case 'q':
                    return PiecesSet.BlackQueen;
                case 'Q':
                    return PiecesSet.WhiteQueen;
                case 'k':
                    return PiecesSet.BlackKing;
                case 'K':
                    return PiecesSet.WhiteKing;
                case '0':
                    return PiecesSet.Empty;
            }
            return PiecesSet.NoMatter;
        }
        /// <summary>
        /// Draw the chess board with the current position and selection.
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
                        if (_position[rank, file].Selected)
                        {
                            switch (_position[rank, file].Color)
                            {
                                case SquareColor.Dark:
                                    gr.FillRectangle(Brushes.Gray, 20 + rank * 60, 20 + file * 60, 60, 60);
                                    break;
                                case SquareColor.Light:
                                    gr.FillRectangle(Brushes.LightGray, 20 + rank * 60, 20 + file * 60, 60, 60);
                                    break;
                                case SquareColor.Indifferent:
                                    gr.FillRectangle(Brushes.SaddleBrown, 20 + rank * 60, 20 + file * 60, 60, 60);
                                    gr.DrawRectangle(Pens.Black, new Rectangle(20 + rank * 60, 20 + file * 60, 60, 60));
                                    break;
                            }
                        }
                        else
                        {
                            gr.FillRectangle(Brushes.SaddleBrown, 20 + rank * 60, 20 + file * 60, 60, 60);
                            gr.DrawRectangle(Pens.Black, new Rectangle(20 + rank * 60, 20 + file * 60, 60, 60));
                        }
                        gr.FillRectangle(Brushes.White, 22 + rank * 60, 22 + file * 60, 8, 8);
                        gr.DrawRectangle(Pens.Black, new Rectangle(22 + rank * 60, 22 + file * 60, 8, 8));
                        if (_position[rank, file].Selected)
                        {
                            using (Pen p = new Pen(Brushes.Black, 2))
                            {
                                gr.DrawLine(p, 22 + rank * 60, 22 + file * 60, 30 + rank * 60, 30 + file * 60);
                                gr.DrawLine(p, 22 + rank * 60, 30 + file * 60, 30 + rank * 60, 22 + file * 60);
                            }
                        }
                        int cpz = _position[rank, file].Count;
                        if (cpz > 0)
                        {
                            if (cpz == 1)
                            {
                                foreach (Bitmap bmp in _position[rank, file].Images)
                                {
                                    gr.DrawImage(bmp, new Rectangle(20 + rank * 60, 20 + file * 60, 60, 60));
                                }
                            }
                            else
                            {
                                int x = 2;
                                int y = 0;
                                foreach (Bitmap bmp in _position[rank, file].Images)
                                {
                                    gr.DrawImage(bmp, new Rectangle(20 + rank * 60 + (x * bmp.Width), 20 + file * 60 + (y * bmp.Height), 20, 20));
                                    x++;
                                    if (x >= 3)
                                    {
                                        x = 0;
                                        y++;
                                    }
                                }
                            }
                        }
                    }
                }
                if (_xyMouse.X >= 0)
                {
                    using (Pen p = new Pen(Color.Black, 1))
                    {
                        p.DashStyle = DashStyle.Dot;
                        gr.DrawRectangle(p, new Rectangle(_xyMouse, new Size(_xySel.X - _xyMouse.X, _xySel.Y - _xyMouse.Y)));
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
        /// Draw a tiny board image based on the current selection.
        /// </summary>
        /// <returns></returns>
        private Bitmap DrawTinyBoard()
        {
            bool simple = true;
            for (int file = Selection.Y; file < Selection.Bottom; file++)
            {
                for (int rank = Selection.X; rank < Selection.Right; rank++)
                {
                    if (_position[rank, file].Count > 1)
                    {
                        simple = false;
                        break;
                    }
                }
            }
            if (simple)
            {
                return DrawSimpleBoard();
            }
            else
            {
                return DrawComplexBoard();
            }
        }
        /// <summary>
        /// Draw a complex board image (regular expression) based on the current selection.
        /// </summary>
        /// <returns></returns>
        private Bitmap DrawComplexBoard()
        {
            Bitmap board = new Bitmap(2 + Selection.Width * 60, 2 + Selection.Height * 60);
            Graphics gr = Graphics.FromImage(board);
            try
            {
                gr.FillRectangle(Brushes.Black, 0, 0, board.Width, board.Height);
                for (int file = Selection.Y; file < Selection.Bottom; file++)
                {
                    int f = file - Selection.Y;
                    for (int rank = Selection.X; rank < Selection.Right; rank++)
                    {
                        int r = rank - Selection.X;
                        switch (_position[rank, file].Color)
                        {
                            case SquareColor.Dark:
                                gr.FillRectangle(Brushes.Gray, 1 + r * 60, 1 + f * 60, 60, 60);
                                break;
                            case SquareColor.Light:
                                gr.FillRectangle(Brushes.LightGray, 1 + r * 60, 1 + f * 60, 60, 60);
                                break;
                            case SquareColor.Indifferent:
                                gr.FillRectangle(Brushes.SaddleBrown, 1 + r * 60, 1 + f * 60, 60, 60);
                                gr.DrawRectangle(Pens.Black, new Rectangle(1 + r * 60, 1 + f * 60, 60, 60));
                                break;
                        }
                        int cpz = _position[rank, file].Count;
                        if (cpz > 0)
                        {
                            if (cpz == 1)
                            {
                                foreach (Bitmap bmp in _position[rank, file].Images)
                                {
                                    gr.DrawImage(bmp, new Rectangle(1 + r * 60, 1 + f * 60, 60, 60));
                                }
                            }
                            else
                            {
                                int x = 2;
                                int y = 0;
                                foreach (Bitmap bmp in _position[rank, file].Images)
                                {
                                    gr.DrawImage(bmp, new Rectangle(1 + r * 60 + (x * bmp.Width), 1 + f * 60 + (y * bmp.Height), 20, 20));
                                    x++;
                                    if (x >= 3)
                                    {
                                        x = 0;
                                        y++;
                                    }
                                }
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
        /// <summary>
        /// Draw a simple board image based on the current selection. No undefined pieces color or multiple pieces are allowed.
        /// </summary>
        /// <returns></returns>
        private Bitmap DrawSimpleBoard()
        {
            Bitmap board = new Bitmap(2 + Selection.Width * 20, 2 + Selection.Height * 20);
            Graphics gr = Graphics.FromImage(board);
            try
            {
                gr.FillRectangle(Brushes.Black, 0, 0, board.Width, board.Height);
                for (int file = Selection.Y; file < Selection.Bottom; file++)
                {
                    int f = file - Selection.Y;
                    for (int rank = Selection.X; rank < Selection.Right; rank++)
                    {
                        int r = rank - Selection.X;
                        switch (_position[rank, file].Color)
                        {
                            case SquareColor.Dark:
                                gr.FillRectangle(Brushes.Gray, 1 + r * 20, 1 + f * 20, 20, 20);
                                break;
                            case SquareColor.Light:
                                gr.FillRectangle(Brushes.LightGray, 1 + r * 20, 1 + f * 20, 20, 20);
                                break;
                            case SquareColor.Indifferent:
                                gr.FillRectangle(Brushes.SaddleBrown, 1 + r * 20, 1 + f * 20, 20, 20);
                                gr.DrawRectangle(Pens.Black, new Rectangle(1 + r * 20, 1 + f * 20, 20, 20));
                                break;
                        }
                        int cpz = _position[rank, file].Count;
                        if (cpz > 0)
                        {
                            if (cpz == 1)
                            {
                                gr.DrawImage(_position[rank, file].TinyImage, new Rectangle(1 + r * 20, 1 + f * 20, 20, 20));
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
        private void ClearAll(object sender, EventArgs e)
        {
            for (int x = Selection.X; x < Selection.Right; x++)
            {
                for (int y = Selection.Y; y < Selection.Bottom; y++)
                {
                    if (_position[x, y].Pieces == PiecesSet.Empty)
                    {
                        _position[x, y].Pieces = PiecesSet.NoMatter;
                    }
                }
            }
            DrawBoard();
        }
        private void EmptyAll(object sender, EventArgs e)
        {
            for (int x = Selection.X; x < Selection.Right; x++)
            {
                for (int y = Selection.Y; y < Selection.Bottom; y++)
                {
                    if (_position[x, y].Pieces == PiecesSet.NoMatter)
                    {
                        _position[x, y].Pieces = PiecesSet.Empty;
                    }
                }
            }
            DrawBoard();
        }

        private void pbBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (new Rectangle(20, 20, 480, 480).Contains(e.Location))
                {
                    _xyMouse = new Point(20 + 60 * ((e.Location.X - 20) / 60), 20 + 60 * ((e.Location.Y - 20) / 60));
                    _xySel = _xyMouse;
                    if (!string.IsNullOrEmpty(_iDrag))
                    {
                        int r = (e.X - 20) / 60;
                        int f = (e.Y - 20) / 60;
                        string pz = _iDrag;
                        _position[r, f].Pieces = _position[r, f].Pieces | Square.GetPieces(_iDrag);
                        //_iDrag = null;
                        //Cursor = Cursors.Default;
                    }
                    else
                    {
                        ClearSelection();
                    }
                    DrawBoard();
                }
                else
                {
                    _xyMouse = new Point(-1, -1);
                }
            }
            else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                if (new Rectangle(20, 20, 480, 480).Contains(e.Location))
                {
                    int r = (e.X - 20) / 60;
                    int f = (e.Y - 20) / 60;
                    _position[r, f].ShowMenu(squareMenu,
                        (Selection != new Rectangle(0, 0, 8, 8)) &&
                        (Selection != Rectangle.Empty) &&
                        (Selection.X == r) &&
                        (Selection.Y == f),
                        pbBoard, e.Location,
                        new EventHandler(ClearAll),
                        new EventHandler(EmptyAll));
                }
            }
        }

        private void pbBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (_xyMouse.X >= 0)
                {
                    if (new Rectangle(20, 20, 480, 480).Contains(e.Location))
                    {
                        Rectangle r = Selection;
                        SquareColor color = _position[Selection.X, Selection.Y].Color;
                        Selection = new Rectangle((_xyMouse.X - 20) / 60,
                            (_xyMouse.Y - 20) / 60,
                            Math.Min(8, 1 + ((e.X - _xyMouse.X - 20) / 60)),
                            Math.Min(8, 1 + ((e.Y - _xyMouse.Y - 20) / 60)));
                        _xySel = e.Location;
                        if (r.Size != Selection.Size)
                        {
                            ClearSelection();
                            SetSelection(color);
                            DrawBoard();
                        }
                    }
                    else
                    {
                        _xyMouse = new Point(-1, -1);
                    }
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

        private void pbBoard_MouseUp(object sender, MouseEventArgs e)
        {
            if (_xyMouse.X >= 0)
            {
                if (Selection == new Rectangle(0, 0, 8, 8))
                {
                    SetSelection(SquareColor.Light);
                }
                _xyMouse = new Point(-1, -1);
                //_iDrag = null;
                //Cursor = Cursors.Default;
                DrawBoard();
            }
        }

        private void pbBoard_MouseLeave(object sender, EventArgs e)
        {
            _iDrag = null;
            Cursor = Cursors.Default;
            if (_xyMouse.X >= 0)
            {
                _xyMouse = new Point(-1, -1);
                DrawBoard();
            }
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
                        case 12:
                            _iDrag = "qQ";
                            break;
                        case 13:
                            _iDrag = "kK";
                            break;
                        case 14:
                            _iDrag = "rR";
                            break;
                        case 15:
                            _iDrag = "nN";
                            break;
                        case 16:
                            _iDrag = "bB";
                            break;
                        case 17:
                            _iDrag = "pP";
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
                    case "pP":
                        Cursor = new Cursor(new MemoryStream(bwpawn));
                        break;
                    case "b":
                        Cursor = new Cursor(new MemoryStream(bbishop));
                        break;
                    case "B":
                        Cursor = new Cursor(new MemoryStream(wbishop));
                        break;
                    case "bB":
                        Cursor = new Cursor(new MemoryStream(bwbishop));
                        break;
                    case "n":
                        Cursor = new Cursor(new MemoryStream(bknight));
                        break;
                    case "N":
                        Cursor = new Cursor(new MemoryStream(wknight));
                        break;
                    case "nN":
                        Cursor = new Cursor(new MemoryStream(bwknight));
                        break;
                    case "r":
                        Cursor = new Cursor(new MemoryStream(brook));
                        break;
                    case "R":
                        Cursor = new Cursor(new MemoryStream(wrook));
                        break;
                    case "rR":
                        Cursor = new Cursor(new MemoryStream(bwrook));
                        break;
                    case "q":
                        Cursor = new Cursor(new MemoryStream(bqueen));
                        break;
                    case "Q":
                        Cursor = new Cursor(new MemoryStream(wqueen));
                        break;
                    case "qQ":
                        Cursor = new Cursor(new MemoryStream(bwqueen));
                        break;
                    case "k":
                        Cursor = new Cursor(new MemoryStream(bking));
                        break;
                    case "K":
                        Cursor = new Cursor(new MemoryStream(wking));
                        break;
                    case "kK":
                        Cursor = new Cursor(new MemoryStream(bwking));
                        break;
                }
            }
        }
        private void SquareChanged(object sender, EventArgs e)
        {
            if (sender is Square)
            {
                if ((sender as Square).ColorChanged)
                {
                    SetSelection((sender as Square).Color);
                }
                DrawBoard();
            }
        }
    }
}
