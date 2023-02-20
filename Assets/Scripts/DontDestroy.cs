using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DontDestroy : MonoBehaviour
{
    public static bool desroyUI;
    public static string scene;
     void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        scene = SceneManager.GetActiveScene().name;
        Debug.Log(scene);
    }
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        scene = SceneManager.GetActiveScene().name;
        if(scene == "BetaText")
        {
            DestrouUI();
        }
       
        
    }
    public void DestrouUI()
    {
        Destroy(this.gameObject);
    }
}
