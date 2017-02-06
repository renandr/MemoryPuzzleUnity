using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>   
    /// Representation of 3D vector and points
    /// Includes random values
    /// Used to setup a random value between min max in the editor for a vector3
    /// </summary>
    [System.Serializable]
    public class RandomVector3
    {
        [Tooltip("a random value for the x axis")]
        [SerializeField]
        private RandomFloat x;
        [Tooltip("a random value for the y axis")]
        [SerializeField]
        private RandomFloat y;
        [Tooltip("a random value for the z axis")]
        [SerializeField]
        private RandomFloat z;

        public RandomVector3(RandomFloat x, RandomFloat y,RandomFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            SetNewRandom();
        }

        /// <summary>
        /// Gets the x coordinate with a predefined random value.
        /// </summary>
        public RandomFloat X
        {
            get { return x; }
        }
        /// <summary>
        /// Gets the y coordinate with a predefined random value.
        /// </summary>
        public RandomFloat Y
        {
            get { return y; }
        }

        /// <summary>
        /// Gets the z coordinate with a predefined random value.
        /// </summary>
        public RandomFloat Z
        {
            get { return z; }
        }

        /// <summary>
        /// Set a new random value for x,y,z
        /// </summary>
        public void SetNewRandom()
        {
            x.SetNewRandom();
            y.SetNewRandom();
            z.SetNewRandom();
        }
    }
}
