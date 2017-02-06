using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Base class for every call out component. 
    /// </summary>
    public abstract class ACallOutComponent : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// The panel id to open when call out is clicked.
        /// </summary>
        public abstract string CallOutPanelId { get; }

        [SerializeField]
        private bool interactable = true;

        private IScreen screen;

        private IScreen Screen
        {
            get
            {
                if (screen == null)
                {
                    screen = GetComponentInParent<IScreen>();
                }

                return screen;
            }
        }

        /// <summary>
        /// True if the call out should react on clicks else false.
        /// </summary>
        public bool Interactable
        {
            get
            {
                return interactable;
            }

            set
            {
                interactable = value;
            }
        }

        /// <summary>
        /// Set the value for Interactable a frame delayed. Use this
        /// to prevent directly reopening the call out when clicking.
        /// </summary>
        /// <param name="value"></param>
        public void SetInteractableDelayed(bool value)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(DelayedInteractionSet(value));
            }
            else
            {
                interactable = value;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }

            IScreen target = Screen;
            if (target != null)
            {
                target.OnCallOutActivated(this);
            }
        }

        private IEnumerator DelayedInteractionSet(bool value)
        {
            yield return null;
            interactable = value;
        }

        /// <summary>
        /// Pointer click won't work without this method. Do not remove.
        /// </summary>
        /// <param name="eventData">The event data</param>
        public void OnPointerDown(PointerEventData eventData)
        {
        }

        /// <summary>
        /// Pointer click won't work without this method. Do not remove.
        /// </summary>
        /// <param name="eventData">The event data</param>
        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}