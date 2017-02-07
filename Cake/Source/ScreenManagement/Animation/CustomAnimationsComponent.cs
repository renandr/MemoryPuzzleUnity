using UnityEngine;

namespace GGS.ScreenManagement
{
    [RequireComponent(typeof(Animator))]
    public class CustomAnimationsComponent : MonoBehaviour
    {
        [SerializeField]
        private AnimationClip startAnim;
        [SerializeField]
        private AnimationClip idleAnim;
        [SerializeField]
        private AnimationClip endAnim;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void RunEndAnimation()
        {
            animator.SetBool("EndAnim",true);
        }
    }
}
