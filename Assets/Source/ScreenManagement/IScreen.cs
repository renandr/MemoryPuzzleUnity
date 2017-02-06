using System;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public interface IScreen
    {
        CanvasGroup CanvasGroup { get; }
        GameObject gameObject { get; }
        ScreenIdentifier ScreenId { get; set; }
        int InstanceIdentifier { get; set; }
        bool IsCreated { get; }

        bool IsUncacheable { get; }
        CachingBehavior CachingBehavior { get; }
        bool IsAnimating { get; }
        bool IsOpen { get; }

        GenericEvent ScreenIsCreated { get; set; }

        event GenericEvent ScreenOpened;

        event GenericEvent PropertiesFetched;

        event GenericEvent<IScreen> CloseAnimationFinished;

        event GenericEvent ScreenClosed;

        event GenericEvent<IScreen> OpenAnimationFinished;

        event GenericEvent ScreenDestroyed;

        void NotifyOpen();
        void PlayOpenAnimation();
        void PlayCloseAnimation();

        void NotifyClose();
        void OnCallOutActivated(ACallOutComponent aCallOutComponent);
        void AddCreationAsset(string assetName, Action<GameObject> callbackAction);
    }
}