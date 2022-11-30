using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragImage : Image, IPointerDownHandler, IPointerUpHandler
{
    public Action OnPointDownEvent;
    public Action OnPointUpEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointUpEvent?.Invoke();
    }
}
