using System;
using System.Text;
using GGS.CakeBox.Logging;

namespace GGS.CakeBox.Preferences
{
    /// <summary>
    /// Very simple string obfuscation which works on all platforms.
    /// This is a just a basic protection to make strings unreadable for humans. It is easy to decrypt these strings.
    /// (this basic approach is used because it's fast and because C# security functions don't seem to work properly on all mobile platforms)
    /// </summary>
    public static class PreferenceStringEncryption
    {
        private static readonly byte[] key = {1, 214, 5, 74, 8, 112, 62, 23, 9, 245, 85, 7, 129, 51, 205, 231};

        /// <summary>
        /// Encrypts a string and encodes it with Base64.
        /// The input string is returned if the operation fails.
        /// </summary>
        /// <param name="input">String to encrypt</param>
        /// <returns>Encrypted result string or input string if encryption/encoding failed</returns>
        public static string Encrypt(string input)
        {
            try
            {
                byte[] inputArray = Encoding.UTF8.GetBytes(input);
                byte[] resultArray = new byte[inputArray.Length];
                for (int i = 0; i < inputArray.Length; i++)
                {
                    resultArray[i] = (byte) ((inputArray[i] + key[i % key.Length]) % 256);
                }
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception e)
            {
                GGLog.LogError("Failed to encrypt string: " + e, Prefs.LogType);
                return input;
            }
        }

        /// <summary>
        /// Decodes and decrypts a Base64 encoded string.
        /// The input string is returned if the operation fails.
        /// </summary>
        /// <param name="input">An encrypted and Base64 encoded string</param>
        /// <returns>The plaintext version of the input string or the input string if decryption/decoding failed</returns>
        public static string Decrypt(string input)
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(input);
                byte[] resultArray = new byte[inputArray.Length];
                for (int i = 0; i < inputArray.Length; i++)
                {
                    resultArray[i] = (byte)((inputArray[i] - key[i % key.Length]) % 256);
                }
                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception e)
            {
                GGLog.LogError("Failed to encrypt string: " + e, Prefs.LogType);
                return input;
            }
        }
    }
}
