using UnityEngine;
using UnityEngine.UI;

namespace GGS.ScreenManagement
{
    public class DialogTabButton : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Button button;

        [SerializeField]
        private GameObject specialEffects;
#pragma warning restore 649

        public Button GetButton()
        {
            return button;
        }

        public void InitButton(string buttonName, Transform parent)
        {
            button.name = buttonName;
            button.gameObject.SetActive(true);
            button.gameObject.transform.SetParent(parent, false);
        }

        public void UpdateSpecialEffectsStatus(bool isOn)
        {
            specialEffects.SetActive(isOn);
        }
    }
}
