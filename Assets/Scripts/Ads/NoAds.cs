using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NoAds : MonoBehaviour
{
    int showAds = 1;
    [SerializeField]Button hideAdsButton;
    [SerializeField]Button showAdsButton;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hideAdsButton != null && showAdsButton != null)
        {
            if(PlayerPrefs.GetInt("Ads") == 1)
            {
                var colorsShow = showAdsButton.colors;

                colorsShow.selectedColor = Color.green;
                colorsShow.normalColor = Color.green;
                showAdsButton.colors = colorsShow;

                var colorsHide = hideAdsButton.colors;

                colorsHide.normalColor = Color.red;
                colorsHide.selectedColor = Color.red;
                hideAdsButton.colors = colorsHide;


            }
            else if (PlayerPrefs.GetInt("Ads") == 0)
            {
                var colorsShow = showAdsButton.colors;

                colorsShow.normalColor = Color.red;
                colorsShow.selectedColor = Color.red;
                showAdsButton.colors = colorsShow;

                var colorsHide = hideAdsButton.colors;

                colorsHide.selectedColor = Color.green;
                colorsHide.normalColor = Color.green;
                hideAdsButton.colors = colorsHide;


            }
        }
    }
    public void HideAds(int adValue)
    {
        showAds = adValue;
        PlayerPrefs.SetInt("Ads", showAds);
    }
    
}
