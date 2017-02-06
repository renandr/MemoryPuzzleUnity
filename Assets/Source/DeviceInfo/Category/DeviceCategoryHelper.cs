using UnityEngine;

namespace GGS.CakeBox.DeviceInfo
{
    /// <summary>
    /// Helper class to determine the device category
    /// </summary>
    public static class DeviceCategoryHelper
    {
        /// <summary>
        /// Minimum screen diagonal in inches required to categorize an Android device as a tablet.
        /// Devices with lower screen diagonal will be categorized as phone.
        /// </summary>
        private const float AndroidMinTabletScreenDiagonal = 7f;

        private const string IPadKeyword = "ipad";

        private const string IPodKeyword = "ipod";

        /// <summary>
        /// Detects and returns the device category (platform and tablet/phone).
        /// </summary>
        /// <param name="physicalScreenDiagonal">screen diagnoal of the device in inches</param>
        /// <returns>The device category</returns>
        public static DeviceCategory GetDeviceCategory(float physicalScreenDiagonal)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return DeviceCategory.Desktop;
#elif UNITY_IOS
            return GetIOSDeviceCategory();
#elif UNITY_ANDROID
            return GetAndroidDeviceCategory(physicalScreenDiagonal);
#else
            return DeviceCategory.Unknown;
#endif
        }

        /// <summary>
        /// Gets the device category for iOS devices, based on the model name
        /// </summary>
        /// <returns>The device category</returns>
        private static DeviceCategory GetIOSDeviceCategory()
        {
            string model = SystemInfo.deviceModel.ToLower();
            if (model.Contains(IPadKeyword))
            {
                return DeviceCategory.Ipad;
            }
            return model.Contains(IPodKeyword) ? DeviceCategory.Ipod : DeviceCategory.Iphone;
        }

        /// <summary>
        /// Gets the device category for Android devices, based on the screen diagonal
        /// </summary>
        /// <param name="physicalScreenDiagonal">device display diagonal in inches</param>
        /// <returns>The device category</returns>
        private static DeviceCategory GetAndroidDeviceCategory(float physicalScreenDiagonal)
        {
            return (physicalScreenDiagonal >= AndroidMinTabletScreenDiagonal) ? DeviceCategory.AndroidTablet : DeviceCategory.AndroidPhone;
        }

    }
}