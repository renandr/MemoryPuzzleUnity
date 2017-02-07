using System;
using System.Collections.Generic;
using System.Globalization;
using GGS.CakeBox.Logging;

namespace GGS.CakeBox.Preferences
{
    /// <summary>
    /// Preference handler which is used on iOS devices
    /// - Writes all preferences using the Unity PlayerPrefs system (using BasePreferenceHandler)
    /// - Additionally saves all values in the iOS keychain which is kept when the app is uninstalled
    /// </summary>
    public class IosPreferenceHandler : BasePreferenceHandler
    {
        private const string IOSKeychainKeyForKeys = "StoredKeys";
        private const char KeyDelimitter = '|';

        private readonly string iOSKeychainServiceName;
        private HashSet<string> storedKeys;

        public IosPreferenceHandler(string keychainServiceName)
        {
            iOSKeychainServiceName = keychainServiceName;
            RefreshStoredKeyHash();
        }

        private void RefreshStoredKeyHash()
        {
            storedKeys = new HashSet<string>();
            string combinedKeysString = IOSKeyChain.Get(iOSKeychainServiceName, IOSKeychainKeyForKeys);

            if (!string.IsNullOrEmpty(combinedKeysString))
            {
                string[] keys = combinedKeysString.Split(KeyDelimitter);

                for (int i = 0; i < keys.Length; ++i)
                {
                    storedKeys.Add(keys[i]);
                }    
            }
        }

        /// <summary>
        /// Deletes all preferenes from the <see cref="PlayerPrefs" />
        /// Also removes all preferences from  the IOS Keychain by iterating over all known keys and removing them
        /// </summary>
        public override void DeleteAll()
        {
            base.DeleteAll();

            foreach (string key in storedKeys)
            {
                IOSKeyChain.Delete(iOSKeychainServiceName, key);
            }

            storedKeys.Clear();
            Save();
        }

        /// <summary>
        /// Deletes a key from the <see cref="PlayerPrefs" />
        /// Also deletes the key from the iOS keychain
        /// </summary>
        /// <param name="key">Key to delete</param>
        public override void DeleteKey(string key)
        {
            base.DeleteKey(key);
            IOSKeyChain.Delete(iOSKeychainServiceName, key);
            storedKeys.Remove(key);
        }

        /// <summary>
        /// Checks if a key is in the <see cref="PlayerPrefs" /> or in the iOS keychain
        /// </summary>
        /// <param name="key">Key to delete</param>
        public override bool HasKey(string key)
        {
            return base.HasKey(key) || (!String.IsNullOrEmpty(IOSKeyChain.Get(iOSKeychainServiceName, key)));
        }

        /// <summary>
        /// Gets a string from the <see cref="PlayerPrefs" /> or from the iOS keychain
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="defaultValue">Fallback value when key was not found</param>
        /// <returns>Value of the key or the fallback value if they key does not exist</returns>
        public override string GetString(string key, string defaultValue = "")
        {
            if (base.HasKey(key))
            {
                return base.GetString(key, defaultValue);
            }
            string value = IOSKeyChain.Get(iOSKeychainServiceName, key);
            return String.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// Gets an integer from the <see cref="PlayerPrefs" /> or from the iOS keychain
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="defaultValue">Fallback value when key was not found</param>
        /// <returns>Value of the key or the fallback value if they key does not exist</returns>
        public override int GetInt(string key, int defaultValue = 0)
        {
            if (base.HasKey(key))
            {
                return base.GetInt(key, defaultValue);
            }
            string value = IOSKeyChain.Get(iOSKeychainServiceName, key);
            if (String.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            int intValue;
            return Int32.TryParse(value, out intValue) ? intValue : defaultValue;
        }

        /// <summary>
        /// Gets a float from the <see cref="PlayerPrefs" /> or the iOS keychain
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="defaultValue">Fallback value when key was not found</param>
        /// <returns>Value of the key or the fallback value if they key does not exist</returns>
        public override float GetFloat(string key, float defaultValue = 0f)
        {
            if (base.HasKey(key))
            {
                return base.GetFloat(key, defaultValue);
            }
            string value = IOSKeyChain.Get(iOSKeychainServiceName, key);
            if (String.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            float floatValue;
            return Single.TryParse(value, out floatValue) ? floatValue : defaultValue;
        }

        /// <summary>
        /// Saves a string in the <see cref="PlayerPrefs" /> and the iOS keychain
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public override void SetString(string key, string value)
        {
            base.SetString(key, value);
            if (!IOSKeyChain.Set(iOSKeychainServiceName, key, value))
            {
                GGLog.LogError("Failed to save iOS preference string key-value-pair for key " + key, Prefs.LogType);
            }
            else
            {
                storedKeys.Add(key);
            }
        }

        /// <summary>
        /// Saves an integer in the <see cref="PlayerPrefs" /> and the iOS keychain
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public override void SetInt(string key, int value)
        {
            base.SetInt(key, value);
            if (!IOSKeyChain.Set(iOSKeychainServiceName, key, Convert.ToString(value)))
            {
                GGLog.LogError("Failed to save iOS preference int key-value-pair for key " + key, Prefs.LogType);
            }
            else
            {
                storedKeys.Add(key);
            }
        }

        /// <summary>
        /// Saves a float in the <see cref="PlayerPrefs" /> and the iOS keychain
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public override void SetFloat(string key, float value)
        {
            base.SetFloat(key, value);
            if (!IOSKeyChain.Set(iOSKeychainServiceName, key, Convert.ToString(value, CultureInfo.InvariantCulture)))
            {
                GGLog.LogError("Failed to save iOS preference float key-value-pair for key " + key, Prefs.LogType);
            }
            else
            {
                storedKeys.Add(key);
            }
        }

        /// <summary>Saves the Unity <see cref="PlayerPrefs" /></summary>
        /// <seealso cref="M:com.goodgamestudios.warlands.preferences.platformHandlers.BasePreferenceHandler.Save()"/>
        public override void Save()
        {
            base.Save();

            string combinedValue = "";
            foreach (string storedKey in storedKeys)
            {
                combinedValue += storedKey + KeyDelimitter;
            }
            IOSKeyChain.Set(iOSKeychainServiceName, IOSKeychainKeyForKeys, combinedValue);
        }
    }
}