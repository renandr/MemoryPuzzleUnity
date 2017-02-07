using UnityEngine;

namespace GGS.CakeBox.GGSInput
{
    /// <summary>
    /// Struct holding all data of an input event
    /// </summary>
    public struct InputEventData
    {
        private static readonly Vector3 defaultVector3 = new Vector3();

        /// <summary>
        /// The event type
        /// </summary>
        public readonly InputEventType EventType;

        /// <summary>
        /// The clicked/touched screen position
        /// </summary>
        public readonly Vector2 ScreenPosition;

        /// <summary>
        /// The float value
        /// </summary>
        public readonly float FloatValue;

        /// <summary>
        /// The vector3 value
        /// </summary>
        public readonly Vector3 Vector3Value;

        public InputEventData(InputEventType eventType, Vector2 screenPosition)
        {
            EventType = eventType;
            ScreenPosition = screenPosition;
            FloatValue = 0f;
            Vector3Value = defaultVector3;
        }

        public InputEventData(InputEventType eventType, Vector2 screenPosition, float floatValue)
        {
            EventType = eventType;
            ScreenPosition = screenPosition;
            FloatValue = floatValue;
            Vector3Value = defaultVector3;
        }

        public InputEventData(InputEventType eventType, Vector2 screenPosition, Vector3 vector3Value)
        {
            EventType = eventType;
            ScreenPosition = screenPosition;
            FloatValue = 0f;
            Vector3Value = vector3Value;
        }

        public InputEventData(InputEventType eventType, Vector2 screenPosition, float floatValue, Vector3 vector3Value)
        {
            EventType = eventType;
            ScreenPosition = screenPosition;
            FloatValue = floatValue;
            Vector3Value = vector3Value;
        }
    }
}