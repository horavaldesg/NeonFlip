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
    private Camera m_ThisCamera;
    
    private Vector3 m_Offset;
    private float m_X;
    private float m_Y;

    private bool m_MouseDown;
    private Vector2 m_CameraInput;
    private Vector2 zoomVal;
    
    private void Awake()
    {
        //setup offset and angles
        TryGetComponent(out m_ThisCamera);
        var position = target.position;
        var transform1 = transform;
        m_Offset = transform.position - target.position;
        Vector3 angles = transform.eulerAngles;
        m_X = angles.y;
        m_Y = angles.x;
        m_MouseDown = false;
        //set initial transform position and rotation
        Quaternion rotation = Quaternion.Euler(m_Y, m_X, 0);
        transform.position = position + rotation * m_Offset;
        transform.LookAt(position);
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
            m_ThisCamera.fieldOfView = clampedVal;
        }
        
        //rotate around target with mouse button
        Quaternion rotation = Quaternion.Euler(m_Y, m_X, 0);
        transform.position = target.position + rotation * m_Offset;

        if (m_MouseDown)
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            PlayerController.ActionMap.FindAction("Look").started += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
            PlayerController.ActionMap.FindAction("Look").canceled += tgb => { m_CameraInput = Vector2.zero; };
#else
            PlayerController.controls.Player.Look.performed += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
            PlayerController.controls.Player.Look.performed += tgb => { m_CameraInput = Vector2.zero; };
#endif
            
            m_X += m_CameraInput.x * xRotSpeed * 0.02f;
            m_Y -= m_CameraInput.y * yRotSpeed * 0.02f;
            //   y = ClampAngle(y, -45, 45);
            // x = ClampAngle(x, -90, 90);
            rotation = Quaternion.Euler(m_Y, m_X, 0);
            transform.position = target.position + rotation * m_Offset;
            transform.LookAt(target.position);
        }
    }
}
