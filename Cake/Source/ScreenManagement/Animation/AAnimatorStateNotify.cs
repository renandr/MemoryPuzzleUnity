using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// This is a helper class to notify your view scripts about the current animation state.
    /// 
    /// Usage:
    /// - Subclass this class  
    /// - Put your child class onto the states in the Animator you want to be notified about  
    /// - Implement IAnimatorStateNotifyable<T> in your view script (a Monobehaviour component placed next to the Animator component)
    /// 
    /// Set the value for LayerState inside the Animator in the UnityInspector. When this state will be entered a IAnimatorStateNotifyable of
    /// the same type will be fetched and called.
    /// </summary>
    /// <typeparam name="T">Type you use for identifing your state.</typeparam>
    public abstract class AAnimatorStateNotify<T> : StateMachineBehaviour
    {
        [SerializeField]
        protected T LayerState;

        protected IAnimatorStateNotifyable<T> notifyable;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (notifyable == null)
            {
                notifyable = animator.GetComponent<IAnimatorStateNotifyable<T>>();
                if (notifyable == null)
                {
                    return;
                }
            }

            notifyable.StateChanged(layerIndex, LayerState);
        }
    }
}
