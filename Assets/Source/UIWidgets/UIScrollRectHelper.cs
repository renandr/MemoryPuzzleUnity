using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollRectHelper : ScrollRect 
{
    public bool IsDragging { get; private set; }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        IsDragging = true;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        IsDragging = false;
        onValueChanged.Invoke(normalizedPosition);
    }
}
