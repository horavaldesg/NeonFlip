using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectButton;
    [SerializeField] private Transform selectorParent;
    private List<string> m_LevelNames = new List<string>();

    private void Awake()
    {
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
    }
}
