using System;
using UnityEngine;

namespace GGS.CakeBox.DeviceInfo
{
    /// <summary>
    /// Static helper class used to detect what OS the device is currently using
    /// </summary>
    public static class OSHelper
    {
        // Define the keyword which is used for splitting, depending on the OS
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        private const string SplitKeyword = "Service Pack";
        private static readonly int SplitOffset = 0;
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        private const string SplitKeyword = "Mac";
        private static readonly int SplitOffset = SplitKeyword.Length+1;
#else
		private const string SplitKeyword = "OS";
		private static readonly int SplitOffset = SplitKeyword.Length+1;
#endif
        private const char Separator = '/';

        /// <summary>
        /// Extracts the version number from the version string
        /// </summary>
        /// <returns>The version of the OS</returns>
        public static string GetVersion()
        {
			string rawOSString = SystemInfo.operatingSystem;

            int osPosition = rawOSString.IndexOf(SplitKeyword, StringComparison.Ordinal);
            if (osPosition > -1)
            {
                osPosition += SplitOffset;
#if !UNITY_EDITOR && UNITY_ANDROID
                string slashlessString = rawOSString.Substring(osPosition);
                int slashPosition = slashlessString.IndexOf(Separator);
                if (slashPosition > -1)
                {
                    return slashlessString.Substring(0, slashPosition - 1);
                }
#endif

                return rawOSString.Substring(osPosition);
            }
            Debug.LogWarning("Failed to extract OS/version from: " + rawOSString);
            return rawOSString;
        }

    }
}