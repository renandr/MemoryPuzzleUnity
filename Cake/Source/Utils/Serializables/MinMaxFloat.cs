using System;
using UnityEngine;

namespace GGS.CakeBox.Utils
{
    [Serializable]
    public class MinMaxFloat
    {
        [Tooltip("the min value")]
        [SerializeField]
        private float min;
        [Tooltip("the max value")]
        [SerializeField]
        private float max;

        public MinMaxFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Min
        {
            get
            {
                return min;
            }
        }

        public float Max
        {
            get
            {
                return max;
            }
        }
    }
}
