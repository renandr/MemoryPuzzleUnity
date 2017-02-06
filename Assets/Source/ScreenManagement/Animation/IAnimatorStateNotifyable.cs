
namespace GGS.ScreenManagement
{
    /// <summary>
    /// An interface designed to be the callback for states in Animator that have AAnimatorStateNotify attached to it.
    /// Uses the same type like in the you defined in your concrete child class of AAnimatorStateNotify.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAnimatorStateNotifyable <T>
    {
        void StateChanged(int layerIndex, T newState);
    }
}
