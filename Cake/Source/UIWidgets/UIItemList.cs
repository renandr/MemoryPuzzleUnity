using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// A direction for the list, organized clockwise
    /// </summary>
    public enum ListDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    /// <summary>
    /// Generic component for automatically positioning list objects
    /// </summary>
    [ExecuteInEditMode]
    public class UIItemList : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        protected ListDirection direction = ListDirection.Down;

        [SerializeField]
        protected bool useWidgetSize = true;

        [SerializeField]
        protected bool allowDifferentSizedElements;

        [SerializeField]
        protected float elementSize;

        [SerializeField]
        protected float spacing;

        [SerializeField]
        protected float additionalSpace;

//        [SerializeField]
//        protected PointerAnimationComponent focusedIndicatorTemplate;
#pragma warning restore 649

//        private PointerAnimationComponent currentIndicator;
        protected List<RectTransform> elements;
        protected int previousElementCount;
        protected RectTransform rectTransform;
        protected float previousSpacing;
        protected float previousAdditionalSpace;

        /// <summary>
        /// Gets or sets the additional empty space in the end of the list.
        /// This is can be used if you want extra space in a scroll rect, for example.
        /// </summary>
        /// <value>
        /// The additional empty space.
        /// </value>
        public float AdditionalEmptySpace
        {
            get { return additionalSpace; }
            set
            {
                previousAdditionalSpace = additionalSpace;
                additionalSpace = value;
                ResizeIfNeeded();
            }
        }

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
        /// Initializes whatever is needed for the list to work.
        /// </summary>
        protected virtual void Initialize()
        {
            if (rectTransform == null)
            {
                rectTransform = gameObject.GetComponent<RectTransform>();
            }

            rectTransform.anchoredPosition = Vector3.zero;
            RefreshElements();
        }

        /// <summary>
        /// Refreshes the list of elements. Only considers active elements.
        /// </summary>
        protected virtual void RefreshElements()
        {
            elements = new List<RectTransform>(transform.childCount);
            if (rectTransform == null)
            {
                rectTransform = gameObject.GetComponent<RectTransform>();
            }
            foreach (Transform child in rectTransform)
            {
                if (child.gameObject.activeSelf && child.GetType().Equals(typeof(RectTransform)))
                {
                    elements.Add((RectTransform)child);
                }
            }
        }

        /// <summary>
        /// Adds an element to the list
        /// </summary>
        /// <param name="element">The element to be added</param>
        public void AddElement(RectTransform element)
        {
            element.SetParent(transform, false);
            elements.Add(element);
            RepositionNow();
        }

        /// <summary>
        /// Removes an element from the list
        /// </summary>
        /// <param name="element">The element to be removed</param>
        public void RemoveElement(RectTransform element)
        {
            if (elements.Contains(element))
            {
                elements.Remove(element);
                DestroyImmediate(element.gameObject);
                RepositionNow();
            }
        }

        /// <summary>
        /// Returns an element from the list
        /// </summary>
        /// <param name="index">the index of the element</param>
        /// <returns>The element to be returned</returns>
        public RectTransform GetElementAt(int index)
        {
            return elements[index];
        }

        /// <summary>
        /// Number of elements.
        /// </summary>
        /// <returns>
        /// The total number of elements.
        /// </returns>
        public int NumOfElements()
        {
            if (elements != null)
            {
                return elements.Count;
            }
            return 0;
        }

        /// <summary>
        /// Removes all elements from the list
        /// </summary>
        public void RemoveAllElements()
        {
            if (elements == null)
            {
                return;
            }
            foreach (RectTransform element in elements)
            {
                DestroyImmediate(element.gameObject);
            }
            elements.Clear();
            RepositionNow();
        }

        /// <summary>
        /// Calculates the size of a line/column
        /// </summary>
        protected virtual void CalculateLineHeight()
        {
            if (useWidgetSize)
            {
                if (rectTransform.childCount > 0 && rectTransform.GetChild(0) != null)
                {
                    elementSize = GetSize(rectTransform.GetChild(0).GetComponent<RectTransform>()) + spacing;
                }
            }
        }

        /// <summary>
        /// Resizes the parent panel if needed
        /// </summary>
        protected virtual void ResizeIfNeeded(bool forceResize = false)
        {
            if (forceResize || elements.Count != previousElementCount || spacing != previousSpacing || additionalSpace != previousAdditionalSpace || !useWidgetSize)
            {
                Resize();
            }
        }

        public void Resize()
        {
            previousSpacing = spacing;
            previousAdditionalSpace = additionalSpace;
            var newSize = elements.Count * elementSize;
            if (allowDifferentSizedElements)
            {
                newSize = 0f;
                for (var i = 0; i < elements.Count; i++)
                {
                    newSize += GetSize(elements[i]);
                }
            }

            newSize += additionalSpace;

            if (direction == ListDirection.Left || direction == ListDirection.Right)
            {
                (transform as RectTransform).sizeDelta = new Vector2(newSize, rectTransform.sizeDelta.y);
            }
            else
            {
                (transform as RectTransform).sizeDelta = new Vector2(rectTransform.sizeDelta.x, newSize);
            }
        }

        /// <summary>
        /// Scrolls down to the bottom of the list, without animation.
        /// </summary>
        public void JumpToBottom()
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }

        /// <summary>   Reposition now. </summary>
        /// <param name="forceResize">  
        /// Force a new size calculation. This is only necessary if the
        /// element count stays equal but the element sizes changed. </param>
        public virtual void RepositionNow(bool forceResize = false)
        {
            RefreshElements();
            CalculateLineHeight();
            ResizeIfNeeded(forceResize);

            float signal = 1;
            if (direction == ListDirection.Left || direction == ListDirection.Down)
            {
                signal = -1;
            }

            var position = 0f;
            var newPosition = Vector2.zero;
            for (int i = 0; i < elements.Count; i++)
            {
                if (direction == ListDirection.Left || direction == ListDirection.Right)
                {
                    newPosition.x = position * signal;
                }
                else
                {
                    newPosition.y = position * signal;
                }
                elements[i].anchoredPosition = newPosition;

                if (allowDifferentSizedElements)
                {
                    elementSize = GetSize(elements[i]);
                }
                position += elementSize;
            }
            previousElementCount = elements.Count;
        }

        /// <summary>
        /// Recalculates the list and repositions its elements on late update.
        /// When updating list elements and calling <see cref="RepositionNow"/> directly afterwards, it doesn't work properly in some cases.
        /// Use this function instead for these cases.
        /// </summary>
        /// <param name="forceResize">Force a new size calculation. This is only necessary if the element count stays equal but the element sizes changed.</param>
        public void RepositionOnLateUpdate(bool forceResize = false)
        {
            StopAllCoroutines();
            StartCoroutine(RepositionOnLateUpdateCoroutine(forceResize));
        }

        /// <summary>
        /// Focus on item.
        /// </summary>
        /// <param name="index">    the index of the element. </param>
        /// <returns>
        /// A GameObject.
        /// </returns>
        public GameObject FocusOnItem(int index)
        {
            if (index >= elements.Count)
            {
                return null;
            }
            float offset = 0;
            for (int i = 0; i < index; i++)
            {
                offset += elements[i].sizeDelta.y;
            }
            Vector3 pos = rectTransform.anchoredPosition;
            pos.y = offset;
            rectTransform.anchoredPosition = pos;

            return elements[index].gameObject;
        }

        /// <summary>
        /// Adds a scroll offset to the current scrolling position
        /// </summary>
        /// <param name="offset">The scroll offset</param>
        public void Scroll(Vector2 offset)
        {
            Vector3 pos = rectTransform.anchoredPosition;
            pos.x += offset.x;
            pos.y += offset.y;
            rectTransform.anchoredPosition = pos;
        }

        private IEnumerator RepositionOnLateUpdateCoroutine(bool forceResize)
        {
            yield return new WaitForEndOfFrame();
            RepositionNow(forceResize);
        }

        /// <summary>
        /// Gets the size of the space that should be added between elements: width if it's a horizontal, height if it's
        /// vertical
        /// </summary>
        /// <param name="rectTransform">The transform to get the size of</param>
        /// <returns>The rect size (either width or height based on how its configured)</returns>
        protected virtual float GetSize(RectTransform rectTransform)
        {
            if (direction == ListDirection.Left || direction == ListDirection.Right)
            {
                return rectTransform.rect.width;
            }
            return rectTransform.rect.height;
        }
    }
}