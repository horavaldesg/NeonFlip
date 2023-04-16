using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightManager : MonoBehaviour
{
    private PlayerController _playerController;
    private void Awake()
    {
        transform.parent.TryGetComponent(out _playerController);
    }

   
    private void OnTriggerExit(Collider other)
    {
        if(!PlayerController.SideView) return;
        if (other.gameObject.layer != 6) return;
        if(!PlayerController.CanDetectCollisions) return;
        /*var whereToRotate = Vector3.Dot(transform.up, Vector3.down) > 0 ? 90 : -90;
        
        if (Vector3.Dot(-transform.right, Vector3.down) > 0)
        {
            whereToRotate = -90;
        }
        */
        var whereToRotate = 90.0f;
        PlayerController.CanDetectCollisions = false;
        StartCoroutine(_playerController.WaitToRotate(whereToRotate));
    }
}
