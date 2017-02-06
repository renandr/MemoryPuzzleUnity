using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public class CallbackStateMachineBehaviour : StateMachineBehaviour
    {
        public event GenericEvent StateStarted;
        public event GenericEvent StateEnded;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateStarted.Fire();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateEnded.Fire();
        }
    }
}
