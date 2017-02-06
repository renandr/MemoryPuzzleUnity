using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// MonoBehaviour enforcing the singleton pattern on a game object
    /// The singleton will initialized on awake and not be destroyed on scene change
    /// </summary>
    /// <typeparam name="T">Type of the game object</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        protected virtual void Awake()
        {
            transform.parent = null;
            if (instance != null && instance != this as T)
            {
                //Destroy other instances
                Destroy(gameObject);
            }
            else
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        /// Sets name
        /// </summary>
        protected virtual void Start()
        {
            transform.SetNameInEditorOnly(typeof(T).Name);
        }

        /// <summary>
        /// Gets the instance
        /// </summary>
        public static T Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }
    }
}
