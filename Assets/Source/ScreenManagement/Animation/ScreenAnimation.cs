using DG.Tweening;
using System;
using GGS.CakeBox.Logging;
using GGS.CakeBox.Utils;
using UnityEngine;


namespace GGS.ScreenManagement
{
    /// <summary>   
    /// Use this class for have a animation for dialogs and panels
    /// </summary>
    [Serializable]
    public class ScreenAnimation
    {
        [Tooltip("the animation when the screen is enable")]
        [SerializeField]
        private ScreenAnimationType startAnimation;
        [Tooltip("the animation when the screen is disable")]
        [SerializeField]
        private ScreenAnimationType endAnimation;

        public event GenericEvent StartAnimationFinished;
        public event GenericEvent EndAnimationFinished;


        private Vector3 tempPosition; //the temp position after complete the tweening
        private float tempAlpha;//the temp alpha after complete the tweening
        private CustomScreenAnimationComponent animator; // cached animator reference

        private IScreen screenCached;

        /// <summary> 
        ///  Play start animation.
        ///</summary>
        /// <param name="screenView"> The screen view. </param>
        public void PlayStartAnimation(IScreen screen)
        {
            screenCached = screen;
            RectTransform transform = (RectTransform)screen.gameObject.transform;
       
            CanvasGroup canvas = screen.CanvasGroup;
            tempPosition = transform.anchoredPosition;
            tempAlpha = canvas.alpha;

            if (!ScreenSystemCanvas.Instance.EnableAnimations)
            {
                OnComplete(true);
                return;
            }

            float startAnimationDuration = ScreenSystemCanvas.Instance.StartAnimationDuration;
            switch (startAnimation)
            {
                case ScreenAnimationType.Bottom:
                    transform.anchoredPosition = new Vector2(tempPosition.x, Bottom);
                    transform.DOAnchorPos(tempPosition, startAnimationDuration)
                        .OnComplete(() => OnComplete(true));
                    break;
                case ScreenAnimationType.Top:
                    transform.anchoredPosition = new Vector2(transform.anchoredPosition.x, Top);
                    transform.DOAnchorPos(tempPosition, startAnimationDuration)
                      .OnComplete(() => OnComplete(true));
                    break;
                case ScreenAnimationType.Left:
                    transform.anchoredPosition = new Vector2(Left, tempPosition.y);
                    transform.DOAnchorPos(tempPosition, startAnimationDuration)
                        .OnComplete(() => OnComplete(true));
                    break;
                case ScreenAnimationType.Right:
                    transform.anchoredPosition = new Vector2(Right, tempPosition.y);
                    transform.DOAnchorPos(tempPosition, startAnimationDuration)
                        .OnComplete(() => OnComplete(true));
                    break;
                case ScreenAnimationType.Fade:
                    screen.CanvasGroup.alpha = 0f;
                    screen.CanvasGroup.DOFade(tempAlpha, startAnimationDuration)
                            .OnComplete(() => OnComplete(true));
                    break;
                case ScreenAnimationType.Custom:
                    animator = screen.gameObject.GetComponent<CustomScreenAnimationComponent>();
                    if (animator != null)
                    {
                        animator.OpenFinished += OnStartFinished;
                        animator.StartOpen();
                    }
                    else
                    {
                        GGLog.LogWarning(string.Format("There is no {0} defined for on {1} a custom screen animation.",
                                typeof(CustomScreenAnimationComponent).Name, screen.gameObject.name), ScreenSystem.LogType);
                        OnComplete(true);
                    }
                    break;
                default:
                    OnComplete(true);
                    break;
            }
        }

        ///   <summary>
        ///   Play end animation.
        ///   </summary>
        /// <param name="screenView">                   The screen view. </param>
        public void PlayEndAnimation(IScreen screen)
        {
            screenCached = screen;
            var transform = (RectTransform)screen.gameObject.transform;
            tempPosition = transform.anchoredPosition;

            if (!ScreenSystemCanvas.Instance.EnableAnimations)
            {
                OnComplete(false);
                return;
            }

            float endAnimationDuration = ScreenSystemCanvas.Instance.EndAnimationDuration;
            switch (endAnimation)
            {
                case ScreenAnimationType.Bottom:
                    tempPosition.y = Bottom;
                    transform.DOAnchorPos(tempPosition, endAnimationDuration)
                        .OnComplete(() => OnComplete(false));
                    break;
                case ScreenAnimationType.Top:
                    tempPosition.y = Top;
                    transform.DOAnchorPos(tempPosition, endAnimationDuration)
                        .OnComplete(() => OnComplete(false));
                    break;
                case ScreenAnimationType.Left:
                    tempPosition.x = Left;
                    transform.DOAnchorPos(tempPosition, endAnimationDuration)
                        .OnComplete(() => OnComplete(false));
                    break;
                case ScreenAnimationType.Right:
                    tempPosition.x = Right;
                    transform.DOAnchorPos(tempPosition, endAnimationDuration)
                        .OnComplete(() => OnComplete(false));
                    break;
                case ScreenAnimationType.Fade:
                    tempAlpha = 0f;
                    screen.CanvasGroup.DOFade(tempAlpha, endAnimationDuration)
                        .OnComplete(() => OnComplete(false));
                    break;
                case ScreenAnimationType.Custom:
                    animator = screen.gameObject.GetComponent<CustomScreenAnimationComponent>();
                    if (animator != null)
                    {
                        animator.CloseFinished += OnCloseFinished;
                        animator.StartClose();
                    }
                    else
                    {
                        GGLog.LogWarning(string.Format("There is no {0} defined for on {1} a custom screen animation.",
                                typeof(CustomScreenAnimationComponent).Name, screen.gameObject.name), ScreenSystem.LogType);
                        OnComplete(false);
                    }
                    break;
                default:
                    OnComplete(false);
                    break;
            }
        }

        private void OnStartFinished()
        {
            if (startAnimation == ScreenAnimationType.Custom && animator != null)
            {
                animator.OpenFinished -= OnStartFinished;
            }

            OnComplete(true);
        }

        private void OnCloseFinished()
        {
            if (EndAnimation == ScreenAnimationType.Custom && animator != null)
            {
                animator.CloseFinished -= OnCloseFinished;
                animator.ResetAnimator();
            }

            OnComplete(false);
        }

        private void OnComplete(bool wasStart)
        {
            var transform = (RectTransform)screenCached.gameObject.transform;
            screenCached.CanvasGroup.alpha = tempAlpha;
            transform.anchoredPosition = tempPosition;

            if (wasStart)
            {
                StartAnimationFinished.Fire();
            }
            else
            {
                // reset to default position
                transform.anchoredPosition = Vector3.zero;
                screenCached.CanvasGroup.alpha = 1f;
                EndAnimationFinished.Fire();
            }
        }

        /// <summary>   
        ///Gets the top screen direction.
        /// </summary>
        private float Top
        {
            get
            {
                return ScreenSystemCanvas.Instance.CanvasRect.rect.height;
                //return CanvasView.Instance.UICanvas.pixelRect.height * 2;
            }
        }

        /// <summary>   
        ///Gets the bottom screen direction.
        /// </summary>
        private float Bottom
        {
            get
            {
                return -ScreenSystemCanvas.Instance.CanvasRect.rect.height;
                //return -CanvasView.Instance.UICanvas.pixelRect.height * 2;
            }
        }

        /// <summary>   
        ///Gets the left screen direction.
        /// </summary>
        private float Left
        {
            get
            {
                return -ScreenSystemCanvas.Instance.CanvasRect.rect.width;
                //return -CanvasView.Instance.UICanvas.pixelRect.width * 2;
            }
        }

        /// <summary>   
        ///Gets the right screen direction.
        /// </summary>
        private float Right
        {
            get
            {
                return ScreenSystemCanvas.Instance.CanvasRect.rect.width;
                //return CanvasView.Instance.UICanvas.pixelRect.width * 2;
            }
        }

        /// <summary>   
        ///The start animation when a open signal is called
        /// </summary>
        public ScreenAnimationType StartAnimation
        {
            set { startAnimation = value; }
            get { return startAnimation; }
        }

        /// <summary>   
        ///The end animation when a close signal is called
        /// </summary>
        public ScreenAnimationType EndAnimation
        {
            set { endAnimation = value; }
            get { return endAnimation; }
        }
    }
}