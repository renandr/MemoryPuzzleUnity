

using UnityEngine;
using UnityEngine.EventSystems;

namespace com.goodgamestudios.warlands.uiWidgets.UIHelpers
{
    /// <summary>A drag slot ui element.</summary>
    ///
    /// <remarks>Slots into which DraggableItems fit into.</remarks>

    public class UIDragSlot : MonoBehaviour, IDropHandler
    {
#pragma warning disable 649
        [HideInInspector]
        public GameObject ObjectInSlot { get; set; }
#pragma warning restore 649

        public void Awake()
        {
            ObjectInSlot = null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            ObjectInSlot = eventData.pointerDrag.GetComponent<UIDraggableItem>()!=null? eventData.pointerDrag : null;
        }

        /// <summary>Clears content without destroying it.</summary>
        public void ClearContent()
        {
            ObjectInSlot = null;
        }

        /// <summary>Destroys the content.</summary>
        public void DestroyContent()
        {
            Destroy(ObjectInSlot);
            ObjectInSlot = null;
        }

        public void RepositionContent()
        {
            if (ObjectInSlot != null)
            {
                ObjectInSlot.transform.position = transform.position;
            }
        }

    }
}
