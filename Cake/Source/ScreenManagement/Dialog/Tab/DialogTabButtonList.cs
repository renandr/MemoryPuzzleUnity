using System.Collections.Generic;
using GGS.CakeBox.Logging;
using GGS.CakeBox.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The component for the tab selection
    /// </summary>
    [DisallowMultipleComponent]
    public class DialogTabButtonList : MonoBehaviour, IDialogTabButtonList//UIHorizontalScrollPanel<UIListItem<AListVO>, AListVO>
    {
#pragma warning disable 649

        [SerializeField]
        protected DialogTabButton buttonTemplate;
#pragma warning restore 649

        private Dictionary<int, DialogTabButton> buttons;
        private List<string> buttonLabels;

        public event GenericEvent<int> TabClickedEvent;

        private RectTransform Content;

        public void Awake()
        {
            Content = GetComponent<RectTransform>();
            buttons = new Dictionary<int, DialogTabButton>();
            buttonLabels = new List<string>();
            buttonTemplate.gameObject.SetActive(false);
            Clear();
        }

        private void Clear()
        {
            //removing buttons that the UI artists might have left there
            DialogTabButton[] leftOverButtons = Content.GetComponentsInChildren<DialogTabButton>();
            foreach (var button in leftOverButtons)
            {
                if (button != buttonTemplate && buttons.ContainsValue(button))
                {
                    Destroy(button.gameObject);
                }
            }
            buttons.Clear();
            buttonLabels.Clear();
        }

        /// <summary>
        /// Adds a tab button dynamically
        /// </summary>
        /// <param name="tabName">The localized name of the tab</param>
        public void AddButton(string tabName, int tabIndex)
        {
            DialogTabButton button = Instantiate(buttonTemplate);
            //TODO screen registry system: button.gameObject.AddComponent<TutorialRegistryComponent>();
            button.InitButton(tabName, Content.transform);

            buttons.Add(tabIndex, button);

            buttonLabels.Add(tabName);

            SetButtonLabel(button.GetButton(), tabName);
            button.GetButton().onClick.AddListener(() => { TabClickedEvent.Fire(tabIndex); });
        }

        /// <summary>
        /// Gets the index for the currently displayed tab
        /// </summary>
        public int CurrentActive
        {
            set
            {
                foreach (var button in buttons)
                {
                    button.Value.GetButton().interactable = button.Key != value;
                }
            }
        }

        public void UpdateButtonAccess(bool isAccessible, int i)
        {
            buttons[i].gameObject.SetActive(isAccessible);
        }

        /// <summary>
        /// Update gameObject.Active of all effects set as button effect.
        /// </summary>
        /// <param name="active"></param>
        public void UpdateButtonEffects(bool active)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                UpdateButtonEffects(active, i);
            }
        }

        /// <summary>
        /// Update gameObject.Active of the effect set for given tab button.
        /// </summary>
        /// <param name="active"></param>
        /// <param name="tabIndex">Index of tab button</param>
        public void UpdateButtonEffects(bool active, int tabIndex)
        {
            if (tabIndex < buttons.Count)
            {
                buttons[tabIndex].UpdateSpecialEffectsStatus(active);
            }
            else
            {
                GGLog.LogWarning("Tried to turn on button effect on a tab that has no effect to manipulate", ScreenSystem.LogType);
            }
        }

        /// <summary>
        /// Sets the badge number for the tab, for example, unread messages.
        /// </summary>
        /// <param name="buttonId">The index for the button</param>
        /// <param name="badgeNumber">The number to display</param>
        public void SetBadge(int buttonId, int badgeNumber)
        {
            SetButtonLabel(buttons[buttonId].GetButton(), badgeNumber == 0 ? buttonLabels[buttonId] : buttonLabels[buttonId] + " (" + badgeNumber + ")");
        }

        private void SetButtonLabel(Button button, string text)
        {
            var label = button.GetComponentInChildren<Text>();
            if (label)
            {
                label.text = text;
            }
        }
    }
}
