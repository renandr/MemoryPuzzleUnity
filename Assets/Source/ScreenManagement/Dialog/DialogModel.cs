using GGS.CakeBox.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GGS.CakeBox.Utils;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The Dialog Model stores the VO's with information about opening Dialogs, which are windows that are modal and queued
    /// Mainly behaves like a queue but it's possible that the current dialog is pushed back to the front of the queue for another dialog takes its place
    /// </summary>
    public class DialogModel : IDialogModelReader
    {
        private readonly List<OpenDialogVO> scheduledDialogs;
        private readonly List<OpenDialogVO> dialogHistory;
        
        public DialogModel()
        {
            scheduledDialogs = new List<OpenDialogVO>();
            dialogHistory = new List<OpenDialogVO>();
        }

        public bool HasDialogsInHistory
        {
            get
            {
                return dialogHistory.Count > 0;
            }
        }

        public bool HasDialogsInQueue()
        {
            return scheduledDialogs.Count > 0;
        }

        /// <summary>
        /// Gets the current dialog.
        /// </summary>
        /// <value>
        /// The previous dialog. null if the history is empty.
        /// </value>
        public OpenDialogVO CurrentDialog
        {
            get
            {
                return dialogHistory.Count > 0 ? dialogHistory.Peek() : null;
            }
        }

        /// <summary>
        /// Returns the next Dialog to open.
        /// </summary>
        /// <value>
        /// The next Dialog to open.
        /// </value>
        public OpenDialogVO GetNextDialog()
        {
            if (dialogHistory.Count > 0)
            {
                return dialogHistory.Pop();
            }

            for (int i = 0; i < scheduledDialogs.Count; i++)
            {
                OpenDialogVO dialogVO = scheduledDialogs[i];
                scheduledDialogs.Remove(dialogVO);
                return dialogVO;
            }

            return null;
        }

        /// <summary>
        /// Adds a dialog to the queue. If the dialog is already in the queue it will be replaced
        /// </summary>
        /// <param name="dialogVO"></param>
        public void Enqueue(OpenDialogVO dialogVO)
        {
            foreach (OpenDialogVO dialog in scheduledDialogs)
            {
                if (dialogVO.Id == dialog.Id)
                {
                    dialog.ReplaceContents(dialogVO);
                    return;
                }
            }

            scheduledDialogs.Enqueue(dialogVO);
            DebugLog("Enqueue " + dialogVO.Id);
        }

        /// <summary>
        /// Removes a dialog specified by ID from queue.
        /// </summary>
        /// <param name="id"> The identifier. </param>
        /// <returns>
        /// The removed OpenDialogVO.
        /// </returns>
        public OpenDialogVO RemoveFromQueue(ScreenIdentifier id)
        {
            OpenDialogVO removedVO = null;

            var tempStack = new Stack<OpenDialogVO>();

            while (scheduledDialogs.Count > 0)
            {
                OpenDialogVO dequeuedVO = scheduledDialogs.Dequeue();

                if (id != dequeuedVO.Id)
                {
                    tempStack.Push(dequeuedVO);
                }
                else
                {
                    removedVO = dequeuedVO;
                }
            }

            while (tempStack.Count > 0)
            {
                OpenDialogVO poppedVO = tempStack.Pop();
                scheduledDialogs.Enqueue(poppedVO);
            }
            DebugLog("RemoveFromQueue " + id);
            return removedVO;
        }

        /// <returns>
        /// The typed properties for a dialog to show when it's opened, set by its open command
        /// </returns>
        public T GetProperties<T>(ScreenIdentifier uuid, int instanceIdentifier)
        {
            IScreenPropertiesVO props = GetProperties(uuid, instanceIdentifier);

            if (props == null)
            {
                return default(T);
            }

            return (T)props;
        }

        /// <returns>
        /// The properties for a dialog to show when it's opened, set by its open command
        /// </returns>
        public IScreenPropertiesVO GetProperties(ScreenIdentifier uuid, int instanceIdentifier)
        {
            for (var i = 0; i < scheduledDialogs.Count; i++)
            {
                OpenDialogVO scheduledDialog = scheduledDialogs[i];
                if (scheduledDialog.InstanceIdentifier == instanceIdentifier)
                {
                    return scheduledDialog.Properties;
                }
            }

            for (var i = 0; i < dialogHistory.Count; i++)
            {
                OpenDialogVO historyDialog = dialogHistory[i];
                if (historyDialog.InstanceIdentifier == instanceIdentifier)
                {
                    return historyDialog.Properties;
                }
            }

            return null;
        }

        public bool Contains(ScreenIdentifier screenIdentifier)
        {
            for (int i = 0; i < scheduledDialogs.Count; i++)
            {
                if (scheduledDialogs[i].Id == screenIdentifier)
                {
                    return true;
                }
            }

            for (int i = 0; i < dialogHistory.Count; i++)
            {
                if (dialogHistory[i].Id == screenIdentifier)
                {
                    return true;
                }
            }

            return false;
        }


        public void AddScreenState(int instanceIdentifier, DialogState state)
        {
            foreach (OpenDialogVO historyDialog in dialogHistory)
            {
                if (historyDialog.InstanceIdentifier == instanceIdentifier)
                {
                    historyDialog.ScreenState = state;
                    return;
                }
            }
        }

        /// <summary>
        /// Gets screen state by instance identifier.
        /// </summary>
        /// <param name="instanceIdentifier"> Identifier for the instance. </param>
        /// <returns>  The screen state by instance identifier.  </returns>
        public DialogState GetScreenStateByInstanceIdentifier(int instanceIdentifier)
        {
            foreach (OpenDialogVO historyDialog in dialogHistory)
            {
                if (historyDialog.InstanceIdentifier == instanceIdentifier)
                {
                    return historyDialog.ScreenState;
                }
            }

            return null;
        }

        /// <summary>
        /// Query if the dialog specified by 'uiid' is scheduled.
        /// </summary>
        /// <param name="uiid"> The uiid of the dialog. </param>
        /// <returns>
        /// true if the dialog is cheduled, false if not.
        /// </returns>
        public bool IsDialogScheduled(ScreenIdentifier uiid)
        {
            for (int i = 0; i < scheduledDialogs.Count; i++)
            {
                OpenDialogVO scheduledDialog = scheduledDialogs[i];
                if (uiid == scheduledDialog.Id)
                    return true;
            }
            return false;
        }

        public bool IsDialogInHistory(ScreenIdentifier uiid)
        {
            for (int i = 0; i < dialogHistory.Count; i++)
            {
                OpenDialogVO scheduledDialog = dialogHistory[i];
                if (uiid == scheduledDialog.Id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Push the current dialog to history and make param as the current one, also remove any existing loops
        /// </summary>
        /// <param name="openDialogVO"></param>
        public void SetCurrentDialog(OpenDialogVO openDialogVO)
        {
            dialogHistory.Push(openDialogVO);

            dialogHistory.RemoveLoops();
            DebugLog("SetCurrentDialog " + openDialogVO.Id);
            LogStackTooBig();
        }

        private void LogStackTooBig()
        {
            int tooMuchDialogs = 5;
            if (dialogHistory.Count > tooMuchDialogs)
            {
                DebugLog("Dialog History Count greater than " + tooMuchDialogs);
            }
        }

        /// <summary>
        /// Removes the current dialog.
        /// </summary>
        public void RemoveCurrentDialog()
        {
            CurrentDialog.IsOpen = false;
            dialogHistory.RemoveLast();
            DebugLog("Removed Current Dialog");
        }

        /// <summary>
        /// Clears the history
        /// </summary>
        public void ClearHistory()
        {
            dialogHistory.Clear();
            DebugLog("ClearHistory");
        }

        [Conditional("DEBUG")]
        public void DebugLog(string action)
        {
            StringBuilder str = new StringBuilder("DIALOG MODEL CHANGED " + action);
            str.AppendLine().AppendLine("Dialog History:");
            for (int i = dialogHistory.Count - 1; i >= 0; i--)
            {
                str.AppendLine("- " + dialogHistory[i].Id);
            }
            str.AppendLine().AppendLine("Dialog Queue:");
            for (int i = scheduledDialogs.Count - 1; i >= 0; i--)
            {
                str.AppendLine("- " + scheduledDialogs[i].Id);
            }
            GGLog.Log(str.ToString(), ScreenSystem.LogType);
        }
    }
}