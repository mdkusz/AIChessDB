using System;
using System.Collections.Generic;

namespace AIChessDatabase.PGNParser
{
    /// <summary>
    /// Matches a move in SAN notation (Standard Algebraic Notation).
    /// </summary>
    public class SANMove : IComparable<SANMove>, IEquatable<SANMove>
    {
        private List<string> _whiteComments = new List<string>();
        private List<string> _blackComments = new List<string>();
        public SANMove()
        {
        }
        /// <summary>
        /// Move number in the game.
        /// </summary>
        public int MoveNum { get; set; }
        /// <summary>
        /// Gets or sets the ply made by the white player in a chess game.
        /// </summary>
        public string WhiteMove { get; set; }
        /// <summary>
        /// Gets or sets the ply made by the black player in a chess game.
        /// </summary>
        public string BlackMove { get; set; }
        /// <summary>
        /// Count of comments on the white player ply.
        /// </summary>
        public int WhiteCommentsCount
        {
            get
            {
                return _whiteComments.Count;
            }
        }
        /// <summary>
        /// Count of comments on the black player ply.
        /// </summary>
        public int BlackCommentsCount
        {
            get
            {
                return _blackComments.Count;
            }
        }
        /// <summary>
        /// Returns the comments on the white player ply.
        /// </summary>
        public IEnumerable<string> WhiteComments
        {
            get
            {
                foreach (string c in _whiteComments)
                {
                    yield return c;
                }
            }
        }
        /// <summary>
        /// Returns the comments on the black player ply.
        /// </summary>
        public IEnumerable<string> BlackComments
        {
            get
            {
                foreach (string c in _blackComments)
                {
                    yield return c;
                }
            }
        }
        /// <summary>
        /// Adds a comment to the white player ply.
        /// </summary>
        /// <param name="c">
        /// Comment to add to the white player ply.
        /// </param>
        public void AddWhiteComment(string c)
        {
            _whiteComments.Add(c);
        }
        /// <summary>
        /// Adds a collection of comments to the white player ply.
        /// </summary>
        /// <param name="c">
        /// Collection of comments to add to the white player ply.
        /// </param>
        public void AddWhiteComment(IEnumerable<string> c)
        {
            _whiteComments.AddRange(c);
        }
        public void AddBlackComment(string c)
        {
            _blackComments.Add(c);
        }
        /// <summary>
        /// Adds a collection of comments to the black player ply.
        /// </summary>
        /// <param name="c">
        /// Collection of comments to add to the black player ply.
        /// </param>
        public void AddBlackComment(IEnumerable<string> c)
        {
            _blackComments.AddRange(c);
        }
        public int CompareTo(SANMove other)
        {
            return MoveNum.CompareTo(other.MoveNum);
        }
        public bool Equals(SANMove other)
        {
            return MoveNum == other.MoveNum;
        }
    }
}
