using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GGS.ScreenManagement
{
    /// <summary> 
    /// A ProgressBar animation component
    /// </summary>
    [AddComponentMenu("Animation/ProgressBar")]
    public class ProgressBarAnimationComponent : AAnimationComponent
    {
        [Tooltip("the slider of the progressbar.")]
        [SerializeField]
        private Slider targetSlider;

        [Tooltip("the textField of the progressbar.")]
        [SerializeField]
        private Text targetText;

        private float currentProgress;
        private float targetProgress;

        private bool isSetup;
        /// <summary>
        ///  Setup the ProgressBar current values
        /// </summary>
        /// <param name="elapsedTime">Progress already completed.</param>
        /// <param name="totalDuration">Total time needed to complete the Progress.</param>       
        public void Setup(float elapsedTime, float totalDuration)
        {
            if (targetSlider == null)
            {
                throw new ArgumentException("The target slider of the ProgressBarAnimation can't be null");
            }
            currentProgress = elapsedTime;
            targetProgress = totalDuration;
            isSetup = true;
        }

        /// <summary>
        ///  Play the ProgressBar animation
        /// </summary>
        protected override void Play()
        {
            if (!isSetup)
            {
                throw new ArgumentException("ProgressBarAnimation must be initialized before playing");
            }
            targetSlider.wholeNumbers = false;
            targetSlider.value = (currentProgress / targetProgress) * targetSlider.maxValue;
            Tween = targetSlider.DOValue(targetSlider.maxValue, targetProgress - currentProgress).SetEase(Ease.Linear);
            if (targetText != null)
            {
                Tween.OnUpdate(UpdateText);
            }
            Tween.OnComplete(OnComplete);
        }

        /// <summary>
        ///  Skip the ProgressBar animation
        /// </summary>
        protected override void Skip()
        {
            Tween.timeScale = (Tween.Duration() - Tween.Elapsed()) * 2;
        }

        /// <summary>
        ///  Complete the ProgressBar animation
        /// </summary>
        protected override void Complete()
        {
            Tween.Complete(true);
        }

        /// <summary>
        ///  Update the text of the ProgressBar to the current time left
        /// </summary>
        protected void UpdateText()
        {
            //TODO localize duration outside targetText.text = Localize.Duration(Tween.Duration() - Tween.Elapsed());
        }
    }
}