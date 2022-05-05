using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannderAd : MonoBehaviour
{
    public BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    public string _androidAdUnitId = "Banner_Android";
    public string _iOsAdUnitId = "Banner_iOS";
    public string _adUnitId;

    private void Start()
    {
        Advertisement.Banner.SetPosition(_bannerPosition);

        BannerLoadOptions loadOptions = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adUnitId, loadOptions);

        Advertisement.Banner.Show(_adUnitId);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }
}
