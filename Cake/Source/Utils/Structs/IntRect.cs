namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// A simple rectangle struct that uses int values instead of floats
    /// </summary>
    public struct IntRect
    {
        /// <summary>
        /// The x position of the rect (origin is the upper left corner)
        /// </summary>
        public readonly int X;

        /// <summary>
        /// The y position of the rect (origin is the upper left corner)
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// The width of the rect
        /// </summary>
        public readonly int SizeX;

        /// <summary>
        /// The height of the rect
        /// </summary>
        public readonly int SizeY;

        /// <summary>
        /// Create a new rect
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <param name="sizeX">width</param>
        /// <param name="sizeY">height</param>
        public IntRect(int x, int y, int sizeX, int sizeY)
        {
            X = x;
            Y = y;
            SizeX = sizeX;
            SizeY = sizeY;
        }

        /// <summary>
        /// Checks if a rect contains/encloses a point
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>True if the point is inside the rect, false otherwise</returns>
        public bool Contains(IntVector2 point)
        {
            return (point.x >= X && point.y >= Y && point.x < X + SizeX && point.y < Y + SizeY);
        }

        public override string ToString()
        {
            return "[" + X + "," + Y + "," + SizeX + "," + SizeY + "]";

        }

        public override bool Equals(object obj)
        {
            return obj is IntRect && Equals((IntRect)obj);
        }

        public bool Equals(IntRect other)
        {
            return other.X == X && other.Y == Y && other.SizeX == SizeX && other.SizeY == SizeY;
        }

        public static bool operator ==(IntRect a, IntRect b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(IntRect a, IntRect b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + X;
            hash = hash * 37 + Y;
            hash = hash * 113 + SizeX;
            hash = hash * 127 + SizeY;
            return hash;
        }
    }
}