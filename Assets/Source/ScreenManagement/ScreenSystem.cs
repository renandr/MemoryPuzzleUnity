using System;
using System.Collections.Generic;
using GGS.GameLocks;
using GGS.CakeBox.Utils;
using GGS.CakeBox.Logging;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public static class ScreenSystem
    {
        internal static DialogModel DialogModelWritter { get; set; }
        internal static PanelModel PanelModelWritter { get; set; }

        internal static event GenericEvent LoadTestEnabled;
        internal static event GenericEvent<ToastVO> ToastRequested;
        internal static event GenericEvent<string, string> PreCacheScreenRequested;
        internal static event GenericEvent<bool> FadeOutPanelsRequested;
        internal static event GenericEvent<ScreenIdentifier, bool> LockAllDialogScrollsRequested;

        public static event GenericEvent<OpenDialogVO> DialogAddedToModel;
        public static event GenericEvent<OpenDialogVO, OpenDialogVO> DialogTakenFromHistory;
        public static event GenericEvent<OpenDialogVO, OpenDialogVO> DialogTakenFromQueue;
        public static event GenericEvent<OpenDialogVO> DialogRemovedFromModel;
        public static event GenericEvent<OpenDialogVO, OpenDialogVO> DialogPushedToHistory;
        public static event GenericEvent DialogModelHistoryEmpty;
        public static event GenericEvent DialogHistoryClosed;

        /// <summary>
        /// Only fired when value changes
        /// </summary>
        public static event GenericEvent<string, bool> LayerRestrictionChanged;

        public static event GenericEvent<OpenPanelVO> PanelAddedToModel;
        public static event GenericEvent<ScreenIdentifier> PanelContentUpdated;
        public static event GenericEvent<OpenPanelVO> PanelRemovedFromModel;

        public static event GenericEvent<string, ScreenIdentifier> ScreenIsActive;
        public static event GenericEvent<int> TabChanged;

        private static Dictionary<string, bool> LayerRestrictions = new Dictionary<string, bool>();

        public static IDialogModelReader DialogModel { get; internal set; }
        public static IPanelModelReader PanelModel { get; internal set; }

        public static readonly string LogType = "ScreenSystem";

        private static Action<string, Action<GameObject>> loadAssetBundleAction;

        public static void Init(Action<string, Action<GameObject>> loadAssetBundleAction)
        {
            GGLog.AddLogType(LogType);
            ScreenSystem.loadAssetBundleAction = loadAssetBundleAction;
            PanelModel = PanelModelWritter = new PanelModel();
            DialogModel = DialogModelWritter = new DialogModel();
        }

        public static void RegisterLayer(string id)
        {
            LayerRestrictions.Add(id, true);
        }


        public static void OpenDialog(ScreenIdentifier screenId, DialogPriority priority)
        {
            OpenDialog(new OpenDialogVO(screenId), priority);
        }

        public static void OpenDialog(OpenDialogVO openDialogVO, DialogPriority priority)
        {

            var command = new OpenDialogCommand();
            command.DialogAddedToModel += DialogAddedToModel;
            command.DialogPushedToHistory += DialogPushedToHistory;
            command.Execute(openDialogVO, priority);
        }

        public static void CloseHistoryDialogs()
        {
            DialogModelWritter.ClearHistory();
            DialogHistoryClosed.Fire();
            if (DialogModelWritter.HasDialogsInQueue())
            {
                OpenDialog(DialogModelWritter.GetNextDialog(), DialogPriority.ForceForeground);
            }
        }

        public static void OpenQueuedLocationDialogs()
        {
            if (DialogModelWritter.CurrentDialog == null)
            {
                if (DialogModelWritter.HasDialogsInQueue())
                {
                    OpenDialogVO next = DialogModelWritter.GetNextDialog();

                    DialogModelWritter.SetCurrentDialog(next);
                    DialogAddedToModel.Fire(next);
                }
            }
        }

        public static void CloseDialog(ScreenIdentifier dialogIdParameter)
        {
            var command = new CloseDialogCommand();
            command.DialogRemovedFromModel += DialogRemovedFromModel;
            command.DialogTakenFromHistory += DialogTakenFromHistory;
            command.DialogTakenFromQueue += DialogTakenFromQueue;
            command.DialogModelHistoryCleared += DialogModelHistoryEmpty;
            command.Execute(dialogIdParameter);
        }

        public static void OpenPanel(OpenPanelVO openPanelVO)
        {
            if (!PanelModel.Contains(openPanelVO.Id))
            {
                PanelModelWritter.Add(openPanelVO);
                PanelAddedToModel.Fire(openPanelVO);
            }
            else
            {
                PanelModelWritter.ReplaceContents(openPanelVO);
                PanelContentUpdated.Fire(openPanelVO.Id);
            }
        }

        public static void ClosePanelsByType(string panelAssetName)
        {
            List<OpenPanelVO> list = PanelModelWritter.RemoveByType(panelAssetName);

            if (list.Count > 0)
            {
                foreach (OpenPanelVO openPanelVO in list)
                {
                    PanelRemovedFromModel.Fire(openPanelVO);
                }
            }
        }

        public static void ClosePanel(ScreenIdentifier panelIdParameter)
        {
            if (LockSystem.Model.ContainsCustomLock(CustomLockReason.LoadingAsset, panelIdParameter.AssetId))
            {
                return;
            }

            OpenPanelVO panelVO = PanelModelWritter.Remove(panelIdParameter);
            if (panelVO != null)
            {
                PanelRemovedFromModel.Fire(panelVO);
            }
        }

        internal static void NotifyScreenActive(string layer, ScreenIdentifier id)
        {
            ScreenIsActive.Fire(layer, id);
        }

        internal static void NotifyTabChange(int tabIndex)
        {
            TabChanged.Fire(tabIndex);
        }

        public static void EnableUILoadTest()
        {
            LoadTestEnabled.Fire();
        }

        public static void ShowToast(ToastVO vo)
        {
            ToastRequested.Fire(vo);
        }

        public static void PreCacheScreen(string id, string type)
        {
            PreCacheScreenRequested.Fire(id, type);
        }

        public static void FadeOutPanels(bool value)
        {
            FadeOutPanelsRequested.Fire(value);
        }

        public static void LockAllDialogScrolls(ScreenIdentifier id, bool value)
        {
            LockAllDialogScrollsRequested.Fire(id, value);
        }

        public static bool IsOpeningScreensEnabled(string layer)
        {
            return LayerRestrictions[layer];
        }
        public static bool ChangeOpeningScreensRestriction(string layer, bool value)
        {
            bool t = LayerRestrictions[layer];
            LayerRestrictions[layer] = value;
            if (t != value)
            {
                LayerRestrictionChanged.Fire(layer, value);
            }
            return value;
        }

        public static void LoadAssetBundle(string asset, Action<GameObject> action)
        {
            if (loadAssetBundleAction == null)
            {
                throw new MissingFieldException("Missing the static method to load the asset bundles");
            }
            loadAssetBundleAction.Invoke(asset, action);
        }

    }
}