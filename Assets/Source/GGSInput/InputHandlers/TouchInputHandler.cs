using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.CakeBox.GGSInput
{
    /// <summary>
    /// Input handler working with touch gestures
    /// </summary>
    public class TouchInputHandler : MonoBehaviour
    {
        #region Settings

        [SerializeField]
        public bool ShowDebugText;

        #endregion

        private readonly Touch?[] prevTouches = { null, null, null };

        private readonly Touch?[] curTouches = { null, null, null };

        private Vector3 lastTouchDelta;

        private Vector3 v;

        /// <summary>
        /// Was more than one finger involved at any time in the last touch operation
        /// This is reset only when all fingers are released
        /// </summary>
        private bool multiTouch;

        private float xAngle;

        private bool rotating;
        private Vector2 startVector;
        private float rotGestureWidth = 200f;
        private float rotAngleMinimum = 0f;

        private void Update()
        {
            switch (Input.touchCount)
            {
                case 0:
                    curTouches[0] = null;
                    curTouches[1] = null;
                    curTouches[2] = null;

                    HandleSingleUp();

                    rotating = false;
                    multiTouch = false;
                    break;
                case 1:
                    curTouches[0] = Input.GetTouch(0);
                    curTouches[1] = null;
                    curTouches[2] = null;

                    HandleSingleTouch();
                    HandleSinglePressed();

                    rotating = false;
                    break;
                case 2:
                    curTouches[0] = Input.GetTouch(0);
                    curTouches[1] = Input.GetTouch(1);
                    curTouches[2] = null;

                    HandlePinch();
                    HandleMultiFingerDrag(InputEventType.TwoFingerDrag);
                    HandleRotation();

                    multiTouch = true;
                    break;
                case 3:
                    curTouches[0] = Input.GetTouch(0);
                    curTouches[1] = Input.GetTouch(1);
                    curTouches[2] = Input.GetTouch(2);

                    HandleMultiFingerDrag(InputEventType.ThreeFingerDrag);
                    rotating = false;
                    multiTouch = true;
                    break;
                default:
                    rotating = false;
                    multiTouch = true;
                    break;
            }

            prevTouches[0] = curTouches[0];
            prevTouches[1] = curTouches[1];
            prevTouches[2] = curTouches[2];
        }

#if DEBUG
        private void OnGUI()
        {
            if (ShowDebugText)
            {
                RenderDebugText();
            }
        }
#endif

        private void RenderDebugText()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                var pos = touch.position;

                if (!UIUtils.IsScreenPositionOverUI(pos))
                {
                    GUI.Label(new Rect(pos.x, Screen.height - pos.y - 110, 200, 50), "Position: " + pos);
                    GUI.Label(new Rect(pos.x, Screen.height - pos.y - 90, 200, 50), "Finger ID: " + touch.fingerId);
                    GUI.Label(new Rect(pos.x, Screen.height - pos.y - 70, 200, 50), "Position Change: " + touch.deltaPosition);
                    GUI.Label(new Rect(pos.x, Screen.height - pos.y - 50, 200, 50), "Time Passed: " + touch.deltaTime);
                    GUI.Label(new Rect(pos.x, Screen.height - pos.y - 30, 200, 50), "Tap Count: " + touch.tapCount);
                    GUI.Label(new Rect(pos.x, Screen.height - pos.y - 10, 200, 50), "Phase: " + touch.phase);
                }
            }
        }

        private void HandleRotation()
        {
            // Store both touches.
            if (!curTouches[0].HasValue || !curTouches[1].HasValue)
            {
                return;
            }
            Touch touchZero = curTouches[0].Value;
            Touch touchOne = curTouches[1].Value;

            if (!rotating)
            {
                startVector = touchOne.position - touchZero.position;
                rotating = startVector.sqrMagnitude > rotGestureWidth * rotGestureWidth;
            }
            else
            {
                var currVector = touchOne.position - touchZero.position;
                var angleOffset = Vector2.Angle(startVector, currVector);
                var LR = Vector3.Cross(startVector, currVector);

                if (angleOffset > rotAngleMinimum)
                {
                    if (LR.z > 0)
                    {
                        // Anticlockwise turn equal to angleOffset.
                        InputManager.AddInputEvent(InputEventType.Rotate, touchZero.position, angleOffset);
                    }
                    else if (LR.z < 0)
                    {
                        // Clockwise turn equal to angleOffset.
                        InputManager.AddInputEvent(InputEventType.Rotate, touchZero.position, -angleOffset);
                    }

                    startVector = currVector;
                }
            }
        }

        private void HandlePinch()
        {
            // Store both touches.
            if (!curTouches[0].HasValue || !curTouches[1].HasValue)
            {
                return;
            }
            Touch touchZero = curTouches[0].Value;
            Touch touchOne = curTouches[1].Value;

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            InputManager.AddInputEvent(InputEventType.Pinch, touchZero.position, deltaMagnitudeDiff);
        }

        private void HandleSingleTouch()
        {
            // single drag
            if (prevTouches[0].HasValue && curTouches[0].HasValue)
            {
                var currentTouchPos = curTouches[0].Value.position;
                if (!multiTouch && !UIUtils.IsScreenPositionOverUI(currentTouchPos))
                {
                    var touchDelta = prevTouches[0].Value.position - currentTouchPos;

                    lastTouchDelta = new Vector3(touchDelta.x / Screen.width, 0, touchDelta.y / Screen.height);

                    InputManager.AddInputEvent(InputEventType.Drag, currentTouchPos, lastTouchDelta);
                }
            }

            // single down
            else if (prevTouches[0] == null && curTouches[0].HasValue)
            {
                Vector3 touchPosition = curTouches[0].Value.position;
                if (!UIUtils.IsScreenPositionOverUI(touchPosition))
                {
                    InputManager.AddInputEvent(InputEventType.SingleDown, touchPosition);
                }
            }
        }

        private void HandleMultiFingerDrag(InputEventType eventType)
        {
            if (prevTouches[0].HasValue && curTouches[0].HasValue)
            {
                var currentTouchPos = curTouches[0].Value.position;
                if (!UIUtils.IsScreenPositionOverUI(currentTouchPos))
                {
                    var touchDelta = prevTouches[0].Value.position - currentTouchPos;

                    lastTouchDelta = new Vector3(touchDelta.x / Screen.width, 0, touchDelta.y / Screen.height);

                    InputManager.AddInputEvent(eventType, currentTouchPos, lastTouchDelta);
                }
            }
        }

        private void HandleSinglePressed()
        {
            if (curTouches[0].HasValue)
            {
                Vector2 touchPosition = curTouches[0].Value.position;
                if (!UIUtils.IsScreenPositionOverUI(touchPosition))
                {
                    InputManager.AddInputEvent(InputEventType.SinglePressed, touchPosition);
                }
            }
        }

        private void HandleSingleUp()
        {
            // single up
            if (prevTouches[0].HasValue && curTouches[0] == null)
            {
                v = lastTouchDelta / Time.deltaTime;

                InputManager.AddInputEvent(InputEventType.SingleUp, prevTouches[0].Value.position, v);
            }
        }
    }
}
