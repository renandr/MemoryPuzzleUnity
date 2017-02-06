using System;
using UnityEngine;

namespace GGS.ScreenManagement
{
    [Serializable]
    public class DialogTabEntry
    {
        public GameObject ViewObject;

        [SerializeField]//TODO solve this localization thing, LocalizedString]
        private string buttonLabelTextId;

        private IDialogTab view;
        public IDialogTab View
        {
            get
            {
                if (view == null)
                {
                    view = ViewObject.GetComponent<IDialogTab>();
                }
                return view;
            }
        }

        private bool isAccessible = true;

        public string ButtonLabelTextId
        {
            get
            {
                return buttonLabelTextId;
            }
        }


        /// <summary>
        /// If the tab button should be in the list (mainly for the restricion system)
        /// </summary>
        public bool IsAccessible
        {
            get
            {
                return isAccessible;
            }
            set
            {
                isAccessible = value;
            }
        }
    }
}