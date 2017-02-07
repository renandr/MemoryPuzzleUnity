using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    ///  An extension for the generic list providing stack like functionality.
    /// </summary>
    public static class ListStackExtention
    {
        /// <summary>
        /// A List&lt;T&gt; extension method that returns the top-of-stack object without removing it.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <returns>
        /// The current top-of-stack object.
        /// </returns>
        public static T Peek<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        /// <summary>
        /// A List&lt;T&gt; extension method that removes and returns the top-of-stack object.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <returns>
        /// The previous top-of-stack object.
        /// </returns>
        public static T Pop<T>(this List<T> list)
        {
            bool any = (list != null && list.Count > 0);

            if (!any)
            {
                return default(T);
            }

            T last = list[list.Count - 1];

            list.RemoveAt(list.Count - 1);

            return last;
        }

        /// <summary>
        /// A List&lt;T&gt; extension method that removes and returns the top-of-stack object.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <returns>
        /// The previous top-of-stack object.
        /// </returns>
        public static void RemoveLast<T>(this List<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// A List&lt;T&gt; extension method that pushes an object onto this stack.
        /// </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <param name="obj">  The object. </param>
        public static void Push<T>(this List<T> list, T obj)
        {
            list.Add(obj);
        }
    }
}