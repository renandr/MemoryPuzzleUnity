using System;
using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets.vos
{
    [Serializable]
    public class LabelIconVO
    {
        [SerializeField]
        private RectTransform parent;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Text text;

        public Image Image
        {
            get
            {
                if (!image)
                {
                    image = Parent.GetComponentInChildren<Image>();
                }
                return image;
            }
        }

        public Text Text
        {
            get
            {
                if (!text)
                {
                    text = Parent.GetComponentInChildren<Text>();
                }
                return text;
            }
        }

        public RectTransform Parent
        {
            get
            {
                return parent;
            }
        }

        public bool Active
        {
            get { return Parent.gameObject.activeSelf; }
            set{ Parent.gameObject.SetActive(value);}
        }
    }
}
