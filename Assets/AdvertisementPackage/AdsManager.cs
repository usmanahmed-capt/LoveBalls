using System;
using System.Collections;
using UnityEngine;
public class AdsManager : AdmobBase
{

    public static AdsManager Instance;

    [HideInInspector]
    public bool AdmobFirst = false;
    [Header("Default Ads Timer and also use in Remote setting ")]
    public int AdsTimerThreshold = 30;
    [HideInInspector]
    public int timer;


    public static bool ShowAd;

    IEnumerator timercoroutine;

    public static bool UseInterstitialToUnlock = false;
    public static bool UseVideoInterstitialToUnlock = false;
    public static bool IsMediationBottomBanner = false;
    public static bool hideBigBannerBool = false;


    static bool RewardOnInterstitial = false;

    public static int AdsTimerAB { get; internal set; }
    public bool CanBannerHideBottom { get; internal set; }
    public bool AdmobFirstInGame = false;
    public void StartTimer()
    {
        timer = 0;
        ShowAd = false;

        if (timercoroutine != null)
        {
            StopCoroutine(timercoroutine);
            timercoroutine = null;
        }
        timercoroutine = AdTimer();
        StartCoroutine(timercoroutine);
    }



    IEnumerator AdTimer()
    {
        yield return new WaitForSeconds(1);

        timer++;

        //Debug.Log("Ads Timer: " + timer);

        if (timer >= AdsTimerThreshold)
        {
            ShowAd = true;
            timer = 0;
        }
        else
        {
            timercoroutine = AdTimer();
            StartCoroutine(timercoroutine);
        }

    }

    private void Awake()
    {
        Instance = this;
    }


    public void ShowAllLastActiveBanner()
    {
        ShowLastActiveBanners();
    }

    public void HideAllBanners()
    {
        HideBannerAll();
        HideBigBannnerAll();

    }

    private void Start()
    {
        StartTimer();


        AdmobAdsManager.Instance.OnAppOpenShow.AddListener(delegate
        {
            HideAllBanners();

        });



        AdmobAdsManager.Instance.OnAppOpenHide.AddListener(delegate
        {
            StartTimer();
            Invoke(nameof(ShowAllLastActiveBanner), 1f);

        });

        AdmobAdsManager.Instance.OnInterstitialAdShow.AddListener(delegate
        {

            // HideAllBanners();

        });
        AdmobAdsManager.Instance.OnBannerShow.AddListener(delegate
        {
            Debug.Log("OnBannerShow///////////////////////////////////");
        });

        AdmobAdsManager.Instance.OnInterstitialAdClose.AddListener(delegate
        {

            Invoke(nameof(ReplayAfterAds), .2f);
            Invoke(nameof(ShowAllLastActiveBanner), 1f);


            if (RewardOnInterstitial)
            {
                RewardOnInterstitial = false;

                try
                {
                    // Rewarded ad was displayed and user should receive the reward
                    //Invoke(nameof(ClaiRewardCaller), .2f);
                    //RewardedVideoAdCaller.instance.VideoWatches();

                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }

        });
        AdmobAdsManager.Instance.OnInterstitialAdFailed.AddListener(delegate
        {
            Invoke(nameof(ReplayAfterAds), .2f);

        });

        AdmobAdsManager.Instance.OnRewardedVideoCompleted.AddListener(delegate
        {
            CanAddShow = false;
            try
            {
                StartTimer();
                // Rewarded ad was displayed and user should receive the reward
                Invoke(nameof(ClaiRewardCaller), .2f);
                //RewardedVideoAdCaller.instance.VideoWatches();

            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

        });

        AdmobAdsManager.Instance.OnRewardedInterstitialCompleted.AddListener(delegate
        {
            CanAddShow = false;
            try
            {
                StartTimer();
                // Rewarded ad was displayed and user should receive the reward
                Invoke(nameof(ClaiRewardCaller), .2f);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        });

        AdmobAdsManager.Instance.OnRewardedAdShow.AddListener(() =>
        {
            CanAddShow = false;
            //SetBannerState();
            //HideAllBanners();

        });

        AdmobAdsManager.Instance.OnRewardedAdClose.AddListener(() =>
        {
            CanAddShow = false;
            Invoke(nameof(ShowAllLastActiveBanner), 1f);
        });
    }

    void ReplayAfterAds()
    {
        Time.timeScale = 1;
        if (GameController.Instance.AfterCallingInterstatialads == 1)
        {
            GamePlayController.Instance.RestarAfterAdsFunCalling();
        }
        if (GameController.Instance.AfterCallingInterstatialads == 2)
        {
            GameController.Instance.InterRefObj.GetComponent<ButtonManager>().ReloadCallingAfterAds();
        }
        if (GameController.Instance.AfterCallingInterstatialads == 3)
        {
            GameController.Instance.InterRefObj.GetComponent<ButtonManager>().homeCallingAfterAds();
        }
        if (GameController.Instance.AfterCallingInterstatialads == 4)
        {
            GameController.Instance.InterRefObj.GetComponent<MainPage>().BackArrowFunFromSelectionLvlCallerAfterAds();
        }
        if (GameController.Instance.AfterCallingInterstatialads == 5)
        {
            GamePlayController.Instance.NextSeneFun();
        }
        GameController.Instance.AfterCallingInterstatialads = 0;
    }
    void ClaiRewardCaller()
    {
        if (GameController.Instance.rewardIndux == 1)
        {
            GameController.Instance.isRewraDoneRecent = true;
            GamePlayController.Instance.SpinWheelShow();
        }
        else if (GameController.Instance.rewardIndux == 2)
        {
            GameController.Instance.isRewraDoneRecent = true;
            GamePlayController.Instance.ShowRandomWinPopup();
        }
        else if (GameController.Instance.rewardIndux == 3)
        {
            GameController.Instance.isRewraDoneRecent = true;
            GamePlayController.Instance.ShowRandomWinPopup();
        }
        else if (GameController.Instance.rewardIndux == 6)
        {
            GameController.Instance.isRewraDoneRecent = true;
            GamePlayController.Instance.SpinAfterReward();
        }

        else if (GameController.Instance.rewardIndux == 7)
        {
            GamePlayController.Instance.GetCoinsReward();
        }
        else if (GameController.Instance.rewardIndux == 8)
        {
            GamePlayController.Instance.HintFun(true);
        }
        else if (GameController.Instance.rewardIndux == 9)
        {
            GamePlayController.Instance.BuyPen(GameController.Instance.CallingIndux);
        }
        else if (GameController.Instance.rewardIndux == 10)
        {
            GamePlayController.Instance.BuyBall(GameController.Instance.CallingIndux);
        }
        else if (GameController.Instance.rewardIndux == 11)
        {
            GamePlayController.Instance.Buybg(GameController.Instance.CallingIndux);
        }

        else if (GameController.Instance.rewardIndux == 12)
        {
            GamePlayController.Instance.AfterAddStarWithRewardVideoCallBack();
        }

    }



    #region Banner APIs
    public void ShowBigBannnerAll()
    {
        if (AdmobAdsManager.Instance != null)
        {

            if (hideBigBannerBool)
            {
                return;
            }

            if (AdmobAdsManager.Instance.UseBigBanner)
            {
                for (int i = 0; i < AdmobAdsManager.Instance.BigBanners.Count; i++)
                {
                    AdmobAdsManager.Instance.BigBanners[i].Banner.ShowBanner();
                }

            }
        }
    }

    public void HideBigBannnerAll()
    {
        if (AdmobAdsManager.Instance != null)
        {
            if (AdmobAdsManager.Instance.UseBigBanner)
            {
                for (int i = 0; i < AdmobAdsManager.Instance.BigBanners.Count; i++)
                {
                    AdmobAdsManager.Instance.BigBanners[i].Banner.HideBanner();
                }
            }
        }
    }

    public void ShowBannerTop()
    {
        if (AdmobAdsManager.Instance != null)
        {

            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.Top
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.TopLeft
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.TopRight)
                {
                    AdmobAdsManager.Instance.Banners[i].Banner.ShowBanner();

                    return;
                }
            }
        }
    }

    public void HideBannerTop()
    {
        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.Top
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.TopLeft
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.TopRight)
                {
                    AdmobAdsManager.Instance.Banners[i].Banner.HideBanner();

                    return;
                }
            }
        }
    }

    public void HideBannerBottom()
    {

        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.Bottom
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.BottomLeft
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.BottomRight)
                {
                    AdmobAdsManager.Instance.Banners[i].Banner.HideBanner();

                    return;
                }
            }
        }
    }

    public void ShowBannerBottom()
    {
        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.Bottom
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.BottomLeft
                    || AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.BottomRight)
                {
                    AdmobAdsManager.Instance.Banners[i].Banner.ShowBanner();

                    return;
                }
            }
        }
    }

    public void ShowLastActiveBanners()
    {
        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].Banner.LastTimeActive)
                {
                    AdmobAdsManager.Instance.Banners[i].Banner.ShowBanner();
                    AdmobAdsManager.Instance.Banners[i].Banner.LastTimeActive = false;
                }
            }

            for (int i = 0; i < AdmobAdsManager.Instance.BigBanners.Count; i++)
            {
                if (AdmobAdsManager.Instance.BigBanners[i].Banner.LastTimeActive)
                {
                    AdmobAdsManager.Instance.BigBanners[i].Banner.ShowBanner();
                    AdmobAdsManager.Instance.BigBanners[i].Banner.LastTimeActive = false;

                }
            }
        }
    }

    public void DisableLastActive()
    {
        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                if (AdmobAdsManager.Instance.Banners[i].Banner.LastTimeActive)
                {
                    if (AdmobAdsManager.Instance.Banners[i].BannerPosition == BannerPos.Bottom)
                        AdmobAdsManager.Instance.Banners[i].Banner.LastTimeActive = false;

                }
            }
        }
    }

    public void ShowBannerAll()
    {
        ShowBannerTop();
        ShowBannerBottom();
    }

    public void HideBannerAll()
    {
        if (AdmobAdsManager.Instance != null)
        {
            for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
            {
                AdmobAdsManager.Instance.Banners[i].Banner.HideBanner();
            }
        }
    }

    #endregion

    #region InterstitialAPIs

    public void ShowVideoAdOnTimer(string placement)
    {
        AdmobAdsManager.AdPlacement = placement + "_inter";
        if (ShowAd)
        {
            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
            {
                SetBannerState();
                HideAllBanners();
                Invoke(nameof(ShowInterAds), .01f);
            }
        }
    }

    public void ShowVideoAdOnTimer_2(string placement)
    {
        AdmobAdsManager.AdPlacement = placement + "_inter";
        SetBannerState();
        HideAllBanners();
        ShowInterAds();
        //  Invoke(nameof(ShowInterAds), .01f);
    }

    public void ShowVideoAdOnTimer_2AdmobFirst(string placement)
    {
        AdmobAdsManager.AdPlacement = placement + "_inter";
        SetBannerState();
        HideAllBanners();
        ShowInterAdsInGames();

    }

    public void ShowVideoAdOnTimerOnLoading(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;
        SetBannerState();
        HideAllBanners();
        Invoke(nameof(ShowInterAds), 1f);
    }
    public void ShowVideoAdOnTimerOnLoadingFixed(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;
        SetBannerState();
        HideAllBanners();
        Invoke(nameof(ShowInterAdsFixed), 1f);
    }

    void ShowInterAdsFixed()
    {
        if (AdmobFirst)
        {
            ShowAdmobFixd();
        }
        else
        {
            showVideoAdFixed();
        }
    }


    void ShowInterAds()
    {
        if (AdmobFirst)
        {
            showAdmobOnTimer();
        }
        else
        {
            //  ShowMediationInterstitial(AdmobAdsManager.AdPlacement);
            showVideoAdOnTimer();
        }
    }

    public void ShowInterstitialWithTimerIGameads(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;

        if (ShowAd && (AdmobAdsManager.Instance.HasInterstitialAvailable()))
        {
            SetBannerState();
            HideAllBanners();
            Invoke(nameof(ShowInterAdsInGames), 1f);
        }
    }

    public void ShowInterstitialFixedInGame(string placement)
    {
        AdmobAdsManager.AdPlacement = placement + "_inter";
        if ((AdmobAdsManager.Instance.HasInterstitialAvailable()))
        {
            SetBannerState();
            HideAllBanners();
            Invoke(nameof(ShowInterFixed), .01f);
        }
    }

    public void ShowInterstitialFixedInGameOutercheck(string placement)
    {
        AdmobAdsManager.AdPlacement = placement + "_inter";
        SetBannerState();
        HideAllBanners();
        Invoke(nameof(ShowInterFixed), .01f);
    }
    void ShowInterFixed()
    {
        if (AdmobFirstInGame)
        {
            ShowAdmobFixd();
        }
        else
        {
            //  ShowMediationInterstitial(AdmobAdsManager.AdPlacement);
            showVideoAdFixed();
        }
    }

    void ShowInterAdsInGames()
    {
        if (AdmobFirstInGame)
        {
            showAdmobOnTimer();
        }
        else
        {
            //  ShowMediationInterstitial(AdmobAdsManager.AdPlacement);
            showVideoAdOnTimer();
        }
    }


    void showVideoAdOnTimer()
    {
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }


        if (ShowAd)
        {
            AdmobAdsManager.Instance.ShowInterstitialVideoAd();
        }
    }

    void showVideoAdFixed()
    {
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }

        AdmobAdsManager.Instance.ShowInterstitial();
    }

    public void ShowAdmobAdOnTimer(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;
        showAdmobOnTimer();
    }

    void showAdmobOnTimer()
    {
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }


        if (ShowAd)
        {
            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
            {

                AdmobAdsManager.Instance.ShowInterstitial();

                StartTimer();

            }
        }
    }

    void ShowAdmobFixd()
    {
        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }
        if (AdmobAdsManager.Instance.HasInterstitialAvailable())
        {
            AdmobAdsManager.Instance.ShowInterstitial();
            StartTimer();
        }
    }

    public void ShowInterstitial(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;
        if (AdmobAdsManager.Instance.HasInterstitialAvailable())
        {
            HideAllBanners();

        }
        Invoke(nameof(showInterstitial), 1f);
    }

    void showInterstitial()
    {

        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }

        if (AdmobFirst)
        {
            ShowAdmobInterstitial(AdmobAdsManager.AdPlacement);
        }
    }

    void ShowAdmobInterstitial(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;

        if (AdmobAdsManager.Reviewbool)
        {
            return;
        }

        if (AdmobAdsManager.Instance != null)
        {
            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
            {
                AdmobAdsManager.Instance.ShowInterstitial();

            }
        }
    }

    #endregion
    private bool CanAddShow;
    #region RewardedVideo
    public void ShowRewardedVideo(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;
        if (!CanAddShow && (AdmobAdsManager.Instance.HasRewardedInterstitialAd() || AdmobAdsManager.Instance.IsRewardedVideoAvaialable()))
        {
            CanAddShow = true;
            SetBannerState();
            HideAllBanners();
            Invoke(nameof(showRewardedVideo), .5f);
        }
    }
    void showRewardedVideo()
    {
        if (UseInterstitialToUnlock)
        {
            RewardOnInterstitial = true;

            if (UseVideoInterstitialToUnlock)
            {
                AdmobAdsManager.Instance.ShowInterstitial();
            }
        }
        else
        {
            AdmobAdsManager.Instance.ShowRewardedVideo();
        }
    }

    public void ShowAdmobRewardedVideo(string placement)
    {
        AdmobAdsManager.AdPlacement = placement;

        if (AdmobAdsManager.Instance.HasRewardedInterstitialAd() || AdmobAdsManager.Instance.IsRewardedVideoAvaialable())
        {
            HideAllBanners();

        }

        Invoke(nameof(AdmobAdsManager.Instance.ShowRewardedInterstitialVideo), 1f);
    }

    private void SetBannerState()
    {
        for (int i = 0; i < AdmobAdsManager.Instance.Banners.Count; i++)
        {
            if (AdmobAdsManager.Instance.Banners[i].Banner.isShowing)
            {
                AdmobAdsManager.Instance.Banners[i].Banner.LastTimeActive = true;
            }
        }
        for (int i = 0; i < AdmobAdsManager.Instance.BigBanners.Count; i++)
        {
            if (AdmobAdsManager.Instance.BigBanners[i].Banner.isShowing)
            {
                AdmobAdsManager.Instance.BigBanners[i].Banner.LastTimeActive = true;
            }
        }
    }
    #endregion

}
