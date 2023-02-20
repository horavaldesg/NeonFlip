using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchCameraToggle : MonoBehaviour
{
    Toggle cameraSwitch;
    // Start is called before the first frame update
    void Start()
    {
        cameraSwitch = GetComponent<Toggle>();
        cameraSwitch.isOn = true;
        cameraSwitch.onValueChanged.AddListener(delegate { SpaceCameraSwitch(cameraSwitch); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
   void SpaceCameraSwitch(Toggle cam)
    {
        SwitchCamera.CanUseSpace();
    }
}
