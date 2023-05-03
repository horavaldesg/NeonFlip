using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    private SwitchScene m_SwitchScene;
    [SerializeField] private GameObject levelEndedPanel;
    
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private TextMeshProUGUI currentTime;

    [SerializeField] private Transform currentCoinsPanel;

    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Transform coinsPanel;

    private StringVal m_PickUpCoinsPath;
    private StringVal m_CurrentTimePath;
    private StringVal m_BestPickUpCoinsPath;
    private StringVal m_BestTimePath;
    private int m_totalCoins;
    private void OnEnable()
    {
        m_totalCoins = GameObject.FindGameObjectsWithTag("PickupCoin").Length;
        TryGetComponent(out m_SwitchScene);
        PlayerController.LevelEnded += ShowPanel;
    }

    private void OnDisable()
    {
        PlayerController.LevelEnded -= ShowPanel;
    }

    private void ShowPanel()
    {
        levelEndedPanel.SetActive(true);
        LevelEnded();
    }

    private void LevelEnded()
    {
        Time.timeScale = 0;
        m_PickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/Paths/CoinsCollectedPath");
        m_CurrentTimePath = Resources.Load<StringVal>("ScriptableObjects/Paths/CurrentTimePath");
        m_BestPickUpCoinsPath = Resources.Load<StringVal>("ScriptableObjects/Paths/BestCoinsCollectedPath");
        m_BestTimePath = Resources.Load<StringVal>("ScriptableObjects/Paths/BestTimePath");


        var currentCoinsVal = PlayerPrefs.GetInt(m_PickUpCoinsPath.val);
        var currentTimeVal = PlayerPrefs.GetFloat(m_CurrentTimePath.val);
        
        var bestCoinsVal = PlayerPrefs.GetInt(m_BestPickUpCoinsPath.val);
        var bestTimeVal = PlayerPrefs.GetFloat(m_BestTimePath.val);

        if (currentCoinsVal >= bestCoinsVal)
        {
            PlayerPrefs.SetInt(m_BestPickUpCoinsPath.val, currentCoinsVal);
        }
        
        if (currentTimeVal < bestTimeVal)
        {
            PlayerPrefs.SetFloat(m_BestTimePath.val, currentTimeVal);
        }
        else if (bestTimeVal == 0)
        {
            PlayerPrefs.SetFloat(m_BestTimePath.val, currentTimeVal);
        }
        
        currentTime.SetText(currentTimeVal.ToString("##"));
        bestTime.SetText(bestTimeVal.ToString("##"));
        ShowTotalCoins(bestCoinsVal,  coinsPanel);
        ShowTotalCoins(currentCoinsVal,  currentCoinsPanel);
    }
    
    private void ShowTotalCoins(int coins, Transform parentTransform)
    { 
        List<Image> coinImages = new();

        for (var i = 0; i < m_totalCoins; i++)
        {
            var coinsCollected = new GameObject("coinsCollected");
            coinsCollected.transform.parent = parentTransform.transform;

            coinsCollected.AddComponent<RectTransform>();
            coinsCollected.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            coinsCollected.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            coinsCollected.GetComponent<RectTransform>().localPosition = Vector3.zero;
            coinsCollected.AddComponent<CanvasRenderer>();
            coinsCollected.GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            coinsCollected.AddComponent<Image>();
            coinsCollected.TryGetComponent(out Image coinImage);
            coinImage.sprite = coinSprite;
            var color = coinImage.color;
            color.a = 0.25f;
            coinImage.color = color;
            coinImages.Add(coinImage);
        }
        
        ShowNumberOfCoins(coins, coinImages);
    }
    
    private void ShowNumberOfCoins(int numberOfCoins, List<Image> coinImages)
    {
        for (var i = 0; i < numberOfCoins; i++)
        {
            var color = coinImages[i].color;
            color.a = 1;
            coinImages[i].color = color;
        }
    }
    
    public void SwitchScene()
    {
        m_SwitchScene.ChangeScene();
    }
}
