using UnityEngine;
using UnityEngine.EventSystems;

namespace GGS.ScreenManagement
{
    public abstract class AUIElementsWrapper<T> : MonoBehaviour where T : UIBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private T[] fields;
#pragma warning restore 649

        public T this[int index]
        {
            get
            {
                return fields[index];
            }

            set
            {
                fields[index] = value;
            }
        }
    }
}