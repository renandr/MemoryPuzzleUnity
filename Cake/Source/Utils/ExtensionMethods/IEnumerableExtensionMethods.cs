using System.Collections.Generic;
using System.Text;

namespace GGS.CakeBox.Utils
{
    public static class IEnumerableExtensionMethods
    {
        /// <summary>
        /// Converts the contents of an IEnumerable to a string.
        /// </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="newLine"> Separate items with new lines? </param>
        /// <returns>A string.</returns>
        public static string ContentToString<T>(this IEnumerable<T> list, bool newLine = false)
        {
            var sb = new StringBuilder();

            foreach (T item in list)
            {
                if (newLine)
                {
                    sb.AppendLine(item.ToString());
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }
    }
}