using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets.UIHelpers
{
    public static class PointHelper
    {
        /// <summary>
        /// Is this point outside of the screen?
        /// </summary>
        /// <param name="screenPoint">The point to be verified</param>
        /// <returns>If the point is visible on screen or not</returns>
        public static bool IsOutOfView(Vector2 screenPoint, Rect viewRect)
        {
            return screenPoint.x < viewRect.xMin || screenPoint.y < viewRect.yMin
                   || screenPoint.x > viewRect.xMax || screenPoint.y > viewRect.yMax;
        }

        /// <summary>
        /// Verifies if a point is inside an ellipse given the radii. This considers
        /// that the center of the ellipse is at 0,0
        /// </summary>
        /// <param name="point"></param>
        /// <param name="hRadius"></param>
        /// <param name="vRadius"></param>
        /// <returns></returns>
        public static bool IsInsideEllipse(Vector2 point, float hRadius, float vRadius)
        {
            return (((point.x * point.x) / (hRadius * hRadius)) +
                    ((point.y * point.y) / (vRadius * vRadius))) <= 1f;
        }

        /// <summary>
        /// Gives a point in a ellipse, given the radii and the angle
        /// </summary>
        /// <param name="horizontalRadius">The horizontal radius</param>
        /// <param name="vertRadius">The vertical radius</param>
        /// <param name="angle">The angle</param>
        /// <returns></returns>
        public static Vector2 ToEllipse(float horizontalRadius, float vertRadius, float angle)
        {
            float rad = Mathf.Deg2Rad * angle;
            return new Vector2(horizontalRadius * Mathf.Cos(rad), vertRadius * Mathf.Sin(rad));
        }
    }
}