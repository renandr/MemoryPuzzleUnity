using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GGS.ScreenManagement
{
    /// <summary> 
    /// A Resize animation component
    /// </summary>
    [RequireComponent(typeof(LayoutElement))]
    [AddComponentMenu("Animation/Resize")]
    public class ResizeAnimationComponent : AAnimationComponent
    {
        [Tooltip("the maximum local size.")]
        [SerializeField]
        protected float maxSize;

        protected float minSize;
        protected float targetSize;

        protected bool isExpanded;

        private LayoutElement targetElement;

        protected override void Awake()
        {
            targetElement = GetComponent<LayoutElement>();
            if (!targetElement)
            {
                throw new Exception(name + "parent object has not a layoutElement component.");
            }
            base.Awake();
        }

        protected override void Start()
        {
            minSize = targetElement.minHeight;

            base.Start();
        }

        /// <summary>
        ///  Play the Resize animation
        /// </summary>
        protected override void Play()
        {
            if (Tween != null && IsPlaying)
            {
                Tween.SmoothRewind();
            }
            else
            {
                targetSize = (isExpanded) ? minSize : maxSize;
                Tween = targetElement.DOMinSize(new Vector2(targetElement.minWidth, targetSize), AnimationDuration).OnComplete(OnComplete);
            }
        }

        /// <summary>
        ///  Skip the Resize animation
        /// </summary>
        protected override void Skip()
        {
            Tween.timeScale = (Tween.Duration() - Tween.Elapsed() * 2); //calculate the rest speed and make it faster
        }

        /// <summary>
        ///  Complete the Resize animation
        /// </summary>
        protected override void Complete()
        {            
            Tween.Complete(true);
        }

        /// <summary>
        ///  Event triggered when the Resize animation ended
        /// </summary>
        protected override void OnComplete()
        {
            base.OnComplete();
            isExpanded = !isExpanded;
        }        
    }
}