using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public static event Action<bool> ChangePerspective;
    public GameObject camera1;
    public GameObject camera2;
    [SerializeField] private LayerMask layerMask;
    public static bool space;
    GameObject[] cameras;
    [SerializeField] bool spacePub;
    [SerializeField] private Material solidGround;
    [SerializeField] private Material transparentGround;
    private MeshRenderer _currentMeshRenderer;

    private Camera _currentCameraSelected;
    private Camera cam1;
    private Camera cam2;
    private Transform m_PlayerTransForm;
    
    private GameObject levelCamera;
    private static readonly int Mode = Shader.PropertyToID("_Mode");

    
    [SerializeField] private Transform target;
    [SerializeField] private float xRotSpeed = 250f;
    [SerializeField] private float yRotSpeed = 120f;

    private Vector3 offset;
    private float x;
    private float y;

    private bool m_MouseDown;

    private Vector2 m_CameraInput;

    private Quaternion m_InitialRotation;
    private Vector3 m_InitialPosition;
    
    // Start is called before the first frame update
    private void Start()
    {
        camera1 = GameObject.FindGameObjectWithTag("MainCamera");
        camera2 = GameObject.FindGameObjectWithTag("MainCamera2");
        levelCamera = GameObject.FindGameObjectWithTag("LevelCam");
        m_PlayerTransForm = GameObject.FindGameObjectWithTag("Player").transform;
        
        camera1.TryGetComponent(out cam1);
        camera2.TryGetComponent(out cam2);
        levelCamera.SetActive(false);

        Switch();
        spacePub = true;
        space = spacePub;
        
        //setup offset and angles
        offset = transform.position - target.position;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        //set initial transform position and rotation
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position);

        m_MouseDown = false;
        m_InitialRotation = transform.localRotation;
        m_InitialPosition = transform.localPosition;
        Debug.Log(m_InitialRotation);
        Debug.Log(m_InitialPosition);
    }

    private void OnEnable()
    {
        PlayerController.ToggleLevelCam += ToggleLevelCam;
        PlayerController.MouseDown += MouseDown;
        PlayerController.MouseUp += MouseUp;
        PlayerController.SwitchCamera += Switch;
        RotateCameraButton.ButtonDown += PointerHandler;
    }

    private void OnDisable()
    {
        PlayerController.ToggleLevelCam -= ToggleLevelCam;
        PlayerController.MouseDown -= MouseDown;
        PlayerController.MouseUp -= MouseUp;
        PlayerController.SwitchCamera -= Switch;
        RotateCameraButton.ButtonDown -= PointerHandler;
    }

    private void MouseDown()
    {
        m_MouseDown = true;
    }

    private void MouseUp()
    {
        m_MouseDown = false;
        transform.localRotation = m_InitialRotation;
        transform.localPosition = m_InitialPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Physics.Raycast(_currentCameraSelected.transform.position, _currentCameraSelected.transform.forward,
                out var hit, 100.0f, layerMask))
        {
            if (layerMask == LayerMask.NameToLayer("Player")) return;
            hit.collider.gameObject.TryGetComponent(out _currentMeshRenderer);
            _currentMeshRenderer.material.SetFloat(Mode, 2);
            transparentGround = _currentMeshRenderer.material;
            transparentGround.color = new Color(transparentGround.color.r, transparentGround.color.g,
                transparentGround.color.b, 0.5f);
            _currentMeshRenderer.material = transparentGround;
            //Debug.Log(hit.collider.gameObject.name + "Collided With Ground");
        }
        else
        {
            if (!_currentMeshRenderer) return;
            solidGround = _currentMeshRenderer.material;
            solidGround.color = new Color(solidGround.color.r, solidGround.color.g,
                solidGround.color.b, 1);
            _currentMeshRenderer.material = solidGround;
        }
    }

    public void Switch()
    {
        //Top View
        if (camera1.activeSelf)
        {
            camera2.SetActive(true);
            camera1.SetActive(false);
            PlayerController.SideView = false;
            SetCurrentCameraTransform(cam2);
            ChangePerspective?.Invoke(!PlayerController.SideView);
        }
        //Side View
        else if (camera2.activeSelf)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
            PlayerController.SideView = true;
            SetCurrentCameraTransform(cam1);
            ChangePerspective?.Invoke(!PlayerController.SideView);
        }
    }

    private void ToggleLevelCam()
    {
        levelCamera.SetActive(!levelCamera.activeSelf);
        ToggleCameras(!levelCamera.activeSelf);
    }

    private void ToggleCameras(bool state)
    {
        camera1.SetActive(state);
        camera2.SetActive(state);
        if (!levelCamera.activeSelf) Switch();
    }

    public static void CanUseSpace()
    {
        space = !space;
    }

    private void SetCurrentCameraTransform(Camera cameraTransform)
    {
        _currentCameraSelected = cameraTransform;
    }
    
    private void LateUpdate()
    {
        if(!PlayerController.SideView) return;
        //rotate around target with mouse button
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        if (m_MouseDown)
        {
            #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            PlayerController.ActionMap.FindAction("Look").performed += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
            #else
            PlayerController.controls.Player.Look.performed += tgb => { m_CameraInput = tgb.ReadValue<Vector2>(); };
            #endif
            
            x += m_CameraInput.x * xRotSpeed * 0.02f;
            y -= m_CameraInput.y * yRotSpeed * 0.02f;
            y = ClampAngle(y, -45, 45);
            x = ClampAngle(x, -90, 90);
            rotation = Quaternion.Euler(y, x, 0);
            transform.position = target.position + rotation * offset;
            transform.LookAt(target.position);
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private void PointerHandler(bool state)
    {
        m_MouseDown = state;
        if (m_MouseDown) return;
        transform.localRotation = m_InitialRotation;
        transform.localPosition = m_InitialPosition;
    }
}
