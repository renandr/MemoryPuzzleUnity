using System;
using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    public static class FlatArray
    {
        /// <summary>
        /// Converts a two-dimensional array into a single array
        /// </summary>
        /// <param name="doubleArray">The two-dimensional array of type T to convert into a single array</param>
        /// <param name="singleArray">The converted array</param>
        /// <typeparam name="T">The type of the arrays</typeparam>
        /// <example>
        /// <code>
        /// class ArraySizing : MonoBehaviour
        /// {
        ///    float[,] randomValues2D = new float[,] { { 0, 1 }, { 2, 3 }, { 4, 5 } };
        ///   float[] randomValues1D;
        ///
        ///    void Start()
        ///    {
        ///        CivGridUtility.Flatten&lt;float&gt;(randomValues2D, out randomValues1D);
        ///    }
        ///    //Output:
        ///    //randomValues1D = { 0, 1, 2, 3, 4, 5 };
        /// }
        /// </code>
        /// </example>
        public static void Flatten<T>(this T[,] doubleArray, out T[] singleArray)
        {
            //list to copy the values from the two-dimensional array into
            List<T> combineList = new List<T>();

            //cycle through all the members and copy them into the List
            foreach (T combine in doubleArray)
            {
                combineList.Add(combine);
            }

            //convert our List into a single array
            singleArray = combineList.ToArray();
        }

        /// <summary>
        /// Checks, if a pair of values is inside the bounds of the given array.
        /// </summary>
        /// <param name="x">First parameter</param>
        /// <param name="y">Second parameter</param>
        /// <param name="arr">The array to check</param>
        /// <returns></returns>
        public static bool IsIn2DBounds(int x, int y, Array arr)
        {
            if (arr.Rank != 2)
            {
                throw new Exception("Array is not two dimensional!");
            }
            return (x >= 0 && y >= 0 && x < arr.GetLength(0) && y < arr.GetLength(1));
        }

        /// <summary>
        /// Iterates over a two-dimensional array an perfoms an action for each element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void ForAll<T>(this T[,] array, Action<T> action)
        {
            for (var y = 0; y < array.GetLength(1); y++)
            {
                for (var x = 0; x < array.GetLength(0); x++)
                {
                    action(array[x, y]);
                }
            }
        }
        /// <summary>
        /// This method will create a new array with the given length and copies over all old values.
        /// If the new length is smaller then the old array, the new array will have the needed size.
        /// </summary>
        /// <typeparam name="T">type of value in array</typeparam>
        /// <param name="oldArray">array to extend</param>
        /// <param name="newLength">new length</param>
        /// <returns></returns>
        public static T[] ToExtendArray<T>(this T[] oldArray, int newLength)
        {
            T[] newArray = new T[Math.Max(newLength, oldArray.Length)];
            oldArray.CopyTo(newArray, 0);
            return newArray;
        }

        /// <summary>
        /// Creates a subarray out of current array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldArray">original array</param>
        /// <param name="startIndex">start index of copy</param>
        /// <param name="endIndex">end index of copy</param>
        /// <returns></returns>
        public static T[] ToSubArray<T>(this T[] oldArray, int startIndex, int endIndex)
        {
            if (oldArray.Length <= startIndex)
            {
                return new T[0];
            }

            if (endIndex <= startIndex)
            {
                throw new ArgumentException("startIndex is greater or equal then endIndex!");
            }

            T[] newArray = new T[endIndex - startIndex + 1];
            for (int i = startIndex, j = 0; i <= endIndex; ++i, ++j)
            {
                newArray[j] = oldArray[i];
            }
            return newArray;
        }

        /// <summary>
        /// Shifts data withing the same array to the indicies!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="sourceStart">StartIndex of data block to shift</param>
        /// <param name="sourceEnd">EndIndex of data block to shift</param>
        /// <param name="targetStart">StartIndex of destination block - write start point</param>
        public static void Shift<T>(this T[] array, int sourceStart, int sourceEnd, int targetStart = 0)
        {
            if (sourceStart < targetStart)
            {
                throw new Exception("Shift only works to the left. sourceStart must be greater than targetStart!");
            }

            if (sourceEnd >= array.Length)
            {
                throw new IndexOutOfRangeException("SourceEnd is greater than array length!");
            }

            for (int i = sourceStart, j = targetStart; i < sourceEnd; ++i, ++j)
            {
                array[j] = array[i];
            }
        }
    }
}
