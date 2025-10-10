using AIChessDatabase.Chess;
using AIChessDatabase.Data;
using AIChessDatabase.Interfaces;
using System;
using System.Collections.Generic;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.PGNParser
{
    /// <summary>
    /// Match representation in PGN format.
    /// </summary>
    public class PGNMatch
    {
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();
        private List<SANMove> _moves = new List<SANMove>();
        private int _plyCount = 0;
        private ChessBoard _board = null;

        public PGNMatch()
        {
        }
        /// <summary>
        /// Comments about the match.
        /// </summary>
        public string MatchComment { get; set; }
        /// <summary>
        /// Match result, e.g. "1-0", "0-1", "1/2-1/2", "*".
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// Match initial board position in custom format.
        /// </summary>
        /// <see cref="ChessBoard"/>
        public string InitialPosition { get; set; }
        /// <summary>
        /// Flag to indicate if the match is void (no moves).
        /// </summary>
        public bool Void { get; private set; }
        /// <summary>
        /// Maximum number of moves in the match.
        /// </summary>
        public int MoveCount
        {
            get
            {
                return _plyCount;
            }
        }
        /// <summary>
        /// Number of full moves in the match (one move for each player).
        /// </summary>
        public int FullMoveCount
        {
            get
            {
                return _moves.Count;
            }
        }
        /// <summary>
        /// Match moves in SAN format.
        /// </summary>
        public IEnumerable<SANMove> Moves
        {
            get
            {
                _moves.Sort();
                foreach (SANMove m in _moves)
                {
                    yield return m;
                }
            }
        }
        /// <summary>
        /// Match keyword count, which includes attributes like Event, Date, White, Black, etc.
        /// </summary>
        public int AttributeCount
        {
            get
            {
                return _attributes.Count;
            }
        }
        /// <summary>
        /// Match attributes and their values.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Attributes
        {
            get
            {
                foreach (KeyValuePair<string, string> kv in _attributes)
                {
                    yield return kv;
                }
            }
        }
        /// <summary>
        /// Find an attribute by its name.
        /// </summary>
        /// <param name="attr">
        /// Attribute name to search for.
        /// </param>
        /// <returns>
        /// Attribute value if found, otherwise an empty string.
        /// </returns>
        public string GetAttribute(string attr)
        {
            if (_attributes.ContainsKey(attr))
            {
                return _attributes[attr];
            }
            return "";
        }
        /// <summary>
        /// Parse a match from a PGN file.
        /// </summary>
        /// <param name="pgn">
        /// PGN file to parse the match from.
        /// </param>
        public void ParseMatch(PGNFile pgn)
        {
            _attributes.Clear();
            _moves.Clear();
            Void = false;
            InitialPosition = INITIAL_BOARD_STANDARD;
            Result = "";
            MatchComment = "";
            GetAttributes(pgn);
            if (pgn.EOF)
            {
                Void = true;
                return;
            }
            if (pgn.TestComment())
            {
                MatchComment = pgn.CurrentCommentString;
                pgn.Seek(MatchComment.Length);
                MatchComment = ProcessComment(MatchComment);
            }
            GetMoves(pgn);
        }
        /// <summary>
        /// Convert to a Match object for database storage.
        /// </summary>
        /// <param name="repository">
        /// Database repository to use for creating Match objects.
        /// </param>
        /// <returns>
        /// Match object with the parsed data from the PGN match.
        /// </returns>
        public Match ConvertToMatch(IObjectRepository repository)
        {
            Match match = repository.CreateObject(typeof(Match)) as Match;
            match.InitialPosition = repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
            match.InitialPosition.Order = 0;
            match.InitialPosition.Board = repository.CreateObject(typeof(Position)) as Position;
            match.InitialPosition.Board.Board = InitialPosition;
            match.ResultText = Result;
            match.Description = GetAttribute("Event");
            match.Date = GetAttribute("Date");
            match.White = GetAttribute("White");
            match.Black = GetAttribute("Black");

            foreach (string key in _attributes.Keys)
            {
                MatchKeyword kw = repository.CreateObject(typeof(MatchKeyword)) as MatchKeyword;
                kw.Name = key;
                kw.Value = _attributes[key];
                match.AddChild(kw);
            }
            int porder = 1;
            _board = new ChessBoard(InitialPosition);
            MatchPosition current = match.InitialPosition;
            _moves.Sort();
            foreach (SANMove mov in _moves)
            {
                MatchMove mm = repository.CreateObject(typeof(MatchMove)) as MatchMove;
                mm.MoveNumber = mov.MoveNum;
                mm.InitialPosition = current;
                if (mov.WhiteMove != "...")
                {
                    mm.Player = 0;
                    mm.ANText = mov.WhiteMove;
                    _board.Move(match.Description, mov.MoveNum, mov.WhiteMove, true);
                    mm.Events = _board.Events;
                    mm.From = _board.From;
                    mm.To = _board.To;
                    mm.FinalPosition = repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                    mm.Order = porder++;
                    mm.FinalPosition.Order = mm.Order;
                    mm.FinalPosition.Board = repository.CreateObject(typeof(Position)) as Position;
                    mm.FinalPosition.Board.Board = _board.Board;
                    mm.FinalPosition.Events = mm.Events;
                    current = mm.FinalPosition;
                    int cn = 0;
                    foreach (string c in mov.WhiteComments)
                    {
                        MoveComment com = repository.CreateObject(typeof(MoveComment)) as MoveComment;
                        com.Comment = c;
                        com.Order = cn++;
                        mm.AddChild(com);
                    }
                    match.AddChild(mm);
                    mm = repository.CreateObject(typeof(MatchMove)) as MatchMove;
                    mm.MoveNumber = mov.MoveNum;
                    mm.InitialPosition = current;
                }
                if (!string.IsNullOrEmpty(mov.BlackMove))
                {
                    mm.Player = 1;
                    mm.ANText = mov.BlackMove;
                    _board.Move(match.Description, mov.MoveNum, mov.BlackMove, false);
                    mm.Events = _board.Events;
                    mm.From = _board.From;
                    mm.To = _board.To;
                    mm.FinalPosition = repository.CreateObject(typeof(MatchPosition)) as MatchPosition;
                    mm.Order = porder++;
                    mm.FinalPosition.Order = mm.Order;
                    mm.FinalPosition.Board = repository.CreateObject(typeof(Position)) as Position;
                    mm.FinalPosition.Board.Board = _board.Board;
                    mm.FinalPosition.Events = mm.Events;
                    current = mm.FinalPosition;
                    int cn = 0;
                    foreach (string c in mov.BlackComments)
                    {
                        MoveComment com = repository.CreateObject(typeof(MoveComment)) as MoveComment;
                        com.Comment = c;
                        com.Order = cn++;
                        mm.AddChild(com);
                    }
                    match.AddChild(mm);
                }
            }
            return match;
        }
        /// <summary>
        /// Set the initial position of the match from a custom board format string.
        /// </summary>
        /// <param name="value">
        /// Board string in custom format.
        /// </param>
        /// <see cref="ChessBoard"/>
        private void SetInitialPosition(string value)
        {
            int i = 0;
            string board = "";
            while ((board.Length < 64) && (i < value.Length))
            {
                switch (char.ToUpper(value[i]))
                {
                    case 'B':
                    case 'R':
                    case 'N':
                    case 'Q':
                    case 'K':
                    case 'P':
                        board = board + value[i++];
                        break;
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                        board += new string('0', int.Parse(value[i++].ToString()));
                        break;
                    default:
                        i++;
                        break;
                }
            }
            if (board.Length == 64)
            {
                InitialPosition = board;
            }
        }
        /// <summary>
        /// Process a comment string to remove unnecessary characters and format it.
        /// </summary>
        /// <param name="c">
        /// Comment string to process.
        /// </param>
        /// <returns>
        /// Processed comment string without leading/trailing characters and unnecessary spaces.
        /// </returns>
        private string ProcessComment(string c)
        {
            if (c.StartsWith("{") || c.StartsWith(";"))
            {
                c = c.Substring(1);
                if (c.EndsWith("}") || c.EndsWith("'"))
                {
                    c = c.Substring(0, c.Length - 1);
                }
            }
            else if (c.StartsWith("("))
            {
                c = c.Replace("'", " ");
            }
            return c;
        }
        /// <summary>
        /// Extract attributes from the PGN file, such as Event, Date, White, Black, etc.
        /// </summary>
        /// <param name="pgn">
        /// PGN file to extract attributes from.
        /// </param>
        /// <exception cref="Exception">
        /// Raised if an attribute name is missing or malformed.
        /// </exception>
        private void GetAttributes(PGNFile pgn)
        {
            pgn.GoTo('[');
            while (pgn.CurrentChar == "[")
            {
                pgn.Seek(1);
                string name = pgn.CurrentAttributeString;
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception(ERR_NOATTRIBUTENAME);
                }
                pgn.Seek(name.Length);
                string value = pgn.CurrentQuotedString;
                pgn.Seek(2 + value.Length);
                if ((name.ToUpper() == "RESULT") && (value == "1/2"))
                {
                    value = "1/2-1/2";
                }
                _attributes.Add(name, value.Replace("'", " "));
                if (name.ToUpper() == "FEN")
                {
                    SetInitialPosition(value);
                }
                pgn.Seek(']');
            }
        }
        /// <summary>
        /// Extract moves from the PGN file, including comments and move numbers.
        /// </summary>
        /// <param name="pgn">
        /// PGN file to extract moves from.
        /// </param>
        /// <exception cref="Exception">
        /// Raised if the moves are malformed, such as missing white or black moves, or if the moves are too long.  
        /// </exception>
        private void GetMoves(PGNFile pgn)
        {
            int cpos = pgn.CurrentPos;
            int crep = 0;
            string end = pgn.TestEnd();
            if (!string.IsNullOrEmpty(end))
            {
                Void = true;
                return;
            }
            while (pgn.TestNumber())
            {
                if (cpos == pgn.CurrentPos)
                {
                    crep++;
                    if (crep > 5)
                    {
                        throw new Exception(ERR_ENDLESS);
                    }
                }
                else
                {
                    cpos = pgn.CurrentPos;
                    crep = 0;
                }
                string nmove = pgn.CurrentNumber;
                pgn.Seek(nmove.Length);
                string wmove = "...";
                string bmove = "";
                bool bcreatemove = false;
                List<string> wcomment = new List<string>();
                List<string> bcomment = new List<string>();
                end = "";
                if (!pgn.Token("..."))
                {
                    if (pgn.CurrentChar == ".")
                    {
                        pgn.Seek(1);
                    }
                    end = pgn.TestEnd();
                    if (string.IsNullOrEmpty(end))
                    {
                        wmove = pgn.CurrentString;
                        if (string.IsNullOrEmpty(wmove) || (wmove.Length > 10))
                        {
                            throw new Exception(ERR_BADWHITEMOVE);
                        }
                        pgn.Seek(wmove.Length);
                        wmove = wmove.Replace(" ", "").Replace("'", "");
                        bcreatemove = true;
                        _plyCount++;
                        while (pgn.TestComment())
                        {
                            string c = pgn.CurrentCommentString;
                            pgn.Seek(c.Length);
                            wcomment.Add(ProcessComment(c));
                        }
                        end = pgn.TestEnd();
                        if (string.IsNullOrEmpty(end))
                        {
                            if (pgn.TestNumber())
                            {
                                string cn = pgn.CurrentNumber;
                                if (cn != nmove)
                                {
                                    throw new Exception(ERR_BADBLACKMOVE);
                                }
                                pgn.Seek(cn.Length);
                                if (!pgn.Token("..."))
                                {
                                    throw new Exception(ERR_BADBLACKMOVE);
                                }
                                pgn.Seek(3);
                            }
                        }
                    }
                }
                else
                {
                    pgn.Seek(3);
                }
                if (string.IsNullOrEmpty(end))
                {
                    bmove = pgn.CurrentString;
                    if (string.IsNullOrEmpty(bmove) || (bmove.Length > 10))
                    {
                        throw new Exception(ERR_BADBLACKMOVE);
                    }
                    pgn.Seek(bmove.Length);
                    bmove = bmove.Replace(" ", "").Replace("'", "");
                    bcreatemove = true;
                    _plyCount++;
                    end = pgn.TestEnd();
                }
                while (pgn.TestComment())
                {
                    string c = pgn.CurrentCommentString;
                    bcomment.Add(ProcessComment(c));
                    pgn.Seek(c.Length);
                }
                if (string.IsNullOrEmpty(end))
                {
                    end = pgn.TestEnd();
                }
                if (bcreatemove)
                {
                    SANMove smove = new SANMove()
                    {
                        MoveNum = int.Parse(nmove),
                        WhiteMove = wmove,
                        BlackMove = bmove
                    };
                    smove.AddWhiteComment(wcomment);
                    smove.AddBlackComment(bcomment);
                    _moves.Add(smove);
                }
                if (!string.IsNullOrEmpty(end))
                {
                    Result = end;
                    return;
                }
            }
        }
        public override string ToString()
        {
            string attr = "";
            if (_attributes != null)
            {
                foreach (string k in _attributes.Keys)
                {
                    attr = attr + "[" + (k ?? "") + " \"" + (_attributes[k] ?? "") + "\"]\r\n";
                }
            }
            attr += _board?.Board == null ? "" : (_board.Board + "\r\n");
            return attr;
        }
    }
}
