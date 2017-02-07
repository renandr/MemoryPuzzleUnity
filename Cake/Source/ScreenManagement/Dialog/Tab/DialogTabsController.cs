using System.Collections.Generic;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public class DialogTabsController : DialogTabsController<NullScreenProperties>
    {
        protected sealed override void OnProperties()
        {
            //Sealing
        }

    }

    public class DialogTabsController<TProperties> : ADialogController
        where TProperties : IScreenPropertiesVO
    {
        private const string SelectedTabStateId = "SelectedTab";

#pragma warning disable 649
        [Header("TabManagement")]
        [SerializeField]
        private int tabIndexToStartAt = 0;

        [SerializeField]
        private DialogTabEntry[] dialogTabEntries;

        [SerializeField]
        private GameObject tabButtonsPrefab;

#pragma warning restore 649

        private IDialogTabButtonList tabButtons;

        public event GenericEvent NotifyPropertiesUpdateEvent;

        private List<IDialogTab> tabViews;

        protected TProperties CurrentProperties { get; private set; }


        /// <summary>
        /// Changes when the tab button is clicked
        /// </summary>
        protected int CurrentTabIndex { get; private set; }

        /// <summary>
        /// This only changes after the tab change is complete (you might need to make a server request before)
        /// </summary>
        protected IDialogTab CurrentVisibleTab { get; private set; }


        protected override void DialogControllerAwake()
        {
            tabViews = new List<IDialogTab>();
            for (int i = 0; i < dialogTabEntries.Length; i++)
            {
                IDialogTab tabView = dialogTabEntries[i].View;
                dialogTabEntries[i].ViewObject.SetActive(false);
                if (!tabViews.Contains(tabView))
                {
                    tabViews.Add(tabView);
                }
            }
            GameObject tabGO = Instantiate(tabButtonsPrefab);
            tabButtons = tabGO.GetComponent<IDialogTabButtonList>();
            tabGO.transform.SetParent(transform, false);
            for (int i = 0; i < dialogTabEntries.Length; i++)
            {
                DialogTabEntry dialogTab = dialogTabEntries[i];
                dialogTab.View.DialogInstanceIdentifier = ParentScreen.InstanceIdentifier;
                dialogTab.View.ScreenId = ParentScreen.ScreenId;
                tabButtons.AddButton(dialogTab.ButtonLabelTextId, i);
            }

            UpdateTabsVisibility();
            //tabButtons.Init();
        }

        private void UpdateTabsVisibility()
        {
            var availableTabs = 0;
            for (int i = 0; i < dialogTabEntries.Length; i++)
            {
                DialogTabEntry dialogTab = dialogTabEntries[i];
                dialogTab.View.gameObject.SetActive(false);
                tabButtons.UpdateButtonAccess(dialogTab.IsAccessible, i);
                if (dialogTab.IsAccessible)
                {
                    availableTabs++;
                }
            }
            if (CurrentVisibleTab != null)
            {
                CurrentVisibleTab.gameObject.SetActive(true);
            }
            tabButtons.CurrentActive = CurrentTabIndex;

            tabButtons.gameObject.SetActive(availableTabs > 1);
            UpdateTabContentPosition();
        }

        protected override sealed void OnScreenOpen()
        {
            tabButtons.TabClickedEvent += OnTabButtonClicked;
            foreach (IDialogTab tabView in tabViews)
            {
                tabView.NotifyDialogOpen();
            }
            UpdateTabsVisibility();
            if (CurrentVisibleTab == null)
            {
                ChangeTab(tabIndexToStartAt);
            }

            //tabButtons.Init();
        }


        protected virtual void OnTabDialogOpen()
        {
            
        }

    private void OnTabButtonClicked(int t)
        {
            CurrentTabIndex = t;
            OnTabSelected(CurrentTabIndex);
        }

        /// <summary>
        /// Override this method if you want to request something to server before changing the tab
        /// </summary>
        protected virtual void OnTabSelected(int tabNumber)
        {
            ChangeTab(tabNumber);
        }

        public void ChangeTab(int newTabIndex)
        {
            CurrentTabIndex = newTabIndex;
            CloseCurrentTab();
            //set up new one
            CurrentVisibleTab = dialogTabEntries[CurrentTabIndex].View;
            tabButtons.CurrentActive = CurrentTabIndex;
            NotifyPropertiesUpdateEvent += CurrentVisibleTab.OnNotifyPropertiesUpdate;
            //set visible
            for (int i = 0; i < dialogTabEntries.Length; i++)
            {
                IDialogTab tab = dialogTabEntries[i].View;
                tab.gameObject.SetActive(tab == CurrentVisibleTab);
            }

            UpdateTabContentPosition();

            ScreenSystem.NotifyTabChange(CurrentTabIndex);
            CurrentVisibleTab.NotifyOpen(CurrentTabIndex);
        }

        protected sealed override void OnScreenProperties()
        {
            CurrentProperties = GetProperties<TProperties>();
            NotifyPropertiesUpdate();
            OnProperties();
        }

        protected virtual void OnProperties()
        {
            
        }

        protected sealed override void OnScreenClose()
        {
            CloseCurrentTab();
            CloseTabs();
            OnTabDialogClose();
        }

        protected virtual void OnTabDialogClose()
        {
            
        }


        public void CloseTabs()
        {
            tabButtons.TabClickedEvent -= OnTabButtonClicked;
            foreach (IDialogTab tabView in tabViews)
            {
                tabView.NotifyDialogClose();
            }
            CurrentVisibleTab = null;
            CurrentTabIndex = tabIndexToStartAt;
        }
        
        public void SetTabAccessibility(int tabIndex, bool isAccessible)
        {
            dialogTabEntries[tabIndex].IsAccessible = isAccessible;
            UpdateTabsVisibility();
        }

        public void CloseCurrentTab()
        {
            //clean up previously active tab
            if (CurrentVisibleTab != null)
            {
                NotifyPropertiesUpdateEvent -= CurrentVisibleTab.OnNotifyPropertiesUpdate;
                CurrentVisibleTab.NotifyClose();
            }
        }

        public void NotifyPropertiesUpdate()
        {
            NotifyPropertiesUpdateEvent.Fire();
        }

        private void UpdateTabContentPosition()
        {
            if (CurrentVisibleTab != null)
            {
                var tabRect = CurrentVisibleTab.gameObject.GetComponent<RectTransform>();
                var tabButtonsComponentRect = tabButtons.gameObject.GetComponent<RectTransform>();
                tabRect.offsetMax = new Vector2(tabRect.offsetMax.x,
                    tabButtons.gameObject.activeSelf ?
                        tabButtonsComponentRect.offsetMin.y :
                        tabButtonsComponentRect.offsetMax.y
                    );
            }
        }

        protected override sealed void RestoreState(DialogState viewState)
        {
            ChangeTab(viewState.ReadIntState(SelectedTabStateId));
            LoadDialogState(viewState);
        }

        protected override sealed DialogState SaveState()
        {
            DialogState state = SaveDialogState();
            if (state == null)
            {
                state = new DialogState();
            }
            state.AddIntState(SelectedTabStateId, CurrentTabIndex);
            return state;
        }

        protected virtual void LoadDialogState(DialogState viewState)
        {
        }

        protected virtual DialogState SaveDialogState()
        {
            return null;
        }

        /// <summary>
        /// A wrapper for the component functionality to change the tab
        /// </summary>
        /// <param name="buttonIndex">The index for the button</param>
        /// <param name="number">The number to display</param>
        protected void SetBadge(int buttonIndex, int number)
        {
            tabButtons.SetBadge(buttonIndex, number);
        }
        /// <summary>
        /// Wrapper for the component functionality to change tab buttons effects.
        /// </summary>
        /// <param name="active"></param>
        public void UpdateButtonEffects(bool active)
        {
            tabButtons.UpdateButtonEffects(active);
        }

        /// <summary>
        /// Update gameObject.Active of the effect set for given tab button.
        /// </summary>
        /// <param name="active"></param>
        /// <param name="tabIndex">Index of tab button</param>
        public void UpdateButtonEffects(bool active, int tabIndex)
        {
            tabButtons.UpdateButtonEffects(active,tabIndex);
        }

        public List<string> GetTabTextIds()
        {
            var result = new List<string>();
            foreach (DialogTabEntry tab in dialogTabEntries)
            {
                result.Add(tab.ButtonLabelTextId);
            }
            return result;
        }
    }
}