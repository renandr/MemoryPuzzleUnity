using System.Collections.Generic;
using com.goodgamestudios.warlands.uiWidgets.UIHelpers;
using GGS.CakeBox.Logging;
using GGS.CakeBox.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>A draggable item.</summary>
    ///
    /// <remarks>Implements basic functionality of a UI item that you can drag.</remarks>
    [RequireComponent(typeof (RectTransform))]
    public class UIDraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UIDragSlot CurrentSlot { get; private set; }
        protected RectTransform draggingT; //RectTransform where you want to drag it. It will be the parent for whenever we are dragging.

        //Useful callbacks.
        //Dragging started.
        public event GenericEvent StartedDraggingEvent;
        //Dragging ended on top of an occupied slot.
        public event GenericEvent<UIDragSlot> DraggedIntoSlotEvent;
        //Draging ended on top of an empty slot.
        public event GenericEvent<UIDragSlot> DraggedIntoFreeSlotEvent;
        //Dragging ended on top of an unknown ui element.
        public event GenericEvent<GameObject> DraggedOntoAGameObjectEvent;

        private Camera UICamera;
        private List<Image> myImages;

        public void Start()
        {
            UICamera = transform.root.GetComponent<Canvas>().worldCamera;
            myImages = new List<Image>(GetComponentsInChildren<Image>());
            Image myImage = GetComponent<Image>();

            if (UICamera == null || myImage == null)
            {
                GGLog.LogError("[UIDraggableItem] Image or UICamera is null. Cannot function!", "UI");
                return;
            }

            myImages.Insert(0, myImage);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (UICamera == null || myImages == null || myImages.Count == 0)
            {
                return;
            }
            DeactivateRaycasting();
            transform.SetParent(draggingT, true);
            transform.localScale = Vector3.one;
            transform.SetAsLastSibling();
            StartedDraggingEvent.Fire();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (UICamera == null || myImages == null || myImages.Count == 0)
            {
                return;
            }

            var rect1 = transform.parent.GetComponent<RectTransform>();
            if (rect1 == null)
            {
                return;
            }

            Vector2 areaCursor;
            var pos1 = eventData.position;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1, UICamera, out areaCursor))
            {
                areaCursor = transform.localPosition;
            }
            transform.localPosition = areaCursor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (UICamera == null || myImages == null || myImages.Count == 0)
            {
                return;
            }
            GameObject objectDroppedOn = eventData.pointerCurrentRaycast.gameObject;

            if (objectDroppedOn != null)
            {
                UIDragSlot dragSlot = objectDroppedOn.GetComponent<UIDragSlot>();
                if (dragSlot != null)
                {
                    DraggedIntoFreeSlotEvent.Fire(dragSlot);
                    return;
                }

                UIDraggableItem draggableItem = objectDroppedOn.GetComponent<UIDraggableItem>();
                if (draggableItem != null)
                {
                    UIDragSlot newslot = draggableItem.CurrentSlot;
                    if (newslot != null)
                    {
                        DraggedIntoSlotEvent.Fire(newslot);
                    }
                    return;
                }
                DraggedOntoAGameObjectEvent.Fire(eventData.pointerCurrentRaycast.gameObject);
            }
            OccupySlot(CurrentSlot);
        }

        /// <summary>Deactivate raycasting.</summary>
        ///
        /// <remarks>So when we drop, we don't think we're dropping on ourselves.</remarks>
        protected void DeactivateRaycasting()
        {
            Image[] images = GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                images[i].raycastTarget = false;
            }
        }

        /// <summary>Activates the raycasting.</summary>
        ///
        /// <remarks>So it can once again be clicked.</remarks>
        protected void ActivateRaycasting()
        {
            Image[] images = GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                images[i].raycastTarget = true;
            }
        }

        /// <summary>Returns to slot it was dragged out of.</summary>
        public void ReturnToSlot()
        {
            OccupySlot(CurrentSlot);
        }

        /// <summary>Occupy slot. Will clear content if not empty</summary>
        /// 
        /// <param name="newSlot">The new slot.</param>
        public void OccupySlot(UIDragSlot newSlot)
        {
            transform.SetParent(newSlot.transform);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            if (CurrentSlot != null && CurrentSlot != newSlot && CurrentSlot.ObjectInSlot == this)
            {
                CurrentSlot.ClearContent();
            }
            CurrentSlot = newSlot;
            CurrentSlot.ObjectInSlot = gameObject;
            ActivateRaycasting();
        }
    }
}