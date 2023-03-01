using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public static event Action<bool> ChangePerspective;
    public GameObject camera1;
    public GameObject camera2;
    public static bool space;
    GameObject[] cameras;
    [SerializeField] bool spacePub;
    // Start is called before the first frame update
    void Start()
    {
        
        camera1 = GameObject.FindGameObjectWithTag("MainCamera");
        camera2 = GameObject.FindGameObjectWithTag("MainCamera2");
        camera2.SetActive(false);
        spacePub = true;
        space = spacePub;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && space)
        //{
        //    Switch();
        //}
    }

    public void Switch()
    {
        //Top View
        if (camera1.activeSelf)
        {
            camera2.SetActive(true);
            camera1.SetActive(false);
            PlayerController.SideView = false;
            ChangePerspective.Invoke(PlayerController.SideView);
        }
        //Side View
        else if(camera2.activeSelf)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
            PlayerController.SideView = true;
            ChangePerspective.Invoke(PlayerController.SideView);
        }


    }
    
    public static void CanUseSpace()
    {
        space = !space;
    }
}
