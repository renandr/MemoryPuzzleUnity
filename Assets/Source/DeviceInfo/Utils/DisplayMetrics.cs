using System;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.CakeBox.DeviceInfo
{
    /// <summary>
    /// Determines and provides the display metrics of a device
    /// http://developer.android.com/reference/android/util/DisplayMetrics.html
    /// </summary>
    public class DisplayMetrics
    {
        private const int ValueUndefined = 0;

        /// <summary>
        /// The resolution in pixels (width x height)
        /// </summary>
        public IntVector2 Resolution { get; private set; }

        /// <summary>
        /// Some devices have a difference in the size of the pixels themselves, resulting in a different dpi between vertical and horizontal.
        /// So this vector contains the information for the exact physical pixels per inch of the screen in the X and Y dimension.
        /// </summary>
        public Vector2 DPIVector { get; private set; }

        /// <summary>
        /// Dots per inch of the device screen
        /// </summary>
        public float DPI { get; private set; }

        /// <summary>
        /// The physical size of the screen in centimeters
        /// </summary>
        public Vector2 ScreenSizeCm { get; private set; }

        /// <summary>
        /// The physical screen diagonal in inches
        /// </summary>
        public float ScreenDiagonalInches { get; private set; }

        /// <summary>
        /// Calculates the display metrics
        /// </summary>
        /// <param name="fallbackDPI">The fallback DPI to be used</param>
        /// <param name="referenceCameraResolution">The reference camera resolution to be used</param>
        public DisplayMetrics(float fallbackDPI, IntVector2 referenceCameraResolution)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
                SetAndroidDisplayMetrics();
#endif
            if (DPI <= ValueUndefined)
            {
                DPI = Screen.dpi > ValueUndefined ? Screen.dpi : fallbackDPI;
            }
            if (DPIVector == Vector2.zero)
            {
                DPIVector = new Vector2(DPI, DPI);
            }
            if (Resolution == IntVector2.zero)
            {
                Resolution = (Screen.width > ValueUndefined && Screen.height > ValueUndefined) ? new IntVector2(Screen.width, Screen.height) : referenceCameraResolution;
            }
            
            ScreenSizeCm = CalculatePhysicalScreenSize(Resolution, DPIVector);
            ScreenDiagonalInches = CalculatePhysicalScreenDiagonal(ScreenSizeCm);
        }

#if !UNITY_EDITOR && UNITY_ANDROID
        private void SetAndroidDisplayMetrics()
        {
            
            using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject metricsInstance = new AndroidJavaObject("android.util.DisplayMetrics"),
                                         activityInstance = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"),
                                         windowManagerInstance = activityInstance.Call<AndroidJavaObject>("getWindowManager"),
                                         displayInstance = windowManagerInstance.Call<AndroidJavaObject>("getDefaultDisplay"))
                {
                    displayInstance.Call("getRealMetrics", metricsInstance);

                    int screenWidth = metricsInstance.Get<int>("widthPixels");
                    int screenHeight = metricsInstance.Get<int>("heightPixels");
                    Resolution = new IntVector2(screenWidth, screenHeight);

                    float xdpi = metricsInstance.Get<float>("xdpi");
                    float ydpi = metricsInstance.Get<float>("ydpi");
                    DPIVector = new Vector2(xdpi, ydpi);
                }
            }
        }
#endif
        private static Vector2 CalculatePhysicalScreenSize(IntVector2 resolution, Vector2 dpi)
        {
            return new Vector2(
                resolution.x / dpi.x,
                resolution.y / dpi.y
            )* ConversionConstants.InchesToCentimeters;
        }

        private static float CalculatePhysicalScreenDiagonal(Vector2 physicalScreenSize)
        {
            double x = Math.Pow(physicalScreenSize.x * ConversionConstants.CentimetersToInches, 2);
            double y = Math.Pow(physicalScreenSize.y * ConversionConstants.CentimetersToInches, 2);
            return (float)Math.Sqrt(x + y);
        }

    }
}