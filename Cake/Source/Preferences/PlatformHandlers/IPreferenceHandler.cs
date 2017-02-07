namespace GGS.CakeBox.Preferences
{
    /// <summary>
    /// Interface for all preference handlers
    /// </summary>
    public interface IPreferenceHandler
    {
        /// <summary>
        /// Deletes ALL preferences.
        /// On Android it also deletes the internal fallback preferences file.
        /// On iOS it also removes the keychain entries.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Deletes the preference with the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">Key of the preference which will be deleted</param>
        void DeleteKey(string key);

        /// <summary>
        /// Checks if the preference with the specified <paramref name="key"/> is set.
        /// The type (string/int/float) of the preference is irrelevant for this command.
        /// </summary>
        /// <param name="key">Key of the preference to check</param>
        /// <returns>true if a preference with this <paramref name="key"/> exists, false otherwise</returns>
        bool HasKey(string key);

        /// <summary>
        /// Saves all preferences to the local file system. This is slow. Do not use in loops/frequently.
        /// </summary>
        void Save();

        /// <summary>
        /// Gets the value of the string preference with the specified <paramref name="key"/>.
        /// The preference must have been set with <see cref="SetString"/> before.
        /// </summary>
        /// <param name="key">Key of the string preference to get</param>
        /// <param name="defaultValue">Value which is returned in case the preference does not exist or is no string preference</param>
        /// <returns>The value of the preference with this <paramref name="key"/> if it exists and is a string preference.
        /// Otherwise <paramref name="defaultValue"/>.</returns>
        string GetString(string key, string defaultValue = "");

        /// <summary>
        /// Gets the value of the int preference with the specified <paramref name="key"/>.
        /// The preference must have been set with <see cref="SetInt"/> before.
        /// </summary>
        /// <param name="key">Key of the int preference to get</param>
        /// <param name="defaultValue">Value which is returned in case the preference does not exist or is no int preference</param>
        /// <returns>The value of the preference with this <paramref name="key"/> if it exists and is an int preference.
        /// Otherwise <paramref name="defaultValue"/>.</returns>
        int GetInt(string key, int defaultValue = 0);

        /// <summary>
        /// Gets the value of the float preference with the specified <paramref name="key"/>.
        /// The preference must have been set with <see cref="SetFloat"/> before.
        /// </summary>
        /// <param name="key">Key of the float preference to get</param>
        /// <param name="defaultValue">Value which is returned in case the preference does not exist or is no float preference</param>
        /// <returns>The value of the preference with this <paramref name="key"/> if it exists and is a float preference.
        /// Otherwise <paramref name="defaultValue"/>.</returns>
        float GetFloat(string key, float defaultValue = 0f);

        /// <summary>
        /// Sets the value of the preference with the specified <paramref name="key"/> to the string <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key for the preference to be set</param>
        /// <param name="value">The string value for the preference</param>
        void SetString(string key, string value);

        /// <summary>
        /// Sets the value of the preference with the specified <paramref name="key"/> to the int <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key for the preference to be set</param>
        /// <param name="value">The int value for the preference</param>
        void SetInt(string key, int value);

        /// <summary>
        /// Sets the value of the preference with the specified <paramref name="key"/> to the float <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key for the preference to be set</param>
        /// <param name="value">The float value for the preference</param>
        void SetFloat(string key, float value);
    }
}