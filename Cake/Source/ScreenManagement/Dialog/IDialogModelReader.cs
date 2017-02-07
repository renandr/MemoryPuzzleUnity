namespace GGS.ScreenManagement
{
    /// <summary>
    /// Interface for read-only access to the Dialog Model
    /// The Dialog Model stores the VO's with information about opening Dialogs, which are windows that are modal and queued
    /// Ideally you shouldn't give the OpenDialogVO for read only access, only the properties
    /// </summary>
    public interface IDialogModelReader : IScreenModelReader
    {
        OpenDialogVO CurrentDialog { get; }
        bool HasDialogsInHistory { get; }

        bool IsDialogScheduled(ScreenIdentifier screenIdentifier);

        bool IsDialogInHistory(ScreenIdentifier uiid);

        DialogState GetScreenStateByInstanceIdentifier(int instanceIdentifier);
        bool HasDialogsInQueue();
    }
}