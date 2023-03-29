using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class MainUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinsCollected;
    private StringVal m_PickUpCoinsPath;
    private float m_T;
    private int m_AmountOfCollectables;

    private void Awake()
    {
        m_PickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/CoinsCollectedPath");
        PlayerPrefs.SetInt(m_PickUpCoinsPath.val, 0);
    }

    private void Start()
    {
        m_AmountOfCollectables = GameObject.FindGameObjectsWithTag("PickupCoin").Length;
    }

    private void Update()
    {
        coinsCollected.SetText(PlayerPrefs.GetInt(m_PickUpCoinsPath.val).ToString() + "/" + m_AmountOfCollectables);
        m_T += Time.deltaTime;
        timerText.SetText(m_T.ToString("##"));
    }
}
