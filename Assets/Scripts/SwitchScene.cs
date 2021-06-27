using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchScene : MonoBehaviour
{
    public string scene;
    public static string sceneLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sceneLevel = scene;
        //sceneLevel =
           
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(scene);
        
    }
    public static void ChangeLevel()
    {
        SceneManager.LoadScene(sceneLevel);
        
    }
}
