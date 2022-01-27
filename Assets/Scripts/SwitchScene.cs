using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchScene : MonoBehaviour
{
    public string scene;
    public static string sceneLevel;
    int levelIndex = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.iOS.Device.hideHomeButton = true;
        UnityEngine.iOS.Device.deferSystemGesturesMode = UnityEngine.iOS.SystemGestureDeferMode.All;
    }

    // Update is called once per frame
    void Update()
    {
        sceneLevel = scene;
        //if(SceneManager.GetActiveScene().buildIndex > 5)
        //{
        //    levelIndex = 0;
        //}
        //sceneLevel =
           
    }
    public void ChangeScene()
    {
        if(SceneManager.GetActiveScene().name == "BetaText")
        {
            levelIndex = 0;
            PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            levelIndex++;
            PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex);

        }
        SceneManager.LoadScene(scene);
        Debug.Log(levelIndex);


    }

    public void LoadLevel()
    {
        int i = PlayerPrefs.GetInt("LevelIndex");
        if(i == 0)
        {
            i ++;
        }
        SceneManager.LoadScene(i);
        //Debug.Log(levelIndex);

    }
    public void ChangeLevel(string scene)
    {
        moveCharacter.currentObj = null;
        levelIndex++;
        PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(scene);
        //Debug.Log(levelIndex);
        
    }
    
}
