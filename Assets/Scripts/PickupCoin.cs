using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupCoin : MonoBehaviour
{
    private StringVal m_PickUpCoinsPath;

    private bool m_PickedUp;
    
    private void Awake()
    {
        m_PickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/Paths/CoinsCollectedPath");
        m_PickedUp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_PickedUp) return;
        if (!other.CompareTag("Player")) return;
        Debug.Log("Collided");
        m_PickedUp = true;
        var currentPickedUp = PlayerPrefs.GetInt(m_PickUpCoinsPath.val);
        currentPickedUp++;
        
        Destroy(gameObject);
        PlayerPrefs.SetInt(m_PickUpCoinsPath.val, currentPickedUp);
    }
}

