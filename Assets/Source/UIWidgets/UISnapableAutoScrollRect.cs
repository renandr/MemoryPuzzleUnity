using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// Enhances the unity ScrollRect with snap and auto scroll functionality
    /// </summary>
    public class UISnapableAutoScrollRect : ScrollRect
    {
        /// <summary>
        /// We need this to circumvent rounding errors
        /// </summary>
        private const float FloatScrollingPrecisionDelta = 0.001f;

        /// <summary>
        /// The speed for automatic scrolling
        /// </summary>
        [SerializeField]
        private float scrollSpeed = 75f;

        /// <summary>
        /// Damping factor for smooth automatic scrolling
        /// </summary>
        [SerializeField]
        private float damping = 0.05f;

        private bool autoScroll;
        private float scrollTarget;
        
        /// <summary>
        /// Set or get the target position for automatic scrolling.
        /// </summary>
        public float ScrollTarget
        {
            get
            {
                return scrollTarget;
            }
            set
            {
                scrollTarget = value;
                autoScroll = true;
            }
        }

        #region Overrides of ScrollRect

        /// <summary>
        /// Executed when the user starts dragging the rect.
        /// In this case automatic scrolling needs to be disabled.
        /// </summary>
        /// <param name="eventData">Required for base method</param>
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            autoScroll = false;
        }

        #endregion

        /// <summary>
        /// When auto scroll is enabled scroll to the position defined with <see cref="ScrollTarget" />
        /// </summary>
        protected void Update()
        {
            if (autoScroll)
            {
                DoAutoScroll();
            }
        }

        private void DoAutoScroll()
        {
            if (Mathf.Abs(horizontalNormalizedPosition - scrollTarget) > FloatScrollingPrecisionDelta)
            {
                float delta = (scrollTarget - (horizontalNormalizedPosition)) * (Time.deltaTime * scrollSpeed);
                horizontalNormalizedPosition = Mathf.Clamp01(horizontalNormalizedPosition + delta * damping);
            }
            else
            {
                horizontalNormalizedPosition = scrollTarget;
                autoScroll = false;
            }
        }
    }
}