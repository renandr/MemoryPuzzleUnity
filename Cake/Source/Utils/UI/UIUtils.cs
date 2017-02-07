using System.Collections.Generic;
using GGS.CakeBox.Logging;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// UI related utilities
    /// </summary>
    public static class UIUtils
    {
        #region Cache Variables

        private static EventSystem eventSystem;

        private static readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

        private static PointerEventData pointerEventData;

        #endregion

        #region Methods / Properties

        /// <summary>
        /// Property to get/set the event system.
        /// If this is not set manually, <see cref="IsScreenPositionOverUI"/> will use the current event system.
        /// </summary>
        public static EventSystem EventSystem
        {
            get { return eventSystem; }
            set
            {
                eventSystem = value;
                pointerEventData = new PointerEventData(eventSystem);
            }
        }

        /// <summary>
        /// Check if a point is over the UI.
        /// Will use the current event system or the event system set with the <see cref="EventSystem"/> property.
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>true if over UI, false otherwise</returns>
        public static bool IsScreenPositionOverUI(Vector2 position)
        {
            // Try to get the event system
            if (eventSystem == null)
            {
                EventSystem = EventSystem.current;
                if (eventSystem == null)
                {
                    return false;
                }
            }

            pointerEventData.position = position;
            eventSystem.RaycastAll(pointerEventData, raycastResults);

            //
            return raycastResults.Count > 0;
        }

        #endregion
    }

}