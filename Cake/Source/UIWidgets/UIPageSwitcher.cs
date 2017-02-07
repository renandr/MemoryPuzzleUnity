using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// A component to switch through pages.
    /// Provides two buttons for switching and a text display the current page and the total number of pages.
    /// The pages which are switched are saved in an assigned <see cref="container"/>.
    /// </summary>
    public class UIPageSwitcher : MonoBehaviour
    {
        /// <summary>
        /// Container for the pages
        /// </summary>
#pragma warning disable 649
        [SerializeField]
        private Transform container;

        /// <summary>
        /// Text to indicate on which page you are and how many pages there are
        /// </summary>
        [SerializeField]
        private Text pageText;

        /// <summary>
        /// Button to go to the previous page
        /// </summary>
        [SerializeField]
        private Button previousPageButton;

        /// <summary>
        /// Button to go to the next page
        /// </summary>
        [SerializeField]
        private Button nextPageButton;
#pragma warning restore 649


        private int currentPage;

        private GameObject[] pages;

        /// <summary>
        /// Gets or sets the currently active page.
        /// Updates the button states and hides/shows the corresponding page.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;

                for (var i = 0; i < pages.Length; i++)
                {
                    pages[i].SetActive(i == currentPage);
                }

                pageText.text = (currentPage + 1) + " of " + pages.Length;
                previousPageButton.interactable = (CurrentPage != 0);
                nextPageButton.interactable = (CurrentPage != pages.Length - 1);
            }
        }

        /// <summary>
        /// The number of pages (readonly)
        /// </summary>
        public int Pages
        {
            get
            {
                return pages.Length;
            }
        }

        public void Awake()
        {
            CachePages();
            CurrentPage = 0;
        }

        /// <summary>
        /// Caches the page items in an array
        /// </summary>
        private void CachePages()
        {
            var i = 0;
            pages = new GameObject[container.childCount];
            foreach (Transform page in container)
            {
                pages[i] = page.gameObject;
                i++;
            }
        }

        public void OnEnable()
        {
            previousPageButton.onClick.AddListener(OnPreviousClick);
            nextPageButton.onClick.AddListener(OnNextClick);
        }

        public void OnDisable()
        {
            previousPageButton.onClick.RemoveAllListeners();
            nextPageButton.onClick.RemoveAllListeners();
        }

        private void OnPreviousClick()
        {
            CurrentPage = Mathf.Max(CurrentPage - 1, 0);
        }

        private void OnNextClick()
        {
            CurrentPage = Mathf.Min(CurrentPage + 1, pages.Length - 1);
        }
    }
}
