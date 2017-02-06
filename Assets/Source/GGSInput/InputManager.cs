using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGS.CakeBox.GGSInput
{
    /// <summary>
    /// Manager which processes enqueued input events and runs the corresponding actions.
    /// There are two possible ways to use this:
    ///     1) Either add this script to a game object to let it process events automatically on LateUpdate
    ///     2) Call <see cref="HandleEvents"/> manually
    /// 
    /// An input handler is required to trigger input events.
    /// Functions have to be attached to the actions to react on input.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        #region Input Actions

        public static Action<InputEventData> OnSingleDown;
        public static Action<InputEventData> OnSinglePressed;
        public static Action<InputEventData> OnSingleUp;

        public static Action<InputEventData> OnDrag;
        public static Action<InputEventData> OnTwoFingerDrag;
        public static Action<InputEventData> OnThreeFingerDrag;

        public static Action<InputEventData> OnPinch;
        public static Action<InputEventData> OnRotate;

        #endregion

        /// <summary>
        /// The event queue
        /// </summary>
        private static readonly List<InputEventData> events = new List<InputEventData>();

        #region Event add functions

        public static void AddInputEvent(InputEventData eventData)
        {
            events.Add(eventData);
        }

        public static void AddInputEvent(InputEventType type, Vector2 screenPosition)
        {
            events.Add(new InputEventData(type, screenPosition));
        }

        public static void AddInputEvent(InputEventType type, Vector2 screenPosition, float value)
        {
            events.Add(new InputEventData(type, screenPosition, value));
        }

        public static void AddInputEvent(InputEventType type, Vector2 screenPosition, Vector3 value)
        {
            events.Add(new InputEventData(type, screenPosition, value));
        }

        #endregion

        #region Unity Callbacks

        void LateUpdate()
        {
            HandleEvents();
        }

        #endregion

        public static void HandleEvents()
        {
            foreach (InputEventData eventData in events)
            {
                switch (eventData.EventType)
                {
                    case InputEventType.None:
                        break;
                    case InputEventType.SingleDown:
                        if (OnSingleDown != null)
                        {
                            OnSingleDown(eventData);
                        }
                        break;
                    case InputEventType.SinglePressed:
                        if (OnSinglePressed != null)
                        {
                            OnSinglePressed(eventData);
                        }
                        break;
                    case InputEventType.SingleUp:
                        if (OnSingleUp != null)
                        {
                            OnSingleUp(eventData);
                        }
                        break;
                    case InputEventType.Drag:
                        if (OnDrag != null)
                        {
                            OnDrag(eventData);
                        }
                        break;
                    case InputEventType.TwoFingerDrag:
                        if (OnTwoFingerDrag != null)
                        {
                            OnTwoFingerDrag(eventData);
                        }
                        break;
                    case InputEventType.ThreeFingerDrag:
                        if (OnThreeFingerDrag != null)
                        {
                            OnThreeFingerDrag(eventData);
                        }
                        break;
                    case InputEventType.Pinch:
                        if (OnPinch != null)
                        {
                            OnPinch(eventData);
                        }
                        break;
                    case InputEventType.Rotate:
                        if (OnRotate != null)
                        {
                            OnRotate(eventData);
                        }
                        break;
                }
            }

            events.Clear();
        }
    }
}
