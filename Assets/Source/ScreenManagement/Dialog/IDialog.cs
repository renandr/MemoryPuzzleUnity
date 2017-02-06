
using GGS.CakeBox.Utils;
namespace GGS.ScreenManagement
{
    public interface IDialog : IScreen
    {

        ActivityLevel ActivityLevel { get; }
        DialogType DialogType { get; set; }
        bool ActivateLayerOverlay { get; set; }

        /// <summary>
        /// Accessed by components
        /// </summary>
        void RequestClose();
        


        void NotifyFocusBack();
        void NotifyFocusLost();

        event GenericEvent FocusLost;
        event GenericEvent FocusRestored;


        event GenericEvent<DialogState> StateRestored;
        /// <summary>
        /// Use this event to save things into the state, which might be null so you have to create one
        /// </summary>
        event GenericEvent<DialogState> SavingState;
    }
}