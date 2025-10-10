using System;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Auxiliary class to store move data.
    /// </summary>
    internal class MoveData : IComparable<MoveData>, IEquatable<MoveData>
    {
        public MoveData()
        {
        }
        /// <summary>
        /// Move order number
        /// </summary>
        public int MoveNumber { get; set; }
        /// <summary>
        /// Move color: true for white, false for black.
        /// </summary>
        public bool Color { get; set; }
        /// <summary>
        /// Algebraic notation of the move.
        /// </summary>
        public string MoveText { get; set; }
        /// <summary>
        /// Flags representing the move events (castle, capture, move, check, etc.)
        /// </summary>
        public ulong Events { get; set; }
        /// <summary>
        /// True if this move satisfies the query's filter criteria that caused the match to be selected;
        /// such moves are highlighted in violet to indicate why the match matched.
        /// </summary>
        public bool Result { get; set; }

        public int CompareTo(MoveData other)
        {
            if (MoveNumber > other.MoveNumber)
            {
                return 1;
            }
            if (MoveNumber < other.MoveNumber)
            {
                return -1;
            }
            if (Color == other.Color)
            {
                return 0;
            }
            if (Color && !other.Color)
            {
                return -1;
            }
            return 1;
        }
        public bool Equals(MoveData other)
        {
            return (Color == other.Color) && (MoveNumber == other.MoveNumber);
        }
    }
}
