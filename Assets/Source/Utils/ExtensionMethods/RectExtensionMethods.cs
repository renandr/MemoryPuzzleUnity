using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Helper class with utility functions for rects
    /// </summary>
    public static class RectExtension
    {
        /// <summary>
        /// Expand a rectangle (increase it's size)
        /// This is an extension method which can be called directly on rect instances when the namespace of this class is included.
        /// </summary>
        /// <param name="rect">The rect to expand</param>
        /// <param name="size">The additional size to add to all sides of the rect</param>
        /// <returns>The expanded rect</returns>
        public static Rect Expand(this Rect rect, int size)
        {
            return new Rect(rect.x - size, rect.y - size, rect.width + (size * 2), rect.height + (size * 2));
        }
    }
}