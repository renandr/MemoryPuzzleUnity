using System.Text;
using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Class for string to array/list conversions
    /// </summary>
    public static class StringToArrayConverter
    {
        #region Split to Ints

        /// <summary>
        /// Splits a string into multiple ints
        /// </summary>
        /// <param name="value">The value to split</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>Array of split parts</returns>
        public static int[] SplitToInts(this string value, char separator = '#')
        {
            if (string.IsNullOrEmpty(value))
            {
                return new int[0];
            }

            string[] parts = value.Split(separator);

            int[] result = new int[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                int intValue;
                if (int.TryParse(parts[i], out intValue))
                {
                    result[i] = intValue;
                }
            }

            return result;
        }

        /// <summary>
        /// Splits a string into a list of ints
        /// </summary>
        /// <param name="value">The value to split</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>List of split parts</returns>
        public static List<int> SplitToIntsList(this string value, char separator = '#')
        {
            return new List<int>(SplitToInts(value, separator));
        }

        #endregion

        #region Split to Floats

        /// <summary>
        /// Splits a string into multiple floats
        /// </summary>
        /// <param name="value">The value to split</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>Array of split parts</returns>
        public static float[] SplitToFloats(this string value, char separator = '#')
        {
            if (string.IsNullOrEmpty(value))
            {
                return new float[0];
            }

            string[] parts = value.Split(separator);

            float[] result = new float[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                float intValue;
                if (float.TryParse(parts[i], out intValue))
                {
                    result[i] = intValue;
                }
            }

            return result;
        }

        /// <summary>
        /// Splits a string into a list of ints
        /// </summary>
        /// <param name="value">The value to split</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>List of split parts</returns>
        public static List<float> SplitToFloatsLists(this string value, char separator = '#')
        {
            return new List<float>(SplitToFloats(value, separator));
        }

        #endregion

        #region Split to Strings

        /// <summary>
        /// Splits a string into multiple strings
        /// </summary>
        /// <param name="value">The value to split</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>Array of split parts</returns>
        public static string[] SplitToStrings(this string value, char separator = '#')
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[0];
            }

            return value.Split(separator);
        }

        /// <summary>
        /// Splits a string into a list of strings
        /// </summary>
        /// <param name="value">The value to split</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>List of split parts</returns>
        public static List<string> SplitToStringsList(this string value, char separator = '#')
        {
            return new List<string>(SplitToStrings(value, separator));
        }

        #endregion

        #region Join to String

        /// <summary>
        /// Joins values to a string
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="values">Array of values to join</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>String of joined values</returns>
        public static string ToSeparatedString<T>(this T[] values, char separator = '#')
        {
            if (values == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (i != 0)
                {
                    builder.Append(separator);
                }
                builder.Append(values[i]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Joins values to a string
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="values">List of values to join</param>
        /// <param name="separator">The separator to use</param>
        /// <returns>String of joined values</returns>
        public static string ToSeparatedString<T>(this List<T> values, char separator = '#')
        {
            if (values == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                if (i != 0)
                {
                    builder.Append(separator);
                }
                builder.Append(values[i]);
            }

            return builder.ToString();
        }

        #endregion
    }
}