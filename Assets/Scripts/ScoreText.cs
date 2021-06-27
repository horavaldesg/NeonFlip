using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI text;
    string scoreText;
    public static float score;
    public float downScore;
    public bool up;
    public string gameOverScene;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RespawnInteractables.respawn)
        {
            score = 0;
        }
        if (up)
        {
            score += Time.deltaTime;
            scoreText = score.ToString("##");
            text.SetText(scoreText);
        }
        else
        {
            downScore -= Time.deltaTime;
            scoreText = downScore.ToString("Time Left: ## seconds");
            text.SetText(scoreText);
            if(downScore <= 0)
            {
                GameObject[] slider;
                string currentScene = SceneManager.GetActiveScene().name;
                if (currentScene == "Level1")
                {
                    slider = GameObject.FindGameObjectsWithTag("Audio");
                    foreach (var obj in slider)
                    {
                        Destroy(obj);
                    }
                }
                GameOver.previousScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(gameOverScene);
            }
        }

        
        
        
    }
}
