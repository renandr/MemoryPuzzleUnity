using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// Class which can be assigned to an image to dynamically switch between sprites which can be assigned in the editor
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UIToggleButton : MonoBehaviour
    {
#pragma warning disable 649
        [Header("Icon")]
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Sprite onIconSprite;

        [SerializeField]
        private Sprite offIconSprite;

        [Header("Button")]
        [SerializeField]
        private Image buttonImage;

        [SerializeField]
        private Sprite onButtonSprite;

        [SerializeField]
        private Sprite offButtonSprite;
#pragma warning restore 649

        public bool IsToggledOn
        {
            get
            {
                if (iconImage != null)
                {
                    return (iconImage.sprite == onIconSprite);
                }
                return false;
            }
            set
            {
                if (iconImage != null)
                {
                    iconImage.sprite = value ? onIconSprite : offIconSprite;
                }
                if (buttonImage != null)
                {
                    buttonImage.sprite = value ? onButtonSprite : offButtonSprite;
                }
                
            }
        }
    }
}