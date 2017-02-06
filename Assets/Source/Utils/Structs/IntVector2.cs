using System;
using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// 2D integer vector, can be used for positions and sizes
    /// </summary>
    [Serializable]
    public struct IntVector2 : IEquatable<IntVector2>
    {
        #region Common IntVector2s

        /// <summary>
        /// An <see cref="IntVector2"/> pointing upwards (0, 1)
        /// </summary>
        public static readonly IntVector2 up = new IntVector2(0, 1);

        /// <summary>
        /// An <see cref="IntVector2"/> pointing downwards (0, -1)
        /// </summary>
        public static readonly IntVector2 down = new IntVector2(0, -1);

        /// <summary>
        /// An <see cref="IntVector2"/> pointing left (-1, 0)
        /// </summary>
        public static readonly IntVector2 left = new IntVector2(-1, 0);

        /// <summary>
        /// An <see cref="IntVector2"/> pointing right (1, 0)
        /// </summary>
        public static readonly IntVector2 right = new IntVector2(1, 0);

        /// <summary>
        /// An <see cref="IntVector2"/> with x and y being 0 (0, 0)
        /// </summary>
        public static readonly IntVector2 zero = new IntVector2(0, 0);

        /// <summary>
        /// An <see cref="IntVector2"/> with x and y being 1 (1, 1)
        /// </summary>
        public static readonly IntVector2 one = new IntVector2(1, 1);

        #endregion

        /// <summary>
        /// X component (e.g.: x position or width)
        /// </summary>
        public int x;

        /// <summary>
        /// Y component (e.g.: y position or height)
        /// </summary>
        public int y;

        /// <summary>
        /// Create a new 2D integer vector
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor that takes to floats
        /// </summary>
        /// <param name="x">    X component. </param>
        /// <param name="y">    Y component. </param>
        public IntVector2(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }

        /// <summary>
        /// Create a new 2D integer vector.
        /// </summary>
        /// <param name="vec">  The vector. </param>
        public IntVector2(Vector2 vec)
        {
            x = (int)vec.x;
            y = (int)vec.y;
        }

        /// <summary>
        /// Create a new 2D integer vector from its nullable version
        /// </summary>
        /// <param name="vec"> The nullable vector. </param>
        public IntVector2(IntVector2? vec)
        {
            x = 0;
            y = 0;
            if (vec != null)
            {
                x = vec.Value.x;
                y = vec.Value.y;
            }
        }

        public float Magnitude()
        {
            return Mathf.Sqrt(x * x + y * y);
        }

        public float magnitude
        {
            get
            {
                return Magnitude();
            }
        }

        public float SqrMagnitude()
        {
            return x + y;
        }

        public float sqrMagnitude
        {
            get
            {
                return SqrMagnitude();
            }
        }

        public IntVector2 normalized
        {
            get
            {
                int distance = x + y;
                return new IntVector2(x / distance, y / distance);
            }
        }

        private float distance()
        {
            return Mathf.Sqrt(x * x + y * y);
        }

        public float Length
        {
            get
            {
                return distance();
            }
        }

        /// <summary>
        /// Normalizes the vector, and rounds x and y to 1 if positive, else to -1. 
        /// </summary>
        /// <returns></returns>
        public IntVector2 NormalizeCeiled()
        {
            x = x > 0 ? 1 : x;
            x = x < 0 ? -1 : x;
            y = y > 0 ? 1 : y;
            y = y < 0 ? -1 : y;

            return this;
        }

        public override string ToString()
        {
            return "[" + x + "," + y + "]";
        }

        /// <summary>
        /// Tests if this object is considered equal to another.
        /// </summary>
        /// <param name="obj"> The object to compare to this IntVector2. 
        /// </param>
        /// <returns>
        /// true if the objects are considered equal, false if they are not.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is IntVector2 && Equals((IntVector2)obj);
        }

        /// <summary>
        /// Tests if this IntVector2 is considered equal to another.
        /// </summary>
        /// <param name="other"> The int vector 2 to compare to this
        /// com.goodgamestudios.warlands.utils.helpers.IntVector2. </param>
        /// <returns>
        /// true if the objects are considered equal, false if they are not.
        /// </returns>
        public bool Equals(IntVector2 other)
        {
            return other.x == x && other.y == y;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a"> The IntVector2 to process. </param>
        /// <param name="b"> The IntVector2 to process. </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static bool operator ==(IntVector2 a, IntVector2 b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="a"> The IntVector2 to process. </param>
        /// <param name="b"> The IntVector2 to process. </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static bool operator !=(IntVector2 a, IntVector2 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Modulus operator.
        /// </summary>
        /// <param name="a"> The IntVector2 to process. </param>
        /// <param name="b"> The int to process. </param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        public static IntVector2 operator %(IntVector2 a, int b)
        {
            return new IntVector2(a.x % b, a.y % b);
        }

        public static IntVector2 operator *(IntVector2 a, int b)
        {
            return new IntVector2(a.x * b, a.y * b);
        }

        public static IntVector2 operator /(IntVector2 a, int b)
        {
            return new IntVector2(a.x / b, a.y / b);
        }

        public static IntVector2 operator *(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a.x * b.x, a.y * b.y);
        }

        public static IntVector2 operator -(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a.x - b.x, a.y - b.y);
        }

        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a.x + b.x, a.y + b.y);
        }

        /// <summary>
        /// Returns a hash code for this IntVector2.
        /// </summary>
        /// <returns>
        /// A hash code for this IntVector2.
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + x;
            hash = hash * 37 + y;
            return hash;
        }

        /// <summary>
        /// Calculates the squared distance of two integer 2D vectors
        /// </summary>
        /// <param name="a">vector a</param>
        /// <param name="b">vector b</param>
        /// <returns>The squared distance</returns>
        public static int DistanceSquared(IntVector2 a, IntVector2 b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
        }

        /// <summary>
        /// Calculates the distance of two integer 2D vectors
        /// </summary>
        /// <param name="a">vector a</param>
        /// <param name="b">vector b</param>
        /// <returns>The distance</returns>
        public static int Distance(IntVector2 a, IntVector2 b)
        {
            return Mathf.CeilToInt(Mathf.Sqrt(DistanceSquared(a, b)));
        }

        /// <summary>
        /// Converts this IntVector2 to regular float version.
        /// </summary>
        /// <returns></returns>
        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }
}