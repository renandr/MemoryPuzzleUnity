using System;
using System.Text;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.CakeBox.DeviceInfo
{
    /// <summary>
    /// Class which holds device related info
    /// Attention: Needs to be initialized with proper <see cref="DeviceConfiguration"/> before accessing any values.
    /// 
    /// Main info categories:
    /// - product
    /// - CPU
    /// - Display
    /// - Used texture variation
    /// - GPU
    /// - OS / platform
    /// - Connection
    /// 
    /// Note: Link for old device related tracking values:
    /// https://docs.google.com/a/goodgamestudios.com/spreadsheet/ccc?key=0AlN2EPXCQVModDB1Z2FiTlF6dHVnWjduN1drSUtYa1E&usp=sharing_eid#gid=134
    /// </summary>
    public static class Device
    {
        #region Product Info

        /// <summary>
        /// Unique ID for the device
        /// </summary>
        public static string UniqueId { get; private set; }

        /// <summary>
        /// The model of the device
        /// For iOS the model might be misleading, it follows these conventions:
        /// http://support.hockeyapp.net/kb/client-integration-ios-mac-os-x/ios-device-types
        /// </summary>
        public static string ProductModel { get; private set; }

        /// <summary>
        /// The brand of the device
        /// </summary>
        public static string ProductBrand { get; private set; }

        /// <summary>
        /// The name of the device
        /// </summary>
        public static string ProductName { get; private set; }

        /// <summary>
        /// The manufacturer of the device
        /// </summary>
        public static string ProductManufacturer { get; private set; }

        /// <summary>
        /// Android Device ID (android.os.Build.DEVICE)
        /// https://jira.goodgamestudios.com/browse/LOHM-4101
        /// </summary>
        public static string AndroidDevice { get; private set; }

        /// <summary>
        /// The category of the device (iOS/Android/..., Tablet/Phone,...)
        /// </summary>
        public static DeviceCategory DeviceCategory { get; private set; }

        #endregion

        #region CPU

        /// <summary>
        /// The CPU architecture of the device
        /// </summary>
        public static string CPUArchitecture { get; private set; }

        /// <summary>
        /// The number of processors in the device
        /// </summary>
        public static int CPUProcessorCount { get; private set; }

        /// <summary>
        /// The total memory (RAM) of the device in bytes
        /// </summary>
        public static int CPUMemorySize { get; private set; }

        /// <summary>
        /// The total free memory (RAM) of the device in bytes
        /// </summary>
        public static int CPUFreeMemorySize { get; private set; }

        /// <summary>
        /// True if the device is using 64 bit, false otherwise
        /// </summary>
        public static bool Is64Bit { get; private set; }

        #endregion

        #region Display

        /// <summary>
        /// The resolution in pixels (width x height)
        /// </summary>
        public static IntVector2 Resolution { get; private set; }

        /// <summary>
        /// Some devices have a difference in the size of the pixels themselves, resulting in a different dpi between vertical and horizontal.
        /// So this vector contains the information for the exact physical pixels per inch of the screen in the X and Y dimension.
        /// </summary>
        public static Vector2 DPIVector { get; private set; }

        /// <summary>
        /// Dots per inch of the device screen
        /// </summary>
        public static float DPI { get; private set; }

        /// <summary>
        /// The physical size of the screen in centimeters
        /// </summary>
        public static Vector2 ScreenSizeCm { get; private set; }

        /// <summary>
        /// The physical screen diagonal in inches
        /// </summary>
        public static float ScreenDiagonalInches { get; private set; }

        /// <summary>
        /// A default dpi to be used for proportional calculations or just fallback when no dpi information is available
        /// </summary>
        public static float FallbackDPI { get; private set; }

        /// <summary>
        /// A default resolution to be used for proportional calculations or just fallback when no resolution information is available
        /// </summary>
        public static IntVector2 ReferenceCameraResolution { get; private set; }

        #endregion

        #region Texture Variations

        /// <summary>
        /// Possible texture size variations
        /// </summary>
        public static float[] TextureSizeVariations { get; private set; }

        /// <summary>
        /// Index of the used texture size  varaiation
        /// </summary>
        public static int TextureSizeVariationIndex { get; private set; }

        /// <summary>
        /// Texture size to be used
        /// </summary>
        public static float TextureSizeVariation
        {
            get
            {
                return TextureSizeVariations[TextureSizeVariationIndex];
            }
        }

        #endregion

        #region GPU

        /// <summary>
        /// Name of the device's GPU
        /// </summary>
        public static string GPUName { get; private set; }

        /// <summary>
        /// Vendor of the device's GPU
        /// </summary>
        public static string GPUVendor { get; private set; }

        /// <summary>
        /// Version of the device's GPU
        /// </summary>
        public static string GPUVersion { get; private set; }

        /// <summary>
        /// Size of the device's GPU memory
        /// </summary>
        public static int GPUMemorySize { get; private set; }

        /// <summary>
        /// Does the device's GPU support multi threaded rendering?
        /// </summary>
        public static bool IsMultiThreadedRendering { get; private set; }

        /// <summary>
        /// Shader level of the GPU, following the Unity standards.
        /// <see cref="http://docs.unity3d.com/ScriptReference/SystemInfo-graphicsShaderLevel.html" />
        /// </summary>
        public static int ShaderLevel { get; private set; }

        /// <summary>
        /// Count of supported render targets
        /// </summary>
        public static int SupportedRenderTargetCount { get; private set; }

        #endregion

        #region Operating System / Platform

        /// <summary>
        /// The operating system currently running on the device
        /// </summary>
        public static string OS { get; private set; }

        /// <summary>
        /// Version of the operating system running on the device
        /// </summary>
        public static string OSVersion { get; private set; }

        public static bool IsApple {
            get
            {
#if UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsAndroid
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        #endregion

        #region Connection

        /// <summary>
        /// Is the device using a wireless network (true) or is it offline/using a mobile carrier network (false)
        /// Unity's NetworkReachability only has the values not reachable, carrier, LAN
        /// We simply assume that LAN is always an indicator for a WIFI connection.
        /// Attention: This value may change at any time!
        /// </summary>
        public static bool IsUsingWifi
        {
            get
            {
                return (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork);
            }
        }

        #endregion

        public static new string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Device Info");

            stringBuilder.AppendFormat("UniqueId: {0}.", UniqueId);
            stringBuilder.AppendLine().AppendFormat("ProductModel: {0}.", ProductModel);
            stringBuilder.AppendLine().AppendFormat("ProductBrand: {0}.", ProductBrand);
            stringBuilder.AppendLine().AppendFormat("ProductName: {0}.", ProductName);
            stringBuilder.AppendLine().AppendFormat("ProductManufacturer: {0}.", ProductManufacturer);
            stringBuilder.AppendLine().AppendFormat("AndroidDevice: {0}.", AndroidDevice);
            stringBuilder.AppendLine().AppendFormat("DeviceCategory: {0}.", DeviceCategory);

            stringBuilder.AppendLine().AppendFormat("CPUArchitecture: {0}.", CPUArchitecture);
            stringBuilder.AppendLine().AppendFormat("CPUProcessorCount: {0}.", CPUProcessorCount);
            stringBuilder.AppendLine().AppendFormat("CPUMemorySize: {0}.", CPUMemorySize);
            stringBuilder.AppendLine().AppendFormat("CPUFreeMemorySize: {0}.", CPUFreeMemorySize);
            stringBuilder.AppendLine().AppendFormat("Is64Bit: {0}.", Is64Bit);

            stringBuilder.AppendLine().AppendFormat("Resolution: {0}x{1} Pixels", Resolution.x, Resolution.y);
            stringBuilder.AppendLine().AppendFormat("DPIVector: {0:F3}x{1:F3} PPI", DPIVector.x, DPIVector.y);
            stringBuilder.AppendLine().AppendFormat("DPI: {0:F3}", DPI);
            stringBuilder.AppendLine().AppendFormat("ScreenSizeCm (physical): {0:F2}x{1:F2} cm", ScreenSizeCm.x, ScreenSizeCm.y);
            stringBuilder.AppendLine().AppendFormat("ScreenDiagonalInches (physical): {0:F2}\", {1:F2} cm", ScreenDiagonalInches, ScreenDiagonalInches * ConversionConstants.InchesToCentimeters);
            stringBuilder.AppendLine().AppendFormat("FallbackDPI: {0:F3}", FallbackDPI);
            stringBuilder.AppendLine().AppendFormat("ReferenceCameraResolution: {0:F3}", ReferenceCameraResolution);

            stringBuilder.AppendLine().AppendFormat("Texture Size Variation: {0:F2}", TextureSizeVariation);

            stringBuilder.AppendLine().AppendFormat("GPUName: {0}.", GPUName);
            stringBuilder.AppendLine().AppendFormat("GPUVendor: {0}.", GPUVendor);
            stringBuilder.AppendLine().AppendFormat("GPUVersion: {0}.", GPUVersion);
            stringBuilder.AppendLine().AppendFormat("GPUMemorySize: {0}.", GPUMemorySize);
            stringBuilder.AppendLine().AppendFormat("IsGPUMultiThreaded: {0}.", IsMultiThreadedRendering);
            stringBuilder.AppendLine().AppendFormat("ShaderLevel: {0}.", ShaderLevel);
            stringBuilder.AppendLine().AppendFormat("SupportedRenderTargetCount: {0}.", SupportedRenderTargetCount);

            stringBuilder.AppendLine().AppendFormat("OS: {0}.", OS);
            stringBuilder.AppendLine().AppendFormat("OSVersion: {0}.", OSVersion);
            stringBuilder.AppendLine().AppendFormat("IsApple: {0}.", IsApple);
            stringBuilder.AppendLine().AppendFormat("IsAndroid: {0}.", IsAndroid);

            stringBuilder.AppendLine().AppendFormat("IsUsingWifi: {0}.", IsUsingWifi);

            return stringBuilder.ToString();
        }

        #region Initialization

        /// <summary>
        /// Initializes the device info based on the device configuration.
        /// Should be called before trying to access any values.
        /// </summary>
        public static void Initialize(DeviceConfiguration deviceConfiguration)
        {
            InitProductInfo();
            InitOSInfo();
            InitScreenInfo(deviceConfiguration);
            InitCPUInfo();
            InitGPUInfo();
            TextureSizeVariationIndex = TextureSizeVariationCalculator.CalculateIndex(deviceConfiguration);
        }

        private static void InitProductInfo()
        {
            UniqueId = SystemInfo.deviceUniqueIdentifier;

            ProductModel = ProductValueHelper.GetProductModel();
            ProductBrand = ProductValueHelper.GetProductBrand();
            ProductName = ProductValueHelper.GetProductName();
            ProductManufacturer = ProductValueHelper.GetProductManufacturer();

            AndroidDevice = ProductValueHelper.GetAndroidDevice();
        }

        private static void InitOSInfo()
        {
            OS = SystemInfo.operatingSystem;
            OSVersion = OSHelper.GetVersion();
        }

        private static void InitScreenInfo(DeviceConfiguration deviceConfiguration)
        {
            FallbackDPI = deviceConfiguration.FallbackDPI;
            ReferenceCameraResolution = deviceConfiguration.ReferenceCameraResolution;
            TextureSizeVariations = deviceConfiguration.TextureSizeVariations;

            // Create a display metrics instance and copy its values directly into the device class for easy access
            DisplayMetrics displayMetrics = new DisplayMetrics(FallbackDPI, ReferenceCameraResolution);
            Resolution = displayMetrics.Resolution;
            DPIVector = displayMetrics.DPIVector;
            DPI = displayMetrics.DPI;
            ScreenSizeCm = displayMetrics.ScreenSizeCm;
            ScreenDiagonalInches = displayMetrics.ScreenDiagonalInches;

            DeviceCategory = DeviceCategoryHelper.GetDeviceCategory(ScreenDiagonalInches);
        }

        private static void InitCPUInfo()
        {
            CPUArchitecture = SystemInfo.processorType;
            CPUProcessorCount = SystemInfo.processorCount;
            CPUMemorySize = SystemInfo.systemMemorySize;
            CPUFreeMemorySize = -1;

#if !UNITY_EDITOR
#if UNITY_ANDROID
            CPUMemorySize = Mathf.RoundToInt((MemoryInfo.minf.memtotal) / 1024.0f);
            CPUFreeMemorySize = Mathf.RoundToInt((MemoryInfo.minf.memfree) / 1024.0f);
#endif
        
#if UNITY_IOS
            CPUMemorySize = Mathf.RoundToInt((MemoryInfo.minf.memtotal) / (1024.0f*1024.0f));	
            CPUFreeMemorySize = Mathf.RoundToInt((MemoryInfo.minf.memfree) / (1024.0f*1024.0f));
#endif
#endif

            Is64Bit = (IntPtr.Size == 8);
        }

        /// <summary>
        /// Note: More values are available http://docs.unity3d.com/ScriptReference/SystemInfo.html
        /// </summary>
        private static void InitGPUInfo()
        {
            GPUName = SystemInfo.graphicsDeviceName;
            GPUVendor = SystemInfo.graphicsDeviceVendor;
            GPUVersion = SystemInfo.graphicsDeviceVersion;
            GPUMemorySize = SystemInfo.graphicsMemorySize;
            IsMultiThreadedRendering = SystemInfo.graphicsMultiThreaded;
            ShaderLevel = SystemInfo.graphicsShaderLevel;
            SupportedRenderTargetCount = SystemInfo.supportedRenderTargetCount;
        }

        #endregion
    }
}