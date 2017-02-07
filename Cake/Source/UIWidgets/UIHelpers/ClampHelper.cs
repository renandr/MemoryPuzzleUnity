using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets.UIHelpers
{
    public static class ClampHelper
    {
        /// <summary>
        /// Clamp position within a rectangle.
        /// </summary>
        /// <param name="screenPoint"> The screen point where the indicator is. </param>
        /// <param name="pointSize"> Indicator size. </param>
        /// <param name="containerRect"> The container rectangle. </param>
        /// <param name="canvas"> Unity's canvas (to fetch the scale factor). </param>
        /// <param name="border"> The configured border. </param>
        /// <param name="offset"> Offset in the position. </param>
        /// <returns>
        /// Position constrained within the rect.
        /// </returns>
        public static Vector2 ClampPositionRect(Vector2 screenPoint, Vector2 pointSize, Rect containerRect, Canvas canvas, Vector2 border, Vector2 offset)
        {
            Vector2 halfSize = pointSize * canvas.scaleFactor * 0.5f;

            float minX = containerRect.xMin + halfSize.x + border.x;
            float maxX = containerRect.xMax - halfSize.x - border.x;
            float minY = containerRect.yMin + halfSize.y + border.y;
            float maxY = containerRect.yMax - halfSize.y - border.y;
            
            float clampedX = Mathf.Clamp(screenPoint.x + offset.x, minX, maxX);
            float clampedY = Mathf.Clamp(screenPoint.y + offset.y, minY, maxY);

            return new Vector2(clampedX, clampedY);
        }

        /// <summary>
        /// Clamps the position of the indicator to a circle.
        /// </summary>
        /// <param name="screenPoint">  The screen point where the indicator is. </param>
        /// <param name="rad">  the radius of the circle. </param>
        /// <param name="offset">   . </param>
        /// <param name="parent">   . </param>
        /// <param name="canvas">   . </param>
        /// <returns>  A Vector2.  </returns>
        public static Vector2 ClampPositionToCircle(Rect elementRect, Vector2 screenPoint, float rad, Vector2 offset, RectTransform parent, Canvas canvas)
        {
            return ClampPositionElliptic(elementRect, screenPoint, rad, rad, offset, parent, canvas);
        }

        /// <summary>
        /// Clamps the position of the indicators to a elliptic form.
        /// </summary>
        /// <param name="screenPoint">  The screen point where the indicator is. </param>
        /// <param name="hRadius">  The horizontal radius of the ellipse. </param>
        /// <param name="vRadius">  The vertical radius of the ellipse. </param>
        /// <param name="offset">   . </param>
        /// <param name="parent">   . </param>
        /// <param name="canvas">   . </param>
        /// <param name="useNormalizedSize">Are the radius in percentage of the canvas (true) or in pixels (false)?</param>
        /// <returns>  Either a position clamped to the ellipse, or the same coordinate as before.  </returns>
        public static Vector2 ClampPositionElliptic(Rect elementRect, Vector2 screenPoint, float hRadius, float vRadius, Vector2 offset, RectTransform parent, Canvas canvas, bool useNormalizedSize = true)
        {
            screenPoint += offset;
            var canvasCorners = new Vector3[4];
            parent.GetWorldCorners(canvasCorners);

            float centerX = (canvasCorners[2].x - canvasCorners[0].x) / 2f;
            float centerY = (canvasCorners[2].y - canvasCorners[0].y) / 2f;

            float angle = Vector2.Angle(Vector2.right, screenPoint.normalized);
            float sign = Mathf.Sign(Vector3.Dot(Vector3.forward, Vector3.Cross(Vector2.right, screenPoint.normalized)));

            float radiusScaleFactorH;
            float radiusScaleFactorV;

            if (useNormalizedSize)
            {
                //  * 0.5f because this is radius, not diameter
                radiusScaleFactorH = Mathf.Clamp01(hRadius) * parent.rect.width * 0.5f;
                radiusScaleFactorV = Mathf.Clamp01(vRadius) * parent.rect.height * 0.5f;
            }
            else
            {
                radiusScaleFactorH = hRadius * canvas.scaleFactor;
                radiusScaleFactorV = vRadius * canvas.scaleFactor;
            }
            
            if (!PointHelper.IsInsideEllipse(screenPoint, radiusScaleFactorH, radiusScaleFactorV))
            {
                Vector2 ellipse = PointHelper.ToEllipse(radiusScaleFactorH, radiusScaleFactorV, angle * sign);
                return new Vector2(ellipse.x + centerX - elementRect.width * 0.5f, ellipse.y + centerY - elementRect.height * 0.5f);
            }
            else
            {
                screenPoint.x -= elementRect.width * 0.5f;
                screenPoint.y -= elementRect.height *0.5f;
                return screenPoint;
            }
        }
    }
}