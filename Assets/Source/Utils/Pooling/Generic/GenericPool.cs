using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// A generic pool component. You can setup the inital pool size, check for already spawned child etc.
    /// Pooling prevents destroying and creating GameObjects, is just hides them and reuse them.
    /// Most fields are public to allow codewise spawning and configuration.
    /// </summary>
    /// <typeparam name="T">Type of script you expect to spawn.</typeparam>
    public abstract class GenericPool<T> : MonoBehaviour where T : AGameObjectPoolable
    {
        #region unity editor

        [Header("Pool")]
        [Tooltip("Should the pool spawns objects the maximal amount on start?")]
        public bool prewarm;

        [Tooltip("The maximal pool size. Below 0 means no limit")]
        public int maxAmount = -1;

        [Tooltip("Should returned elements be deactivated?")]
        public bool deactivatePooledElements = true;

        [Tooltip("The prefab that should be spawned")]
        public T prefab;

        [Tooltip("A custom parent each spawned element should have, if null this is used")]
        public Transform elementParent;

        #endregion

        private bool prefabHasRectTransform;

        protected Stack<T> pool;

        public bool HasBeenIntialized { get; private set; }

        protected virtual void Start()
        {
            if (!HasBeenIntialized)
            {
                Init();
            }
        }

        /// <summary>
        /// Initialises the pool with the setup values.
        /// </summary>
        public virtual void Init()
        {
            if (HasBeenIntialized)
            {
                Debug.LogWarning("Pool " + gameObject.name + " has already been initialised.");
            }

            pool = maxAmount > 0 ? new Stack<T>(maxAmount) : new Stack<T>();

            PrefabCheck();

            // Check if the prefab has a RectTransform
            prefabHasRectTransform = prefab.GetComponent<RectTransform>() != null;

            // Set the parent if none has been defined
            if (elementParent == null)
            {
                elementParent = transform;
            }

            // Disable the prefab
            if (deactivatePooledElements)
            {
                prefab.gameObject.SetActive(false);
            }

            // Check if there are already elements / spawn if prewarm is enabled
            if (maxAmount > 0)
            {
                CheckForAlreadySpawned();

                if (prewarm)
                {
                    DoPrewarm();
                }
            }

            HasBeenIntialized = true;
        }

        /// <summary>
        /// Checks if there are already elements spawned and if so adds them to the pool.
        /// </summary>
        private void CheckForAlreadySpawned()
        {
            T[] childs = elementParent.GetComponentsInChildren<T>(true);
            for (int i = 0; i < childs.Length; i++)
            {
                T child = childs[i];
                if (deactivatePooledElements)
                {
                    child.gameObject.SetActive(false);
                }
                pool.Push(child);
            }
        }

        /// <summary>
        /// Creates elements until the pool is full
        /// </summary>
        private void DoPrewarm()
        {
            for (int i = pool.Count; i < maxAmount; i++)
            {
                T spawn = SpawnElement();
                if (deactivatePooledElements)
                {
                    spawn.gameObject.SetActive(false);
                }
                pool.Push(spawn);
            }
        }

        /// <summary>
        /// Spawns a new instance of an element
        /// </summary>
        /// <returns>The new instance</returns>
        protected virtual T SpawnElement()
        {
            return Instantiate(prefab, elementParent, !prefabHasRectTransform);
        }

        /// <summary>
        /// Gets an element from the pool.
        /// Enables the game object if <see cref="deactivatePooledElements"/> is true.
        /// Calls <see cref="AGameObjectPoolable.OnGetFromPool"/> on the object.
        /// </summary>
        public virtual T GetElementFromPool()
        {
            T result = pool.Count > 0 ? pool.Pop() : SpawnElement();

            if (deactivatePooledElements)
            {
                result.gameObject.SetActive(true);
            }

            result.OnGetFromPool();

            return result;
        }

        /// <summary>
        /// Returns an element to the pool or destroys it if there is not enough space in the pool.
        /// Calls <see cref="AGameObjectPoolable.OnReturnToPool"/> on the object (unless it's destroyed).
        /// Disables the game object if <see cref="deactivatePooledElements"/> is true (unless it's desroyed).
        /// </summary>
        /// <param name="element">Element to return</param>
        public virtual void ReturnToPool(T element)
        {
            if (maxAmount < 0 || pool.Count < maxAmount)
            {
                // Store disabled object in pool
                element.OnReturnToPool();
                pool.Push(element);
                if (deactivatePooledElements)
                {
                    element.gameObject.SetActive(false);
                }
            }
            else
            {
                // If pool size limited is exceeded, just destroy the game obejct
                Destroy(element.gameObject);
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void PrefabCheck()
        {
            if (prefab == null)
            {
                Debug.LogError("Pool " + gameObject.name + " has no prefab setup. Prepare for NullPointerExceptions!");
            }
        }
    }
}