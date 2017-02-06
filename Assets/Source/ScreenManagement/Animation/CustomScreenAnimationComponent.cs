using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Takes care of custom screen animations controlled by an
    /// Animator. Wrappes the parameter calls and notifies for state changes.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CustomScreenAnimationComponent : MonoBehaviour, IAnimatorStateNotifyable<CustomAnimationState>
    {
        public event GenericEvent OpenFinished;

        public event GenericEvent CloseFinished;

        private const string EndAnimationBoolName = "EndAnim";
        private const string StartAnimationBoolName = "StartAnim";

        private CustomAnimationState currentState = CustomAnimationState.Undefined;

        protected Animator animator;

        protected void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void StartOpen()
        {
            if (currentState == CustomAnimationState.Visible)
            {
                OpenFinished.Fire();
                return;
            }
            animator.SetBool(StartAnimationBoolName, true);
        }

        public void StartClose()
        {
            if (currentState == CustomAnimationState.Invisible)
            {
                CloseFinished.Fire();
                return;
            }
            animator.SetBool(EndAnimationBoolName, true);
        }

        public void ResetAnimator()
        {
            animator.SetBool(StartAnimationBoolName, false);
            animator.SetBool(EndAnimationBoolName, false);

            animator.Rebind();
            currentState = CustomAnimationState.Undefined;
        }

        // implementation of IAnimatorStateNotifyable
        public void StateChanged(int layerIndex, CustomAnimationState newState)
        {
            currentState = newState;
            switch (newState)
            {
                case CustomAnimationState.Invisible:
                    animator.SetBool(EndAnimationBoolName, false);
                    CloseFinished.Fire();
                    break;

                case CustomAnimationState.Visible:
                    animator.SetBool(StartAnimationBoolName, false);
                    OpenFinished.Fire();
                    break;
            }
        }
    }
}