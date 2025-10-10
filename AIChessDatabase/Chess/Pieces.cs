using AIChessDatabase.Data;
using AIChessDatabase.Properties;
using System;
using System.Linq;

namespace AIChessDatabase.Chess
{
    /// <summary>
    /// Base class for all chess pieces.
    /// </summary>
    [Serializable]
    public abstract class PieceBase
    {
        public PieceBase(int file, int rank, bool color)
        {
            if ((file < 0) || (file > 7) || (rank < 0) || (rank > 7))
            {
                throw new Exception(UIResources.ERR_BADPOSITION);
            }
            File = file;
            Rank = rank;
            Color = color;
        }
        /// <summary>
        /// Event flag for piece movement.
        /// </summary>
        public abstract ulong Event1 { get; }
        /// <summary>
        /// Event flag for piece captured.
        /// </summary>
        public abstract ulong Event2 { get; }
        /// <summary>
        /// Event flag for piece promotion.
        /// </summary>
        public virtual ulong Event3 { get { return 0; } }
        /// <summary>
        /// Special event flags.
        /// </summary>
        public virtual ulong EventSpecial { get { return 0; } }
        /// <summary>
        /// Piece color: true for white, false for black.
        /// </summary>
        public bool Color { get; protected set; }
        /// <summary>
        /// File where the piece is located (0-7).
        /// </summary>
        public int File { get; set; }
        /// <summary>
        /// Rank where the piece is located (0-7).
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// Check character name of the piece.
        /// </summary>
        /// <param name="letter">
        /// Letter to check, e.g. "P" for Pawn, "R" for Rook, etc.
        /// </param>
        /// <returns>
        /// True if piece matches the letter, false otherwise.
        /// </returns>
        public abstract bool Is(string letter);
        /// <summary>
        /// Get the piece character at the specified file and rank in the board string.
        /// </summary>
        /// <param name="file">
        /// Boad file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <param name="board">
        /// Board piece disposition string:
        /// This is a 64 character string representing the board position.
        /// 0: are for empty squares:
        /// b or B are for black and white bishops respectively,
        /// r or R are for black and white rooks respectively,
        /// n or N are for black and white knights respectively,
        /// q or Q are for black and white queens respectively,
        /// k or K are for black and white kings respectively,
        /// p or P are for black and white pawns respectively.
        /// </param>
        /// <returns>
        /// Character representing the piece at the specified position.
        /// </returns>
        public char GetPiece(int file, int rank, string board)
        {
            return board[rank + ((7 - file) * 8)];
        }
        /// <summary>
        /// Ask whether the piece can move to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <returns>
        /// True if the piece can move to the specified position, false otherwise.
        /// </returns>
        public virtual bool CanMove(int file, int rank)
        {
            return !((file < 0) || (file > 7) || (rank < 0) || (rank > 7));
        }
        /// <summary>
        /// Ask whether the piece can move to the specified file and rank in a given board.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <param name="board">
        /// Board piece disposition string.
        /// </param>
        /// <returns>
        /// True if the piece can move to the specified position, false otherwise.
        /// </returns>
        public virtual bool CanMove(int file, int rank, string board)
        {
            return CanMove(file, rank);
        }
        /// <summary>
        /// Move piece to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <exception cref="Exception">
        /// Raised if piece cannot be moved to the specified position.
        /// </exception>
        public virtual void MoveTo(int file, int rank)
        {
            if ((file < 0) || (file > 7) || (rank < 0) || (rank > 7))
            {
                throw new Exception(UIResources.ERR_BADPOSITION);
            }
        }
    }
    /// <summary>
    /// Piece implementation for Pawn.
    /// </summary>
    [Serializable]
    public class Pawn : PieceBase
    {
        private ulong _special = 0;
        public Pawn(int file, int rank, bool color)
            : base(file, rank, color)
        {
        }
        /// <summary>
        /// Match event flags for Pawn movement.
        /// </summary>
        public override ulong Event1
        {
            get
            {
                return (ulong)MatchEvent.Event.Pawn1;
            }
        }
        /// <summary>
        /// Match event flags for Pawn captured.
        /// </summary>
        public override ulong Event2
        {
            get
            {
                return (ulong)MatchEvent.Event.Pawn2;
            }
        }
        /// <summary>
        /// Match special event flags.
        /// </summary>
        public override ulong EventSpecial
        {
            get
            {
                return _special;
            }
        }
        /// <summary>
        /// Check whether the Pawn can move to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <returns>
        /// True if the Pawn can move to the specified position, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank)
        {
            if (base.CanMove(file, rank))
            {
                if (rank == Rank)
                {
                    if (Color)
                    {
                        if ((file - File) == 1)
                        {
                            return true;
                        }
                        else if ((file - File) == 2)
                        {
                            if (File == 1)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if ((File - file) == 1)
                        {
                            return true;
                        }
                        else if ((File - file) == 2)
                        {
                            if (File == 6)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    if ((Math.Abs(Rank - rank) == 1) && (Math.Abs(File - file) == 1))
                    {
                        if (Color)
                        {
                            if ((file - File) == 1)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if ((File - file) == 1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Check whether the Pawn can move to the specified file and rank in a given board.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <param name="board">
        /// Board piece disposition string.
        /// </param>
        /// <returns>
        /// True if the Pawn can move to the specified position in the given board, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank, string board)
        {
            if (CanMove(file, rank))
            {
                _special = 0;
                if (rank == Rank)
                {
                    if (Math.Abs(File - file) == 1)
                    {
                        return GetPiece(file, rank, board) == '0';
                    }
                    else if (Color)
                    {
                        return (GetPiece(file - 1, rank, board) == '0') && (GetPiece(file, rank, board) == '0');
                    }
                    else
                    {
                        return (GetPiece(file + 1, rank, board) == '0') && (GetPiece(file, rank, board) == '0');
                    }
                }
                else
                {
                    char pz = GetPiece(file, rank, board);
                    if (pz != '0')
                    {
                        return (Color && char.IsLower(pz)) || (!Color && char.IsUpper(pz));
                    }
                    else
                    {
                        // En passant?                        
                        if (Color)
                        {
                            if (File == 4)
                            {
                                pz = GetPiece(File, rank, board);
                                if (pz == 'p')
                                {
                                    _special = (ulong)MatchEvent.Event.EnPassant;
                                    FileEP = File;
                                    RankEP = rank;
                                    return true;
                                }
                            }
                            else if (File == 5)
                            {
                                pz = GetPiece(File - 1, rank, board);
                                if (pz == 'p')
                                {
                                    pz = GetPiece(File, rank, board);
                                    if (pz == '0')
                                    {
                                        _special = (ulong)MatchEvent.Event.EnPassant;
                                        FileEP = File - 1;
                                        RankEP = rank;
                                        return true;
                                    }
                                }
                                pz = GetPiece(File, rank, board);
                                if (pz == 'p')
                                {
                                    _special = (ulong)MatchEvent.Event.EnPassant;
                                    FileEP = File;
                                    RankEP = rank;
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (File == 3)
                            {
                                pz = GetPiece(File, rank, board);
                                if (pz == 'P')
                                {
                                    _special = (ulong)MatchEvent.Event.EnPassant;
                                    FileEP = File;
                                    RankEP = rank;
                                    return true;
                                }
                            }
                            else if (File == 2)
                            {
                                pz = GetPiece(File + 1, rank, board);
                                if (pz == 'P')
                                {
                                    pz = GetPiece(File, rank, board);
                                    if (pz == '0')
                                    {
                                        _special = (ulong)MatchEvent.Event.EnPassant;
                                        FileEP = File + 1;
                                        RankEP = rank;
                                        return true;
                                    }
                                }
                                pz = GetPiece(File, rank, board);
                                if (pz == 'P')
                                {
                                    _special = (ulong)MatchEvent.Event.EnPassant;
                                    FileEP = File;
                                    RankEP = rank;
                                    return true;
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Move the Pawn to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <exception cref="Exception">
        /// Raised if the Pawn cannot be moved to the specified position.
        /// </exception>
        public override void MoveTo(int file, int rank)
        {
            base.MoveTo(file, rank);
            if (CanMove(file, rank))
            {
                File = file;
                Rank = rank;
            }
            else
            {
                throw new Exception(UIResources.ERR_BADMOVE);
            }
        }
        /// <summary>
        /// Check whether the Pawn matches the specified letter.
        /// </summary>
        /// <param name="letter">
        /// Letter to check, e.g. "P" for Pawn.
        /// </param>
        /// <returns>
        /// True if the Pawn matches the letter, false otherwise.
        /// </returns>
        public override bool Is(string letter)
        {
            return string.IsNullOrEmpty(letter);
        }
        /// <summary>
        /// Rank to capture en passant.
        /// </summary>
        public int RankEP { get; private set; }
        /// <summary>
        /// File to capture en passant.
        /// </summary>
        public int FileEP { get; private set; }
        /// <summary>
        /// Pawn letter representation.
        /// </summary>
        /// <returns>
        /// Uppercase "P" for white Pawn, lowercase "p" for black Pawn.
        /// </returns>
        public override string ToString()
        {
            return Color ? "P" : "p";
        }
    }
    /// <summary>
    /// Bishop piece implementation.
    /// </summary>
    [Serializable]
    public class Bishop : PieceBase
    {
        public Bishop(int file, int rank, bool color)
            : base(file, rank, color)
        {
        }
        /// <summary>
        /// Event flags for Bishop movement.
        /// </summary>
        public override ulong Event1
        {
            get
            {
                return (((File ^ Rank) & 1) == 0) ? (ulong)MatchEvent.Event.LightBishop1 : (ulong)MatchEvent.Event.DarkBishop1;
            }
        }
        /// <summary>
        /// Match event flags for Bishop captured.
        /// </summary>
        public override ulong Event2
        {
            get
            {
                return (((File ^ Rank) & 1) == 0) ? (ulong)MatchEvent.Event.LightBishop2 : (ulong)MatchEvent.Event.DarkBishop2;
            }
        }
        /// <summary>
        /// Match event flags for Pawn promoted to Bishop.
        /// </summary>
        public override ulong Event3
        {
            get
            {
                return (ulong)MatchEvent.Event.Bishop3;
            }
        }
        /// <summary>
        /// Check whether the Bishop can move to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <returns>
        /// True if the Bishop can move to the specified position, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank)
        {
            return base.CanMove(file, rank) && ((Math.Abs(File - file) == Math.Abs(Rank - rank))
                && (Math.Abs(Rank - rank) != 0));
        }
        /// <summary>
        /// Check whether the Bishop can move to the specified file and rank in a given board.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <param name="board">
        /// Board piece disposition string.
        /// </param>
        /// <returns>
        /// True if the Bishop can move to the specified position in the given board, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank, string board)
        {
            if (CanMove(file, rank))
            {
                int pf = (file > File) ? 1 : -1;
                int pr = (rank > Rank) ? 1 : -1;
                int f = File;
                int r = Rank;
                char pz = '0';
                while ((f != file) && (r != rank))
                {
                    f += pf;
                    r += pr;
                    pz = GetPiece(f, r, board);
                    if ((f != file) && (r != rank))
                    {
                        if (pz != '0')
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return (pz == '0') || (Color && char.IsLower(pz)) || (!Color && char.IsUpper(pz));
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Move the Bishop to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <exception cref="Exception">
        /// Raised if the Bishop cannot be moved to the specified position.
        /// </exception>
        public override void MoveTo(int file, int rank)
        {
            base.MoveTo(file, rank);
            if (CanMove(file, rank))
            {
                File = file;
                Rank = rank;
                return;
            }
            throw new Exception(UIResources.ERR_BADMOVE);
        }
        /// <summary>
        /// Check whether the Bishop matches the specified letter.
        /// </summary>
        /// <param name="letter">
        /// Letter to check, e.g. "B" for Bishop.
        /// </param>
        /// <returns>
        /// True if the Bishop matches the letter, false otherwise.
        /// </returns>
        public override bool Is(string letter)
        {
            return letter == "B";
        }
        /// <summary>
        /// Returns a string representation of the object, indicating its state based on the color property.
        /// </summary>
        /// <returns>
        /// A string "B" if the bishop is white otherwise, "b".
        /// </returns>
        public override string ToString()
        {
            return Color ? "B" : "b";
        }
    }
    /// <summary>
    /// Rook piece implementation.
    /// </summary>
    [Serializable]
    public class Rook : PieceBase
    {
        public Rook(int file, int rank, bool color)
            : base(file, rank, color)
        {
            CanCastle = true;
        }
        /// <summary>
        /// Check if this Rook can castle.
        /// </summary>
        public bool CanCastle { get; set; }
        /// <summary>
        /// Match event flags for Rook movement.
        /// </summary>
        public override ulong Event1
        {
            get
            {
                return (ulong)MatchEvent.Event.Rook1;
            }
        }
        /// <summary>
        /// Match event flags for Rook captured.
        /// </summary>
        public override ulong Event2
        {
            get
            {
                return (ulong)MatchEvent.Event.Rook2;
            }
        }
        /// <summary>
        /// Match event flags for Pawn promoted to Rook.
        /// </summary>
        public override ulong Event3
        {
            get
            {
                return (ulong)MatchEvent.Event.Rook3;
            }
        }
        /// <summary>
        /// Check whether the Rook can move to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <returns>
        /// True if the Rook can move to the specified position, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank)
        {
            return base.CanMove(file, rank) && ((File == file) || (Rank == rank))
                && ((File != file) || (Rank != rank));
        }
        /// <summary>
        /// Check whether the Rook can move to the specified file and rank in a given board.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <param name="board">
        /// Board piece disposition string.
        /// </param>
        /// <returns>
        /// True if the Rook can move to the specified position in the given board, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank, string board)
        {
            if (CanMove(file, rank))
            {
                int pf = (file == File) ? 0 : ((file > File) ? 1 : -1);
                int pr = (rank == Rank) ? 0 : ((rank > Rank) ? 1 : -1);
                int f = File;
                int r = Rank;
                char pz = '0';
                while ((f != file) || (r != rank))
                {
                    f += pf;
                    r += pr;
                    pz = GetPiece(f, r, board);
                    if ((f != file) || (r != rank))
                    {
                        if (pz != '0')
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return (pz == '0') || (Color && char.IsLower(pz)) || (!Color && char.IsUpper(pz));
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Move the Rook to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <exception cref="Exception">
        /// Raised if the Rook cannot be moved to the specified position.
        /// </exception>
        public override void MoveTo(int file, int rank)
        {
            base.MoveTo(file, rank);
            if (CanMove(file, rank))
            {
                File = file;
                Rank = rank;
                CanCastle = false;
                return;
            }
            throw new Exception(UIResources.ERR_BADMOVE);
        }
        /// <summary>
        /// Check whether the Rook matches the specified letter.
        /// </summary>
        /// <param name="letter">
        /// Letter to check, e.g. "R" for Rook.
        /// </param>
        /// <returns>
        /// True if the Rook matches the letter, false otherwise.
        /// </returns>
        public override bool Is(string letter)
        {
            return letter == "R";
        }
        public override string ToString()
        {
            return Color ? "R" : "r";
        }
    }
    /// <summary>
    /// Knight piece implementation.
    /// </summary>
    [Serializable]
    public class Knight : PieceBase
    {
        public Knight(int file, int rank, bool color)
            : base(file, rank, color)
        {
        }
        /// <summary>
        /// Match event flags for Knight movement.
        /// </summary>
        public override ulong Event1
        {
            get
            {
                return (ulong)MatchEvent.Event.Knight1;
            }
        }
        /// <summary>
        /// Match event flags for Knight captured.
        /// </summary>
        public override ulong Event2
        {
            get
            {
                return (ulong)MatchEvent.Event.Knight2;
            }
        }
        /// <summary>
        /// Match event flags for Pawn promoted to Knight.
        /// </summary>
        public override ulong Event3
        {
            get
            {
                return (ulong)MatchEvent.Event.Knight3;
            }
        }
        /// <summary>
        /// Check whether the Knight can move to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <returns>
        /// True if the Knight can move to the specified position, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank)
        {
            return base.CanMove(file, rank)
                && (((Math.Abs(File - file) == 2) && (Math.Abs(Rank - rank) == 1))
                    || ((Math.Abs(File - file) == 1) && (Math.Abs(Rank - rank) == 2)));
        }
        /// <summary>
        /// Check whether the Knight can move to the specified file and rank in a given board.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <param name="board">
        /// Board piece disposition string.
        /// </param>
        /// <returns>
        /// True if the Knight can move to the specified position in the given board, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank, string board)
        {
            if (CanMove(file, rank))
            {
                char pz = GetPiece(file, rank, board);
                return (pz == '0') || (Color && char.IsLower(pz)) || (!Color && char.IsUpper(pz));
            }
            return false;
        }
        /// <summary>
        /// Move the Knight to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <exception cref="Exception">
        /// Raised if the Knight cannot be moved to the specified position.
        /// </exception>
        public override void MoveTo(int file, int rank)
        {
            base.MoveTo(file, rank);
            if (CanMove(file, rank))
            {
                File = file;
                Rank = rank;
                return;
            }
            throw new Exception(UIResources.ERR_BADMOVE);
        }
        /// <summary>
        /// Check whether the Knight matches the specified letter.
        /// </summary>
        /// <param name="letter">
        /// Letter to check, e.g. "N" for Knight.
        /// </param>
        /// <returns>
        /// True if the Knight matches the letter, false otherwise.
        /// </returns>
        public override bool Is(string letter)
        {
            return letter == "N";
        }
        public override string ToString()
        {
            return Color ? "N" : "n";
        }
    }
    /// <summary>
    /// Queen piece implementation.
    /// </summary>
    [Serializable]
    public class Queen : PieceBase
    {
        public Queen(int file, int rank, bool color)
            : base(file, rank, color)
        {
        }
        /// <summary>
        /// Match event flags for Queen movement.
        /// </summary>
        public override ulong Event1
        {
            get
            {
                return (ulong)MatchEvent.Event.Queen1;
            }
        }
        /// <summary>
        /// Match event flags for Queen captured.
        /// </summary>
        public override ulong Event2
        {
            get
            {
                return (ulong)MatchEvent.Event.Queen2;
            }
        }
        /// <summary>
        /// Match event flags for Pawn promoted to Queen.
        /// </summary>
        public override ulong Event3
        {
            get
            {
                return (ulong)MatchEvent.Event.Queen3;
            }
        }
        /// <summary>
        /// Check whether the Queen can move to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// Board file (0-7).
        /// </param>
        /// <param name="rank">
        /// Board rank (0-7).
        /// </param>
        /// <returns>
        /// True if the Queen can move to the specified position, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank)
        {
            return base.CanMove(file, rank) && (((File == file) || (Rank == rank) ||
                (Math.Abs(File - file) == Math.Abs(Rank - rank))) &&
                ((File != file) || (Rank != rank)));
        }
        /// <summary>
        /// Check whether the Queen can move to the specified file and rank in a given board.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <param name="board">
        /// Board piece disposition string.
        /// </param>
        /// <returns>
        /// True if the Queen can move to the specified position in the given board, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank, string board)
        {
            if (CanMove(file, rank))
            {
                int pf = (file == File) ? 0 : ((file > File) ? 1 : -1);
                int pr = (rank == Rank) ? 0 : ((rank > Rank) ? 1 : -1);
                int f = File;
                int r = Rank;
                char pz = '0';
                while ((f != file) || (r != rank))
                {
                    f += pf;
                    r += pr;
                    pz = GetPiece(f, r, board);
                    if ((f != file) || (r != rank))
                    {
                        if (pz != '0')
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return (pz == '0') || (Color && char.IsLower(pz)) || (!Color && char.IsUpper(pz));
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Move the Queen to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <exception cref="Exception">
        /// Raised if the Queen cannot be moved to the specified position.
        /// </exception>
        public override void MoveTo(int file, int rank)
        {
            base.MoveTo(file, rank);
            if (CanMove(file, rank))
            {
                File = file;
                Rank = rank;
                return;
            }
            throw new Exception(UIResources.ERR_BADMOVE);
        }
        /// <summary>
        /// Check whether the Queen matches the specified letter.
        /// </summary>
        /// <param name="letter">
        /// Letter to check, e.g. "Q" for Queen.
        /// </param>
        /// <returns>
        /// True if the Queen matches the letter, false otherwise.
        /// </returns>
        public override bool Is(string letter)
        {
            return letter == "Q";
        }
        public override string ToString()
        {
            return Color ? "Q" : "q";
        }
    }
    /// <summary>
    /// King piece implementation.
    /// </summary>
    [Serializable]
    public class King : PieceBase
    {
        private bool _canCastle = true;
        public King(int file, int rank, bool color)
            : base(file, rank, color)
        {

        }
        /// <summary>
        /// Match event flags for King movement.
        /// </summary>
        public override ulong Event1
        {
            get
            {
                return (ulong)MatchEvent.Event.King1;
            }
        }
        /// <summary>
        /// Match event flags for King captured.
        /// </summary>
        public override ulong Event2
        {
            get
            {
                return (ulong)MatchEvent.Event.King1;
            }
        }
        /// <summary>
        /// Check whether the King can move.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <returns>
        /// True if the King can move to the specified position, false otherwise.
        /// </returns>
        public override bool CanMove(int file, int rank)
        {
            return base.CanMove(file, rank) && (((File == file) || (Rank == rank) ||
                (Math.Abs(File - file) == Math.Abs(Rank - rank))) &&
                ((File != file) || (Rank != rank)) &&
                ((Math.Abs(File - file) == 1) || (Math.Abs(Rank - rank) == 1)));
        }
        /// <summary>
        /// Move the King to the specified file and rank.
        /// </summary>
        /// <param name="file">
        /// File to move to (0-7).
        /// </param>
        /// <param name="rank">
        /// Rank to move to (0-7).
        /// </param>
        /// <exception cref="Exception">
        /// Raised if the King cannot be moved to the specified position.
        /// </exception>
        public override void MoveTo(int file, int rank)
        {
            base.MoveTo(file, rank);
            if (CanMove(file, rank))
            {
                File = file;
                Rank = rank;
                _canCastle = false;
                return;
            }
            throw new Exception(UIResources.ERR_BADMOVE);
        }
        /// <summary>
        /// Check if the King matches the specified letter.
        /// </summary>
        /// <param name="letter">
        /// Letter to check, e.g. "K" for King.
        /// </param>
        /// <returns>
        /// True if the King matches the letter, false otherwise.
        /// </returns>
        public override bool Is(string letter)
        {
            return letter == "K";
        }
        /// <summary>
        /// Match event flag for the check type
        /// </summary>
        /// <param name="board">
        /// Current board piece disposition string.
        /// </param>
        /// <param name="fmove">
        /// File to check (0-7).
        /// </param>
        /// <param name="rmove">
        /// Rank to check (0-7).
        /// </param>
        /// <returns>
        /// Match event flags indicating the type of check.
        /// </returns>
        public ulong CheckType(string board, int fmove, int rmove)
        {
            ulong checkev = 0;
            int chkc = 0;
            bool pzmove = false;
            char pz = '0';
            string straight = Color ? "rq" : "RQ";
            string diag = Color ? "bq" : "BQ";
            char pawn = Color ? 'p' : 'P';
            char knight = Color ? 'n' : 'N';
            for (int r = Rank - 1; r >= 0; r--)
            {
                pz = GetPiece(File, r, board);
                if (straight.Contains(pz))
                {
                    chkc++;
                    if ((File == fmove) && (r == rmove))
                    {
                        pzmove = true;
                    }
                    break;
                }
                else if (pz != '0')
                {
                    break;
                }
            }
            for (int r = Rank + 1; r < 8; r++)
            {
                pz = GetPiece(File, r, board);
                if (straight.Contains(pz))
                {
                    chkc++;
                    if ((File == fmove) && (r == rmove))
                    {
                        pzmove = true;
                    }
                    break;
                }
                else if (pz != '0')
                {
                    break;
                }
            }
            for (int f = File - 1; f >= 0; f--)
            {
                pz = GetPiece(f, Rank, board);
                if (straight.Contains(pz))
                {
                    chkc++;
                    if ((f == fmove) && (Rank == rmove))
                    {
                        pzmove = true;
                    }
                    break;
                }
                else if (pz != '0')
                {
                    break;
                }
            }
            for (int f = File + 1; f < 8; f++)
            {
                pz = GetPiece(f, Rank, board);
                if (straight.Contains(pz))
                {
                    chkc++;
                    if ((f == fmove) && (Rank == rmove))
                    {
                        pzmove = true;
                    }
                    break;
                }
                else if (pz != '0')
                {
                    break;
                }
            }
            if (Color)
            {
                if (File < 7)
                {
                    if ((Rank > 0) && (GetPiece(File + 1, Rank - 1, board) == pawn))
                    {
                        chkc++;
                        if (((File + 1) == fmove) && ((Rank - 1) == rmove))
                        {
                            pzmove = true;
                        }
                    }
                    if ((Rank < 7) && (GetPiece(File + 1, Rank + 1, board) == pawn))
                    {
                        chkc++;
                        if (((File + 1) == fmove) && ((Rank + 1) == rmove))
                        {
                            pzmove = true;
                        }
                    }
                }
            }
            else
            {
                if (File > 0)
                {
                    if ((Rank > 0) && (GetPiece(File - 1, Rank - 1, board) == pawn))
                    {
                        chkc++;
                        if (((File - 1) == fmove) && ((Rank - 1) == rmove))
                        {
                            pzmove = true;
                        }
                    }
                    if ((Rank < 7) && (GetPiece(File - 1, Rank + 1, board) == pawn))
                    {
                        chkc++;
                        if (((File - 1) == fmove) && ((Rank + 1) == rmove))
                        {
                            pzmove = true;
                        }
                    }
                }
            }
            if (File > 0)
            {
                int f = File - 1;
                for (int r = Rank - 1; (r >= 0) && (f >= 0); r--)
                {
                    pz = GetPiece(f, r, board);
                    if (diag.Contains(pz))
                    {
                        chkc++;
                        if ((f == fmove) && (r == rmove))
                        {
                            pzmove = true;
                        }
                        break;
                    }
                    else if (pz != '0')
                    {
                        break;
                    }
                    f--;
                }
                f = File - 1;
                for (int r = Rank + 1; (r < 8) && (f >= 0); r++)
                {
                    pz = GetPiece(f, r, board);
                    if (diag.Contains(pz))
                    {
                        chkc++;
                        if ((f == fmove) && (r == rmove))
                        {
                            pzmove = true;
                        }
                        break;
                    }
                    else if (pz != '0')
                    {
                        break;
                    }
                    f--;
                }
            }
            if (File < 7)
            {
                int f = File + 1;
                for (int r = Rank - 1; (r >= 0) && (f < 8); r--)
                {
                    pz = GetPiece(f, r, board);
                    if (diag.Contains(pz))
                    {
                        chkc++;
                        if ((f == fmove) && (r == rmove))
                        {
                            pzmove = true;
                        }
                        break;
                    }
                    else if (pz != '0')
                    {
                        break;
                    }
                    f++;
                }
                f = File + 1;
                for (int r = Rank + 1; (r < 8) && (f < 8); r++)
                {
                    pz = GetPiece(f, r, board);
                    if (diag.Contains(pz))
                    {
                        chkc++;
                        if ((f == fmove) && (r == rmove))
                        {
                            pzmove = true;
                        }
                        break;
                    }
                    else if (pz != '0')
                    {
                        break;
                    }
                    f++;
                }
            }
            if ((File > 1) && (Rank > 0))
            {
                if (GetPiece(File - 2, Rank - 1, board) == knight)
                {
                    chkc++;
                    if (((File - 2) == fmove) && ((Rank - 1) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if ((File > 1) && (Rank < 7))
            {
                if (GetPiece(File - 2, Rank + 1, board) == knight)
                {
                    chkc++;
                    if (((File - 2) == fmove) && ((Rank + 1) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if ((File > 0) && (Rank < 6))
            {
                if (GetPiece(File - 1, Rank + 2, board) == knight)
                {
                    chkc++;
                    if (((File - 1) == fmove) && ((Rank + 2) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if ((File < 7) && (Rank < 6))
            {
                if (GetPiece(File + 1, Rank + 2, board) == knight)
                {
                    chkc++;
                    if (((File + 1) == fmove) && ((Rank + 2) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if ((File < 6) && (Rank < 7))
            {
                if (GetPiece(File + 2, Rank + 1, board) == knight)
                {
                    chkc++;
                    if (((File + 2) == fmove) && ((Rank + 1) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if ((File < 6) && (Rank > 0))
            {
                if (GetPiece(File + 2, Rank - 1, board) == knight)
                {
                    chkc++;
                    if (((File + 2) == fmove) && ((Rank - 1) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if ((File < 7) && (Rank > 1))
            {
                if (GetPiece(File + 1, Rank - 2, board) == knight)
                {
                    chkc++;
                    if (((File + 1) == fmove) && ((Rank - 2) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if ((File > 0) && (Rank > 1))
            {
                if (GetPiece(File - 1, Rank - 2, board) == knight)
                {
                    chkc++;
                    if (((File - 1) == fmove) && ((Rank - 2) == rmove))
                    {
                        pzmove = true;
                    }
                }
            }
            if (chkc > 0)
            {
                checkev = (ulong)MatchEvent.Event.Check;
                if (chkc > 1)
                {
                    checkev |= (ulong)(MatchEvent.Event.MultipleCheck | MatchEvent.Event.DiscovedCheck);
                }
                else
                {
                    if (!pzmove)
                    {
                        checkev |= (ulong)MatchEvent.Event.DiscovedCheck;
                    }
                }
            }
            return checkev;
        }
        /// <summary>
        /// Check whether the King can castle.
        /// </summary>
        /// <param name="board">
        /// Board piece disposition string.
        /// </param>
        /// <param name="side">
        /// Castling side, true for King side (right), false for Queen side (left).
        /// </param>
        /// <returns>
        /// True if the King can castle on the specified side, false otherwise.
        /// </returns>
        public bool CanCastle(string board, bool side)
        {
            if (_canCastle)
            {
                if (side)
                {
                    // King side
                    if ((GetPiece(File, Rank + 1, board) == '0') &&
                        (GetPiece(File, Rank + 2, board) == '0'))
                    {
                        int oldrank = Rank;
                        if (CheckType(board, -1, -1) == 0)
                        {
                            Rank++;
                            if (CheckType(board, -1, -1) == 0)
                            {
                                Rank++;
                                if (CheckType(board, -1, -1) == 0)
                                {
                                    Rank = oldrank;
                                    return true;
                                }
                            }
                        }
                        Rank = oldrank;
                    }
                }
                else
                {
                    // Queen side
                    if ((GetPiece(File, Rank - 1, board) == '0') &&
                        (GetPiece(File, Rank - 2, board) == '0') &&
                        (GetPiece(File, Rank - 3, board) == '0'))
                    {
                        int oldrank = Rank;
                        if (CheckType(board, -1, -1) == 0)
                        {
                            Rank--;
                            if (CheckType(board, -1, -1) == 0)
                            {
                                Rank--;
                                if (CheckType(board, -1, -1) == 0)
                                {
                                    Rank = oldrank;
                                    return true;
                                }
                            }
                        }
                        Rank = oldrank;
                    }
                }
            }
            return false;
        }
        public override string ToString()
        {
            return Color ? "K" : "k";
        }
    }
}
