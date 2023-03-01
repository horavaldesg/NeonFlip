﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
public class BannerAd : MonoBehaviour
{

    public string gameId = "4530066";
    public string placementId = "Banner_iOS";
    public bool testMode = true;

#if UNITY_IOS
 private void Awake()
    {
        gameId = "4530066";

        placementId = "Banner_iOS";

    }
#elif UNITY_ANDROID
    private void Awake()
    {
        gameId = "4530067";
        placementId = "Banner_Android";
    }
#endif

    void Start()
    {
        if (PlayerPrefs.GetInt("Ads") == 1)
        {
            // Initialize the SDK if you haven't already done so:
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

            StartCoroutine(ShowBannerWhenReady());
        }
    }
    
    void FixedUpdate()
    {
        if(PlayerPrefs.GetInt("Ads") == 1)
        {
            StartCoroutine(ShowBannerWhenReady());
        }
        
        else if(PlayerPrefs.GetInt("Ads") == 0)
        {
            Advertisement.Banner.Hide();
        }
    }

    IEnumerator ShowBannerWhenReady()
    {
        Advertisement.Banner.Load(placementId);
        while (!Advertisement.Banner.isLoaded)
            yield return null;

        Advertisement.Banner.Show(placementId);
    }

}