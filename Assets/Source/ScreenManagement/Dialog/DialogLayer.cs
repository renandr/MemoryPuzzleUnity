using System;
using System.Collections.Generic;
using GGS.CakeBox.Utils;
using GGS.CakeBox.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The view that handles the dialog layer, it should decide how it's gonna load/instantiate and unload/destroy the dialogs
    /// Currently it's just loading the dialogs from resources.
    /// </summary>
    [RequireComponent(typeof (Image))]
    public class DialogLayer : ALayer
    {
        private Image overlayImage;
        private GraphicRaycaster raycaster;

        private IDialog currentDialog;

        public static event GenericEvent<bool> FullscreenDialogOnScreen;
        public static event GenericEvent<ActivityLevel> DialogActivityLevelChanged;

        protected override void LayerStart()
        {
            overlayImage = GetComponent<Image>();
            raycaster = gameObject.AddComponent<GraphicRaycaster>();
            if (gameObject.GetComponent<Canvas>() == null)
            {
                gameObject.AddComponent<Canvas>();
            }
            UpdateOverlay();

            ScreenSystem.DialogAddedToModel += OnDialogAddedToModel;
            ScreenSystem.DialogRemovedFromModel += RemoveDialog;
            ScreenSystem.DialogPushedToHistory += ReplaceDialogs;
            ScreenSystem.DialogTakenFromHistory += OnDialogTakenFromHistory;
            ScreenSystem.DialogTakenFromQueue += OnDialogTakenFromQueue;
            ScreenSystem.DialogHistoryClosed += CloseEverything;
            ScreenSystem.LockAllDialogScrollsRequested += LockAllScrolls;

        }

        protected override void OnPanelDestroy()
        {
            ScreenSystem.DialogAddedToModel -= OnDialogAddedToModel;
            ScreenSystem.DialogRemovedFromModel -= RemoveDialog;
            ScreenSystem.DialogPushedToHistory -= ReplaceDialogs;
            ScreenSystem.DialogTakenFromHistory -= OnDialogTakenFromHistory;
            ScreenSystem.DialogTakenFromQueue -= OnDialogTakenFromQueue;
            ScreenSystem.DialogHistoryClosed -= CloseEverything;
            ScreenSystem.LockAllDialogScrollsRequested -= LockAllScrolls;
        }

        private void OnDialogAddedToModel(OpenDialogVO dialog)
        {
            ShowScreen(dialog, null, !ScreenSystem.IsOpeningScreensEnabled(Id));
        }

        private void OnDialogTakenFromHistory(OpenDialogVO outDialogVO, OpenDialogVO inDialogVO)
        {
            TakeDialogBack(outDialogVO, inDialogVO);
        }

        protected override void OnScreenOpeningRestrictionChange(bool openingEnabled)
        {
            if (openingEnabled)
            {
                TryOpenNextQueuedScreen();
            }
        }

        private void OnOtherVisibilityChanged(bool active)
        {
            FullscreenDialogOnScreen.Fire(active);
            GGLog.Log("Full Screen Dialog On Screen " + active.ToString().ToUpper(), ScreenSystem.LogType);
        }

        public override void ShowScreen(OpenScreenVO screenVO, Action<IScreen, OpenScreenVO> callback = null, bool forceEnqueue = false)
        {
            UpdatePerformanceOptimizations(false);
            base.ShowScreen(screenVO, delegate(IScreen view, OpenScreenVO openScreenVO)
            {
                SetupDialogView(view, openScreenVO);
                UpdateOverlay();
                currentDialog = view as IDialog;
                if (callback != null)
                {
                    callback(view, openScreenVO);
                }
                UpdatePerformanceOptimizations(true, currentDialog);
            }, forceEnqueue);
        }

        /// <summary>
        /// Replace the old dialog with the new one and check if the animation should be played
        /// </summary>
        /// <param name="oldDialog"> The old dialog</param>
        /// <param name="newDialog"> The new dialog</param>
        public void ReplaceDialogs(OpenDialogVO oldDialogVO, OpenDialogVO newDialogVO)
        {
            ShowScreen(newDialogVO, delegate (IScreen view, OpenScreenVO screenVO)
            {
                IScreen oldScreen;
                if (ActiveViews.TryGetValue(oldDialogVO.Id, out oldScreen))
                {
                    IDialog oldDialog = GetViewAsDialog(oldScreen);
                    oldDialog.NotifyFocusLost();
                }

                var dialogView = view as IDialog;
                if (dialogView == null)
                {
                    throw new ArgumentException("The view is of the wrong type. It should be an IDialogScreenView", "view");
                }
                UpdateOverlay();
            });
        }
        public void LockAllScrolls(ScreenIdentifier screenId, bool isLocked)
        {
            IDialog[] screens = gameObject.GetComponentsInChildren<IDialog>(true);
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].ScreenId == screenId)
                {
                    LockAllScrollsInScreen(screens[i].gameObject, isLocked);
                }
            }
        }

        private void LockAllScrollsInScreen(GameObject screenGO, bool isLocked)
        {
            ScrollRect[] scrolls = screenGO.GetComponentsInChildren<ScrollRect>(true);
            for (int i = 0; i < scrolls.Length; i++)
            {
                scrolls[i].enabled = !isLocked;
            }
        }

        /// <summary>
        /// Sets up the view by setting the InstanceIdentifier.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="view"> The dialog view. </param>
        /// <param name="screenVO"> The screen vo. </param>
        private void SetupDialogView(IScreen view, OpenScreenVO screenVO)
        {
            IDialog dialog = GetViewAsDialog(view);

            var dialogVO = screenVO as OpenDialogVO;

            if (dialogVO == null)
            {
                throw new ArgumentException("The screenVO is of the wrong type. It should be an OpenDialogVO", "screenVO");
            }

            dialog.ActivateLayerOverlay = dialogVO.ActivateOverlay;
            dialog.InstanceIdentifier = dialogVO.InstanceIdentifier;
            //dialog.DialogType = DialogType.Popup;
        }

        private IDialog GetViewAsDialog(IScreen view)
        {
            var dialogScreenView = view as IDialog;

            if (dialogScreenView == null)
            {
                throw new ArgumentException("The view is of the wrong type. It should be an IDialogScreenView", "view");
            }
            return dialogScreenView;
        }

        public void TakeDialogBack(OpenDialogVO outDialogVO, OpenDialogVO inDialogVO)
        {
            CloseScreen(outDialogVO);
            if (inDialogVO.IsOpen)
            {
                IScreen screen;
                inDialogVO.SetAsSecondSibling = true;
                if (ActiveViews.TryGetValue(inDialogVO.Id, out screen))
                {
                    IDialog dialog = GetViewAsDialog(screen);
                    dialog.NotifyFocusBack();
                }
                else
                {
                    inDialogVO.PreventAnimation = true;
                    ShowScreen(inDialogVO);
                }
            }
            CheckForChangedFullScreenCondition(inDialogVO.Id);
        }
        public void OnDialogTakenFromQueue(OpenDialogVO outDialogVO, OpenDialogVO inDialogVO)
        {
            CloseScreen(outDialogVO);
            inDialogVO.SetAsSecondSibling = true;
            ShowScreen(inDialogVO);
        }

        public void RemoveDialog(OpenDialogVO dialog)
        {
            CloseScreen(dialog);
            CheckForChangedFullScreenCondition(dialog.Id);
        }

        public override void CloseScreen(OpenScreenVO screenVO, bool animate = true)
        {
            base.CloseScreen(screenVO, animate);
            UpdatePerformanceOptimizations(false);//so it animates nicely
            
        }

        private void CheckForChangedFullScreenCondition(ScreenIdentifier exceptionId = new ScreenIdentifier())
        {
            bool hasFullScreen = false;
            foreach (KeyValuePair<ScreenIdentifier, IScreen> valuePair in ActiveViews)
            {
                if (valuePair.Key == exceptionId || valuePair.Value.IsAnimating || !valuePair.Value.IsOpen)
                {
                    continue;
                }
                if (((IDialog) valuePair.Value).DialogType == DialogType.FullScreen)
                {
                    hasFullScreen = true;
                    break;
                }
            }

            OnOtherVisibilityChanged(hasFullScreen);
        }

        protected override void OnCloseScreen(ScreenIdentifier screenId)
        {
            base.OnCloseScreen(screenId);
            UpdateOverlay();
        }

        public override void CloseEverything()
        {
            base.CloseEverything();
            UpdateOverlay();
            OnOtherVisibilityChanged(false);
        }

        private void UpdateOverlay()
        {
            bool active = false;
            foreach (IScreen v in ActiveViews.Values)
            {
                var child = (IDialog) v;
                if (child.DialogType == DialogType.Popup && child.ActivateLayerOverlay)
                {
                    active = true;
                    break;
                }
            }
            overlayImage.enabled = active;

            raycaster.enabled = ActiveViews.Count > 0;
        }

        protected override void OnOpenAnimationFinished(IScreen screen)
        {
            base.OnOpenAnimationFinished(screen);
            if (currentDialog.DialogType != DialogType.Popup)
            {
                var list = transform.GetComponentsInChildren<IScreen>();
                foreach (IScreen s in list)
                {
                    if (s != currentDialog && s.gameObject.activeInHierarchy)
                    {
                        var child = (IDialog) s;
                        if (child.IsOpen)
                        {
                            OnCloseScreen(child.ScreenId);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }
            UpdatePerformanceOptimizations(true, GetViewAsDialog(screen));
            CheckForChangedFullScreenCondition();
        }

        private void UpdatePerformanceOptimizations(bool souldDecrease, IDialog dialog = null)
        {
            if (dialog == null)
            {
                TryGetCurrentDialog(out dialog);
            }
            if (dialog == null || !souldDecrease)
            {
                DialogActivityLevelChanged.Fire(ActivityLevel.Dynamic);
                return;
            }
            
            switch (dialog.ActivityLevel)
            {
                case ActivityLevel.Static:
                    //Application.targetFrameRate = Device.Device.Configuration.StaticDialogFPS;
                    DialogActivityLevelChanged.Fire(ActivityLevel.Static);
                    GC.Collect();
                    break;
                case ActivityLevel.AlmostStatic:
                    //Application.targetFrameRate = Device.Device.Configuration.AlmostStaticDialogFPS;
                    DialogActivityLevelChanged.Fire(ActivityLevel.AlmostStatic);
                    break;
            }
        }

        private bool TryGetCurrentDialog(out IDialog currentDialog)
        {
            currentDialog = null;
            foreach (KeyValuePair<ScreenIdentifier, IScreen> valuePair in ActiveViews)
            {
                if (valuePair.Value.IsOpen)
                {
                    currentDialog = (IDialog)valuePair.Value;
                    return true;
                }
            }
            return false;
        }

//        protected override void OnCloseAnimationFinished(IScreenView screenView)
//        {
//            base.OnCloseAnimationFinished(screenView);
//            UpdatePerformanceOptimizations(true);
//        }
    }
}