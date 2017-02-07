using System;
using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// An extension for the generic list providing queue like functionality.
    /// </summary>
    public static class ListQueueExtension
    {
        /// <summary>
        /// A List&lt;T&gt; extension method that removes the first object from this list.
        /// </summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <returns>  The head object from this queue.  </returns>
        public static T Dequeue<T>(this List<T> list)
        {
            T head = default(T);
            if (list != null && list.Count > 0)
            {
                T unknown = list[0];
                head = unknown;
            }

            if (head == null)
            {
                throw new InvalidOperationException("Can't dequeue from an empty list.");
            }
            list.RemoveAt(0);
            return head;
        }

        /// <summary>
        /// A List&lt;T&gt; extension method that adds an object onto the end of this list.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <param name="element">  The element. </param>
        public static void Enqueue<T>(this List<T> list, T element)
        {
            list.Add(element);
        }
    }
}