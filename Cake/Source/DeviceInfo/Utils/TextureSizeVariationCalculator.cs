using UnityEngine;

namespace GGS.CakeBox.DeviceInfo
{
    public static class TextureSizeVariationCalculator
    {
        public static int CalculateIndex(DeviceConfiguration deviceConfiguration)
        {
            //Get the nearest texture resolution
            float propResolution = (float)Device.Resolution.y / deviceConfiguration.ReferenceTextureHeight;
            float graphicUpscaleRatio = deviceConfiguration.GraphicUpscaleRatio_iOS;
            float[] textureSizeVariations = Device.TextureSizeVariations;

            //iOS devices don't need as much RAM, so there's no need for changing this value
            if (Device.IsAndroid)
            {
                graphicUpscaleRatio = deviceConfiguration.GraphicUpscaleRatio;
                float propMemory = (float)deviceConfiguration.ReferenceGPUMemory / Device.GPUMemorySize;
                if (propMemory > 1)
                {
                    propResolution /= propMemory;
                }
            }

            int i;
            for (i = 0; i < textureSizeVariations.Length; i++)
            {
                float currentSize = textureSizeVariations[i];
                float minRange = 0;
                if (i > 0)
                {
                    float previousSize = textureSizeVariations[i - 1];
                    minRange = previousSize + ((currentSize - previousSize) * graphicUpscaleRatio);
                }

                float maxRange = float.MaxValue;
                if (i < textureSizeVariations.Length - 1)
                {
                    float nextSize = textureSizeVariations[i + 1];
                    maxRange = currentSize + ((nextSize - currentSize) * graphicUpscaleRatio);
                }
                if (minRange <= propResolution && propResolution < maxRange)
                {
                    break;
                }
            }
            return Mathf.Clamp(i, 0, textureSizeVariations.Length - 1);
        } 

    }
}