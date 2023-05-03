using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinsCollected;
    [SerializeField] private Material[] rotationImages;
    [SerializeField] private  Image rotationImage;
    private StringVal m_PickUpCoinsPath;
    private StringVal m_BestPickUpCoinsPath;
    private StringVal m_CurrentTime;
    private StringVal m_BestCurrentTime;
    private float m_T;
    private int m_AmountOfCollectables;
    private List<GameObject> m_CoinCollectables;


    private void Awake()
    {
        m_CoinCollectables = GameObject.FindGameObjectsWithTag("PickupCoin").ToList();

        //Initialize Scriptable Objects
        m_PickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/Paths/CoinsCollectedPath");
        m_CurrentTime = Resources.Load<StringVal>("ScriptableObjects/Paths/CurrentTimePath");
        m_BestPickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/Paths/BestCoinsCollectedPath");
        m_BestCurrentTime = Resources.Load<StringVal>("ScriptableObjects/Paths/BestTimePath");

        //CurrentTimes
        m_PickUpCoinsPath.val = "PickUpCoins" + SceneManager.GetActiveScene().name;
        m_CurrentTime.val = "CurrentTime" + SceneManager.GetActiveScene().name;

        //Best Times
        m_BestPickUpCoinsPath.val = "BestPickUp" + SceneManager.GetActiveScene().name;
        m_BestCurrentTime.val = "BestTime" + SceneManager.GetActiveScene().name;

        //Set Player Prefs
        PlayerPrefs.SetInt(m_PickUpCoinsPath.val, 0);
        PlayerPrefs.SetFloat(m_CurrentTime.val, 0);
        PlayerPrefs.GetInt(m_BestPickUpCoinsPath.val, 0);

        PlayerPrefs.GetFloat(m_BestCurrentTime.val, 0);

        m_AmountOfCollectables = m_CoinCollectables.Count;
    }

    private void OnEnable()
    {
        SwitchCamera.ChangePerspective += TurnOnCoins;
        PlayerController.ChangeIndicator += ChangeMaterial;
    }

    private void OnDisable()
    {
        SwitchCamera.ChangePerspective -= TurnOnCoins;
        PlayerController.ChangeIndicator -= ChangeMaterial;
    }

   
    private void TurnOnCoins(bool state)
    {
        m_CoinCollectables = GameObject.FindGameObjectsWithTag("PickupCoin").ToList();
        foreach (var coin in m_CoinCollectables)
        {
            if (!coin) return;
            coin.TryGetComponent(out Collider collider);
            coin.TryGetComponent(out MeshRenderer meshRenderer);
            var currentColor = meshRenderer.material.color;
            currentColor.a = state ? 1 : 0.25f;
            meshRenderer.material.color = currentColor;
            collider.enabled = state;
        }
    }

    private void Update()
    {
        coinsCollected.SetText($"{PlayerPrefs.GetInt(m_PickUpCoinsPath.val)}/{m_AmountOfCollectables}");
        m_T += Time.deltaTime;
        PlayerPrefs.SetFloat(m_CurrentTime.val, m_T);
        timerText.SetText(PlayerPrefs.GetFloat(m_CurrentTime.val).ToString("##"));
    }

    private void ChangeMaterial(int index)
    {
        rotationImage.color = rotationImages[index].color;
    }
}
