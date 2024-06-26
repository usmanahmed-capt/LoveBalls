using System;
using System.Collections;
using System.Collections.Generic;
//using ToastPlugin;
using UnityEngine;
using UnityEngine.Advertisements;
public class InilizationRewardedVideo : MonoBehaviour
{
    public static GameObject callBackObject;
    public static InilizationRewardedVideo instance;
    void Start()
    {
        instance = this;
    }

    public void CallingRewardedVideo()
    {
        if (InternetConnectivity._instance.CanCheckInterConnection)
        {
            InternetConnectivity._instance.CheckInternetConnection(CheckAndPlay);
        }
        else 
        {
            AdsManager.Instance.ShowRewardedVideo(GameController.Instance.rewardPlacement);
        }
        
    }
    void CheckAndPlay(bool isAvailable, Ping ping = null)
    {
        if (isAvailable)
        {
            try
            {
                //ShowRewardedVideoScript.Instance.ShowRewardedVideoButtonClicked();
                //MediationRewardedVideo.Instance.ShowRewardedVideoButtonClicked();

                //if (AdmobAdsManager.Instance.IsRewardedVideoAvaialable())
                //{
                    AdsManager.Instance.ShowRewardedVideo(GameController.Instance.rewardPlacement);

                //}
                //else
                //{
                //    StaticRef.instnace.ShowNoVideo();
                //}

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        else
        {
            if (AdmobAdsManager.Instance.IsRewardedVideoAvaialable())
            { 
                AdmobAdsManager.Instance.ShowRewardedVideo();
            }
            else
            {
             
            }
        }
    }
}
