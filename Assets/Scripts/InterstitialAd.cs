using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersitialAd : MonoBehaviour
{
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
  private string _adUnitId = "ca-app-pub-9567172455942164/9190708669";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/5575463023";
#else
    private string _adUnitId = "unused";
#endif

    GoogleMobileAds.Api.InterstitialAd _interstitialAd;

    private void Start()
    {
        MobileAds.Initialize(initStatus => { Debug.Log("Mobile Ads Initialized"); });
        RequestAd();
    }

    public void Show()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad not ready");
        }
    }

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void RequestAd()
    {
        Debug.Log("Creating banner view");

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        AdRequest request = new AdRequest();
        InterstitialAd.Load(_adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Cannot load interstitial ad: " + error);
                return;
            }

            _interstitialAd = ad;
            _interstitialAd.OnAdFullScreenContentClosed += () => RequestAd();
        });
    }

}
