using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public interface IDialogTabButtonList
    {
        //void Init();

        void AddButton(string buttonLabelTextId, int i);

        void UpdateButtonAccess(bool isAccessible, int i);
        int CurrentActive { set; }
        GameObject gameObject { get; }

        event GenericEvent<int> TabClickedEvent;
        void UpdateButtonEffects(bool active);

        void UpdateButtonEffects(bool active, int tabIndex);

        void SetBadge(int buttonIndex, int number);
    }
}