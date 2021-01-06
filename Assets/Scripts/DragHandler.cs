using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public bool isDrag { get; private set; }

    public void OnBeginDrag(PointerEventData data)
    {
        isDrag = true;
        TriggerBeginDrag();
    }

    public void OnEndDrag(PointerEventData data)
    {
        isDrag = false;
        TriggerEndDrag();
    }

    public event Action DragBegin;
    public void TriggerBeginDrag()
    {
        if(DragBegin != null)
        {
            DragBegin();
        }
    }

    public event Action DragEnd;
    public void TriggerEndDrag()
    {
        if(DragBegin != null)
        {
            DragEnd();
        }
    }
}
