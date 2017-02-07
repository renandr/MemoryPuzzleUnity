using System;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary> 
    /// A Resize animation component for the Scrollable Lists
    /// </summary>
    [AddComponentMenu("Animation/ScrollableListResize")]
    public class ScrollableListAnimationComponent : ResizeAnimationComponent
    {
        private RectTransform parentRectTransform;

        /// <summary>  
        ///  Get the rectTranfsorm of the parent object
        /// </summary>
        protected override void Awake()
        {
            parentRectTransform = transform.parent.GetComponentInParent<RectTransform>();
            if (!parentRectTransform)
            {
                throw new Exception(name + "parent object has not a rect transform. it must be a child of a canvas");
            }
            base.Awake();
        }

        /// <summary>
        ///  Play the Resize animation and modify the size
        /// </summary>
        protected override void Play()
        {
            float tempSize;
            
            //claculate the new size for the scrollable list
            if (Tween != null && IsPlaying)
            {
                tempSize = maxSize - minSize;
            }
            else
            {
                tempSize = minSize - maxSize;
            }
            
            parentRectTransform.sizeDelta += Vector2.up * (isExpanded ? tempSize : -tempSize);
            base.Play();
        }
    }
}