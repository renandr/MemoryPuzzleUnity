using UnityEngine;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    [AddComponentMenu("UI/UIImage")]
    public class UIImage : Image
    {
        [SerializeField]
        private bool interactable = true;

        public Color disabledColor = new Color(0.784f, 0.784f, 0.784f, 0.502f);

        private Color tempColor;

        protected override void Awake()
        {
            base.Awake();
            tempColor = color;
        }

        public bool Interactable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
                Greyout(interactable);
            }
        }

        private void Greyout(bool active)
        {
            if (Application.isPlaying)
            {
                color = active ? tempColor : disabledColor;
            }
        }
    }
}
