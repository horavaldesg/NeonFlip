using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    private SwitchScene m_SwitchScene;
    [SerializeField] private GameObject levelEndedPanel;
    
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private TextMeshProUGUI currentTime;

    [SerializeField] private Transform bestCoinsPanel;
    [SerializeField] private Transform currentCoinsPanel;

    [SerializeField] private Sprite coinSprite;

    private StringVal m_PickUpCoinsPath;
    private StringVal m_CurrentTimePath;
    private StringVal m_BestPickUpCoinsPath;
    private StringVal m_BestTimePath;
    
    private void OnEnable()
    {
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

        if (currentCoinsVal > bestCoinsVal)
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
        
        PauseGame.ShowNumberOfCoins(m_BestPickUpCoinsPath, bestCoinsPanel.transform, coinSprite);
        PauseGame.ShowNumberOfCoins(m_PickUpCoinsPath, currentCoinsPanel.transform, coinSprite);

        bestTime.SetText(PlayerPrefs.GetFloat(m_BestTimePath.val).ToString("##"));
        currentTime.SetText(PlayerPrefs.GetFloat(m_CurrentTimePath.val).ToString("##"));
    }

    public void SwitchScene()
    {
        m_SwitchScene.ChangeScene();
    }
}
