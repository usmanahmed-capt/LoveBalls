using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPage : MonoBehaviour
{
    public GameObject lvlSelection;
    public GameObject ModeSelection;
    public GameObject MainPageObject;

    public GameObject MainlvlSelection;
    public GameObject SubLvlSelection;
    public string youtubeURL = "https://www.youtube.com/"; // URL of the YouTube video or channel you want to open
    public string instagramURL = "https://www.instagram.com/your_username_here/"; // Replace "your_username_here" with your Instagram username
    public string facebookURL = "https://www.facebook.com/"; // URL of your Facebook profile or page

    public string Link;
    private string url;
    private const string AndroidRatingURI = "http://play.google.com/store/apps/details?id={0}";
    private const string iOSRatingURI = "itms://itunes.apple.com/us/app/apple-store/{0}?mt=8";
    public GameObject[] ShopredBox;
    public GameObject[] ShopYellowBox;

    public GameObject ExitPanal;
    private void Start()
    {
        url = AndroidRatingURI.Replace("{0}", Application.identifier);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (Time.timeScale == 1) 
            {
                GameController.Instance.CanPlayOn = false;
                ExitPanal.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void Yes()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Application.Quit();
    }
    public void No()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Time.timeScale = 1;
        ExitPanal.SetActive(false);
        GameController.Instance.CanPlayOn = true;
    }
    public void playbtnClick()
    {
        SoundManager.Instance.PlayButtonClickSound();
        MainPageObject.SetActive(false);
        FirebaseInitialize.instance.LogEventGame("mainmanu_to_mainslection");//sohail
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("mainmanu_to_mainslection");//sohail 
       
        if (!GameController.Instance.IsFistTimeClickOnMainPage) 
        {
            lvlSelection.SetActive(true);
            MainlvlSelection.SetActive(true);
        }

        if (GameController.Instance.IsFistTimeClickOnMainPage)
        {
            //ModeSelection.SetActive(true);
            //lvlSelection.SetActive(true);
            //MainlvlSelection.SetActive(true);
            lvlSelection.SetActive(true);
            MainlvlSelection.SetActive(true);
        }
        GameController.Instance.IsFistTimeClickOnMainPage = true;

        AdsManager.Instance.ShowBannerBottom();
    }

    public void HomeBtnClickFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Debug.LogError("MainPageObject");
        MainPageObject.SetActive(true);
        ModeSelection.SetActive(false);
    }

    public void BasicLvlFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
        lvlSelection.SetActive(true);
        ModeSelection.SetActive(false);
    }

    public void BackArrowFunFromSelectlvl()
    {
        SoundManager.Instance.PlayButtonClickSound();

        if (AdsManager.ShowAd)
        {
            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
            {
                GameController.Instance.AfterCallingInterstatialads = 4;
                GameController.Instance.InterRefObj = this.gameObject;
                AdsManager.Instance.ShowVideoAdOnTimer_2("backtohome");
            }
            else 
            {
                BackArrowFunFromSelectionLvlCallerAfterAds();
            }
        }
        else 
        {
            BackArrowFunFromSelectionLvlCallerAfterAds();
        }

       
        


    }

 internal void BackArrowFunFromSelectionLvlCallerAfterAds() 
    {
        if (!SubLvlSelection.activeSelf)
        {
            lvlSelection.SetActive(false);
            //  ModeSelection.SetActive(true);
            MainPageObject.SetActive(true);
            MainlvlSelection.SetActive(true);
            SubLvlSelection.SetActive(false);
            FirebaseInitialize.instance.LogEventGame("mainslection_to_mainmanu");//sohail
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent("mainslection_to_mainmanu");//sohail 
        }

        if (SubLvlSelection.activeSelf)
        {
            lvlSelection.SetActive(true);
            //  ModeSelection.SetActive(true);
            MainPageObject.SetActive(false);
            MainlvlSelection.SetActive(true);
            SubLvlSelection.SetActive(false);
            FirebaseInitialize.instance.LogEventGame("lvlslection_to_mainslection");//sohail
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent("lvlslection_to_mainslection");//sohail 
        }
    }

    public void MainLvlSelectiontoSublvlSelection()
    {
        SoundManager.Instance.PlayButtonClickSound();
        SubLvlSelection.SetActive(true);
        MainlvlSelection.SetActive(false);
    }

    public void YoutubeFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Application.OpenURL(youtubeURL);
        AdmobAdsManager.blockAppOpen = true;
        Invoke(nameof(unblockappOpen), 1f);
    }




    public void OpenInstagramURL()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Application.OpenURL(instagramURL);
        AdmobAdsManager.blockAppOpen = true;
        Invoke(nameof(unblockappOpen), 1f);
    }
    void unblockappOpen()
    {
        AdmobAdsManager.blockAppOpen = false;
    }

    public void OpenFacebookURL()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Application.OpenURL(facebookURL);
    }

    public void PrivacyPolicyURL()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Application.OpenURL(Link);
        AdmobAdsManager.blockAppOpen = true;
        Invoke(nameof(unblockappOpen), 1f);
    }

    public void RateUsFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
        Application.OpenURL(url);
        AdmobAdsManager.blockAppOpen = true;
        Invoke(nameof(unblockappOpen), 1f);
    }

    public void ShopBoxAtive(int ActiveIndux) 
    {
        for (int i = 0; i < ShopredBox.Length; i++)
        {
            if (i == ActiveIndux) 
            {
                ShopredBox[i].SetActive(false);
                ShopYellowBox[i].SetActive(true);
            }
            else
            {
                ShopredBox[i].SetActive(true);
                ShopYellowBox[i].SetActive(false);
            }
        }
       
}
}
