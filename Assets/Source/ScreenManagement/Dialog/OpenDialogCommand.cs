using GGS.CakeBox.Utils;
using GGS.CakeBox.Logging;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Check the priority and the state of the Dialog Model and decide if should already show the dialog or enqueue it
    /// </summary>
    public class OpenDialogCommand
    {
        private OpenDialogVO propertyOpenDialogVO;

        public event GenericEvent<OpenDialogVO> DialogAddedToModel;

        public event GenericEvent<OpenDialogVO, OpenDialogVO> DialogPushedToHistory;

        public void Execute(OpenDialogVO propertyOpenDialogVO, DialogPriority propertyPriority)
        {
            this.propertyOpenDialogVO = propertyOpenDialogVO;
            switch (propertyPriority)
            {
                case DialogPriority.Enqueue:
                    CheckDialogQueue();
                    break;
                case DialogPriority.ForceForeground:
                    ForceDialogToForeground();
                    break;
            }
        }

        /// <summary>
        /// Forces the dialog to the front of the the other dialogs
        /// - If there is already a dialog queued with the same id, it removes it and add this one as foreground
        /// - Then, the old current dialog is pushed to the history stack
        /// </summary>
        private void ForceDialogToForeground()
        {
            // If there is already a dialog queued with the same id, it removes it and add this one as foreground
            if (ScreenSystem.DialogModelWritter.IsDialogScheduled(propertyOpenDialogVO.Id))
            {
                ScreenSystem.DialogModelWritter.RemoveFromQueue(propertyOpenDialogVO.Id);
            }

            if (ScreenSystem.DialogModelWritter.CurrentDialog != null)
            {
                OpenDialogVO currentDialog = ScreenSystem.DialogModelWritter.CurrentDialog;

                if (currentDialog.Id == propertyOpenDialogVO.Id)
                {
                    GGLog.LogWarning( "Dialog " + propertyOpenDialogVO.Id + " tried to open twice.", ScreenSystem.LogType);
                    return;
                }
                ScreenSystem.DialogModelWritter.SetCurrentDialog(propertyOpenDialogVO);
                DialogPushedToHistory.Fire(currentDialog, propertyOpenDialogVO);
            }
            else
            {
                ScreenSystem.DialogModelWritter.SetCurrentDialog(propertyOpenDialogVO);
                DialogAddedToModel.Fire(propertyOpenDialogVO);
            }

        }

        /// <summary>
        /// Adds the dialog to the queue of scheduled dialogs
        /// - If there's no dialog currently open, it just opens the dialog
        /// - If there is already a dialog queued with the same id, the content is replaced
        /// </summary>
        private void CheckDialogQueue()
        {
            if (ScreenSystem.DialogModelWritter.CurrentDialog != null)
            {
                Enqueue();
            }
            else
            {
                ScreenSystem.DialogModelWritter.SetCurrentDialog(propertyOpenDialogVO);
                DialogAddedToModel.Fire(propertyOpenDialogVO);
            }
        }

        private void Enqueue()
        {
            if (ScreenSystem.DialogModelWritter.CurrentDialog != null && ScreenSystem.DialogModelWritter.CurrentDialog.Id == propertyOpenDialogVO.Id)
            {
                GGLog.LogWarning(
                    "Trying to queue  " + ScreenSystem.DialogModelWritter.CurrentDialog.Id + " when another is currently open.",
                    ScreenSystem.LogType
                    );
            }
            else
            {
                ScreenSystem.DialogModelWritter.Enqueue(propertyOpenDialogVO);
            }
        }
    }
}