using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{


    /// <summary>
    /// Child class for Tabs without properties
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public abstract class ADialogTab : ADialogTab<NullScreenProperties>
    {
        protected sealed override void OnDialogPropertiesUpdate()
        {
            //Sealing
        }

    }

    /// <summary>
    /// Abstract class for the tabs that go inside the dialog
    /// </summary>
    public abstract class ADialogTab<TProperties> : MonoBehaviour, IDialogTab//TODO screen registry system: ,ITutorialRegistrableView, ITutorialRegistrableView
        where TProperties : IScreenPropertiesVO
    {
        
        protected TProperties Properties { private set; get; }

        /// <summary>
        /// Same as the parent, don't you ever change this
        /// </summary>
        public int DialogInstanceIdentifier { get; set; }
        /// <summary>
        /// The parent dialog's screen Id
        /// </summary>
        public ScreenIdentifier ScreenId { get; set; }

        public bool IsOpen { get; private set; }

        public bool IsDialogOpen { get; private set; }

        private bool isStarted;

        /// <summary>
        /// It is advised to create an enum and cast this when you need it
        /// It seems that it's not possible to create a generic enum to make your life easier (if you know how let me know)
        /// </summary>
        public int CurrentTabIndex { get; private set; }

        public event GenericEvent ScreenOpened;
        public event GenericEvent ScreenClosed;


        /// <summary>
        /// The parent view tells this view that its properties was updated
        /// </summary>

        protected void Start()
        {
            isStarted = true;

            TabStart();
            if (IsDialogOpen)
            {
                OnDialogOpen();
            }

            if (IsOpen)
            {
                OnTabOpen();
            }
            
        }
        protected abstract void TabStart();


        protected void OnDestroy()
        {
            OnTabDestroy();
        }
        protected abstract void OnTabDestroy();

        /// <summary>
        /// Used by the parent dialog to notify that the the dialog was opened
        /// </summary>
        public void NotifyDialogOpen()
        {
            IsDialogOpen = true;
            if (isStarted)
            {
                OnDialogOpen();
            }
        }
        protected virtual void OnDialogOpen()
        {
        }

        /// <summary>
        /// Used by the parent dialog to notify that the the dialog was closed 
        /// </summary>
        public void NotifyDialogClose()
        {
            IsDialogOpen = false;
            OnDialogClose();
        }
        protected virtual void OnDialogClose()
        {
        }

        /// <summary>
        /// Player opened it or it's the default one
        /// </summary>
        /// <param name="newTabIndex"></param>
        public void NotifyOpen(int newTabIndex)
        {
            IsOpen = true;
            CurrentTabIndex = newTabIndex;
            if (isStarted)
            {
                OnTabOpen();
            }
            ScreenOpened.Fire();
        }

        private void OnTabOpen()
        {
            GetNewProperties();
            OnOpen();
        }
        protected abstract void OnOpen();

        /// <summary>
        /// Player opened another one, so this tab is closing
        /// </summary>
        public void NotifyClose()
        {
            IsOpen = false;
            OnClose();
            ScreenClosed.Fire();
        }
        protected abstract void OnClose();


        private void UpdateProperties()
        {
            GetNewProperties();
            OnDialogPropertiesUpdate();
        }

        protected abstract void OnDialogPropertiesUpdate();

        private void GetNewProperties()
        {
            if (NeedsProperties)
            {
                Properties = ScreenSystem.DialogModel.GetProperties<TProperties>(ScreenId, DialogInstanceIdentifier);
            }
        }

        public bool NeedsProperties
        {
            get
            {
                return typeof(TProperties) != typeof(NullScreenProperties);
            }
        }

        /// <summary>
        /// Whenever the properties are changed from the parent
        /// </summary>
        public void OnNotifyPropertiesUpdate()
        {
            if(isStarted && IsOpen)UpdateProperties();
        }
        
    }
}