using System;
using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// A collection of extension methods for the generic list useful for cycle detection and removing.
    /// </summary>
    public static class StackListCycleDetector
    {
        /// <summary>
        /// Tests if two List are considered equal.
        /// </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="a"> The first list. </param>
        /// <param name="b"> The second list. </param>
        /// <returns>  true if the lists are considered equal, false if they are not. </returns>
        private static bool Match<T>(this List<T> a, List<T> b) where T : IEquatable<T>
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            for (int i = 0; i < a.Count; i++)
            {
                if (!a[i].Equals(b[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// An extension method that gets the indexes of all occurences of the passed obj.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <param name="obj">  The object. </param>
        /// <returns> The indexes of the passed object inside the list.  </returns>
        private static List<int> GetIndexesOf<T>(this List<T> list, T obj) where T : IEquatable<T>
        {
            var result = new List<int>();

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(obj))
                {
                    result.Add(i);
                }
            }

            return result;
        }

        /// <summary>
        /// A List&lt;T&gt; extension method that returns a sub list.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <param name="startIndex">   The start index. </param>
        /// <param name="endIndex"> The end index. </param>
        /// <returns>
        /// A sub List&lt;T&gt;
        /// </returns>
        public static List<T> SubList<T>(this List<T> list, int startIndex, int endIndex)
        {
            var result = new List<T>();

            if (startIndex < 0)
            {
                return result;
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                result.Add(list[i]);
            }

            return result;
        }

        /// <summary>
        /// A List&lt;T&gt; extension method that removes the loops described from the list.
        /// The list is viewed like a stack
        /// </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="stack"> The stack to act on. </param>
        public static void RemoveLoops<T>(this List<T> stack) where T : IEquatable<T>
        {
            T last = stack.Peek();

            if (last == null)
            {
                return;
            }

            List<int> indexes = stack.GetIndexesOf(last);

            if (indexes.Count < 2)
            {
                return;
            }

            int lastIndex = indexes[indexes.Count - 1];

            for (int i = indexes.Count - 2; i >= 0; i--)
            {
                int index = indexes[i];

                int patternLength = lastIndex - index;

                List<T> pattern = stack.SubList(index + 1, lastIndex);
                List<T> sequence = stack.SubList(index - patternLength + 1, index);

                if (sequence.Match(pattern))
                {
                    stack.RemoveRange(index - patternLength + 1, patternLength);

                    break;
                }
            }
        }
    }
}