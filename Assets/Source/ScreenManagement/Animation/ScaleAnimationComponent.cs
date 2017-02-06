using DG.Tweening;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary> 
    /// A Scale animation component
    /// </summary>
    [AddComponentMenu("Animation/Scale")]
    public class ScaleAnimationComponent : AAnimationComponent
    {
        [SerializeField]
        private Vector3 endValue;
        public Vector3 EndValue 
        {
            get
            {
                return endValue;
            }
            set
            {
                endValue = value;
            }
        }
        protected override void Play()
        {
            Tween = transform.DOScale(endValue, AnimationDuration).OnComplete(OnComplete);
        }

        protected override void Skip()
        {
            Tween.Complete(true);
        }

        protected override void Complete()
        {
            Tween.Complete(true);
        }
    }
}