using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    public string gameId = "4530066";
    public string placementId = "Banner_iOS";
    public bool testMode = true;

#if UNITY_IOS
 private void Awake()
    {
        gameId = "4530066";

        placementId = "Interstitial_iOS";

    }
#elif UNITY_ANDROID
    private void Awake()
    {
        gameId = "4530067";
        placementId = "Rewarded_Android";
        PlayerPrefs.SetInt("Ads", 1);
    }
#endif


    public void OnInitializationComplete()
    {
        if (PlayerPrefs.GetInt("Ads") == 1)
        {
            Advertisement.Initialize(gameId);
        }
        
        Advertisement.Show(placementId);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        throw new System.NotImplementedException();
    }
}
