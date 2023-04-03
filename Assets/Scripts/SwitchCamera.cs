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
    }

    private void OnEnable()
    {
        PlayerController.ToggleLevelCam += ToggleLevelCam;
    }

    private void OnDisable()
    {
        PlayerController.ToggleLevelCam -= ToggleLevelCam;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Physics.Raycast(_currentCameraSelected.transform.position, _currentCameraSelected.transform.forward,
                out var hit, 100.0f, layerMask))
        {
            if (layerMask == LayerMask.NameToLayer("Player")) return;
            hit.collider.gameObject.TryGetComponent(out _currentMeshRenderer); 
            _currentMeshRenderer.material = transparentGround;
            //Debug.Log(hit.collider.gameObject.name + "Collided With Ground");
        }
        else
        {
            if (!_currentMeshRenderer) return;
            _currentMeshRenderer.material = solidGround;
        }
    }

    public void Switch()
    {
        //Top View
        if (camera1.activeSelf)
        {
            camera2.SetActive(true);
            camera2.transform.LookAt(m_PlayerTransForm.transform.position);
            camera1.SetActive(false);
            PlayerController.SideView = false;
            SetCurrentCameraTransform(cam2);
            ChangePerspective?.Invoke(PlayerController.SideView);
        }
        //Side View
        else if (camera2.activeSelf)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
            PlayerController.SideView = true;
            SetCurrentCameraTransform(cam1);
            ChangePerspective?.Invoke(PlayerController.SideView);
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
}
