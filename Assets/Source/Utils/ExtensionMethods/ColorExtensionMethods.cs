using UnityEngine;

namespace GGS.CakeBox.Utils
{
    public static class ColorExtensionMethods
    {
        /// <summary>
        /// A Color extension method that gets hexadecimal color, like '#FF0000'
        /// </summary>
        /// <param name="color"> The color </param>
        /// <returns> The hexadecimal color as string </returns>
        public static string ToHex(this Color color)
        {
            string rgbString = string.Format("#{0:X2}{1:X2}{2:X2}",
                (int) (color.r * 255),
                (int) (color.g * 255),
                (int) (color.b * 255));
            return rgbString;
        }
    }
}