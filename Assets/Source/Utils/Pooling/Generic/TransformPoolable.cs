using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Basic implementation for a poolable object which resets the transform of the game object when it is received from the pool
    /// - position is set to 0,0,0
    /// - rotation is set to 0,0,0 (Quaternion.identity)
    /// - scale is set to 1,1,1
    /// </summary>
    public class TransformPoolable : AGameObjectPoolable
    {
        [Header("TransformPoolable")]
        [SerializeField]
        private bool resetPositon = true;

        [SerializeField]
        private bool resetRotation = true;

        [SerializeField]
        private bool resetScale = true;

        public override void OnGetFromPool()
        {
            if (resetPositon)
            {
                transform.position = Vector3.zero;
            }

            if (resetRotation)
            {
                transform.rotation = Quaternion.identity;
            }

            if (resetScale)
            {
                transform.localScale = Vector3.one;
            }
        }
    }
}