using GGS.GameLocks;
using GGS.CakeBox.Utils; using GGS.CakeBox.Logging;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Closes the current Dialog if there's any, and check if it should open the next queued dialog
    /// </summary>
    internal class CloseDialogCommand
    {
        public event GenericEvent<OpenDialogVO> DialogRemovedFromModel;

        public event GenericEvent<OpenDialogVO, OpenDialogVO> DialogTakenFromHistory;

        public event GenericEvent<OpenDialogVO, OpenDialogVO> DialogTakenFromQueue;

        public event GenericEvent DialogModelHistoryCleared;


        public void Execute(ScreenIdentifier dialogIdParameter)
        {
            if (LockSystem.Model.ContainsCustomLock(CustomLockReason.LoadingAsset, dialogIdParameter.AssetId))
            {
                GGLog.LogError("Dialog " + dialogIdParameter.AssetId + " couldn't close because it was still loading.", ScreenSystem.LogType);
                //TODO AssetProvider.Instance.CancelLoad(DialogIdParameter.AssetId);
                return;
            }

            if (ScreenSystem.DialogModelWritter.IsDialogScheduled(dialogIdParameter))
            {
                ScreenSystem.DialogModelWritter.RemoveFromQueue(dialogIdParameter);
            }

            if (ScreenSystem.DialogModelWritter.CurrentDialog != null && ScreenSystem.DialogModelWritter.CurrentDialog.Id == dialogIdParameter)
            {
                OpenDialogVO curDialog = ScreenSystem.DialogModelWritter.CurrentDialog;
                ScreenSystem.DialogModelWritter.RemoveCurrentDialog();

                if (ScreenSystem.DialogModelWritter.HasDialogsInHistory)
                {
                    OpenDialogVO next = ScreenSystem.DialogModelWritter.GetNextDialog();
                    ScreenSystem.DialogModelWritter.SetCurrentDialog(next);
                    DialogTakenFromHistory.Fire(curDialog, next);
                }
                else
                {
                    if (ScreenSystem.DialogModelWritter.HasDialogsInQueue())
                    {
                        OpenDialogVO next = ScreenSystem.DialogModelWritter.GetNextDialog();

                        ScreenSystem.DialogModelWritter.SetCurrentDialog(next);
                        //maybe create another signal called "taken from queue"
                        DialogTakenFromQueue.Fire(curDialog, next);
                    }
                    else
                    {
                        DialogRemovedFromModel.Fire(curDialog);
                    }

                    // Used to reset the resource bar, since the history navigation is gone
                    DialogModelHistoryCleared.Fire();
                }
            }
        }
    }
}
