using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public bool isDrag { get; private set; }

    public void OnBeginDrag(PointerEventData data)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData data)
    {
        isDrag = false;
    }
}
