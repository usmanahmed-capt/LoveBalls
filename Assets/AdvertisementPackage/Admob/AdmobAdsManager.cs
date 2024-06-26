using GameAnalyticsSDK;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdmobAdsManager : AdmobBase
{
    public static AdmobAdsManager Instance;

    [Header("Use AppOpen")]
    public bool UseAppOpen;

    [Header("Use Rewarded Video")]
    public bool UseRewardedVideo;

    [Header("Use Big Banner")]
    public bool UseBigBanner;

    [HideInInspector]
    public UnityEvent OnRewardedVideoCompleted;

    [HideInInspector]
    public UnityEvent OnRewardedInterstitialCompleted;

    [HideInInspector]
    public UnityEvent NoVideoAvailble = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnRewardedAdShow = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnInterstitialAdShow = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnInterstitialAdClose = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnInterstitialAdFailed = new UnityEvent();

    [HideInInspector]
    public PositionEvent OnBannerLoaded;

    [HideInInspector]
    public UnityEvent OnBannerHide = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnBannerShow = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnAppOpenShow = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnAppOpenHide = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnRewardedAdClose = new UnityEvent();

    [Header("Review bool For Ads Off Get from Unity Dashboard. If no needs to off leave it empty")]
    public string ReviewBool;

    [Header("Screen Orientation")]
    public ScreenOrientation screenOrientation = ScreenOrientation.AutoRotation;

    [Header("Check this bool if your game is using personlized Ads")]
    public bool FamilyAds;

    char[] purposeConsents;
    char[] purposeConsentsDefault = new char[11];
    char purposeOneString;
    char purposetwoString;
    char purposethreeString;
    char purposefourString;
    char ispersonlizd;
    void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.Log(error);
            return;
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        if (ConsentInformation.IsConsentFormAvailable())
        {
            LoadForm();
        }
        else
        {
            IntializeAdmob();
        }
    }

    private ConsentForm _consentForm;

    void LoadForm()
    {
        // Loads a consent form.
        ConsentForm.Load(OnLoadConsentForm);
    }


    void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.Log(error);
            return;
        }

        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = consentForm;


        // You are now ready to show the form.
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }
        else if (ConsentInformation.ConsentStatus == ConsentStatus.NotRequired || ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
        {
            IntializeAdmob();
        }


    }

    void OnShowForm(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }
        Debug.LogError(" 1 ");
        string purposeConsents = ApplicationPreferences.GetString("IABTCF_PurposeConsents");
        Debug.LogError(" 2 purposeConsents.Length" + purposeConsents.Length);
        int GdprApplies = ApplicationPreferences.GetInt("IABTCF_gdprApplies");
        Debug.LogError(" 3 " + purposeConsents + " 2 " + GdprApplies);
        for (int i = 0; i < purposeConsentsDefault.Length; i++)
        {
            purposeConsentsDefault[i] = '0';
        }
        if (!string.IsNullOrEmpty(purposeConsents))
        {
            for (int i = 0; i < purposeConsents.Length; i++)
            {
                purposeConsentsDefault[i] = purposeConsents[i];//11111111
                Debug.Log(purposeConsents[i]);
            }

            if (purposeConsents[0] == '1' && purposeConsents[1] == '1' && purposeConsents[2] == '1' && purposeConsents[3] == '1' && purposeConsents[6] == '1' && purposeConsents[8] == '1' && purposeConsents[9] == '1')
            {
                //personalized
                ispersonlizd = '1';

            }
            else if (purposeConsents[0] == '1' && purposeConsents[1] == '1' && purposeConsents[6] == '1' && purposeConsents[8] == '1' && purposeConsents[9] == '1')
            {
                //nonpersonalized
                ispersonlizd = '0';
            }
            purposeOneString = purposeConsentsDefault[0];


            FirebaseInitialize.instance.SendFirebaseConsentDetail(ispersonlizd);
            //AdjustCustomEventNew.Instance.AdjustUpdateOption(GdprApplies.ToString(), ispersonlizd, ispersonlizd);

        }
        Debug.LogError(" 1 " + purposeConsents + " 2 " + GdprApplies);


        // Handle dismissal by reloading form.
        LoadForm();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Admob App ID")]
    public string AppId;

    [Header("1 GB Device Check")]
    public bool UseDeviceCheck = false;

    [Header("Banners you want to show in your game")]
    public List<MyBanner> Banners = new List<MyBanner>();

    [Header("Big Banners you want to show in your game")]
    public List<MyBanner> BigBanners = new List<MyBanner>();

    public static bool blockAppOpen;

    public static bool deviceCheck;


    public static string AdPlacement = "";

    public static bool IsDeviceValid()
    {
#if UNITY_ANDROID

        if (deviceCheck)
        {
            if (SystemInfo.systemMemorySize <= 1024)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
#endif

        return true;
    }

    public static bool Reviewbool = false;

    private void Start()
    {
        deviceCheck = UseDeviceCheck;

        GameAnalyticsSDK.GameAnalytics.Initialize();


        GameAnalyticsSDK.GameAnalyticsILRD.SubscribeIronSourceImpressions();



        // Create a ConsentRequestParameters object.
        ConsentRequestParameters request = new ConsentRequestParameters();

        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);

    }


    public void IntializeAdmob()
    {

        if (!IsValidSDK())
        {
            return;
        }

        MobileAds.SetiOSAppPauseOnBackground(true);
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus =>
        {
            LoadOpenApp();

            for (int i = 0; i < Banners.Count; i++)
            {
                Banners[i].Banner.RequestBanner();
            }

            if (UseBigBanner)
                Invoke(nameof(LoadBigBanner), 3f);

            Invoke(nameof(RequestInterstitial), 5f);

            Invoke(nameof(RequestRewardedInterstitial), 5f);

            Invoke(nameof(RequestRewardedVideo), 5f);

        });
    }

    #region Banner APIs
    public void DestoryBannerBottom()
    {
        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.Bottom)
                {
                    AdmobAdsManager.Instance.Banners[i].Banner.DestoryBanner();

                }
            }
        }

    }
    public void DestoryBannerTop()
    {
        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.Top)
                {
                    AdmobAdsManager.Instance.Banners[i].Banner.DestoryBanner();

                }
            }
        }
    }

    public void LoadBigBanner()
    {
        for (int i = 0; i < BigBanners.Count; i++)
        {
            BigBanners[i].Banner.RequestBanner();
        }
    }

    public void LoadBanner()
    {
        for (int i = 0; i < Banners.Count; i++)
        {
            Banners[i].Banner.RequestBanner();
        }
    }

    public void ShowBanner()
    {
        for (int i = 0; i < Banners.Count; i++)
        {
            Banners[i].Banner.ShowBanner();
        }
    }

    public void HideBanner()
    {
        for (int i = 0; i < Banners.Count; i++)
        {
            Banners[i].Banner.HideBanner();
        }
    }


    public void ShowBigBanner()
    {
        for (int i = 0; i < BigBanners.Count; i++)
        {
            BigBanners[i].Banner.ShowBanner();
        }
    }

    public void HideBigBanner()
    {
        for (int i = 0; i < BigBanners.Count; i++)
        {
            BigBanners[i].Banner.HideBanner();
        }
    }



    #endregion

    #region RewardedVideo APIs

    public void LoadRewardedVideo()
    {
        RequestRewardedVideo();
    }

    public void ShowRewardedVideoAPI()
    {
        if (IsRewardedVideoAvaialable())
        {
            ShowRewardedVideo();
        }
    }

    public void ShowrewardedInterstitial()
    {
        if (HasRewardedInterstitialAd())
        {
            ShowRewardedInterstitialAd();
        }
    }

    public void LaodRewardedInterstitial()
    {
        RequestRewardedInterstitial();
    }

    #endregion

    #region Appopen 
    internal AppOpenAd appopenad;
    private bool isShowingAd = false;

    [Header("Show App Open On Start")]
    public bool isAppOpenShowOnStart = false;

    [ConditionalHide("UseAppOpen", false)]
    public string AppOpenId;


    private DateTime expiretime;
    private readonly TimeSpan TIMEOUT = TimeSpan.FromHours(4);
    static bool firstAppopen = false;

    public void LoadOpenApp()
    {
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }

        if (PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
        {
            return;
        }

        if (!UseAppOpen)
        {
            return;
        }

        AdRequest request = new AdRequest();

        AppOpenAd.Load(AppOpenId, Screen.orientation, request, ((AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.Log("Failed to load the ad with error : " + error);
                return;
            }

            if (ad == null)
            {
                Debug.Log("Unexpected error: App open ad load event fired with " +
                               " null ad and null error.");
                return;
            }
            Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());

            appopenad = ad;

            expiretime = DateTime.Now + TIMEOUT;

            if (isAppOpenShowOnStart)
            {
                showAppOpenOnStart();
            }

        }));

    }

    public void showAppOpenOnStart()
    {
        if (!firstAppopen)
        {
            firstAppopen = true;
            ShowAppOpenAdIfAvailable();
        }
    }

    public void ShowAppOpenAdIfAvailable()
    {
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }


        if (PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
        {
            return;
        }

        if (blockAppOpen)
        {
            return;
        }


        if (!UseAppOpen)
        {
            return;
        }


        if (appopenad == null || isShowingAd || !IsAppOpenAdTimedOut)
        {
            return;
        }

        appopenad.OnAdFullScreenContentClosed += HandleAdDidDismissFullScreenContent;
        appopenad.OnAdFullScreenContentOpened += HandleAdDidPresentFullScreenContent;
        appopenad.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
        appopenad.OnAdImpressionRecorded += HandleAdDidRecordImpression;
        appopenad.OnAdPaid += HandlePaidEvent;

        appopenad.Show();
        ////AppsFlyerObjectScript.AdImperssionEvent("AppOpen");

    }

    public void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            if (!blockAppOpen)
            {
                if (!CanAppOpenOrInter)
                {
                    if (SceneManager.GetActiveScene().name != "Shop")
                        ShowAppOpenAdIfAvailable();
                }
                else
                {
                    if (SceneManager.GetActiveScene().name != "Shop")
                        AdsManager.Instance.ShowVideoAdOnTimer("OnPauseInter");
                }
            }
        }



    }

    private bool IsAppOpenAdTimedOut
    {
        get
        {
            return appopenad != null && appopenad.CanShowAd() && DateTime.Now < expiretime;
        }
    }

    public bool CanAppOpenOrInter { get; internal set; }

    private void HandleAdDidDismissFullScreenContent()
    {
        Debug.Log("Closed app open ad");
        appopenad = null;
        isShowingAd = false;
        LoadOpenApp();

        Invoke(nameof(unblockAppOpen), 1f);

        OnAppOpenHide.Invoke();

    }

    private void HandleAdFailedToPresentFullScreenContent(AdError error)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", error);
        appopenad = null;
    }

    private void HandleAdDidPresentFullScreenContent()
    {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;

        OnAppOpenShow.Invoke();

    }

    private void HandleAdDidRecordImpression()
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(AdValue adValue)
    {

        Debug.LogFormat("Received paid event.");
        double _adValue = Double.Parse(adValue.Value.ToString());
        _adValue = (_adValue / 1000000f);

        Debug.Log("Ad Value: " + _adValue);

        FirebaseInitialize.instance.CustomAdEvent("AppOpen", SceneManager.GetActiveScene().name);
    }

    int interstitialCount = 0;
    int rewardedvideoCount = 0;
    private void HandleInterstitialPaidEvent(AdValue adValue)
    {

        Debug.LogFormat("Received paid event.");

        double _adValue = Double.Parse(adValue.Value.ToString());
        _adValue = (_adValue / 1000000f);
        Debug.Log("Ad Value: " + _adValue);
    }
    private void HandleRewardedPaidEvent(AdValue adValue)
    {

        Debug.LogFormat("Received paid event.");

        double _adValue = Double.Parse(adValue.Value.ToString());
        _adValue = (_adValue / 1000000f);

        Debug.Log("Ad Value: " + _adValue);

    }

    #endregion

    #region Interstitial

    public string InterstitialId;

    private InterstitialAd interstitial;


    public bool HasInterstitialAvailable()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void ShowInterstitialVideoAd()
    {
        if (!IsValidSDK())
        {
            return;
        }
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }

        if (!AdsManager.UseInterstitialToUnlock)
        {

            if (PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
            {
                return;
            }
        }



        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
            //AppsFlyerObjectScript.AdImperssionEvent("Interstitial");

            FirebaseInitialize.instance.LogEvent("interstitial");

            FirebaseInitialize.instance.CustomAdEvent("interstitial", AdPlacement);


        }
        else
        {
            RequestInterstitial();
        }


    }


    public void ShowInterstitial()
    {
        if (!IsValidSDK())
        {
            return;
        }
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }
        if (PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
        {
            return;
        }
        if (!AdsManager.UseInterstitialToUnlock)
        {


        }


        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
            //AppsFlyerObjectScript.AdImperssionEvent("Interstitial");
            FirebaseInitialize.instance.CustomAdEvent("Admob_FailOver", AdPlacement);
            FirebaseInitialize.instance.LogEvent("interstitial");
            FirebaseInitialize.instance.CustomAdEvent("interstitial", AdPlacement);

            GameAnalytics.NewDesignEvent("Interstitial");


        }
        else
        {
            RequestInterstitial();
        }


    }



    public void RequestInterstitial()
    {
        if (!IsValidSDK())
        {
            return;
        }
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }

        if (!AdsManager.UseInterstitialToUnlock)
        {

            if (PlayerPrefs.GetInt("RemoveAdsOnly") == 1)
            {
                return;
            }
        }


        if (interstitial != null)
        {
            Debug.Log("Destroying interstitial ad.");
            interstitial.Destroy();
            interstitial = null;
        }




        AdRequest request = new AdRequest();

        if (AdmobAdsManager.Instance.FamilyAds)
        {
            request = new AdRequest.Builder().AddExtra("npa", "1").Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Request, GameAnalyticsSDK.GAAdType.Interstitial, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());


        InterstitialAd.Load(InterstitialId, request, (InterstitialAd ad, LoadAdError error) =>
        {

            if (error != null)
            {
                Debug.Log("Interstitial ad failed to load an ad with error : " + error);
                return;
            }
            if (ad == null)
            {
                Debug.Log("Unexpected error: Interstitial load event fired with null ad and null error.");
                return;
            }

            GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Loaded, GameAnalyticsSDK.GAAdType.Interstitial, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());


            Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
            interstitial = ad;

            this.interstitial.OnAdClicked += InterstitialAdClicked;
            this.interstitial.OnAdFullScreenContentFailed += InterstitialHandleOnAdFailedToShow;
            this.interstitial.OnAdFullScreenContentOpened += InterstitialHandleOnAdOpened;
            this.interstitial.OnAdFullScreenContentClosed += InterstitialHandleOnAdClosed;
            this.interstitial.OnAdPaid += HandleInterstitialPaidEvent;

        });


    }

    void unblockAppOpen()
    {
        blockAppOpen = false;
    }

    private void InterstitialHandleOnAdClosed()
    {
        Debug.Log("HandleAdClosed event received");

        OnInterstitialAdClose.Invoke();

        Invoke(nameof(unblockAppOpen), 1f);

        RequestInterstitial();


    }

    private void InterstitialHandleOnAdOpened()
    {
        Debug.Log("HandleAdOpened event received");

        OnInterstitialAdShow.Invoke();

        blockAppOpen = true;

        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Show, GameAnalyticsSDK.GAAdType.Interstitial, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());


    }


    private void InterstitialHandleOnAdFailedToShow(AdError error)
    {
        OnInterstitialAdFailed.Invoke();
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                        + error);


        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.FailedShow, GameAnalyticsSDK.GAAdType.Interstitial, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());



    }

    private void InterstitialAdClicked()
    {
        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Clicked, GameAnalyticsSDK.GAAdType.Interstitial, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());

    }


    #endregion

    #region RewardedVideo
    private RewardedAd rewardedAd;

    public string RewardedAdID;

    public bool IsRewardedVideoAvaialable()
    {


        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void ShowRewardedVideo()
    {
        if (!IsValidSDK())
        {
            return;
        }

        blockAppOpen = true;

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            Debug.Log("Show Rewarded Video");

            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));

                //AppsFlyerObjectScript.AdImperssionEvent("RewardedVideo");

                FirebaseInitialize.instance.CustomAdEvent("rewarded_video", AdmobAdsManager.AdPlacement);

                FirebaseInitialize.instance.LogEvent("rewarded_video");

                GameAnalytics.NewDesignEvent("RewardedVideo");


                OnRewardedVideoCompleted.Invoke();

            });


        }
        else if (rewardedInterstitialAd != null && isRewardedInterLoaded)
        {
            isRewardedInterLoaded = false;

            Debug.Log("Show Rewarded IntersTitial Video");


            rewardedInterstitialAd.Show(userEarnedRewardCallback);

            //AppsFlyerObjectScript.AdImperssionEvent("RewardedVideo");

            FirebaseInitialize.instance.CustomAdEvent("rewarded_video", AdmobAdsManager.AdPlacement);

            FirebaseInitialize.instance.LogEvent("rewarded_video");

            GameAnalytics.NewDesignEvent("RewardedVideo");



        }
        else
        {

            Debug.Log("No Video Available!");

            NoVideoAvailble.Invoke();

            blockAppOpen = false;
        }

        RequestRewardedVideo();

        RequestRewardedInterstitial();

    }

    public void ShowRewardedInterstitialVideo()
    {
        if (!IsValidSDK())
        {
            return;
        }

        blockAppOpen = true;

        if (rewardedInterstitialAd != null && isRewardedInterLoaded)
        {
            isRewardedInterLoaded = false;

            Debug.Log("Show Rewarded IntersTitial Video");


            rewardedInterstitialAd.Show(userEarnedRewardCallback);
            //AppsFlyerObjectScript.AdImperssionEvent("RewardedVideo");


        }

        else if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            Debug.Log("Show Rewarded Video");

            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));
                OnRewardedVideoCompleted.Invoke();
                //AppsFlyerObjectScript.AdImperssionEvent("RewardedVideo");
            });



        }
        else
        {

            Debug.Log("No Video Available!");

            NoVideoAvailble.Invoke();

            blockAppOpen = false;
        }

        RequestRewardedVideo();

        RequestRewardedInterstitial();

    }

    public void RequestRewardedVideo()
    {
        if (!UseRewardedVideo)
        {
            return;
        }

        if (!IsValidSDK())
        {
            return;
        }
        if (AdmobAdsManager.Reviewbool)
        {
            return;
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
        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Request, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());


        RewardedAd.Load(RewardedAdID, request, (RewardedAd ad, LoadAdError error) =>
        {

            if (error != null)
            {
                RequestRewardedVideo();

                Debug.Log("Rewarded ad failed to load an ad with error : " + error);
                return;
            }

            if (ad == null)
            {
                RequestRewardedVideo();

                Debug.Log("Unexpected error: Rewarded load event fired with null ad and null error.");
                return;
            }

            GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Loaded, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());


            rewardedAd = ad;

            this.rewardedAd.OnAdClicked += OnRewardedAdClicked;

            this.rewardedAd.OnAdFullScreenContentOpened += HandleRewardedAdOpening;
            this.rewardedAd.OnAdFullScreenContentFailed += HandleRewardedAdFailedToShow;

            this.rewardedAd.OnAdFullScreenContentClosed += HandleRewardedAdClosed;

            this.rewardedAd.OnAdPaid += HandleRewardedPaidEvent;
        });
    }



    public void HandleRewardedAdOpening()
    {
        Debug.Log("HandleRewardedAdOpening event received");

        blockAppOpen = true;

        OnRewardedAdShow.Invoke();

        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Show, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());

    }

    public void HandleRewardedAdFailedToShow(AdError error)
    {
        Debug.Log(
            "HandleRewardedAdFailedToShow event received with message: "
                             + error);

        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.FailedShow, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());

    }

    public void OnRewardedAdClicked()
    {
        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Clicked, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());

    }

    public void HandleRewardedAdClosed()
    {
        Debug.Log("HandleRewardedAdClosed event received");

        Invoke(nameof(unblockAppOpen), 1f);

        RequestRewardedVideo();

        OnRewardedAdClose.Invoke();

    }

    #endregion

    #region RewardedInterstitial

    private RewardedInterstitialAd rewardedInterstitialAd;
    AdRequest request;

    public string RewardedInterstitial;

    bool isRewardedInterLoaded = false;


    void RequestRewardedInterstitial()
    {
        if (!UseRewardedVideo)
        {
            return;
        }

        isRewardedInterLoaded = false;

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

        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Request, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());


        // Load the rewarded ad with the request.
        RewardedInterstitialAd.Load(RewardedInterstitial, request, adRewardedInterLoadCallback);
    }


    private void adRewardedInterLoadCallback(RewardedInterstitialAd ad, LoadAdError error)
    {
        if (error == null)
        {
            GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Loaded, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());

            // Ads is now available Junaid is saying
            rewardedInterstitialAd = ad;

            isRewardedInterLoaded = true;

            rewardedInterstitialAd.OnAdClicked += OnRewardedInterstitialAdClicked;
            rewardedInterstitialAd.OnAdFullScreenContentFailed += HandleAdFailedToPresent;
            rewardedInterstitialAd.OnAdFullScreenContentOpened += HandleAdDidPresent;
            rewardedInterstitialAd.OnAdFullScreenContentClosed += HandleAdDidDismiss;
            rewardedInterstitialAd.OnAdPaid += HandleRewardedPaidEvent;
        }
        else
        {

            Debug.Log("Rewarded Interstitial LOad Fialed");

            RequestRewardedInterstitial();
        }
    }

    void OnRewardedInterstitialAdClicked()
    {
        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Clicked, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());

    }

    private void HandleAdFailedToPresent(AdError error)
    {
        print("Rewarded interstitial ad has failed to present.");

        isRewardedInterLoaded = false;

        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.FailedShow, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());


    }

    private void HandleAdDidPresent()
    {
        print("Rewarded interstitial ad has presented.");

        OnRewardedAdShow.Invoke();

        GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.Show, GameAnalyticsSDK.GAAdType.RewardedVideo, "admob", AdmobAdsManager.AdPlacement.ToLower().Trim());

    }

    private void HandleAdDidDismiss()
    {
        print("Rewarded interstitial ad has dismissed presentation.");

        isRewardedInterLoaded = false;


        Invoke(nameof(unblockAppOpen), 1f);

        OnRewardedAdClose.Invoke();

    }



    #endregion

    #region RewardedInterstitial

    public bool HasRewardedInterstitialAd()
    {
        if (isRewardedInterLoaded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ShowRewardedInterstitialAd()
    {


        blockAppOpen = true;

        if (rewardedInterstitialAd != null && isRewardedInterLoaded)
        {
            isRewardedInterLoaded = false;
            Debug.Log(" -------------------- Rewarded Interstitial --------------------");
            rewardedInterstitialAd.Show(userEarnedRewardCallback);

        }
        else
        {
            blockAppOpen = false;
        }

        RequestRewardedInterstitial();

    }

    private void userEarnedRewardCallback(Reward reward)
    {
        try
        {
            OnRewardedInterstitialCompleted.Invoke();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

    }
    #endregion

}


public enum BannerType
{
    AdaptiveBanner, NormalBanner, SmartBanner, MediumRectangle
}

public enum BannerPos
{
    Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight, Center
}


[Serializable]
public class MyBanner
{
    [SerializeField]
    public BannerPos BannerPosition;


    [SerializeField]
    public AdmobBanner Banner;

}


[System.Serializable]
public class PositionEvent : UnityEvent<AdPosition>
{

}
