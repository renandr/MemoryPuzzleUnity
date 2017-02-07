using UnityEngine;

namespace GGS.CakeBox.Preferences
{
    /// <summary>
    /// Base preference handler which is using the Unity preference system.
    /// See http://docs.unity3d.com/ScriptReference/PlayerPrefs.html
    /// It should be used as primary system for saving preferences on all platforms.
    /// </summary>
    public class BasePreferenceHandler : IPreferenceHandler
    {

        /// <summary>
        /// Deletes all preferenes from the <see cref="PlayerPrefs" />
        /// </summary>
        public virtual void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Deletes a key from the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to delete</param>
        public virtual void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        /// <summary>
        /// Checks if a key is in the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>true if key exists, false otherwise</returns>
        public virtual bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// Saves the Unity <see cref="PlayerPrefs" />
        /// </summary>
        public virtual void Save()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Gets a string from the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="defaultValue">Fallback value when key was not found</param>
        /// <returns>Value of the key or the fallback value if they key does not exist</returns>
        public virtual string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        /// <summary>
        /// Gets an integer from the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="defaultValue">Fallback value when key was not found</param>
        /// <returns>Value of the key or the fallback value if they key does not exist</returns>
        public virtual int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        /// <summary>
        /// Gets a float from the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="defaultValue">Fallback value when key was not found</param>
        /// <returns>Value of the key or the fallback value if they key does not exist</returns>
        public virtual float GetFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// Saves a string in the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public virtual void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// Saves an integer in the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public virtual void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        /// <summary>
        /// Saves a float in the <see cref="PlayerPrefs" />
        /// </summary>
        /// <param name="key">Key to use for saving the value</param>
        /// <param name="value">Value to save</param>
        public virtual void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
    }
}