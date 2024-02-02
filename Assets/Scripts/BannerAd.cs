using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAd : MonoBehaviour
{
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
  private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private string _adUnitId = "unused";
#endif

    BannerView _bottomBanner;
    BannerView _topBanner;

    private void Start()
    {
        MobileAds.Initialize(initStatus => { Debug.Log("Mobile Ads Initialized"); });
        //RequestBanner();
    }

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void RequestBanner()
    {
        Debug.Log("Creating banner view");

        if (_bottomBanner != null)
        {
            _bottomBanner.Destroy();
        }
        _bottomBanner = new BannerView(_adUnitId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom);
        AdRequest request = new AdRequest();
        _bottomBanner.LoadAd(request);

        if (_topBanner != null)
        {
            _topBanner.Destroy();
        }
        _topBanner = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);
        request = new AdRequest();
        _topBanner.LoadAd(request);
    }

}
