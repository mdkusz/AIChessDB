using AIChessDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AIChessDatabase.PGNParser
{
    /// <summary>
    /// PGNFile class represents a PGN file containing multiple chess matches.
    /// </summary>
    public class PGNFile
    {
        private List<PGNMatch> _pgnmatches = new List<PGNMatch>();
        private Data.Match[] _matches = null;
        private string _content = null;
        private int _currentPosition = 0;

        public PGNFile()
        {
        }
        public PGNFile(List<PGNMatch> matches)
        {
            _pgnmatches = matches;
        }
        /// <summary>
        /// Path to the PGN file.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Count of matches in the PGN file.
        /// </summary>
        public int MatchCount
        {
            get
            {
                return _pgnmatches.Count;
            }
        }
        /// <summary>
        /// Get a specific PGN match by its index.
        /// </summary>
        /// <param name="nm">
        /// Match index to retrieve.
        /// </param>
        /// <returns>
        /// PGNMatch object corresponding to the specified index, or null if no matches exist.
        /// </returns>
        public PGNMatch GetPGNMatch(int nm)
        {
            if (_pgnmatches.Count == 0)
            {
                return null;
            }
            return _pgnmatches[nm];
        }
        /// <summary>
        /// Get a specific match by its index, converting it to a Data.Match object if necessary.
        /// </summary>
        /// <param name="m">
        /// Match index to retrieve.
        /// </param>
        /// <param name="rep">
        /// Database repository to convert the match to a Data.Match object.
        /// </param>
        /// <returns>
        /// Match object corresponding to the specified index, converted from PGNMatch if necessary.
        /// </returns>
        public Data.Match GetMatch(int m, IObjectRepository rep)
        {
            if ((_matches == null) || (m < 0) || (m >= _matches.Length) || (_matches[m] == null))
            {
                return _pgnmatches[m].ConvertToMatch(rep);
            }
            return _matches[m];
        }
        /// <summary>
        /// All matches in the PGN file.
        /// </summary>
        public IEnumerable<PGNMatch> Matches
        {
            get
            {
                foreach (PGNMatch m in _pgnmatches)
                {
                    yield return m;
                }
            }
        }
        /// <summary>
        /// Current character in the PGN content. Null if not a character.
        /// </summary>
        public string CurrentChar
        {
            get
            {
                if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
                {
                    if (_currentPosition >= 0)
                    {
                        return _content.Substring(_currentPosition, 1);
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Current attribute string in the PGN content. Null if not an attribute.
        /// </summary>
        public string CurrentAttributeString
        {
            get
            {
                if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
                {
                    Regex rex = new Regex(@"^[0-9A-Za-z]+");
                    System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(_currentPosition));
                    if (m.Success)
                    {
                        return m.Value;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Current parser position in the PGN content.
        /// </summary>
        public int CurrentPos
        {
            get
            {
                return _currentPosition;
            }
        }
        /// <summary>
        /// End of file indicator.
        /// </summary>
        public bool EOF
        {
            get
            {
                return _currentPosition < 0;
            }
        }
        /// <summary>
        /// Current string in the PGN content. Null if not a string.
        /// </summary>
        public string CurrentString
        {
            get
            {
                if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
                {
                    Regex rex = new Regex(@"^[^ ']+");
                    System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(_currentPosition));
                    if (m.Success)
                    {
                        return m.Value;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Current quoted string in the PGN content. Null if not a quoted string.
        /// </summary>
        public string CurrentQuotedString
        {
            get
            {
                if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
                {
                    Regex rex = new Regex("^\"[^\"]*\"");
                    System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(_currentPosition));
                    if (m.Success)
                    {
                        if (m.Length <= 2)
                        {
                            return "";
                        }
                        return m.Value.Substring(1, m.Length - 2);
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Current comment string in the PGN content. Null if not a comment.
        /// </summary>
        public string CurrentCommentString
        {
            get
            {
                if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
                {
                    Regex rex = new Regex(@"^\{[^\}]*\}");
                    System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(_currentPosition));
                    if (m.Success)
                    {
                        return m.Value;
                    }
                    rex = new Regex("^;.*'");
                    m = rex.Match(_content.Substring(_currentPosition));
                    if (m.Success)
                    {
                        return m.Value;
                    }
                    rex = new Regex(@"^\$[0-9]+");
                    m = rex.Match(_content.Substring(_currentPosition));
                    if (m.Success)
                    {
                        return m.Value;
                    }
                    if (CurrentChar == "(")
                    {
                        string nc = "";
                        CurrentNestedComment(_currentPosition, ref nc);
                        return nc;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Current number in the PGN content. Null if not a number.
        /// </summary>
        public string CurrentNumber
        {
            get
            {
                if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
                {
                    Regex rex = new Regex("^[0-9]+");
                    System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(_currentPosition));
                    if (m.Success)
                    {
                        return m.Value;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Parse a PGN file from the specified filename.
        /// </summary>
        /// <param name="filename">
        /// PGN file path to parse.
        /// </param>
        /// <returns>
        /// Match count parsed from the PGN file.
        /// </returns>
        /// <exception cref="Exception">
        /// Raises an exception if the file cannot be read or parsed correctly.
        /// </exception>
        public int Parse(string filename)
        {
            StreamReader rdr = null;
            _matches = null;
            try
            {
                FileName = filename;
                rdr = new StreamReader(filename);
                _content = rdr.ReadToEnd().Replace("\n", "'").Replace("\r", "'").Replace("\t", " ");
                rdr.Close();
                rdr = null;
                _pgnmatches.Clear();
                _currentPosition = 0;
                bool append = false;
                while (_currentPosition >= 0)
                {
                    PGNMatch match = new PGNMatch();
                    match.ParseMatch(this);
                    if (!match.Void)
                    {
                        _pgnmatches.Add(match);
                    }
                    append = true;
                }
                return _pgnmatches.Count;
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(_content))
                {
                    throw new Exception(ex.Message);
                }
                throw new Exception(_content.Substring(_currentPosition,
                    Math.Min(100, _content.Length - _currentPosition)), ex);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }
        /// <summary>
        /// Parse a string in PGN format.
        /// </summary>
        /// <param name="content">
        /// PGN formatted string to parse.
        /// </param>
        /// <returns>
        /// Number of matches parsed from the PGN string.
        /// </returns>
        /// <exception cref="Exception">
        /// Raises an exception if the string cannot be parsed correctly.
        /// </exception>
        public int ParseString(string content)
        {
            try
            {
                _content = content.Replace("\n", "'").Replace("\r", "'").Replace("\t", " ");
                _pgnmatches.Clear();
                _currentPosition = 0;
                while (_currentPosition >= 0)
                {
                    PGNMatch match = new PGNMatch();
                    match.ParseMatch(this);
                    if (!match.Void)
                    {
                        _pgnmatches.Add(match);
                    }
                }
                return _pgnmatches.Count;
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(_content))
                {
                    throw new Exception(ex.Message);
                }
                throw new Exception(_content.Substring(_currentPosition,
                    Math.Min(100, _content.Length - _currentPosition)), ex);
            }
        }
        /// <summary>
        /// Convert all PGNMatch objects to Data.Match objects using the provided database repository.
        /// </summary>
        /// <param name="rep">
        /// Database repository to use for converting PGNMatch to Data.Match.
        /// </param>
        /// <exception cref="Exception">
        /// Raises an exception if any match conversion fails.
        /// </exception>
        public void ConvertAllMatches(IObjectRepository rep)
        {
            _matches = new Data.Match[_pgnmatches.Count];
            string error = "";
            try
            {
                Parallel.For(0, _pgnmatches.Count, (m) =>
                {
                    try
                    {
                        _matches[m] = _pgnmatches[m].ConvertToMatch(rep);
                    }
                    catch (Exception ex)
                    {
                        error = _pgnmatches[m].ToString() + ex.Message;
                        throw;
                    }
                });
            }
            catch
            {
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                throw;
            }
        }
        /// <summary>
        /// Seek to a specific position in the PGN content.
        /// </summary>
        /// <param name="cnt">
        /// Integer offset to seek from the current position.
        /// </param>
        public void Seek(int cnt)
        {
            _currentPosition = _currentPosition + cnt;
            if (_currentPosition >= _content.Length)
            {
                _currentPosition = -1;
            }
        }
        /// <summary>
        /// Seek to the next position from a specific character in the PGN content.
        /// </summary>
        /// <param name="to">
        /// Character to seek in the PGN content.
        /// </param>
        public void Seek(char to)
        {
            while ((_currentPosition >= 0) && (_content[_currentPosition] != to))
            {
                _currentPosition++;
                if (_currentPosition >= _content.Length)
                {
                    _currentPosition = -1;
                }
            }
            if ((_currentPosition >= 0) && (_content[_currentPosition] == to))
            {
                _currentPosition++;
                if (_currentPosition >= _content.Length)
                {
                    _currentPosition = -1;
                }
            }
        }
        /// <summary>
        /// Go to the position of a specific character in the PGN content.
        /// </summary>
        /// <param name="to">
        /// Character to go to in the PGN content.
        /// </param>
        public void GoTo(char to)
        {
            while ((_currentPosition >= 0) && (_content[_currentPosition] != to))
            {
                _currentPosition++;
                if (_currentPosition >= _content.Length)
                {
                    _currentPosition = -1;
                }
            }
        }
        /// <summary>
        /// Check if the current position in the PGN content matches a specific token.
        /// </summary>
        /// <param name="token">
        /// Token string to match at the current position in the PGN content.
        /// </param>
        /// <returns>
        /// True if the current position matches the token, false otherwise.
        /// </returns>
        public bool Token(string token)
        {
            if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
            {
                return _content.Substring(_currentPosition, token.Length) == token;
            }
            return false;
        }
        /// <summary>
        /// Check if the current position in the PGN content is a valid number.
        /// </summary>
        /// <returns>
        /// True if the current position is a valid number, false otherwise.
        /// </returns>
        public bool TestNumber()
        {
            if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
            {
                Regex rex = new Regex("^[0-9]+");
                System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(_currentPosition));
                return m.Success;
            }
            return false;
        }
        /// <summary>
        /// Check if the current position in the PGN content is a valid comment.
        /// </summary>
        /// <returns>
        /// True if the current position is a valid comment, false otherwise.
        /// </returns>
        public bool TestComment()
        {
            if ((_currentPosition = SkeepBlanks(_currentPosition)) >= 0)
            {
                Regex rex = new Regex(@"^\{[^\}]*\}");
                System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(_currentPosition));
                Regex rex2 = new Regex("^;[^']*'");
                System.Text.RegularExpressions.Match m2 = rex2.Match(_content.Substring(_currentPosition));
                string cc = CurrentChar;
                bool boc = (cc == "(") || (cc == "$");
                return m.Success || m2.Success || boc;
            }
            return false;
        }
        /// <summary>
        /// Check if the current position in the PGN content is a valid match result.
        /// </summary>
        /// <returns>
        /// True if the current position is a valid match result, false otherwise.
        /// </returns>
        public string TestEnd()
        {
            string cs = CurrentString;
            string css = cs.Replace(" ", "");
            if ((css == "0-1") || (css == "1-0") || (css == "1/2-1/2") || (css == "*") || (css == "1/2"))
            {
                Seek(cs.Length);
                return css == "1/2" ? "1/2-1/2" : css;
            }
            return "";
        }
        /// <summary>
        /// Skip blanks in the PGN content starting from a specific position.
        /// </summary>
        /// <param name="from">
        /// Starting position in the PGN content to skip blanks from.
        /// </param>
        /// <returns>
        /// New position in the PGN content after skipping blanks, or -1 if the position is invalid.
        /// </returns>
        private int SkeepBlanks(int from)
        {
            if ((from >= 0) && (from < _content.Length))
            {
                Regex rex = new Regex(@"^(\s|')+");
                System.Text.RegularExpressions.Match m = rex.Match(_content.Substring(from));
                if (m.Success)
                {
                    return from + m.Length;
                }
                return from;
            }
            return -1;
        }
        /// <summary>
        /// Get the current nested comment in the PGN content starting from a specific position.
        /// </summary>
        /// <param name="from">
        /// Starting position in the PGN content to get the nested comment from.
        /// </param>
        /// <param name="comment">
        /// Comment string to append the nested comment to.
        /// </param>
        /// <returns>
        /// New position in the PGN content after processing the nested comment, or -1 if the position is invalid.
        /// </returns>
        private int CurrentNestedComment(int from, ref string comment)
        {
            if ((from >= 0) && (from < _content.Length))
            {
                if (_content[from] == '(')
                {
                    comment += _content[from++];
                    while ((from < _content.Length) && (_content[from] != ')'))
                    {
                        if (_content[from] != '(')
                        {
                            comment += _content[from++];
                        }
                        else
                        {
                            from = CurrentNestedComment(from, ref comment);
                        }
                    }
                    if ((from < _content.Length) && (_content[from] == ')'))
                    {
                        comment += _content[from++];
                    }
                }
            }
            return from;
        }
    }
}
