using UnityEngine;

namespace GGS.CakeBox.DeviceInfo
{
    /// <summary>
    /// Static class used to get the product information about the device
    /// </summary>
    public static class ProductValueHelper
    {
        /// <summary>
        /// Brand and manufacturer are always "Apple" on Apple devices
        /// </summary>
        private const string AppleBrand = "Apple";
        private const string AppleManufacturer = "Apple";

        /// <summary>
        /// There is no android device id on Apple devices - we just send an empty string for them
        /// </summary>
        private const string AppleAndroidDevice = "";

        /// <summary>
        /// Gets the model of the product
        /// </summary>
        /// <returns>The product model</returns>
        public static string GetProductModel()
        {
#if !UNITY_EDITOR && UNITY_IOS
            return SystemInfo.deviceModel;
#elif !UNITY_EDITOR && UNITY_ANDROID
            AndroidJavaClass androidBuildClass = new AndroidJavaClass("android.os.Build");
            return androidBuildClass.GetStatic<string>("MODEL");
#else
            return SystemInfo.deviceModel;
#endif
        }

        /// <summary>
        /// Gets the brand of the product
        /// </summary>
        /// <returns>The product brand</returns>
        public static string GetProductBrand()
        {
#if !UNITY_EDITOR && UNITY_IOS
            return AppleBrand;
#elif !UNITY_EDITOR && UNITY_ANDROID
            AndroidJavaClass androidBuildClass = new AndroidJavaClass("android.os.Build");
            return androidBuildClass.GetStatic<string>("BRAND");
#else
            return SystemInfo.deviceModel;
#endif
        }

        /// <summary>
        /// Gets the name of the product
        /// </summary>
        /// <returns>The product name</returns>
        public static string GetProductName()
        {
#if !UNITY_EDITOR && UNITY_IOS
            return SystemInfo.deviceModel;
#elif !UNITY_EDITOR && UNITY_ANDROID
            AndroidJavaClass androidBuildClass = new AndroidJavaClass("android.os.Build");
            return androidBuildClass.GetStatic<string>("PRODUCT");
#else
            return SystemInfo.deviceModel;
#endif
        }

        /// <summary>
        /// Gets the manufacturer of the product
        /// </summary>
        /// <returns>The product manufacturer</returns>
        public static string GetProductManufacturer()
        {
#if !UNITY_EDITOR && UNITY_IOS
            return AppleManufacturer;
#elif !UNITY_EDITOR && UNITY_ANDROID
            AndroidJavaClass androidBuildClass = new AndroidJavaClass("android.os.Build");
            return androidBuildClass.GetStatic<string>("MANUFACTURER");
#else
            return SystemInfo.deviceModel;
#endif
        }

        /// <summary>
        /// Gets the Android device value
        /// </summary>
        /// <returns>The Android device value</returns>
        public static string GetAndroidDevice()
        {
#if !UNITY_EDITOR && UNITY_IOS
            return AppleAndroidDevice;
#elif !UNITY_EDITOR && UNITY_ANDROID
            AndroidJavaClass androidBuildClass = new AndroidJavaClass("android.os.Build");
            return androidBuildClass.GetStatic<string>("DEVICE");
#else
            return "Unity Editor";
#endif
        }

    }
}