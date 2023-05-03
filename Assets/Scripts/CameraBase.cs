using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class CameraBase : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float xRotSpeed = 250f;
    [SerializeField] protected float yRotSpeed = 120f;
    [SerializeField] protected float zoomMin = 7;
    [SerializeField] protected float zoomMax = 120f;
    protected Camera m_ThisCamera;

    protected Vector3 m_Offset;
    protected float m_X;
    protected float m_Y;

    protected bool m_MouseDown;
    protected Vector2 m_CameraInput;
    protected Vector2 zoomVal;
    private float m_KbXSpeed;
    private float m_KbYSpeed;

    protected float m_ZoomInAmount;
    private string currentControlScheme;
    private const string Controller = "Gamepad";
    private const string KbM = "Keyboard&Mouse";
    
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("CameraTarget").transform;
        m_KbXSpeed = xRotSpeed;
        m_KbYSpeed = yRotSpeed;
        //setup offset and angles
        TryGetComponent(out m_ThisCamera);
        var position = target.position;
        var transform1 = transform;
        m_Offset = transform.position - target.position;
        Vector3 angles = transform.eulerAngles;
        m_X = angles.y;
        m_Y = angles.x;
        //set initial transform position and rotation
        Quaternion rotation = Quaternion.Euler(m_Y, m_X, 0);
        transform.position = position + rotation * m_Offset;
        transform.LookAt(position);
    }

    protected virtual void Start()
    {
        
        //if(PlayerController.Instance.controls.currentControlScheme == PlayerController.Instance.controls.)
       
    }

    protected virtual void OnEnable()
    {
        if(PlayerController.Instance != null)
        {
            currentControlScheme = PlayerController.Instance.controls.currentControlScheme;

            var controllerXSpeed = m_KbXSpeed / 2;
            var controllerYSpeed = m_KbYSpeed / 2;
            if (currentControlScheme == Controller)
            {
                m_ZoomInAmount = 0.75f;
                xRotSpeed = controllerXSpeed;
                yRotSpeed = controllerYSpeed;
            }
            else
            {
                m_MouseDown = false;
                m_ZoomInAmount = 2;
                xRotSpeed = m_KbXSpeed;
                yRotSpeed = m_KbYSpeed;
            }
        }
        
        PlayerController.MouseDown += MouseDown;
        PlayerController.MouseUp += MouseUp;
    }

    protected virtual void OnDisable()
    {
        PlayerController.MouseDown -= MouseDown;
        PlayerController.MouseUp -= MouseUp;
    }

    private void MouseDown()
    {
        m_MouseDown = true;
    }

    protected virtual void MouseUp()
    {
        m_MouseDown = false;
    }

    protected virtual void LateUpdate()
    {
        PlayerController.Instance.ActionMap.FindAction("Zoom").started += tgb => { zoomVal = tgb.ReadValue<Vector2>(); };
        PlayerController.Instance.ActionMap.FindAction("Zoom").canceled += tgb => { zoomVal =Vector2.zero; };
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        PlayerController.Instance.ActionMap.FindAction("Look").started += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
        PlayerController.Instance.ActionMap.FindAction("Look").canceled += tgb => { m_CameraInput = Vector2.zero; };
#else
            PlayerController.controls.Player.Look.performed += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
            PlayerController.controls.Player.Look.performed += tgb => { m_CameraInput = Vector2.zero; };
#endif
        if (currentControlScheme == Controller)
        {
            if (m_CameraInput.magnitude != 0)
            {
                m_MouseDown = true;
            }
        }
        //rotate around target with mouse button
        Quaternion rotation = Quaternion.Euler(m_Y, m_X, 0);
        transform.position = target.position + rotation * m_Offset;

        if (m_MouseDown)
        {
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
