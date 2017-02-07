using System;
using UnityEditor;
using UnityEngine;

namespace GGS.CakeBox.Logging
{

    /// <summary>
    /// Custom inspector for the log configuration
    /// </summary>
    [CustomEditor(typeof(LogConfiguration))]
    public class LogConfigurationInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            LogConfiguration config = (LogConfiguration)target;

            DrawInfo();
            DrawTopButtons(config);
            DrawMessageTypeList(config);
            DrawBottomButtons(config);
        }

        private void DrawInfo()
        {
            GUILayout.TextArea(
                "Config to enable/disable log types." + Environment.NewLine +
                "Run the game if a newly added log type is missing." + Environment.NewLine +
                "Click 'Clear Cache' if old log types are still listed here.");
        }

        private void DrawTopButtons(LogConfiguration config)
        {
            Color color = GUI.color;

            GUI.color = Color.red;
            if (GUILayout.Button("Log Nothing"))
            {
                config.SetAll(false);
                EditorUtility.SetDirty(target);
            }

            GUI.color = Color.green;
            if (GUILayout.Button("Log Everything"))
            {
                config.SetAll(true);
                EditorUtility.SetDirty(target);
            }

            GUI.color = color;
        }

        private void DrawMessageTypeList(LogConfiguration config)
        {
            GUILayout.Space(10);

            string[] logTypes = GGLog.LogTypes;

            for (int i = 0; i < logTypes.Length; i++)
            {
                string logType = logTypes[i];

                bool isEnabled = config[logType];
                config[logType] = EditorGUILayout.Toggle(logType, isEnabled);
            }

            if (logTypes.Length == 0)
            {
                GUILayout.TextArea(
                    "No log types in cache." + Environment.NewLine +
                    "They have to be registered with:" + Environment.NewLine +
                    "GGLog.AddLogType" + Environment.NewLine +
                    "Run the game to refresh the cache!");
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawBottomButtons(LogConfiguration config)
        {
            Color color = GUI.color;

            GUILayout.Space(10);

            if (EditorApplication.isPlaying)
            {
                GUILayout.TextArea("[ Stop play mode for cache options! ]");
                return;
            }

            GUI.color = Color.white;
            if (GUILayout.Button("Refresh Cache (run the game)"))
            {
                EditorApplication.isPlaying = true;
            }

            GUI.color = Color.yellow;
            if (GUILayout.Button("Clear Cache"))
            {
                if (EditorUtility.DisplayDialog("Clear cache?",
                    "Do you want to clear the log type cache? You have to run the game again afterwards to rebuild it. You will also lose your current log config settings.",
                    "Okay, clear it!", "Oops! No, thanks!"))
                {
                    if (EditorApplication.isPlaying)
                    {
                        EditorApplication.isPlaying = false;
                    }

                    config.Clear();
                    GGLog.ClearCache();
                    EditorUtility.SetDirty(target);

                    if (EditorUtility.DisplayDialog("Cache cleared!", "The cache has been cleared! You have to run the game to see the log types in the config again!", "Okay, run the game!", "Do not run it now."))
                    {
                        EditorApplication.isPlaying = true;
                    }
                }
            }

            GUI.color = color;
        }
    }
}