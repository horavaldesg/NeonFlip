using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectButton;
    [SerializeField] private Transform selectorParent;
    [SerializeField] private EventSystem eventSystem;
    
    private List<string> m_LevelNames = new List<string>();

    private void Awake()
    {
        Time.timeScale = 1;
        for (var i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            m_LevelNames.Add(sceneName);
        }
    }

    public void ShowLevelSelector()
    {
        foreach (var t in m_LevelNames)
        {
            CreateLevelMenu(t);
        }
    }
    
    private void CreateLevelMenu(string levelName)
    {
        var levelButton = Instantiate(levelSelectButton, selectorParent);
        levelButton.transform.SetParent(selectorParent);
        levelButton.TryGetComponent(out LevelSelectorButton levelSelectorButtonComp);
        levelSelectorButtonComp.SetText(levelName);
        if (levelName == "NfTutorial")
        {
            eventSystem.firstSelectedGameObject = levelButton;
            eventSystem.SetSelectedGameObject(levelButton);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
