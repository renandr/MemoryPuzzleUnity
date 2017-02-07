using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// A progressbar with an overlay text which can be changed on demand
    /// </summary>
    public class UIProgressBarText : MonoBehaviour
    {
        /// <summary>
        /// The overlay textfield for the progressbar
        /// </summary>
#pragma warning disable 649
        [SerializeField]
        private Text textField;
#pragma warning restore 649


        /// <summary>
        /// The text of the progress bar
        /// </summary>
        public string Text
        {
            get
            {
                return (textField != null) ? textField.text : "";
            }
            set
            {
                if (textField != null)
                {
                    textField.text = value;
                }
            }
        }
    }
}