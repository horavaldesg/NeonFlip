using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCameraButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public static event Action<bool> ButtonDown; 
    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonDown?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonDown?.Invoke(false);
    }
}
