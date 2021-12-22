using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public static bool space;
    GameObject[] cameras;
    [SerializeField] bool spacePub;
    // Start is called before the first frame update
    void Start()
    {
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        camera1 = cameras[0];
        camera2 = cameras[1];
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
        if (camera1.activeSelf == true)
        {
            camera2.SetActive(true);
            camera1.SetActive(false);
            moveCharacter.topView = true;
            moveCharacter.sideView = false;
        }
        //Side View
        else if(camera2.activeSelf == true)
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
            moveCharacter.sideView = true;
            moveCharacter.topView = false;
        }
        
        
    }
    
    public static void CanUseSpace()
    {
        space = !space;
    }
}
