using System;
using System.Collections.Generic;
using System.IO;
using GGS.CakeBox.Logging;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.CakeBox.Preferences
{
    /// <summary>
    /// Preference handler which is used on Android devices
    /// - Writes all preferences using the Unity PlayerPrefs system (using BasePreferenceHandler)
    /// - Additionally saves all values to a fallback file which is kept when the app is uninstalled
    /// </summary>
    public class AndroidPreferenceHandler : BasePreferenceHandler
    {
        /// <summary>
        /// File format version for the preference file. Can be used for format changes / extensions in future
        /// </summary>
        private const int FileVersion = 1;

        /// <summary>
        /// File name for the Android fallback file
        /// </summary>
        private const string FileName = "preferences";

        /// <summary>
        /// Path for the Android fallback file
        /// </summary>
        private static readonly string filePath = FileUtil.MultiPathCombine("/mnt/sdcard/Android/data", Application.companyName, Application.productName, FileName);

        /// <summary>
        /// The Android 6 SDK level.
        /// Every Android version BELOW this will read/write a fallback file.
        /// Every version >= this will not try to read/write a fallback file because it will fail due to missing permissions.
        /// </summary>
        private const int Android6SdkLevel = 23;

        /// <summary>
        /// The detected Android SDK level
        /// </summary>
        private int androidSdkLevel;

        /// <summary>
        /// Dictionaries to cache the contents of the Android fallback file at runtime.
        /// This way the actual file only needs to be read once (at startup) from the file system.
        /// </summary>
        private readonly Dictionary<string, string> strings = new Dictionary<string, string>();
        private readonly Dictionary<string, int> ints = new Dictionary<string, int>();
        private readonly Dictionary<string, float> floats = new Dictionary<string, float>();

        /// <summary>
        /// Constructor which loads all preferences from the fallback preference file into dictionaries
        /// </summary>
        public AndroidPreferenceHandler()
        {
            DetectAndroidSdkLevel();
            ReadPreferenceFile();
        }

        /// <summary>
        /// Deletes all preferenes from the <see cref="PlayerPrefs" /> and the Android fallback file/dictionaries
        /// </summary>
        public override void DeleteAll()
        {
            base.DeleteAll();
            strings.Clear();
            ints.Clear();
            floats.Clear();

            // Skip the fallback file handling for Android 6+
            if (androidSdkLevel >= Android6SdkLevel)
            {
                return;
            }

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
                // When failed to remove try to write file without values
                GGLog.LogError("Failed to remove Android fallback pref files " + filePath, Prefs.LogType);
                Save();
            }
        }

        /// <summary>
        /// Deletes a key from the <see cref="PlayerPrefs" /> and the Android fallback dictionaries
        /// </summary>
        /// <param name="key">Key to delete</param>
        public override void DeleteKey(string key)
        {
            base.DeleteKey(key);
            strings.Remove(key);
            ints.Remove(key);
            floats.Remove(key);
        }

        /// <summary>
        /// Checks if a key is in the <see cref="PlayerPrefs" /> or the Android fallback dictionaries
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>true if key exists, false otherwise</returns>
        public override bool HasKey(string key)
        {
            return base.HasKey(key) || strings.ContainsKey(key) || ints.ContainsKey(key) || floats.ContainsKey(key);
        }

        /// <summary>
        /// Gets a string from the <see cref="PlayerPrefs" /> or the Android fallback dictionaries
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
            string fallbackString;
            return strings.TryGetValue(key, out fallbackString) ? fallbackString : defaultValue;
        }

        /// <summary>
        /// Gets an integer from the <see cref="PlayerPrefs" /> or the Android fallback dictionaries
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
            int fallbackInt;
            return ints.TryGetValue(key, out fallbackInt) ? fallbackInt : defaultValue;
        }

        /// <summary>
        /// Gets a float from the <see cref="PlayerPrefs" /> or the Android fallback dictionaries
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
            float fallbackFloat;
            return floats.TryGetValue(key, out fallbackFloat) ? fallbackFloat : defaultValue;
        }

        /// <summary>
        /// Saves a string in the <see cref="PlayerPrefs" /> and the Android fallback dictionaries
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public override void SetString(string key, string value)
        {
            base.SetString(key, value);
            strings[key] = value;
            ints.Remove(key);
            floats.Remove(key);
        }

        /// <summary>
        /// Saves an integer in the <see cref="PlayerPrefs" /> and the Android fallback dictionaries
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public override void SetInt(string key, int value)
        {
            base.SetInt(key, value);
            strings.Remove(key);
            ints[key] = value;
            floats.Remove(key);
        }

        /// <summary>
        /// Saves a float in the <see cref="PlayerPrefs" /> and the Android fallback dictionaries
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public override void SetFloat(string key, float value)
        {
            base.SetFloat(key, value);
            strings.Remove(key);
            ints.Remove(key);
            floats[key] = value;
        }

        /// <summary>
        /// Reads the preference file.
        /// Attention: The order (strings/ints/floats) is important and must not be changed!
        /// </summary>
        private void ReadPreferenceFile()
        {
            // Skip this for Android 6+
            if (androidSdkLevel >= Android6SdkLevel)
            {
                return;
            }

            if (File.Exists(filePath))
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                    {
                        // File format version. Can be used later to handle format changes, currently unused
                        reader.ReadInt32();

                        ReadStrings(reader);
                        ReadInts(reader);
                        ReadFloats(reader);
                    }
                }
                catch (Exception exception)
                {
                    GGLog.LogError("Failed to load Android fallback preferences: " + exception.Message + " path: " + filePath, Prefs.LogType);
                }
            }
        }

        private void ReadStrings(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string keyString = reader.ReadString();
                string value = reader.ReadString();
                try
                {
                    SetString(keyString, value);
                }
                catch (ArgumentException)
                {
                    GGLog.LogError("'" + keyString + "' is no valid PreferenceKey.", Prefs.LogType);
                }
            }
        }

        private void ReadInts(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string keyString = reader.ReadString();
                int value = reader.ReadInt32();
                try
                {
                    SetInt(keyString, value);
                }
                catch (ArgumentException)
                {
                    GGLog.LogError("'" + keyString + "' is no valid PreferenceKey.", Prefs.LogType);
                }
            }
        }

        private void ReadFloats(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string keyString = reader.ReadString();
                float value = reader.ReadSingle();
                try
                {
                    SetFloat(keyString, value);
                }
                catch (ArgumentException)
                {
                    GGLog.LogError("'" + keyString + "' is no valid PreferenceKey.", Prefs.LogType);
                }
            }
        }

        /// <summary>
        /// Saves the Unity <see cref="PlayerPrefs" /> and saves the values from the dictionaries in the Android fallback file
        /// </summary>
        public override void Save()
        {
            base.Save();
            SavePreferenceFile();
        }

        /// <summary>
        /// Saves the preference dictionary in a file
        /// Attention: The order (strings/ints/floats) is important and must not be changed!
        /// </summary>
        private void SavePreferenceFile()
        {
            // Skip this for Android 6+
            if (androidSdkLevel >= Android6SdkLevel)
            {
                return;
            }

            try
            {
                CreateDirectories();

                using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                {
                    writer.Write(FileVersion);

                    WriteStrings(writer);
                    WriteInts(writer);
                    WriteFloats(writer);
                }
            }
            catch (Exception exception)
            {
                GGLog.LogError("Failed to save Android preference file: " + exception.Message, Prefs.LogType);
            }
        }

        private void CreateDirectories()
        {
            String path = Path.GetDirectoryName(filePath);
            if (!String.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void WriteStrings(BinaryWriter writer)
        {
            writer.Write(strings.Count);
            foreach (KeyValuePair<string, string> fallbackString in strings)
            {
                writer.Write(fallbackString.Key);
                writer.Write(fallbackString.Value);
            }
        }

        private void WriteInts(BinaryWriter writer)
        {
            writer.Write(ints.Count);
            foreach (KeyValuePair<string, int> fallbackInt in ints)
            {
                writer.Write(fallbackInt.Key);
                writer.Write(fallbackInt.Value);
            }
        }

        private void WriteFloats(BinaryWriter writer)
        {
            writer.Write(floats.Count);
            foreach (KeyValuePair<string, float> fallbackFloat in floats)
            {
                writer.Write(fallbackFloat.Key);
                writer.Write(fallbackFloat.Value);
            }
        }

        private void DetectAndroidSdkLevel()
        {
            androidSdkLevel = 0;
            try
            {
                using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    androidSdkLevel = version.GetStatic<int>("SDK_INT");
                }
            }
            catch (Exception e)
            {
                GGLog.LogError("Failed to detect Android SDK level: " + e, Prefs.LogType);
            }
            if (androidSdkLevel < Android6SdkLevel)
            {
                GGLog.Log("Android SDK level is " + androidSdkLevel + "<" + Android6SdkLevel + " - will use fallback preference file", Prefs.LogType);
            }
            else
            {
                GGLog.Log("Android SDK level is " + androidSdkLevel + ">=" + Android6SdkLevel + " - will NOT use fallback preference file due to missing permissions", Prefs.LogType);
            }
        }

    }
}
