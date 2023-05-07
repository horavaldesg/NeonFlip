using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FindCamera : MonoBehaviour
{
    GameObject UICamera;
    Canvas cameraCanvas;
    
    void Start()
    {
        UICamera = GameObject.FindGameObjectWithTag("UICamera");
        cameraCanvas = GetComponent<Canvas>();
        cameraCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        cameraCanvas.worldCamera = UICamera.GetComponent<Camera>();
    }
}
