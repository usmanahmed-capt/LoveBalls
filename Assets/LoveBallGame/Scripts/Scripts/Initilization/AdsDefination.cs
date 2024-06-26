
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Purchasing;
using UnityEditor;
using GameAnalyticsSDK;
public class AdsDefination : MonoBehaviour
{
    public static AdsDefination _instance;
    [Header("Unity InApp Define")]
    public InAppKeys[] InAppIds;
   // public static InAppKeys[] keys;

    void Awake()
    {
        _instance = this;
    }
    #region In App Call Back
    public  void AfterPurchased(int value)
    {
        if (InAppIds[value].supergift)
        {
            PlayerPrefs.SetInt("RemoveAds", 1);
            PlayerPrefs.SetInt("RemoveAdsOnly", 1);
            GameController.Instance.AddCoins(InAppIds[value].rewardAmount);
            GamePlayController.Instance.BuyBall(9);
            print("supergift");
            try
            {
                AdsManager.Instance.HideBannerAll();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
        else
        if (InAppIds[value].coinpack_1)
        {
            GameController.Instance.AddCoins(InAppIds[value].rewardAmount);
            print("coinpack_1");
        }
        else
        if (InAppIds[value].coinpack_2)
        {
            GameController.Instance.AddCoins(InAppIds[value].rewardAmount);
            print("coinpack_2");
        }
        else
        if (InAppIds[value].coinpack_3)
        {
            GameController.Instance.AddCoins(InAppIds[value].rewardAmount);
            print("coinpack_3");
        }
        else
        if (InAppIds[value].removeAds)
        {
            PlayerPrefs.SetInt("RemoveAds", 1);
            PlayerPrefs.SetInt("RemoveAdsOnly", 1);
            print("RemoveAds");
            try
            {
                AdsManager.Instance.HideBannerAll();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }
    #endregion

    public static void WEeeklySubBuyedExpire()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            PlayerPrefs.SetInt("subscrptionOn", 0);
        }
        Debug.Log("Illyas WEeeklySubBuyedExpire");
        if (PlayerPrefs.GetInt("RemoveAdsOnlyNonSub") == 0)
        {
            PlayerPrefs.SetInt("RemoveAdsOnly", 0);
        }
        if (PlayerPrefs.GetInt("freeHandNonSub") == 0)
        {
            PlayerPrefs.SetInt("freeHand", 0);
        }
    }
}



[System.Serializable]
public class InAppKeys
{
    public string Id;
    public ProductType Type;
    public bool supergift, coinpack_1, coinpack_2, coinpack_3,removeAds;
    public int rewardAmount;
}
