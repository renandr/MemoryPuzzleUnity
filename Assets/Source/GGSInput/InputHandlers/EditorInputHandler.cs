using System.Diagnostics;
using GGS.CakeBox.Utils;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GGS.CakeBox.GGSInput
{
    /// <summary>
    /// Input handler used in the editor.
    /// Works with mouse and keyboard.
    /// Always disabled on devices and in non-debug mode
    /// </summary>
    public class EditorInputHandler : MonoBehaviour
    {
        public float KeyboardScrollSpeedFactor = 0.05f;

        private bool isSingleDown;

        private bool noRotationAxisDefined;

#if DEBUG
        public void Update()
        {
            EditorUpdate();
        }
#endif

        [Conditional("UNITY_EDITOR")]
        private void EditorUpdate()
        {
            const float zoomSpeed = 20f;

            float zoomWheel = Input.GetAxis("Mouse ScrollWheel");
            float zoomSpeedFactor = 1f;

            if (zoomWheel != 0)
            {
                zoomSpeedFactor = Mathf.Abs(zoomWheel) * 200f;
            }

            if (!noRotationAxisDefined)
            {
                float rotationDelta;
                try
                {
                    rotationDelta = Input.GetAxis("Rotation");
                }
                catch
                {
                    noRotationAxisDefined = true;
                    rotationDelta = 0;
                    Debug.LogWarning("'Rotation' input axis not defined. Define it to use rotation in editor.");
                }

                if (rotationDelta != 0f)
                {
                    InputManager.AddInputEvent(InputEventType.Rotate, Vector2.zero, rotationDelta);
                }
            }

            if (Input.GetKey(KeyCode.Comma) || zoomWheel < 0)
            {
                // zoom out
                InputManager.AddInputEvent(InputEventType.Pinch, Vector2.zero, zoomSpeed * zoomSpeedFactor * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.Period) || zoomWheel > 0)
            {
                // ZoomType in
                InputManager.AddInputEvent(InputEventType.Pinch, Vector2.zero, - zoomSpeed * zoomSpeedFactor * Time.deltaTime);
            }

            if (Input.GetMouseButton(0))
            {
                if (!UIUtils.IsScreenPositionOverUI(Input.mousePosition))
                {

                    Vector2 mousePosition = Input.mousePosition;
                    if (!isSingleDown)
                    {
                        InputManager.AddInputEvent(InputEventType.SingleDown, mousePosition);
                        isSingleDown = true;
                    }
                    InputManager.AddInputEvent(InputEventType.SinglePressed, mousePosition);

                    float mx = -Input.GetAxis("Mouse X");
                    float my = -Input.GetAxis("Mouse Y");
                    Vector3 mouseDeltaVector = new Vector3(mx / Screen.width, 0, my / Screen.height);
                    InputManager.AddInputEvent(InputEventType.Drag, mousePosition, mouseDeltaVector);
                }
            }
            else if (isSingleDown)
            {
                isSingleDown = false;
                float mx = -Input.GetAxis("Mouse X");
                float my = -Input.GetAxis("Mouse Y");
                Vector3 mouseDeltaVector = new Vector3(mx, 0, my);
                InputManager.AddInputEvent(InputEventType.SingleUp, Input.mousePosition, mouseDeltaVector);
            }

            KeyboardScroll();
        }

        private void KeyboardScroll()
        {
            float xDelta = Input.GetAxis("Horizontal");
            float zDelta = Input.GetAxis("Vertical");

            if (xDelta != 0f || zDelta != 0f)
            {
                InputManager.AddInputEvent(InputEventType.Drag, Vector2.zero, new Vector3(xDelta * KeyboardScrollSpeedFactor, 0, zDelta * KeyboardScrollSpeedFactor));
                InputManager.AddInputEvent(InputEventType.TwoFingerDrag, Vector2.zero, new Vector3(xDelta * KeyboardScrollSpeedFactor, 0, zDelta * KeyboardScrollSpeedFactor));
            }
        }
    }
}
