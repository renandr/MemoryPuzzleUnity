using System.Collections.Generic;
using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// This class places all the children object of this RecTransform into a circle-like shape.
    /// </summary>
    [ExecuteInEditMode]
    public class UICircularList : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private float radius;

        [SerializeField]
        private float angleSpacing;

        [SerializeField]
        private float startAngle;

        [SerializeField]
        private bool counterClockWise;
#pragma warning restore 649

        /// <summary>
        /// The radius for the circle
        /// </summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        /// <summary>
        /// The spacing between each element, in degrees
        /// </summary>
        public float AngleSpacing
        {
            get
            {
                return angleSpacing;
            }
            set
            {
                angleSpacing = value;
            }
        }

        /// <summary>
        /// The angle where the elements start being positioned
        /// </summary>
        public float StartAngle
        {
            get
            {
                return startAngle;
            }
            set
            {
                startAngle = value;
            }
        }

        private List<RectTransform> elements;
        private RectTransform rectTransform;

        private void OnEnable()
        {
            Initialize();
        }

        /// <summary>
        /// Called by Unity when hierarchy of this object's children is called.
        /// </summary>
        private void OnTransformChildrenChanged()
        {
            RepositionNow();
        }

        /// <summary>
        /// Called by Unity when the monobehaviour's values change. This calls RepositionNow();
        /// </summary>
        private void OnValidate()
        {
            RepositionNow();
        }

        /// <summary>
        /// Initializes whatever is needed for the list to work.
        /// </summary>
        private void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = gameObject.GetComponent<RectTransform>();
            }

            RefreshElements();
        }

        /// <summary>
        /// Refreshes the list of elements. Only considers active elements.
        /// </summary>
        public void RefreshElements()
        {
            if (rectTransform == null)
            {
                rectTransform = gameObject.GetComponent<RectTransform>();
            }
            elements = new List<RectTransform>();
            foreach (Transform child in rectTransform)
            {
                if (child.gameObject.activeSelf && child is RectTransform)
                {
                    elements.Add((RectTransform) child);
                }
            }
        }

        /// <summary>
        /// Returns the X,Y position for an element given a radius and an angle (in degrees)
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Vector2 ToCircle(float radius, float angle)
        {
            float rad = Mathf.Deg2Rad * angle;
            return new Vector2(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad));
        }

        /// <summary>
        /// Refreshes the elements and repositions all objects
        /// </summary>
        public void RepositionNow()
        {
            RefreshElements();
            float angle = startAngle;

            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].anchoredPosition = ToCircle(radius, angle);
                if (counterClockWise)
                {
                    angle += angleSpacing;
                }
                else
                {
                    angle -= angleSpacing;
                }
            }
        }
    }
}