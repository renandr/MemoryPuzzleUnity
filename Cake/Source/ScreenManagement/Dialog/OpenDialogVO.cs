using System;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Information about opening dialogs, if it should be closeable with Android's back button and the dialog priority
    /// </summary>
    public class OpenDialogVO : OpenScreenVO, IEquatable<OpenDialogVO>
    {
        private DialogState screenState;

        public bool BackButtonCloseable { get; set; }

        public bool IsPopup { get; set; }

        public bool ActivateOverlay { get; set; }

        public bool IsOpen;

        public OpenDialogVO(ScreenIdentifier uniqueId, IScreenPropertiesVO properties = null)
            : base(uniqueId, properties)
        {
            BackButtonCloseable = true;
            ActivateOverlay = true;
            IsOpen = true;
        }

        public void ReplaceContents(OpenDialogVO newDialogVO)
        {
            base.ReplaceContents(newDialogVO);
            BackButtonCloseable = newDialogVO.BackButtonCloseable;
        }


        public DialogState ScreenState
        {
            set
            {
                screenState = value;
            }
            get
            {
                DialogState state = screenState;
                screenState = null;
                return state; 
            }
        }

        #region Implementation of IEquatable<OpenDialogVO>

        public bool Equals(OpenDialogVO other)
        {
            return Id.Equals(other.Id);
        }

        #endregion
    }
}