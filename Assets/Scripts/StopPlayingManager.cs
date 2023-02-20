using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayingManager : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.stopPlaying = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
