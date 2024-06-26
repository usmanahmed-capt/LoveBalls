using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour {

    public Animator HintAnim;
    public GameObject ReplayMoveToCanter;
	void Start ()
    {
		
	}
    public void Reload()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.CanPlayOn = false;
        ReplayMoveToCanter.SetActive(false);
        if (AdsManager.ShowAd)
        {
            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
            {
                GameController.Instance.AfterCallingInterstatialads = 2;
                GameController.Instance.InterRefObj = this.gameObject;
                AdsManager.Instance.ShowVideoAdOnTimer_2("Replay");
            }
            else
            {
                ReloadCallingAfterAds();
            }
        }
        else
        {
            ReloadCallingAfterAds();
        }
       
      
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

  internal void ReloadCallingAfterAds() 
    {
        GamePlayController.Instance.RestartFun();
        FirebaseInitialize.instance.LogEventGame("Level_" + GameController.Instance.currenLvlClick + "_replay");//sohail
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("Level_" + GameController.Instance.currenLvlClick + "_replay");//sohail 
    }

    public void ReplayOnCanerBtnCalled()
    {
        Reload();
        gameObject.SetActive(false);
    }

    public void Home()
    {
        SoundManager.Instance.PlayButtonClickSound();
        if (AdsManager.ShowAd)
        {
            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
            {
                GameController.Instance.AfterCallingInterstatialads = 3;
                GameController.Instance.InterRefObj = this.gameObject;
                AdsManager.Instance.ShowVideoAdOnTimer_2("backtoselection");
            }
            else
            {
                homeCallingAfterAds();
            }
        }
        else
        {
            homeCallingAfterAds();
        }
    }

  internal  void homeCallingAfterAds() 
    {
        AdsManager.Instance.ShowBannerBottom();
        GamePlayController.Instance.HidCurrentLvl();
        GamePlayController.Instance.BackSnapPos();
        FirebaseInitialize.instance.LogEventGame("level_" + GameController.Instance.currenLvlClick + "to_lvlslection");//sohail
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("level_" + GameController.Instance.currenLvlClick + "to_lvlslection");//sohail 
    }
    int contc;
    public void Play()
    {
        SoundManager.Instance.PlayButtonClickSound();
        SceneManager.LoadScene("LevelMenu");
    }
    public void Hint()
    {
        SoundManager.Instance.PlayButtonClickSound();
        HintAnim.enabled = false;
        HintAnim.transform.localScale = Vector3.one* 1.0707f;
        GameController.Instance.rewardPlacement = "Hint_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 8;
         //   GamePlayController.Instance.HintFun(true);
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("Hint_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("Hint_reward");//sohail 
    }
    public void NextLevel()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.nextSceneAddCheck(); //for some additional work in main scripts 
        //if (SceneManager.GetActiveScene().name=="5")
        //    SceneManager.LoadScene(0);
        //else
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Close()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void ReplayBackToOrogioanPos()
    {
        GamePlayController.Instance.RePlayBtnMoveToCenter(false);
    }

    public void SpinWheelRewardClickFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "spinwheel_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 1;
            InilizationRewardedVideo.callBackObject = gameObject;
         // GamePlayController.Instance.SpinWheelShow();
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }

        //FirebaseInitialize.instance.LogEventGame("spinwheel_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("spinwheel_reward");//sohail 
    }
    public void RandomRewardClickFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "random_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 2;
            InilizationRewardedVideo.callBackObject = gameObject;
           // GamePlayController.Instance.ShowRandomWinPopup();
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("random_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("random_reward");//sohail 
    }
    public void LuckyDrawClickFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
    }
    public void SpinAgain()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "againspin_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 6;
            InilizationRewardedVideo.callBackObject = gameObject;
           // GamePlayController.Instance.SpinAfterReward(); // calling ads here
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("againspin_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("againspin_reward");//sohail 
    }
    public void BounusSkinClickFun()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "bonus_skin_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 3;
            InilizationRewardedVideo.callBackObject = gameObject;
           // GamePlayController.Instance.ShowRandomWinPopup();
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("bonus_skin_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("bonus_skin_reward");//sohail 
    }

    public void SpinWheelClose()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.AfterCloseSpinWheelPop();
        //do someAddition Work
    }
    public void Shaire()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.ShareFun();
        //do someAddition Work
    }
    public void CloseWinPopup()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.rewardWinPop.SetActive(false);
        //do someAddition Work
    }

    public void GetCoinsWithReward()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "coins_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 7;
            InilizationRewardedVideo.callBackObject = gameObject;
          //  GamePlayController.Instance.GetCoinsReward(); /// add calling
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("coins_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("coins_reward");//sohail 
    }

    public void GetPenWithReward()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "pen_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 9;
            InilizationRewardedVideo.callBackObject = gameObject;
          //  GamePlayController.Instance.BuyPen(GameController.Instance.CallingIndux);
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("pen_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("pen_reward");//sohail 
    }

    public void GetballWithReward()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "ball_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 10;
            InilizationRewardedVideo.callBackObject = gameObject;
           // GamePlayController.Instance.BuyBall(GameController.Instance.CallingIndux);
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("ball_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("ball_reward");//sohail 
    }

    public void GetbgWithReward()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "bg_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 11;
            InilizationRewardedVideo.callBackObject = gameObject;
           // GamePlayController.Instance.Buybg(GameController.Instance.CallingIndux);
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("bg_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("bg_reward");//sohail 
    }

    public void GetMoreStarReward()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GameController.Instance.rewardPlacement = "star_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.rewardIndux = 12;
            InilizationRewardedVideo.callBackObject = gameObject;
          //  GamePlayController.Instance.AfterAddStarWithRewardVideoCallBack();
            InilizationRewardedVideo.instance.CallingRewardedVideo();// uncommit
        }
        else
        {

        }
        //FirebaseInitialize.instance.LogEventGame("bg_reward");//sohail
        //GameAnalyticsSDK.GameAnalytics.NewDesignEvent("bg_reward");//sohail 
    }
}
