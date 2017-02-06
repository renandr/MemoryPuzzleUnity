using GGS.CakeBox.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// A progressbar with steps, a status text and a description text
    /// </summary>
    [ExecuteInEditMode]
    public class UIProgressBarSteps : MonoBehaviour
    {
        /// <summary>
        /// The number of steps to display
        /// </summary>
#pragma warning disable 649
        [Range(0, 50)]
        [SerializeField]
        private int steps;

        /// <summary>
        /// A slider used to display the progressbar
        /// </summary>
        [SerializeField]
        private Slider progressBar;

        /// <summary>
        /// Background of the progress bar
        /// </summary>
        [SerializeField]
        private RectTransform progressBarBG;

        /// <summary>
        /// Image used to display steps
        /// </summary>
        [SerializeField]
        private Image stepImage;

        /// <summary>
        /// A container used as parent for the step images, overlaying the pogress bar
        /// </summary>
        [SerializeField]
        private HorizontalLayoutGroup stepContainer;

        /// <summary>
        /// Progress bar status text
        /// </summary>
        [SerializeField]
        private Text status;

        /// <summary>
        /// Progress bar description text
        /// </summary>
        [SerializeField]
        private Text description;

        /// <summary>
        /// String used to separate the current and the max value of the progress bar in the status text
        /// </summary>
        [SerializeField]
        private string statusSeparator = "/";
#pragma warning restore 649

        /// <summary>
        /// Gets or sets the maximum value of the progress bar
        /// </summary>
        public float MaxValue
        {
            get
            {
                return progressBar.maxValue;
            }

            set
            {
                progressBar.maxValue = value;
                UpdateStatusText();
            }
        }

        /// <summary>
        /// Gets or sets the current value of the progress bar
        /// </summary>
        public float Value
        {
            get
            {
                return progressBar.value;
            }

            set
            {
                progressBar.value = value;
                UpdateStatusText();
            }
        }

        /// <summary>
        /// Gets or sets the description text
        /// </summary>
        public string Description
        {
            get
            {
                return description.text;
            }
            set
            {
                description.text = value;
            }
        }

        private float stepImageSize;

        private int oldSteps;

        /// <summary>
        /// Cache the step image size at startup.
        /// Set <see cref="oldSteps"/> to -1 to enforce execution of <see cref="UpdateProgressBar"/> at the next update.
        /// </summary>
        public void Awake()
        {
            stepImageSize = stepImage.GetComponent<LayoutElement>().preferredWidth;
            oldSteps = -1;
        }

        /// <summary>
        /// if the amount of steps was changed the progress bar gets rebuild
        /// </summary>
        public void Update()
        {
            if (steps != oldSteps)
            {
                UpdateProgressBar();
                oldSteps = steps;
            }
        }

        private void UpdateProgressBar()
        {
            ClearSteps();
            CreateSteps();
            UpdateStatusText();
        }

        private void ClearSteps()
        {
            stepContainer.transform.DestroyAllChildrenImmediate();
        }

        private void CreateSteps()
        {
            // Step images only need to be created when there are at least 2 steps
            if (steps < 2)
            {
                return;
            }

            float stepSize = progressBarBG.rect.width / steps - stepImageSize;

            stepContainer.spacing = stepSize;
            stepContainer.padding.left = (int) stepSize;

            for (var i = 0; i < steps - 1; i++)
            {
                Image step = Instantiate(stepImage);
                step.transform.SetParent(stepContainer.gameObject.transform, false);
                step.gameObject.SetActive(true);

                // Names are only visible in the editor so only set them there
#if UNITY_EDITOR
                step.name = "StepImage " + (i + 1);
#endif
            }
        }

        public void UpdateStatusText()
        {
            status.text = progressBar.value + statusSeparator + progressBar.maxValue;
        }
    }
}