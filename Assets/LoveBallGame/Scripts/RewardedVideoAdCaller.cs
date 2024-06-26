using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RewardedVideoAdCaller : MonoBehaviour
{


    public static bool HomeDialogActive;
    public static bool isRewardAd = false;
    // Use this for initialization

    private void Awake()
    {
            if (PlayerPrefs.GetInt("RewardAdsOnSpecialLvl") == 1)
            {
                if (PlayerPrefs.GetInt("RemoveAds") == 0 && PlayerPrefs.GetInt(gameObject.name) == 0)
                {
                  
                }
                else
                {
                   
                }
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
    }


    public void WatchRewardedVideo()
    {
        GameController.Instance.rewardPlacement = "lvlSkip_reward";
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Debug.Log("internetReachability");
            InilizationRewardedVideo.callBackObject = gameObject;
            InilizationRewardedVideo.instance.CallingRewardedVideo();
        }
        else
        {

        }
    }
    public void VideoWatches()
    {
       GiveReward();
    }

    private void GiveReward()
    {
    }
}
