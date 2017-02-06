using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Base component for all elements in a GenericPool.
    /// </summary>
    public abstract class AGameObjectPoolable : MonoBehaviour
    {
        /// <summary>
        /// Called after received from pool.
        /// </summary>
        public virtual void OnGetFromPool()
        {
        }

        /// <summary>
        /// Called before returning to pool.
        /// </summary>
        public virtual void OnReturnToPool()
        {
        }
    }
}