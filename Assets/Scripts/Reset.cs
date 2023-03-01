using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerController playerController);
            playerController.RespawnPlayer();
           // moveCharacter.resetPlayer = true;
        }
       
        //Debug.Log("Reset");
    }
}
