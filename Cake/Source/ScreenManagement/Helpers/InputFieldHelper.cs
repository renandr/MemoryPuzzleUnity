using System;

namespace GGS.ScreenManagement
{
    public static class InputFieldHelper
    {
        /// <summary>
        /// This is for fixing an issue with Emojis crashing the game, because Unity works with UTF-16.
        /// You should add this to every InputField's onValidateInput event.
        /// </summary>
        public static char ValidateUtf32(string input, int charIndex, char addedChar)
        {
            try
            {
                char.ConvertFromUtf32(addedChar);
            }
            catch (Exception)
            {
                return '\0';
            }
            return addedChar;
        }

    }
}