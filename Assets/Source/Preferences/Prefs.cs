using System;
using System.Text.RegularExpressions;
using GGS.CakeBox.Logging;
using UnityEngine;

namespace GGS.CakeBox.Preferences
{

    /// <summary>
    /// Class which provides methods to load and save preferences. Works like UnityEngine.PlayerPrefs.
    /// Values are actually stored 
    /// https://sites.google.com/a/goodgamestudios.com/warlands/development/client-mobile/technical-docs/preferences
    /// </summary>
    public static class Prefs
    {
        public const string LogType = "Preferences";

        private static readonly IPreferenceHandler preferenceHandler;

        #region Constructor

        static Prefs()
        {
            GGLog.AddLogType(LogType);

#if !UNITY_EDITOR && UNITY_ANDROID
            preferenceHandler = new AndroidPreferenceHandler();
#elif !UNITY_EDITOR && UNITY_IOS
            string prefServiceName = Application.productName;
            prefServiceName = Regex.Replace(prefServiceName, "[^0-9a-zA-Z]+", "");
            preferenceHandler= new IosPreferenceHandler(prefServiceName + "-Keys");
#else
            preferenceHandler = new BasePreferenceHandler();
#endif
        }

        #endregion

        #region Delete Functions 

        /// <summary>
        /// Deletes ALL preferences.
        /// On Android it also deletes the internal fallback preferences file.
        /// On iOS it also removes the keychain entries.
        /// </summary>
        public static void DeleteAll()
        {
            preferenceHandler.DeleteAll();
        }

        /// <summary>
        /// Deletes the preference with the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">Key of the preference which will be deleted</param>
        public static void DeleteKey(string key)
        {
            preferenceHandler.DeleteKey(key);
        }

        #endregion

        #region Check Function

        /// <summary>
        /// Checks if the preference with the specified <paramref name="key"/> is set.
        /// The type (string/int/float) of the preference is irrelevant for this command.
        /// </summary>
        /// <param name="key">Key of the preference to check</param>
        /// <returns>true if a preference with this <paramref name="key"/> exists, false otherwise</returns>
        public static bool HasKey(string key)
        {
            return preferenceHandler.HasKey(key);
        }

        #endregion

        #region Save Function

        /// <summary>
        /// Saves all preferences to the local file system. This is slow. Do not use in loops/frequently.
        /// </summary>
        public static void Save()
        {
            preferenceHandler.Save();
        }

        #endregion

        #region Value Getters

        /// <summary>
        /// Gets the value of the string preference with the specified <paramref name="key"/>.
        /// The preference must have been set with <see cref="SetString"/> before.
        /// </summary>
        /// <param name="key">Key of the string preference to get</param>
        /// <param name="defaultValue">Value which is returned in case the preference does not exist or is no string preference</param>
        /// <returns>The value of the preference with this <paramref name="key"/> if it exists and is a string preference.
        /// Otherwise <paramref name="defaultValue"/>.</returns>
        public static string GetString(string key, string defaultValue = "")
        {
            string result = preferenceHandler.GetString(key, defaultValue);
            return (!String.IsNullOrEmpty(result) && result != defaultValue) ? PreferenceStringEncryption.Decrypt(result) : result;
        }

        /// <summary>
        /// Gets the value of the int preference with the specified <paramref name="key"/>.
        /// The preference must have been set with <see cref="SetInt"/> before.
        /// </summary>
        /// <param name="key">Key of the int preference to get</param>
        /// <param name="defaultValue">Value which is returned in case the preference does not exist or is no int preference</param>
        /// <returns>The value of the preference with this <paramref name="key"/> if it exists and is an int preference.
        /// Otherwise <paramref name="defaultValue"/>.</returns>
        public static int GetInt(string key, int defaultValue = 0)
        {
            return preferenceHandler.GetInt(key, defaultValue);
        }

        /// <summary>
        /// Gets the value of the float preference with the specified <paramref name="key"/>.
        /// The preference must have been set with <see cref="SetFloat"/> before.
        /// </summary>
        /// <param name="key">Key of the float preference to get</param>
        /// <param name="defaultValue">Value which is returned in case the preference does not exist or is no float preference</param>
        /// <returns>The value of the preference with this <paramref name="key"/> if it exists and is a float preference.
        /// Otherwise <paramref name="defaultValue"/>.</returns>
        public static float GetFloat(string key, float defaultValue = 0f)
        {
            return preferenceHandler.GetFloat(key, defaultValue);
        }

        #endregion

        #region Value Setters

        /// <summary>
        /// Sets the value of the preference with the specified <paramref name="key"/> to the string <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key for the preference to be set</param>
        /// <param name="value">The string value for the preference</param>
        public static void SetString(string key, string value)
        {
            preferenceHandler.SetString(key, PreferenceStringEncryption.Encrypt(value));
        }

        /// <summary>
        /// Sets the value of the preference with the specified <paramref name="key"/> to the int <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key for the preference to be set</param>
        /// <param name="value">The int value for the preference</param>
        public static void SetInt(string key, int value)
        {
            preferenceHandler.SetInt(key, value);
        }

        /// <summary>
        /// Sets the value of the preference with the specified <paramref name="key"/> to the float <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key for the preference to be set</param>
        /// <param name="value">The float value for the preference</param>
        public static void SetFloat(string key, float value)
        {
            preferenceHandler.SetFloat(key, value);
        }

        #endregion
    }
}