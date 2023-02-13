using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static bool gravityChange;
    private SwitchCamera _switchCamera;
    [SerializeField] private GameObject player;
    
    [SerializeField] private Quaternion cam2Rotation;
    [SerializeField] private Quaternion cam1Rotation;
    [SerializeField] private Vector3 cam2OffSet;
    [SerializeField] private Vector3 cam1OffSet;
    [SerializeField] private float dampSpeed;
    
    private Vector3 speed = Vector3.zero;

    private void Awake()
    {
        _switchCamera = GetComponent<SwitchCamera>();
    }

    private void Update()
    {
        if (gravityChange)
        {
            // cam2.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - player.transform.position.y + 1, player.transform.position.z);
            //   cam2.transform.rotation = Quaternion.Euler(-90, 0, 0);

        }
        else
        {
          //  cam2.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 12, player.transform.position.z);
          //  cam2.transform.rotation = cam2Rotation;
        }

        (_switchCamera.camera1.activeSelf ? (Action)Camera1 : Camera2)();


    }

    private void Camera1()
    {
        var playerPos = player.transform.position + cam2OffSet;
        _switchCamera.camera1.transform.localRotation = cam1Rotation;
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref speed, dampSpeed);
        
    } 
    private void Camera2()
    {
        var playerPos = player.transform.position + cam1OffSet;
        _switchCamera.camera2.transform.localRotation = cam2Rotation;
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref speed, dampSpeed);
    }
}
