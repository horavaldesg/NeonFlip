using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour
{
    string gameId = "4530066";
    bool testMode = true;
    void Start()
    {
        if (PlayerPrefs.GetInt("Ads") == 1)
        {
            Advertisement.Initialize(gameId, testMode);

        }
    }
}


