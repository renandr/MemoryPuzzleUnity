using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets.UIHelpers
{
    public static class UIInteractionHelper
    {
        private static string savedInputText;

        public static void SaveInput(string textToSave)
        {
            savedInputText = textToSave;
        }

        public static string GetSavedInput()
        {
            return savedInputText;
        }
    }
}
