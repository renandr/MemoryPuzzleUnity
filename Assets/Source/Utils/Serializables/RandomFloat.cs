using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Calculate a random value between min and max and cached it.
    /// Used to setup a random value between min max in the editor.
    /// </summary>
    [System.Serializable]
    public class RandomFloat
    {
        [Tooltip("the min value for the random")]
        [SerializeField]
        private float min;
        [Tooltip("the max value for the random")]
        [SerializeField]
        private float max;

        private float value;

        public RandomFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
            SetNewRandom();
        }

        /// <summary>
        /// Gets the cached random value
        /// </summary>
        public float Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the min value for the random
        /// Set a new min value and update the random value
        /// </summary>
        public float Min
        {
            get { return min; }
            set
            {
                min = value;
                SetNewRandom();
            }
        }

        /// <summary>
        /// Gets the max value for the random
        /// Set a new max value and update the random value
        /// </summary>
        public float Max
        {
            get { return max; }
            set
            {
                max = value;
                SetNewRandom();
            }
        }

        /// <summary>
        /// Set a new random value between min and max
        /// </summary>
        public void SetNewRandom()
        {
            value = Random.Range(min, max);
        }
    }
}
