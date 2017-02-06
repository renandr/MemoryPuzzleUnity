using System;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.CakeBox.Logging
{
    /// <summary>
    /// Log configuration used to enable / disable logs of certain types
    /// </summary>
    [Serializable]
    public class LogConfiguration : ScriptableObject
    {
        /// <summary>
        /// Dictionary logging the log type string to state of being enabled (true) / disabled (false)
        /// </summary>
        [Serializable]
        public class LogConfigurationDictionary : ASerializableDictionary<string, bool>
        {
        }

        [SerializeField]
        private LogConfigurationDictionary logMessageTypeConfigDictionary = new LogConfigurationDictionary();

        /// <summary>
        /// Gets or sets the state of a certain log type
        /// </summary>
        /// <param name="type">The log type</param>
        /// <returns>The state. True for enabled, false for disabled</returns>
        public bool this[string type]
        {
            get
            {
                bool enabled;
                if (logMessageTypeConfigDictionary.TryGetValue(type, out enabled))
                {
                    return enabled;
                }
                return true;
            }
            set
            {
                logMessageTypeConfigDictionary[type] = value;
            }
        }

        /// <summary>
        /// Sets the state of all log types to the same value.
        /// </summary>
        /// <param name="enabled">The state. True for enabled, false for disabled</param>
        public void SetAll(bool enabled)
        {
            string[] logTypes = GGLog.LogTypes;

            for (int i = 0; i < logTypes.Length; i++)
            {
                logMessageTypeConfigDictionary[logTypes[i]] = enabled;
            }
        }

        /// <summary>
        /// Clears the config
        /// </summary>
        public void Clear()
        {
            logMessageTypeConfigDictionary.Clear();
        }
    }
}