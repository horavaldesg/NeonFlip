using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupCoin : MonoBehaviour
{
    private StringVal m_PickUpCoinsPath;

    private void Awake()
    {
        m_PickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/CoinsCollectedPath");
        m_PickUpCoinsPath.val = "PickUpCoins" + SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var currentPickedUp = PlayerPrefs.GetInt(m_PickUpCoinsPath.val);
        currentPickedUp++;
        
        Destroy(gameObject);
        PlayerPrefs.SetInt(m_PickUpCoinsPath.val, currentPickedUp);
    }
}

