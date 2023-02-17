using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour
{
    string gameId = "4530066";
    bool testMode = true;
#if UNITY_IOS
 private void Awake()
    {
        gameId = "4530066";

    }
#elif UNITY_ANDROID
    private void Awake()
    {
        gameId = "4530067";
    }
#endif
    // Initialize the Ads service:
    void Start()
    {
        if (PlayerPrefs.GetInt("Ads") == 1)
        {


            Advertisement.Initialize(gameId, testMode);
            ShowAd();
        }
    }
    public void ShowAd()
    {
        if(PlayerPrefs.GetInt("Ads") == 1)
            Advertisement.Show();
        

    }
    
    // Show an ad:
}
