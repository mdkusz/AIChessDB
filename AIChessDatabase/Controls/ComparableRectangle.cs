using System;
using System.Drawing;

namespace AIChessDatabase.Controls
{
    /// <summary>
    /// Auxiliary class to compare rectangles based on whether a point is contained within them.
    /// </summary>
    internal class ComparableRectangle : IComparable<ComparableRectangle>, IEquatable<ComparableRectangle>
    {
        public ComparableRectangle(Rectangle rect)
        {
            Rect = rect;
        }
        public Rectangle Rect { get; set; }
        public int MoveDataIndex { get; set; }
        public int CompareTo(ComparableRectangle rect)
        {
            Point pt = rect.Rect.Location;
            if (Rect.Contains(pt))
            {
                return 0;
            }
            if ((pt.X < Rect.X) || (pt.Y < Rect.Y))
            {
                return 1;
            }
            return -1;
        }
        public bool Equals(ComparableRectangle rect)
        {
            return Rect.Contains(rect.Rect.Location);
        }
    }
}
