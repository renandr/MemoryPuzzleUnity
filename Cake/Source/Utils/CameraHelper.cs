
using UnityEngine;

namespace GGS.CakeBox.Utils
{
    public static class CameraHelper
    {
        private const float Epsilon = 1.2f; //small epsilon to keep the position infront of the camera

        /// <summary>
        /// Adjusts the world position for the WorldToScreenPoint calculation bug
        /// </summary>
        /// <param name="position"></param>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Vector3 CalculateAdjustedWorldPosition(this Camera cam, Vector3 position)
        {
            return CalculateAdjustedWorldPositionByCamera(cam, position);
        }

        public static Rect CalculateViewportSpaceBounds(this Camera cam, Bounds bounds, RectTransform canvasRect)
        {
            Vector3[] pts = new Vector3[8];

            pts[0] = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z);
            pts[1] = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z);
            pts[2] = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z);
            pts[3] = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z);
            pts[4] = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z);
            pts[5] = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z);
            pts[6] = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z);
            pts[7] = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z);

            //Get them in GUI space
            pts[0] = UIElementPositionInViewportSpace(cam, pts[0], canvasRect);
            Vector3 min = pts[0];
            Vector3 max = pts[0];
            for (int i = 1; i < pts.Length; i++)
            {
                pts[i] = UIElementPositionInViewportSpace(cam, pts[i], canvasRect);
                min = Vector3.Min(min, pts[i]);
                max = Vector3.Max(max, pts[i]);
            }

            //Construct a rect of the min and max positions and apply some margin
            Rect rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
            return rect;
        }

        public static Vector3 CalculateAdjustedWorldPositionByCamera(Camera cam, Vector3 position)
        {
            //if the point is behind the camera then project it onto the camera plane
            Vector3 camNormal = cam.transform.forward;
            Vector3 vectorFromCam = position - cam.transform.position;
            float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);
            if (camNormDot <= 0f)
            {
                //we are beind the camera, project the position on the camera plane
                float camDot = Vector3.Dot(camNormal, vectorFromCam);
                Vector3 proj = (camNormal * camDot * Epsilon);
                position = cam.transform.position + (vectorFromCam - proj);
            }

            return position;
        }

        public static Vector2 UIElementPositionInViewportSpace(Camera uiCamera, Vector3 worldPosition, RectTransform canvasRect)
        {
            Vector2 objectViewportPosition = uiCamera.WorldToViewportPoint(worldPosition);
            Vector2 objectCanvasPosition = new Vector2(objectViewportPosition.x * canvasRect.rect.width,
                objectViewportPosition.y * canvasRect.rect.height);

            return objectCanvasPosition;
        }

        public static Vector2 WorldspacePositionInCanvasSpace(Camera viewingCamera, Vector3 worldspacePosition, RectTransform canvasRect)
        {
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
            worldspacePosition = CalculateAdjustedWorldPositionByCamera(viewingCamera, worldspacePosition);
            Vector2 objectViewportPosition = viewingCamera.WorldToViewportPoint(worldspacePosition);
            Vector2 objectCanvasPosition = new Vector2(((objectViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                                                                ((objectViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            return objectCanvasPosition;
        }
    }
}