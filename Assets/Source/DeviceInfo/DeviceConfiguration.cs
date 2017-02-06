using System;
using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.CakeBox.DeviceInfo
{
    [Serializable]
    public class DeviceConfiguration : ScriptableObject
    {
#pragma warning disable 649
        [SerializeField]
        private int releaseFPS = 30;

        [SerializeField]
        private int debugFPS = 30;

        [SerializeField]
        private int almostStaticDialogFPS = 15;

        [SerializeField]
        private int staticDialogFPS = 5;

        [SerializeField]
        private float fallbackDPI = 220f;

        [SerializeField]
        private IntVector2 referenceCameraResolution = new IntVector2(480, 800);

        [SerializeField]
        private int referenceTextureHeight;

        [SerializeField]
        private int referenceGPUMemory = 384;

        [SerializeField]
        private float[] textureSizeVariations = { 0.5f, 1f };

        [SerializeField]
        private float graphicUpscaleRatio_iOS;

        [SerializeField]
        private float graphicUpscaleRatio;
#pragma warning restore 649

        public int ReleaseFPS
        {
            get
            {
                return releaseFPS;
            }
        }
        public int DebugFPS
        {
            get
            {
                return debugFPS;
            }
        }

        public int AlmostStaticDialogFPS
        {
            get
            {
                return almostStaticDialogFPS;
            }
        }

        public int StaticDialogFPS
        {
            get
            {
                return staticDialogFPS;
            }
        }

        public float FallbackDPI
        {
            get
            {
                return fallbackDPI;
            }
        }

        public IntVector2 ReferenceCameraResolution
        {
            get
            {
                return referenceCameraResolution;
            }
        }

        public int ReferenceTextureHeight
        {
            get
            {
                return referenceTextureHeight;
            }
        }

        public int ReferenceGPUMemory
        {
            get
            {
                return referenceGPUMemory;
            }
        }

        public float[] TextureSizeVariations
        {
            get
            {
                return textureSizeVariations;
            }
        }

        public float GraphicUpscaleRatio
        {
            get
            {
                return graphicUpscaleRatio;
            }
        }

        public float GraphicUpscaleRatio_iOS
        {
            get
            {
                return graphicUpscaleRatio_iOS;
            }
        }

    }
}