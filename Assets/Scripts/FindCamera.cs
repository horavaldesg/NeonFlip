using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FindCamera : MonoBehaviour
{
    GameObject UICamera;
    Canvas cameraCanvas;
    // Start is called before the first frame update
    void Start()
    {
        UICamera = GameObject.FindGameObjectWithTag("UICamera");
        cameraCanvas = GetComponent<Canvas>();
        cameraCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        cameraCanvas.worldCamera = UICamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
