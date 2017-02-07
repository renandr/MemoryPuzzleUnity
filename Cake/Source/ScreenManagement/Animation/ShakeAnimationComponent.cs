using DG.Tweening;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>  
    /// A shake animation component.
    /// </summary>
    [AddComponentMenu("Animation/Shake")]
    public class ShakeAnimationComponent : AAnimationComponent
    {
        [Tooltip("the strengh of the shaking")]
        [SerializeField]
        private float strength;

        /// <summary>
        ///  Play the shake animation
        /// </summary>
        protected override void Play()
        {
            Tween = transform.DOShakePosition(AnimationDuration, Vector3.one * strength).OnComplete(OnComplete);
        }

        /// <summary>
        ///  Skip the shake animation
        /// </summary>
        protected override void Skip()
        {
            Tween.timeScale = (Tween.Duration() - Tween.Elapsed() * 2); //calculate the rest speed and make it faster
        }

        /// <summary>
        ///  Complete the shake animation
        /// </summary>
        protected override void Complete()
        {
            Tween.Complete(true);
        }
    }
}
