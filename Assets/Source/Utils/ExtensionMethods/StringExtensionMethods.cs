using System;

namespace GGS.CakeBox.Utils
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// A string extension which makes the first letter uppercase and the rest lowercase
        /// </summary>
        /// <param name="text"> The text to act on. </param>
        /// <returns> The input text with first letter being uppercase and the rest lowercase</returns>
        public static string FirstToUpper(this string text)
        {
            if (text.Length == 1)
            {
                return text.ToUpperInvariant();
            }
            return char.ToUpperInvariant(text[0]) + text.Substring(1).ToLower();
        }

        /// <summary>
        /// Faster alternative to C#'s StartsWith
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <param name="start">The start to check against</param>
        /// <returns>True if text starts with start, false otherwise</returns>
        public static bool FastStartsWith(this string text, string start)
        {
            int startLength = start.Length;

            // match is impossible if the text to check is shorter than the start text
            if (text.Length < startLength)
            {
                return false;
            }

            // Compare all start chars
            for (int i = 0; i < startLength; i++)
            {
                if (text[i] != start[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Faster alternative to C#'s EndsWith
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <param name="end">The end to check against</param>
        /// <returns>True if text ends with end, false otherwise</returns>
        public static bool FastEndsWith(this string text, string end)
        {
            int endLength = end.Length;
            int textLength = text.Length;

            // match is impossible if the text to check is shorter than the end text
            if (textLength < endLength)
            {
                return false;
            }

            // Compare all start chars
            for (int i = 1; i <= endLength; i++)
            {
                if (text[textLength - i] != end[endLength - i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Converts a string to an enum value
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The string value</param>
        /// <returns>The enum value</returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}