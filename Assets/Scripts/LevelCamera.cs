using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    [SerializeField] private Camera zoomCamera;
    
    [SerializeField] private Transform target;
    [SerializeField] private float xRotSpeed = 250f;
    [SerializeField] private float yRotSpeed = 120f;
    [SerializeField] private float zoomMin = 7;
    [SerializeField] private float zoomMax = 120f;
    private Vector3 offset;
    private float x;
    private float y;

    private bool m_MouseDown;
    private Vector2 m_CameraInput;
    private Vector2 zoomVal;
    
    private void Awake()
    {
        //setup offset and angles
        offset = transform.position - target.position;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        m_MouseDown = false;
        
    }

    private void Start()
    {
        PlayerController.ActionMap.FindAction("Zoom").started += tgb => { zoomVal = tgb.ReadValue<Vector2>(); };
        PlayerController.ActionMap.FindAction("Zoom").canceled += tgb => { zoomVal =Vector2.zero; };
    }

    private void OnEnable()
    {
        PlayerController.MouseDown += MouseDown;
        PlayerController.MouseUp += MouseUp;
    }

    private void OnDisable()
    {
        PlayerController.MouseDown -= MouseDown;
        PlayerController.MouseUp -= MouseUp;
    }

    private void MouseDown()
    {
        m_MouseDown = true;

    }

    private void MouseUp()
    {
        m_MouseDown = false;
    }

    private void LateUpdate()
    {
        
        
        
        if(zoomVal.magnitude != 0)
        {
            var camFov = zoomCamera.fieldOfView + -zoomVal.y * 2;
            var clampedVal = Mathf.Clamp(camFov, zoomMin, zoomMax);
            zoomCamera.fieldOfView = clampedVal;
        }
        
        //rotate around target with mouse button
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.position = target.position + rotation * offset;

        if (m_MouseDown)
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            PlayerController.ActionMap.FindAction("Look").started += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
            PlayerController.ActionMap.FindAction("Look").canceled += tgb => { m_CameraInput = Vector2.zero; };
#else
            PlayerController.controls.Player.Look.performed += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
            PlayerController.controls.Player.Look.performed += tgb => { m_CameraInput = Vector2.zero; };
#endif
            
            x += m_CameraInput.x * xRotSpeed * 0.02f;
            y -= m_CameraInput.y * yRotSpeed * 0.02f;
            //   y = ClampAngle(y, -45, 45);
            // x = ClampAngle(x, -90, 90);
            rotation = Quaternion.Euler(y, x, 0);
            transform.position = target.position + rotation * offset;
            transform.LookAt(target.position);
        }
    }
}
