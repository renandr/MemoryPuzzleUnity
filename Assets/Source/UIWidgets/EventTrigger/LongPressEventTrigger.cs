using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace com.goodgamestudios.warlands.uiWidgets.eventTrigger
{
    /// <summary>
    /// An event trigger UIBehaviou to handle long presses on buttons.
    /// </summary>
    /// <seealso cref="T:UnityEngine.EventSystems.UIBehaviour"/>
    /// <seealso cref="T:UnityEngine.EventSystems.IPointerDownHandler"/>
    /// <seealso cref="T:UnityEngine.EventSystems.IPointerUpHandler"/>
    /// <seealso cref="T:UnityEngine.EventSystems.IPointerExitHandler"/>
    public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Tooltip("How long must pointer be down on this object to trigger a long press")]
        [SerializeField]
        private float durationThreshold = 1.0f;

        [SerializeField]
        private UnityEvent onLongPress = new UnityEvent();

        [SerializeField]
        private UnityEvent onPointerExit = new UnityEvent();

        [SerializeField]
        private UnityEvent onPointerDown = new UnityEvent();

        private bool isPointerDown;
        private bool longPressTriggered;
        private float timePressStarted;

        /// <summary>
        /// Returns the current duration of the press
        /// </summary>
        /// <value>
        /// The current duration.
        /// </value>
        public float CurrentDuration
        {
            get
            {
                return (Time.time - timePressStarted) / durationThreshold;
            }
        }

        public void Update()
        {
            if (isPointerDown && !longPressTriggered && Time.time - timePressStarted > durationThreshold)
            {
                longPressTriggered = true;
                onLongPress.Invoke();
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown.Invoke();

            timePressStarted = Time.time;
            isPointerDown = true;
            longPressTriggered = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerDown = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerDown = false;

            onPointerExit.Invoke();
        }
    }
}