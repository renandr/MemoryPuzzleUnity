using DG.Tweening;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// A jump animation component for 3D gameobjects
    /// </summary>
    [AddComponentMenu("Animation/Jump")]
    public class JumpAnimationComponent : AAnimationComponent
    {
        [Tooltip("jump into a random direction between min and max")]
        [SerializeField] 
        private RandomVector3 randomDirection;
        [Tooltip("random duration for the animation between min and max")]
        [SerializeField] 
        private RandomFloat randomDuration;
        [Tooltip("the amount of jumps befor complete")]
        [SerializeField] 
        private int amount;
        [Tooltip("the power of the jump")]
        [SerializeField] 
        public float power;

        /// <summary>
        /// Play the animation from begin on
        ///</summary>
        protected override void Play()
        {
            //Create new random values
            randomDirection.SetNewRandom();
            randomDuration.SetNewRandom();

            //start the jump tween
            Tween = transform.DOJump(PositionOffset, power, amount, AnimationDuration).OnComplete(OnComplete);
        }

        /// <summary>
        /// Complete the animation with a short delay
        ///</summary>
        protected override void Skip()
        {
            Tween.timeScale = (Tween.Duration() - Tween.Elapsed() * 2); //calculate the rest speed and make it faster
        }

        /// <summary>
        /// Complete the animation directly
        ///</summary>
        protected override void Complete()
        {
            Tween.Complete(true);
        }

        /// <summary>   
        /// Gets the position offset with the random values
        /// </summary>

        private Vector3 PositionOffset
        {
            get
            {
                Vector3 position = transform.position;
                position.x += randomDirection.X.Value;
                position.y += randomDirection.Y.Value;
                position.z += randomDirection.Z.Value;
                return position;
            }
        }
    }
}
