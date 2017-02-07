using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GGS.CakeBox.Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;
using FileUtil = GGS.CakeBox.Utils.FileUtil;

namespace GGS.CakeBox.Logging
{
    /// <summary>
    /// Logging class which provides logging methods
    /// Configuration in editor @ Assets/Resources/Configuration/LogConfiguration (file should be put on git ignore)
    /// 
    /// The logging system is using strings for the log types.
    /// Each log type should be registered using <see cref="AddLogType"/> before using it for the first time.
    /// This way the system knows about it and exposes a checkbox for it in the log configuration.
    /// 
    /// When you run the game in the editor, the log system will:
    /// - Create a log configuration if none is present.
    /// - Create a log type cache. This cache is used to keep track of used log types.
    /// </summary>
    public static class GGLog
    {
        private const string TimestampFormat = "HH:mm:ss";

        private const string NewLine = "\n";

        #region Path and folder names

        private const string ConfigurationFolder = "Configuration";
        private const string ResourcesFolder = "Resources";
        private const string AssetExtension = ".asset";

        private static readonly string LogConfigurationPath = Path.Combine(ConfigurationFolder, "LogConfiguration");

#if UNITY_EDITOR
        private static readonly string LogTypeCachePath = Path.Combine(ConfigurationFolder, "LogTypeCache");
        private static readonly string SavePathPrefix = FileUtil.MultiPathCombine("Assets", ResourcesFolder);
#endif

#endregion

        private static readonly HashSet<string> logTypes = new HashSet<string>();

        private static readonly LogConfiguration logConfiguration;

        private static readonly LogTypeCache logTypeCache;
        
        #region Constructor

        /// <summary>
        /// Static constructor, automatically called when log system is used.
        /// Loads the config.
        /// </summary>
        static GGLog()
        {
            logConfiguration = Resources.Load<LogConfiguration>(LogConfigurationPath);
#if UNITY_EDITOR
            bool refreshDatabase = false;

            // Create log type cache if it does not exist yet
            logTypeCache = Resources.Load<LogTypeCache>(LogTypeCachePath);
            if (logTypeCache == null)
            {
                logTypeCache = ScriptableObject.CreateInstance<LogTypeCache>();
                CreateFolders();
                AssetDatabase.CreateAsset(logTypeCache, Path.Combine(SavePathPrefix, LogTypeCachePath) + AssetExtension);
                refreshDatabase = true;
            }

            // Create log configuration if it does not exist yet
            if (logConfiguration == null)
            {
                logConfiguration = ScriptableObject.CreateInstance<LogConfiguration>();
                CreateFolders();
                AssetDatabase.CreateAsset(logConfiguration, Path.Combine(SavePathPrefix, LogConfigurationPath) + AssetExtension);
                refreshDatabase = true;
            }

            if (refreshDatabase)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
#endif
        }

        #endregion

        #region Log Types

        /// <summary>
        /// Adds a log type to the list of existing log types
        /// </summary>
        /// <param name="logType">The log type</param>
        public static void AddLogType(string logType)
        {
            if (logTypes.Contains(logType))
            {
                Debug.LogError("Tried to add log type '" + logType + "' multiple times");
                return;
            }
            logTypes.Add(logType);

#if UNITY_EDITOR
            if (logTypeCache != null)
            {
                foreach (string type in logTypeCache.LogTypes)
                {
                    if (type == logType)
                    {
                        return;
                    }
                }
                logTypeCache.AddLogType(logType);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        /// <summary>
        /// Array of log types
        /// </summary>
        public static string[] LogTypes
        {
            get
            {
                if (logTypeCache == null)
                {
                    return new string[0];
                }
                return logTypeCache.LogTypes;
            }
        }

        /// <summary>
        /// Clears the log type cache
        /// </summary>
        public static void ClearCache()
        {
            logTypeCache.Clear();
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        #endregion

        #region Basic Log Functions

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="logType">The log type</param>
        /// <param name="splunkLog">The splunk log (optional)</param>
        public static void Log(string message, string logType, SplunkLog splunkLog = null)
        {
            UnityLog(LogType.Log, message, logType);
            if (splunkLog != null)
            {
                splunkLog.Send(message);
            }
        }

        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="message">The warning message</param>
        /// <param name="logType">The log type</param>
        /// <param name="splunkLog">The splunk log (optional)</param>
        public static void LogWarning(string message, string logType, SplunkLog splunkLog = null)
        {
            UnityLog(LogType.Warning, message, logType);
            if (splunkLog != null)
            {
                splunkLog.Send(message);
            }
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="logType">The log type</param>
        /// <param name="splunkLog">The splunk log (optional)</param>
        public static void LogError(string message, string logType, SplunkLog splunkLog = null)
        {
            UnityLog(LogType.Error, message, logType);
            if (splunkLog != null)
            {
                splunkLog.Send(message);
            }
        }

        #endregion

        [Conditional("DEBUG")]
        private static void UnityLog(LogType unityLogType, string message, string logType)
        {
            if (!logTypes.Contains(logType))
            {
                Debug.LogError("Using undeclared log type '" + logType + "' - please declare this log type with GGLog.AddLogType");
            }

            if (unityLogType == LogType.Error || IsLogTypeEnabled(logType))
            {
                int hash = logType.GetHashCode();
                Color color = new Color(0.75f + Mathf.Sin(hash * 30) * 0.25f, 0.75f + Mathf.Cos(hash * 60) * 0.25f, 0.75f + Mathf.Tan(hash * 90) * 0.25f);
                string typePrefix = DateTime.Now.ToString(TimestampFormat) + " <color=" + color.ToHex() + ">[" + logType + "]</color> ";
                switch (unityLogType)
                {
                    case LogType.Log:
                        Debug.Log(typePrefix + message + NewLine);
                        break;
                    case LogType.Warning:
                        Debug.LogWarning(typePrefix + message + NewLine);
                        break;
                    case LogType.Error:
                        Debug.LogError(typePrefix + message + NewLine);
                        break;
                    case LogType.Assert:
                        break;
                    case LogType.Exception:
                        Debug.LogException(new Exception(message));
                        break;
                    default:
                        Debug.LogException(new Exception("Unknown log type!"));
                        break;
                }
            }
        }

        private static bool IsLogTypeEnabled(string logType)
        {
            // All log types are enabled by default if no configuration is present
            if (logConfiguration == null)
            {
                return true;
            }

            return logConfiguration[logType];
        }

        private static void CreateFolders()
        {
            // Create resources folder if not present yet
            string resourcesPath = Path.Combine(Application.dataPath, ResourcesFolder);
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
            }

            // Create configuration folder in resources folder if not present yet
            string configurationPath = Path.Combine(resourcesPath, ConfigurationFolder);
            if (!Directory.Exists(configurationPath))
            {
                Directory.CreateDirectory(configurationPath);
            }
        }
    }
}