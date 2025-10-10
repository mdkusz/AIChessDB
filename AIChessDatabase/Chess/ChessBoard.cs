using AIChessDatabase.Data;
using AIChessDatabase.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.Chess
{
    /// <summary>
    /// Chess board implementation.
    /// </summary>
    /// <remarks>
    /// This class can validate and perform moves on a chess board.
    /// It can be used to create a chess game, validate moves, and manage the board state.
    /// </remarks>
    public class ChessBoard
    {
        private PieceBase[,] _board = new PieceBase[8, 8];

        public ChessBoard(string board)
        {
            if (board.Length != 64)
            {
                throw new Exception(ERR_BADBOARD);
            }
            Board = board;
        }
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
        public string Board
        {
            get
            {
                string board = "";
                for (int file = 7; file >= 0; file--)
                {
                    for (int rank = 0; rank < 8; rank++)
                    {
                        if (_board[file, rank] == null)
                        {
                            board += "0";
                        }
                        else
                        {
                            board += _board[file, rank].ToString();
                        }
                    }
                }
                return board;
            }
            private set
            {
                for (int file = 0; file < 8; file++)
                {
                    for (int rank = 0; rank < 8; rank++)
                    {
                        _board[file, rank] = CreatePiece(file, rank, value[rank + (7 - file) * 8]);
                    }
                }
            }
        }
        /// <summary>
        /// Move events for the current position.
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
        /// 2048 ('EVENT_WBISHOP1'): This event represents a light-square bishop involved in a move (the piece that moves, captures, or gives check).
        /// 4096 ('EVENT_KNIGHT1'): This event represents a knight involved in a move (the piece that moves, captures, or gives check).
        /// 8192 ('EVENT_ROOK1'): This event represents a rook involved in a move (the piece that moves, captures, or gives check).
        /// 16384 ('EVENT_QUEEN1'): This event represents a queen involved in a move (the piece that moves, captures, or gives check).
        /// 32768 ('EVENT_KING1'): This event represents a king involved in a move (the piece that moves or captures).
        /// 65536 ('EVENT_PAWN2'): This event represents a pawn involved in a move (the piece that is captured).
        /// 131072 ('EVENT_WBISHOP2'): This event represents a light-square bishop involved in a move (the piece that is captured).
        /// 262144 ('EVENT_KNIGHT2'): This event represents a knight involved in a move (the piece that is captured).
        /// 524288 ('EVENT_ROOK2'): This event represents a rook involved in a move (the piece that is captured).
        /// 1048576 ('EVENT_QUEEN2'): This event represents a queen involved in a move (the piece that is captured).
        /// 2097152 ('EVENT_MOVE'): This event represents a movement.
        /// 4194304 ('EVENT_BBISHOP1'): This event represents a dark-square bishop involved in a move (the piece that moves, captures, or gives check).
        /// 8388608 ('EVENT_BBISHOP2'): This event represents a dark-square bishop involved in a move (the piece that is captured).
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
        public ulong Events { get; private set; }
        /// <summary>
        /// Origin square for the current move.
        /// </summary>
        /// <remarks>
        /// Squares arepresented by an integer from 1 to 64, where 1 is a1 and 64 is h8.
        /// X is the square from which the piece is moved, and Y is the square to which the piece is moved.
        /// </remarks>
        public byte From { get; private set; }
        /// <summary>
        /// Destination square for the current move.
        /// </summary>
        /// <remarks>
        /// Squares arepresented by an integer from 0 to 63, where 0 is a1 and 63 is h8.
        /// X is the square from which the piece is moved, and Y is the square to which the piece is moved.
        /// </remarks>
        public byte To { get; private set; }
        /// <summary>
        /// Get the King piece for the specified color.
        /// </summary>
        /// <param name="color">
        /// Piece color: true for white, false for black.
        /// </param>
        /// <returns>
        /// King piece for the specified color.
        /// </returns>
        private King GetKing(bool color)
        {
            for (int file = 7; file >= 0; file--)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    if ((_board[file, rank] is King) && (_board[file, rank].Color == color))
                    {
                        return _board[file, rank] as King;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Make a ply (move) on the chess board.
        /// </summary>
        /// <param name="match">
        /// Match name or description, used for error reporting.
        /// </param>
        /// <param name="movenum">
        /// Ply number, used for error reporting.
        /// </param>
        /// <param name="move">
        /// Algebraic notation of the move to be made.
        /// </param>
        /// <param name="color">
        /// Color of the player making the move: true for white, false for black.
        /// </param>
        /// <exception cref="Exception">
        /// Raised when the move is invalid.
        /// </exception>
        public void Move(string match, int movenum, string move, bool color)
        {
            string origmove = move;
            string currentboard = Board;
            move = origmove;
            bool capture = false;
            Events = 0;
            int vdr = 0;
            int vdf = 0;
            List<PieceBase> pieces = null;
            if (move.StartsWith("O-O-O"))
            {
                // Queen castle
                Castle(currentboard, color, false, match, movenum, origmove);
                move = move.Length > 5 ? move.Substring(5) : "";
            }
            else if (move.StartsWith("O-O"))
            {
                // King castle
                Castle(currentboard, color, true, match, movenum, origmove);
                move = move.Length > 3 ? move.Substring(3) : "";
            }
            else
            {
                Regex reg = new Regex("^[NRBQK].*");
                if (reg.IsMatch(move))
                {
                    // Move piece, not pawn
                    pieces = FindPieces(move.Substring(0, 1), color);
                    move = move.Substring(1);
                    reg = new Regex("^[a-h][1-8].*");
                    if (!reg.IsMatch(move))
                    {
                        // Move needs disambiguation
                        reg = new Regex("^[a-h].*");
                        if (reg.IsMatch(move))
                        {
                            int pz = pieces.Count - 1;
                            int vr = char.ConvertToUtf32(move, 0) - 97;
                            while (pz >= 0)
                            {
                                if (pieces[pz].Rank != vr)
                                {
                                    pieces.RemoveAt(pz);
                                }
                                pz--;
                            }
                            move = move.Substring(1);
                        }
                        else
                        {
                            reg = new Regex("^[1-8].*");
                            if (reg.IsMatch(move))
                            {
                                int pz = pieces.Count - 1;
                                int vf = int.Parse(move.Substring(0, 1)) - 1;
                                while (pz >= 0)
                                {
                                    if (pieces[pz].File != vf)
                                    {
                                        pieces.RemoveAt(pz);
                                    }
                                    pz--;
                                }
                                move = move.Substring(1);
                            }
                        }
                        if (char.ToLower(move[0]) == 'x')
                        {
                            // Capture
                            capture = true;
                            move = move.Substring(1);
                        }
                    }
                }
                else
                {
                    pieces = FindPieces("", color);
                    reg = new Regex("^[a-h][1-8].*");
                    if (reg.IsMatch(move))
                    {
                        // Move pawn with or without capture
                        if (move.Length > 2)
                        {
                            if (char.ToLower(move[2]) == 'x')
                            {
                                // Capture
                                // Filter pawns
                                capture = true;
                                int pz = pieces.Count - 1;
                                int vr = char.ConvertToUtf32(move, 0) - 97;
                                int vf = int.Parse(move.Substring(0, 1)) - 1;
                                while (pz >= 0)
                                {
                                    if ((pieces[pz].Rank != vr) || (pieces[pz].File != vf))
                                    {
                                        pieces.RemoveAt(pz);
                                    }
                                    pz--;
                                }
                                move = move.Substring(3);
                            }
                        }
                    }
                    else
                    {
                        reg = new Regex("^[a-h].*");
                        if (reg.IsMatch(move))
                        {
                            // Move pawn with capture                            
                            if (char.ToLower(move[1]) == 'x')
                            {
                                // Capture
                                // Filter pawns
                                capture = true;
                                int pz = pieces.Count - 1;
                                int vr = char.ConvertToUtf32(move, 0) - 97;
                                while (pz >= 0)
                                {
                                    if (pieces[pz].Rank != vr)
                                    {
                                        pieces.RemoveAt(pz);
                                    }
                                    pz--;
                                }
                                move = move.Substring(2);
                            }
                            else
                            {
                                Board = currentboard;
                                throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE1, match, movenum, color ? "." : "...", origmove));
                            }
                        }
                        else
                        {
                            Board = currentboard;
                            throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE2, match, movenum, color ? "." : "...", origmove));
                        }
                    }
                }
                reg = new Regex("^[a-h][1-8].*");
                if (reg.IsMatch(move))
                {
                    // Destination square
                    vdr = char.ConvertToUtf32(move, 0) - 97;
                    vdf = int.Parse(move.Substring(1, 1)) - 1;
                    int pz = pieces.Count - 1;
                    while (pz >= 0)
                    {
                        if (!pieces[pz].CanMove(vdf, vdr))
                        {
                            pieces.RemoveAt(pz);
                        }
                        pz--;
                    }
                    if (pieces.Count > 1)
                    {
                        // Disambiguation, more than one piece can be moved
                        pz = pieces.Count - 1;
                        while (pz >= 0)
                        {
                            if (!pieces[pz].CanMove(vdf, vdr, currentboard))
                            {
                                pieces.RemoveAt(pz);
                            }
                            else
                            {
                                PieceBase pzm = pieces[pz];
                                int oldr = pzm.Rank;
                                int oldf = pzm.File;
                                bool cc = false;
                                if (pzm is Rook)
                                {
                                    cc = (pzm as Rook).CanCastle;
                                }
                                _board[oldf, oldr] = null;
                                PieceBase oldpz = _board[vdf, vdr];
                                pzm.MoveTo(vdf, vdr);
                                _board[vdf, vdr] = pzm;
                                if (GetKing(color).CheckType(Board, -1, -1) != 0)
                                {
                                    pieces.RemoveAt(pz);
                                }
                                pzm.Rank = oldr;
                                pzm.File = oldf;
                                if (pzm is Rook)
                                {
                                    (pzm as Rook).CanCastle = cc;
                                }
                                _board[oldf, oldr] = pzm;
                                _board[vdf, vdr] = oldpz;
                            }
                            pz--;
                        }
                    }
                    if (pieces.Count == 1)
                    {
                        Events |= pieces[0].Event1;
                        if (capture && (pieces[0] is Pawn))
                        {
                            // Test en passant
                            Pawn pwn = pieces[0] as Pawn;
                            pwn.CanMove(vdf, vdr, currentboard);
                            Events |= pwn.EventSpecial;
                            if (capture)
                            {
                                if (pwn.EventSpecial == 0)
                                {
                                    if (_board[vdf, vdr] == null)
                                    {
                                        Board = currentboard;
                                        throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE3, match, movenum, color ? "." : "...", origmove));
                                    }
                                    Events |= _board[vdf, vdr].Event2 | (uint)MatchEvent.Event.Capture;
                                }
                                else
                                {
                                    Events |= _board[pwn.FileEP, pwn.RankEP].Event2 | (uint)MatchEvent.Event.Capture;
                                    _board[pwn.FileEP, pwn.RankEP] = null;
                                }
                            }
                            else
                            {
                                Events |= (uint)MatchEvent.Event.Move;
                            }
                        }
                        else
                        {
                            if (capture)
                            {
                                if (_board[vdf, vdr] == null)
                                {
                                    Board = currentboard;
                                    throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE3, match, movenum, color ? "." : "...", origmove));
                                }
                                Events |= _board[vdf, vdr].Event2 | (uint)MatchEvent.Event.Capture;
                            }
                            else
                            {
                                Events |= (uint)MatchEvent.Event.Move;
                            }
                        }
                        if ((_board[vdf, vdr] != null) && ((_board[vdf, vdr].Color == color) || !capture))
                        {
                            Board = currentboard;
                            throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE4, match, movenum, color ? "." : "...", origmove));
                        }
                        if (!pieces[0].CanMove(vdf, vdr, currentboard))
                        {
                            Board = currentboard;
                            throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE14, match, movenum, color ? "." : "...", origmove));
                        }
                        _board[pieces[0].File, pieces[0].Rank] = null;
                        From = (byte)(pieces[0].Rank + (pieces[0].File * 8));
                        pieces[0].MoveTo(vdf, vdr);
                        _board[vdf, vdr] = pieces[0];
                        To = (byte)(vdr + (vdf * 8));
                        move = move.Length > 2 ? move.Substring(2) : "";
                        if (GetKing(color).CheckType(Board, -1, -1) != 0)
                        {
                            Board = currentboard;
                            throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE5, match, movenum, color ? "." : "...", origmove));
                        }
                    }
                    else if (pieces.Count > 1)
                    {
                        Board = currentboard;
                        throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE6, match, movenum, color ? "." : "...", origmove));
                    }
                    else if (pieces.Count < 1)
                    {
                        Board = currentboard;
                        throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE13, match, movenum, color ? "." : "...", origmove));
                    }
                }
                else
                {
                    Board = currentboard;
                    throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE7, match, movenum, color ? "." : "...", origmove));
                }
            }
            if (!string.IsNullOrEmpty(move))
            {
                if ("=QBNR".Contains(move[0]))
                {
                    // Pawn promoted
                    if ((pieces != null) && (pieces[0] is Pawn))
                    {
                        PieceBase nwpz = CreatePiece(pieces[0].File, pieces[0].Rank, pieces[0].Color ? char.ToUpper(move[move[0] == '=' ? 1 : 0]) : char.ToLower(move[move[0] == '=' ? 1 : 0]));
                        if ((nwpz is Pawn) || (nwpz is King))
                        {
                            throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE8, match, movenum, color ? "." : "...", origmove));
                        }
                        _board[nwpz.File, nwpz.Rank] = nwpz;
                        if (move[0] == '=')
                        {
                            move = move.Length > 2 ? move.Substring(2) : "";
                        }
                        else
                        {
                            move = move.Length > 1 ? move.Substring(1) : "";
                        }
                        Events |= (uint)MatchEvent.Event.PawnPromoted;
                        Events |= nwpz.Event3;
                    }
                    else
                    {
                        Board = currentboard;
                        throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE9, match, movenum, color ? "." : "...", origmove));
                    }
                }
            }
            ulong check = GetKing(!color).CheckType(Board, vdf, vdr);
            if (!string.IsNullOrEmpty(move))
            {
                // Check or Checkmate, en passant, etc.
                switch (move[0])
                {
                    case '+':
                        if (check == 0)
                        {
                            Board = currentboard;
                            throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE10, match, movenum, color ? "." : "...", origmove));
                        }
                        Events |= check;
                        break;
                    case '#':
                        if (check == 0)
                        {
                            Board = currentboard;
                            throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE11, match, movenum, color ? "." : "...", origmove));
                        }
                        Events |= (uint)MatchEvent.Event.Checkmate;
                        break;
                }
            }
            else if (check != 0)
            {
                // Accept SAN without '+'/'#' and record the actual state (check or mate). Check mate cannot be identified.
                Events |= check;
                // Or uncomment the following lines to enforce the use of '+'/'#' in SAN.
                /*
                Board = currentboard;
                throw new Exception(string.Format(ERR_BADMOVE1, LAB_BADMOVE12, match, movenum, color ? "." : "...", origmove));
                */
            }
        }
        /// <summary>
        /// Find pieces of a given kind and color on the board.
        /// </summary>
        /// <param name="letter">
        /// Letter representing the piece type:
        /// </param>
        /// <param name="color">
        /// Piece color: true for white, false for black.
        /// </param>
        /// <returns>
        /// List of pieces of the specified type and color found on the board.
        /// </returns>
        private List<PieceBase> FindPieces(string letter, bool color)
        {
            List<PieceBase> pieces = new List<PieceBase>();
            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    if ((_board[file, rank] != null) &&
                        (_board[file, rank].Color == color) &&
                        _board[file, rank].Is(letter))
                    {
                        pieces.Add(_board[file, rank]);
                    }
                }
            }
            return pieces;
        }
        /// <summary>
        /// Peform castling on the board.
        /// </summary>
        /// <param name="board">
        /// Board position as a string, used for validation.
        /// </param>
        /// <param name="color">
        /// Player color: true for white, false for black.
        /// </param>
        /// <param name="side">
        /// Castling side: true for kingside (O-O), false for queenside (O-O-O).
        /// </param>
        /// <param name="match">
        /// Match name or description, used for error reporting.
        /// </param>
        /// <param name="movenum">
        /// Ply number, used for error reporting.
        /// </param>
        /// <param name="move">
        /// Algebraic notation of the move to be made, used for error reporting.
        /// </param>
        /// <exception cref="Exception">
        /// Raised when castling is not possible due to the king or rook not being able to castle,
        /// </exception>
        private void Castle(string board, bool color, bool side, string match, int movenum, string move)
        {
            King king = FindPieces("K", color)[0] as King;
            if (!king.CanCastle(board, side))
            {
                throw new Exception(string.Format(UIResources.ERR_BADMOVE1, match, movenum, color ? "." : "...", move));
            }
            List<PieceBase> rooks = FindPieces("R", color);
            _board[king.File, king.Rank] = null;
            From = (byte)(king.Rank + (king.File * 8));
            int newrank = side ? 6 : 2;
            if (side)
            {
                // King side
                _board[king.File, 6] = new King(king.File, 6, color);
                if ((rooks[0].Rank == 7) && (rooks[0].File == (color ? 0 : 7)))
                {
                    if (!(rooks[0] as Rook).CanCastle)
                    {
                        throw new Exception(string.Format(UIResources.ERR_BADMOVE1, match, movenum, color ? "." : "...", move));
                    }
                    _board[rooks[0].File, rooks[0].Rank] = null;
                    rooks[0].MoveTo(rooks[0].File, 5);
                    _board[rooks[0].File, rooks[0].Rank] = rooks[0];
                }
                else
                {
                    if (!(rooks[1] as Rook).CanCastle)
                    {
                        throw new Exception(string.Format(UIResources.ERR_BADMOVE1, match, movenum, color ? "." : "...", move));
                    }
                    _board[rooks[1].File, rooks[1].Rank] = null;
                    rooks[1].MoveTo(rooks[1].File, 5);
                    _board[rooks[1].File, rooks[1].Rank] = rooks[1];
                }
                Events |= (uint)MatchEvent.Event.KingCastle;
            }
            else
            {
                // Queen side
                _board[king.File, 2] = new King(king.File, 2, color);
                if ((rooks[0].Rank == 0) && (rooks[0].File == (color ? 0 : 7)))
                {
                    if (!(rooks[0] as Rook).CanCastle)
                    {
                        throw new Exception(string.Format(UIResources.ERR_BADMOVE1, match, movenum, color ? "." : "...", move));
                    }
                    _board[rooks[0].File, rooks[0].Rank] = null;
                    rooks[0].MoveTo(rooks[0].File, 3);
                    _board[rooks[0].File, rooks[0].Rank] = rooks[0];
                }
                else
                {
                    if (!(rooks[1] as Rook).CanCastle)
                    {
                        throw new Exception(string.Format(UIResources.ERR_BADMOVE1, match, movenum, color ? "." : "...", move));
                    }
                    _board[rooks[1].File, rooks[1].Rank] = null;
                    rooks[1].MoveTo(rooks[1].File, 3);
                    _board[rooks[1].File, rooks[1].Rank] = rooks[1];
                }
                Events |= (uint)MatchEvent.Event.QueenCastle;
            }
            To = (byte)(newrank + (king.File * 8));
        }
        /// <summary>
        /// Create a piece based on its letter representation and put it on a file and rank.
        /// </summary>
        /// <param name="file">
        /// File (column) for the piece, from 0 to 7.
        /// </param>
        /// <param name="rank">
        /// Rank (row) for the piece, from 0 to 7.
        /// </param>
        /// <param name="letter">
        /// Piece letter representation. Uppercase for white pieces, lowercase for black pieces:
        /// </param>
        /// <returns>
        /// New piece instance or null if the letter is not recognized.
        /// </returns>
        private PieceBase CreatePiece(int file, int rank, char letter)
        {
            switch (letter)
            {
                case 'p':
                    return new Pawn(file, rank, false);
                case 'P':
                    return new Pawn(file, rank, true);
                case 'b':
                    return new Bishop(file, rank, false);
                case 'B':
                    return new Bishop(file, rank, true);
                case 'n':
                    return new Knight(file, rank, false);
                case 'N':
                    return new Knight(file, rank, true);
                case 'r':
                    return new Rook(file, rank, false);
                case 'R':
                    return new Rook(file, rank, true);
                case 'q':
                    return new Queen(file, rank, false);
                case 'Q':
                    return new Queen(file, rank, true);
                case 'k':
                    return new King(file, rank, false);
                case 'K':
                    return new King(file, rank, true);
                default:
                    return null;
            }
        }

    }
}
