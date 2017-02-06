using System.Collections.Generic;
using System.Diagnostics;
using com.goodgamestudios.warlands.uiWidgets.enums;
using com.goodgamestudios.warlands.uiWidgets.List;
using GGS.CakeBox.Utils;
using GGS.WMO.UI;
using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// A horizontal scrollpanel which 
    /// </summary>
    [ExecuteInEditMode]
    public abstract class UIHorizontalScrollPanel<T, D> : MonoBehaviour where T : UIListItem<D> where D : AListVO
    {
        [Header("Prefab Template")]
        [SerializeField]
        private T itemTemplate;

        [Header("ValueObject Template")]
        [SerializeField]
        private D valueObject;

        /// <summary>
        /// Should scrolling (via buttons) be snapped to items?
        /// </summary>
        [SerializeField]
        private bool snapToItem;

        /// <summary>
        /// The scroll steps (in items) when clicking the buttons
        /// </summary>
        [Range(1, 25)]
        [SerializeField]
        private int buttonScrollSteps;

        /// <summary>
        /// The number of maximal displayed items
        /// Less items are displayed when there are less <see cref="maxDisplayedItems"/> than  in the container
        /// </summary>
        [Range(1, 25)]
        [SerializeField]
        private float maxDisplayedItems;

        /// <summary>
        /// Button used as previous-button
        /// </summary>
        [SerializeField]
        private Button previousButton;

        /// <summary>
        /// Button used as next-button
        /// </summary>
        [SerializeField]
        private Button nextButton;

        /// <summary>
        /// The <see cref="UISnapableAutoScrollRect" /> used to display and scroll the items
        /// </summary>
        [SerializeField]
        private UISnapableAutoScrollRect scrollRect;

        /// <summary>
        /// Content panel of the <see cref="UISnapableAutoScrollRect" />, which contains the items
        /// </summary>
        [SerializeField]
        private RectTransform content;

        /// <summary>
        /// true to resize contents.
        /// </summary>
        [SerializeField]
        private bool resizeContents = true;

        /// <summary>
        /// true to center item.
        /// </summary>
        [SerializeField]
        private bool centerItem;

        [SerializeField]
        private bool useRectSize;

        [SerializeField]
        private int Id;


        private RectTransform selfRectTransform;
        private RectTransform scrollRectRectTransform;

        private float scrollRectWidth;

        private int itemCount;
        private int currentPosition;

        /// <summary>
        /// Gets the content with the scrolled items
        /// </summary>
        public RectTransform Content
        {
            get
            {
                return content;
            }
        }

        public virtual void Awake()
        {
            itemCount = 1;
            scrollRectWidth = -1;
            scrollRectRectTransform = scrollRect.GetComponent<RectTransform>();
            selfRectTransform = (RectTransform) transform;

            previousButton.onClick.AddListener(Prev);
            nextButton.onClick.AddListener(Next);

            
        }

        public void SetData(List<D> itemVOs)
        {
            for (int i = 0; i < maxDisplayedItems+1; i++)
            {
                D tempVO = itemVOs[i];

                GameObject obj = Instantiate(itemTemplate.gameObject);
                obj.transform.SetParent(content, false);

                obj.GetComponent<T>().PopulateData(tempVO);
            }
        }

        /// <summary>
        /// Initializes the component.
        /// Has to be called if children have been added/removed by code.
        /// </summary>
        public void Init()
        {
            RefreshWidget();
            UpdateContentPanelWidth(itemCount);
            UpdateScrollButtons(itemCount);
        }

        void Update()
        {
            EditorUpdate();
            if (resizeContents)
            {
                UpdateContentPanelWidth(itemCount);
            }

            UpdateScrollButtons(itemCount);
        }

        /// <summary>
        /// Scroll to position.
        /// </summary>
        /// <param name="direction">    1 to scroll right, -1 to scroll left. </param>
        /// <param name="position"> The position. </param>
        public void ScrollToPosition(UIHorizontalScrollDirection direction, int position)
        {
            currentPosition = position - 1;

            Update();

            float itemScrollRange = itemCount - maxDisplayedItems;
            float scrollRectPosition = scrollRect.horizontalNormalizedPosition;
            float diff = (centerItem) ? 1.25f : 1;
            float offset = (position - diff) / (float) itemScrollRange;
            float snapOffset = snapToItem ? scrollRectPosition % (1f / itemScrollRange) : 0;
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRectPosition + ((offset) * (float) direction) - snapOffset);
        }

        /// <summary>
        /// Updates the width of the content panel according to the amount of items taking the maxDisplayed items into account
        /// </summary>
        /// <param name="itemCount"></param>
        private void UpdateContentPanelWidth(int itemCount)
        {
            float width = scrollRectRectTransform.rect.width;
            if (scrollRectWidth != width)
            {
                scrollRectWidth = width;

                if (itemCount > maxDisplayedItems)
                {
                    float sizePerItem = (scrollRectWidth / maxDisplayedItems);

                    content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizePerItem * itemCount);
                }
                else
                {
                    content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollRectWidth);
                }
            }
        }

        /// <summary>
        /// Updates the visibility of the scroll buttons according to the scroll position
        /// </summary>
        /// <param name="itemCount"></param>
        private void UpdateScrollButtons(int itemCount)
        {
            bool enableArrows;
            if (useRectSize)
            {
                enableArrows = content.rect.width > selfRectTransform.rect.width * 1.1f; 
            }
            else
            {
                enableArrows = itemCount > maxDisplayedItems;
            }

            if (enableArrows)
            {
                float scrollPos = scrollRect.horizontalNormalizedPosition;
                float precisionDelta = (1f / itemCount) * 0.05f;
                previousButton.gameObject.SetActive(scrollPos > precisionDelta);
                nextButton.gameObject.SetActive(scrollPos < (1 - precisionDelta));
            }
            else
            {
                previousButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
            }
            
        }

        /// <summary>
        /// Only called in editor, forces the widget to refresh
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        private void EditorUpdate()
        {
            RefreshWidget();
        }

        public void RefreshWidget()
        {
            itemCount = content.transform.GetActiveChildCount();
            scrollRectRectTransform = scrollRect.GetComponent<RectTransform>();
            scrollRectWidth = -1;
        }


        private void Next()
        {
            Scroll(UIHorizontalScrollDirection.Right);
            currentPosition++;
        }

        private void Prev()
        {
            Scroll(UIHorizontalScrollDirection.Left);
            currentPosition--;
        }

        /// <summary>
        /// Scroll to the specified direction.
        /// Uses the defined <see cref="buttonScrollSteps"/> and takes the <see cref="snapToItem"/>-setting into account.
        /// </summary>
        /// <param name="direction">1 to scroll right, -1 to scroll left</param>
        private void Scroll(UIHorizontalScrollDirection direction)
        {
            float itemScrollRange = itemCount - maxDisplayedItems;
            float scrollRectPosition = scrollRect.horizontalNormalizedPosition;
            float centerOffset = (currentPosition == 0) ? -0.25f : 0.75f;
            float diff = (centerItem) ? centerOffset : 0.01f; // there are rounding errors, which breaks the ability to scroll with the buttons. We use 0.01f for a workaround.
            float offset = (buttonScrollSteps + (diff * (float) direction)) / itemScrollRange;
            float snapOffset = snapToItem ? scrollRectPosition % (1f / itemScrollRange) : 0;
            scrollRect.ScrollTarget = Mathf.Clamp01(scrollRectPosition + ((offset) * (float) direction) - snapOffset);
        }
    }
}