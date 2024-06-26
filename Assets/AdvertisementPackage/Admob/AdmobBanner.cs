using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdmobBanner : AdmobBase
{

    [Header("Show Banner Ad on load")]
    public bool ShowBannerOnLoad;


    [Header("Set Banner Position")]
    public AdPosition BannerPosition = AdPosition.Top;


    [Header("Set Banner Type")]
    public BannerType BannerType = BannerType.AdaptiveBanner;


    #region Banner

    [Header("Banner Ids")]

    public string BannerId;

    BannerView bannerView;

    bool BannerLoaded;

    //  [HideInInspector]
    public bool isShowing;
    public void RequestBanner()
    {
        if (BannerType != BannerType.MediumRectangle)
            return;

        if (PlayerPrefs.GetInt("RemoveAdsOnly") == 1 || AdmobAdsManager.Reviewbool)
        {
            return;
        }


        if (!IsValidSDK())
        {
            return;
        }


        try
        {
            DestoryBanner();

            if (bannerView == null)
            {

                if (BannerType == BannerType.AdaptiveBanner)
                {
                    AdSize adaptiveSize =
                         AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

                    bannerView = new BannerView(BannerId, adaptiveSize, BannerPosition);

                }
                else if (BannerType == BannerType.SmartBanner)
                {
                    // Create a 320x50 banner at the top of the screen.
                    bannerView = new BannerView(BannerId, AdSize.SmartBanner, BannerPosition);
                }
                else if (BannerType == BannerType.NormalBanner)
                {
                    // Create a 320x50 banner at the top of the screen.
                    bannerView = new BannerView(BannerId, AdSize.Banner, BannerPosition);
                }
                else if (BannerType == BannerType.MediumRectangle)
                {
                    // Create a 320x50 banner at the top of the screen.
                    bannerView = new BannerView(BannerId, AdSize.MediumRectangle, BannerPosition);
                }

                // Called when an ad request has successfully loaded.
                bannerView.OnBannerAdLoaded += HandleOnAdLoaded;

                // Called when an ad request failed to load.
                bannerView.OnBannerAdLoadFailed += HandleOnAdFailedToLoad;

                // Called when an ad is clicked.
                bannerView.OnAdFullScreenContentOpened += HandleOnAdOpened;

                // Called when the user returned from the app after an ad click.
                bannerView.OnAdFullScreenContentClosed += HandleOnAdClosed;

                bannerView.OnAdPaid += HandlePaidEvent;

            }


            AdRequest request;

            if (AdmobAdsManager.Instance.FamilyAds)
            {
                //Create an empty ad request.
                request = new AdRequest.Builder().AddExtra("npa", "1").Build();
            }
            else
            {
                //Create an empty ad request.
                request = new AdRequest.Builder().Build();
            }

            // Load the banner with the request.

            if (bannerView != null && request != null)
            {
                bannerView.LoadAd(request);
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());

        }
    }
    public void DestoryBanner()
    {
        try
        {
            if (BannerType != BannerType.MediumRectangle)
                return;

            if (bannerView != null)
            {
                bannerView.Destroy();
            }
            bannerView = null;

            BannerLoaded = false;

            if (BannerType != BannerType.MediumRectangle)
                AdmobAdsManager.Instance.OnBannerHide?.Invoke();



        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    // [HideInInspector]
    public bool LastTimeActive;

    public void HideBanner()
    {

        if (BannerType != BannerType.MediumRectangle)
            return;
        try
        {
            if (bannerView != null)
            {
                bannerView.Hide();
            }

            if (BannerType != BannerType.MediumRectangle)
                AdmobAdsManager.Instance.OnBannerHide?.Invoke();

            //  LastTimeActive = isShowing;

            isShowing = false;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    public void ShowBanner()
    {
        if (BannerType != BannerType.MediumRectangle)
            return;

        if (!IsValidSDK())
        {
            return;
        }

        if (AdmobAdsManager.Reviewbool || PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
        {
            return;
        }

        if (BannerType != BannerType.MediumRectangle)
            HideBanner();

        try
        {
            if (BannerLoaded && bannerView != null)
            {

                bannerView.Show();
                //AppsFlyerObjectScript.AdImperssionEvent("Banner");

                if (BannerType != BannerType.MediumRectangle)
                    AdmobAdsManager.Instance.OnBannerShow?.Invoke();

                isShowing = true;

            }


        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void ShowBanner(AdPosition adpos)
    {

        if (!IsValidSDK())
        {
            return;
        }

        if (BannerType != BannerType.MediumRectangle)
            return;

        if (AdmobAdsManager.Reviewbool || PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
        {
            return;
        }

        if (BannerType != BannerType.MediumRectangle)
            HideBanner();

        try
        {


            if (BannerLoaded && bannerView != null)
            {


                bannerView.SetPosition(adpos);

                bannerView.Show();
                //AppsFlyerObjectScript.AdImperssionEvent("Banner");
                if (BannerType != BannerType.MediumRectangle)
                    AdmobAdsManager.Instance.OnBannerShow?.Invoke();

                isShowing = true;

            }


            BannerPosition = adpos;
            if (BannerType != BannerType.MediumRectangle)
                AdmobAdsManager.Instance.OnBannerLoaded?.Invoke(adpos);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void HandleOnAdLoaded()
    {
        Debug.Log("HandleAdLoaded event received");

        Debug.Log("Junaid ././/././.././. Bannaer Loaded!");

        BannerLoaded = true;

        if (BannerType == BannerType.MediumRectangle)
        {
            HideBanner();
        }
        else
        {
            HideBanner();


            if (AdmobAdsManager.Reviewbool || PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
            {
                return;
            }

            if (!ShowBannerOnLoad)
            {
                ShowBannerOnLoad = true;

                return;
            }

            if (bannerView != null)
            {
                bannerView.Show();
                //AppsFlyerObjectScript.AdImperssionEvent("Banner");
                if (BannerType != BannerType.MediumRectangle)
                    AdmobAdsManager.Instance.OnBannerShow?.Invoke();
                isShowing = true;
            }
            if (BannerType != BannerType.MediumRectangle)
                AdmobAdsManager.Instance.OnBannerLoaded?.Invoke(BannerPosition);
        }


    }

    public void HandleOnAdFailedToLoad(LoadAdError error)
    {

        Debug.Log("Junaid ././/././.././. Bannaer Failed!");

        BannerLoaded = false;

        RequestBanner();
    }

    public void HandleOnAdOpened()
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed()
    {
        Debug.Log("HandleAdClosed event received");
    }



    #endregion


    private void HandlePaidEvent(AdValue adValue)
    {

        Debug.LogFormat("Received paid event.");

        double _adValue = Double.Parse(adValue.Value.ToString());
        _adValue = (_adValue / 1000000f);

        Debug.Log("Ad Value: " + _adValue);

    }
}

