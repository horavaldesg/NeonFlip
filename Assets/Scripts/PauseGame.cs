using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButtonPanel;
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private Transform coinsPanel;
    [SerializeField] private Sprite coinSprite;
    
    private StringVal m_BestScoreText;
    private StringVal m_BestTimeText;

    private void OnEnable()
    {
        PlayerController.ShowOptions += PauseMenu;
    }

    private void OnDisable()
    {
        PlayerController.ShowOptions -= PauseMenu;
    }

    private void Start()
    {
        m_BestScoreText = Resources.Load<StringVal>("ScriptableObjects/Paths/BestCoinsCollectedPath");
        m_BestTimeText = Resources.Load<StringVal>("ScriptableObjects/Paths/BestTimePath");
        PlayerPrefs.SetInt(m_BestScoreText.val, 5);
        ShowNumberOfCoins(m_BestScoreText,coinsPanel, coinSprite);
        pauseMenu.SetActive(false);
    }

    public void PauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        pauseButtonPanel.SetActive(!pauseButtonPanel.activeSelf);
        bestTime.SetText(PlayerPrefs.GetFloat(m_BestTimeText.val).ToString("##"));
        OptionsPause();
    }

    public static void ShowNumberOfCoins(StringVal m_BestScoreText, Transform coinsPanel, Sprite coinSprite)
    {
        var numberOfCoins = PlayerPrefs.GetInt(m_BestScoreText.val);
        for(var i = 0; i < numberOfCoins; i++)
        {
            var coinsCollected = new GameObject("coinsCollected");
            coinsCollected.transform.parent = coinsPanel.transform;

            coinsCollected.AddComponent<RectTransform>();
            coinsCollected.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            coinsCollected.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            coinsCollected.GetComponent<RectTransform>().localPosition = Vector3.zero;
            coinsCollected.AddComponent<CanvasRenderer>();
            coinsCollected.GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            coinsCollected.AddComponent<Image>();
            coinsCollected.GetComponent<Image>().sprite = coinSprite;
        }
    }
    
    
    public void OptionsPause()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
