using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
    [SerializeField] private GameObject rebindMenu;
    #elif UNITY_ANDROID || UNITY_IOS
    [SerializeField] private GameObject rebindButton;
    private void Awake()
    {
        Destroy(rebindButton);
    }
#endif
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
        ShowNumberOfCoins(m_BestScoreText, coinsPanel, coinSprite);
        pauseMenu.SetActive(false);
    }

    public void PauseMenu()
    {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX 
        if (rebindMenu.activeSelf)
        {
            rebindMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
#else
        pauseMenu.SetActive(!pauseMenu.activeSelf);
#endif
        pauseButtonPanel.SetActive(!pauseButtonPanel.activeSelf);
        bestTime.SetText(PlayerPrefs.GetFloat(m_BestTimeText.val).ToString("##"));
        OptionsPause();
    }
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
    public void ShowRebinds(bool state)
    {
        rebindMenu.SetActive(state);
    }
#endif
    public void ShowOptions(bool state)
    {
        pauseMenu.SetActive(state);
    }


    public static void ShowNumberOfCoins(StringVal m_BestScoreText, Transform coinsPanel, Sprite coinSprite)
    {
        var numberOfCoins = PlayerPrefs.GetInt(m_BestScoreText.val);
        for (var i = 0; i < numberOfCoins; i++)
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
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
