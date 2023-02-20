using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    GameObject[] slider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Respawn()
    {
        RespawnInteractables.respawn = true;
    }
    public void ReloadScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Level1")
        {
            slider = GameObject.FindGameObjectsWithTag("Audio");
            foreach (var obj in slider)
            {
                Destroy(obj);
            }
        }

        moveCharacter.doubleJump = false;
        SwitchCamera.space = false;
        FollowPlayer.gravityChange = false;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);

    }
    public static void CountDownTimer()
    {
        GameObject[] slider;
        
        if (GameOver.previousScene == "Level1")
        {
            slider = GameObject.FindGameObjectsWithTag("Audio");
            foreach (var obj in slider)
            {
                Destroy(obj);
            }
        }

        moveCharacter.doubleJump = false;
        SwitchCamera.space = false;
        FollowPlayer.gravityChange = false;
        string sceneName = SceneManager.GetActiveScene().name;
        if (GameOver.previousScene != "Level1")
        {
            AudioManager.reloadScene = true;
        }
        SceneManager.LoadScene(sceneName);
        

    }

}
