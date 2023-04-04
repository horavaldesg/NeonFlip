using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string scene;
    int levelIndex = 0;

    private void Start()
    {
        //UnityEngine.iOS.Device.hideHomeButton = true;
        //UnityEngine.iOS.Device.deferSystemGesturesMode = UnityEngine.iOS.SystemGestureDeferMode.All;
        if (SceneManager.GetActiveScene().name == "BetaText")
        {
            levelIndex = 0;
            PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    public void ChangeScene()
    {
        levelIndex++;
        PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(scene);
        Debug.Log(levelIndex);
    }

    public void LoadLevel()
    {
        var i = PlayerPrefs.GetInt("LevelIndex");
        if (i == 0)
        {
            i++;
        }
        
        SceneManager.LoadScene(i);
        //Debug.Log(levelIndex);
    }

    public void ChangeLevel()
    {
        moveCharacter.currentObj = null;
        levelIndex++;
        PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(scene);
        //Debug.Log(levelIndex);
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
