namespace GGS.ScreenManagement
{
    public abstract class ADialogController : AScreenController
    {
        private IDialog parentDialog;

        protected IDialog ParentDialog
        {
            get
            {
                return parentDialog;
            }
        }

        protected sealed override void ScreenControllerAwake()
        {
            parentDialog = ParentScreen as IDialog;
            DialogControllerAwake();
        }

        protected virtual void DialogControllerAwake()
        {
        }

        protected T GetProperties<T>()
        {
            return ScreenSystem.DialogModel.GetProperties<T>(parentDialog.ScreenId, parentDialog.InstanceIdentifier);
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            parentDialog.FocusLost += OnDialogFocusLost;
            parentDialog.FocusRestored += OnDialogFocusBack;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            parentDialog.FocusLost += OnDialogFocusLost;
            parentDialog.FocusRestored += OnDialogFocusBack;
        }
        private void OnDialogFocusLost()
        {
        }
        private void OnDialogFocusBack()
        {
        }

        protected virtual DialogState SaveState()
        {
            return null;
        }

        protected virtual void RestoreState(DialogState viewState)
        {

        }
        
    }
}