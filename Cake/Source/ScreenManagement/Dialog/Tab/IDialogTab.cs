using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Had to creat this interface because casting the ATabView to ATabView<> doesn't work
    /// </summary>
    public interface IDialogTab
    {
        int DialogInstanceIdentifier { get; set; }
        ScreenIdentifier ScreenId { get; set; }
        GameObject gameObject { get; }
        void OnNotifyPropertiesUpdate();
        void NotifyOpen(int currentTabIndex);
        void NotifyClose();
        void NotifyDialogOpen();
        void NotifyDialogClose();
    }
}