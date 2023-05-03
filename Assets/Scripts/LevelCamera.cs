using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class LevelCamera : CameraBase
{
    [SerializeField] private Camera zoomCamera;

    protected override void LateUpdate()
    {
        if(zoomVal.magnitude != 0)
        {
            var camFov = zoomCamera.fieldOfView + -zoomVal.y * m_ZoomInAmount;
            var clampedVal = Mathf.Clamp(camFov, zoomMin, zoomMax);
            zoomCamera.fieldOfView = clampedVal;
            m_ThisCamera.fieldOfView = clampedVal;
        }
        
        base.LateUpdate();
    }
}
