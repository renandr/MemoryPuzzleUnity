using System;
using UnityEngine;

namespace GGS.CakeBox.Logging
{
    /// <summary>
    /// Scriptable object used to keep track of existing log types.
    /// This is updated dynamically when the game is run in the Unity Editor.
    /// </summary>
    [Serializable]
    public class LogTypeCache : ScriptableObject
    {
        [SerializeField]
        private string[] logTypes = new string[0];

        public string[] LogTypes
        {
            get { return logTypes; }
        }

        /// <summary>
        /// Adds a log type
        /// </summary>
        /// <param name="logType">The log type to add</param>
        public void AddLogType(string logType)
        {
            string[] types = new string[logTypes.Length + 1];
            for (int i = 0; i < logTypes.Length; i++)
            {
                types[i] = logTypes[i];
            }
            types[types.Length - 1] = logType;
            logTypes = types;
        }

        /// <summary>
        /// Clears the log type cache
        /// </summary>
        public void Clear()
        {
            logTypes = new string[0];
        }
    }
}