using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public abstract class ADialog : ADialog<NullScreenProperties>
    {
        protected sealed override void OnProperties()
        {
            //Sealing
        }

    }

    /// <summary>
    /// This is just for Dialogs, not for tabs or any other object inside a dialog
    /// </summary>
    public abstract class ADialog<TProperties> : AScreen<TProperties, IDialogModelReader>, IDialog
        where TProperties : IScreenPropertiesVO
    {
#pragma warning disable 649
        [Header("ADialogScreenView")]
        [SerializeField]
        private ActivityLevel activityLevel;

        [SerializeField]
        private DialogType dialogType;

        [SerializeField]
        private bool activateLayerOverlay = true;

        

#pragma warning restore 649

        public bool ActivateLayerOverlay
        {
            get
            {
                return activateLayerOverlay;
                
            }
             set
            {
                activateLayerOverlay = value;
                
            }
        }

        public DialogType DialogType
        {
            get
            {
                return dialogType;

            }
            set
            {
                dialogType = value;

            }
        }

        public event GenericEvent FocusLost;
        public event GenericEvent FocusRestored;
        public event GenericEvent<DialogState> StateRestored;
        public event GenericEvent<DialogState> SavingState;


        public ActivityLevel ActivityLevel
        {
            get
            {
                return activityLevel;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            ModelReader = ScreenSystem.DialogModel;
            DialogAwake();
        }

        protected virtual void DialogAwake()
        {
        }

        protected sealed override void OnOpen()
        {
            DialogState state = ModelReader.GetScreenStateByInstanceIdentifier(InstanceIdentifier);

            if (state != null)
            {
                RestoreDialogState(state);
            }
            OnDialogOpen();
        }
        protected abstract void OnDialogOpen();

        public void UI_OnCloseClicked()
        {
            RequestClose();
        }

        /// <summary>
        /// Don't ever call this method on the OnOpen, why would would do that?
        /// </summary>
        public virtual void RequestClose()
        {
            ScreenSystem.CloseDialog(ScreenId);
        }

        protected override sealed void OnClose()
        {
            DialogState state = SaveState();
            SavingState.Fire(state);
            if (state != null)
            {
                ScreenSystem.DialogModelWritter.AddScreenState(InstanceIdentifier, state);
            }
            OnDialogClose();
        }
        protected abstract void OnDialogClose();


        /// <summary>
        /// Called by the layer 
        /// </summary>
        public void NotifyFocusBack()
        {
            OnFocusBack();
            FocusLost.Fire();
        }

        protected virtual void OnFocusBack()
        {
            
        }
        /// <summary>
        /// Called by the layer 
        /// </summary>
        public void NotifyFocusLost()
        {
            OnFocusLost();
            FocusLost.Fire();
        }
        protected virtual void OnFocusLost()
        {

        }
        /// <summary>
        /// Creates a ScreenViewState for later restore of this screens state
        /// </summary>
        /// <returns></returns>
        protected virtual DialogState SaveState()
        {
            return null;
        }

        /// <summary>
        /// Restores the state of this screen to the given DialogScreenState
        /// </summary>
        /// <param name="viewState"></param>
        private void RestoreDialogState(DialogState viewState)
        {
            RestoreState(viewState);
            StateRestored.Fire(viewState);
        }

        protected virtual void RestoreState(DialogState viewState)
        {

        }

    }
}