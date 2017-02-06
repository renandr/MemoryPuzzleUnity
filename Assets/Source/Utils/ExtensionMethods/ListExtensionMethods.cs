using System.Collections;
using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    public static class ListExtensions
    {
        /// <summary>
        /// Swaps to elements in a List by their indexes.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list">     The list to act on. </param>
        /// <param name="index1">   The first index. </param>
        /// <param name="index2">   The second index. </param>
        /// <returns>
        /// A List&lt;T&gt;
        /// </returns>
        public static List<T> Swap<T>(this List<T> list, int index1, int index2)
        {
            if (index1 == index2)
            {
                return list;
            }

            if (index1 < 0 || index2 < 0)
            {
                return list;
            }

            if (index1 > list.Count || index2 > list.Count)
            {
                return list;
            }

            T element = list[index1];
            list[index1] = list[index2];
            list[index2] = element;
            return list;
        }

        /// <summary>
        /// Determines whether the given IList object [is null or empty].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the given IList object [is null or empty]; otherwise, <c>false</c>.</returns>
        /// Source: http://extensionmethod.net/csharp/ilist/isnullorempty
        public static bool IsNullOrEmpty(this IList obj)
        {
            return obj == null || obj.Count == 0;
        }
    }
}