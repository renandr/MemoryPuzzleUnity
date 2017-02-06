using com.goodgamestudios.warlands.uiWidgets.UIHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    [System.Serializable]
    public class UILabelledImage
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Text label;

        private Sprite sprite;
        private string text;

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
                if (image != null)
                {
                    image.sprite = sprite;
                }
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                if (label != null)
                {
                    label.text = text;
                }
            }
        }

        public Image Image
        {
            get
            {
                return image;
            }
        }

        public Text Label
        {
            get
            {
                return label;
            }
        }

        public UILabelledImage(Sprite sprite, string text)
        {
            Sprite = sprite;
            Text = text;
        }

        public void Setup(Sprite sprite, string text)
        {
            Sprite = sprite;
            Text = text;
        }

        public void Setup(UILabelledImage info)
        {
            Sprite = info.Sprite;
            Text = info.Text;
        }
    }
}
