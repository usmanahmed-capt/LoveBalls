using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class splesh : MonoBehaviour
{
    public Image spleshImageClid;
    //public RectTransform rectTransform;
    public float TimeInSecond;
    IEnumerator coroHold;
    private bool CanShowAppOpen;
    public float LoadingTime;
    public GameObject MainPage;
    void Start()
    {
        AdmobAdsManager.Instance.OnAppOpenHide.AddListener(delegate
        {
            if (gameObject.activeSelf) 
            {
                CancelInvoke(nameof(showLoadingIfApOpenNotavaible));
                 MoveToZeroXAferAddCalling();
            }
               
        });

        if (PlayerPrefs.GetInt("FirsTimeAppOpen") == 1)
        {
            CanShowAppOpen = true;
            TimeInSecond = 4.2f;
            LoadingTime = 4.5f;
        }
        else
        {
            CanShowAppOpen = true;
            TimeInSecond = 3.2f;
            LoadingTime = 3.5f;
        }

        StartFillBar();
        Invoke(nameof(EventSent),2f);
     
    }

    void EventSent() 
    {
        FirebaseInitialize.instance.LogEventGame("loading_start");//sohail
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("loading_start");//sohail 
    }
    public IEnumerator MoveOverSeconds()
    {
        if (CanShowAppOpen)
        {
            float elapsedTime = 0;
            while (elapsedTime < TimeInSecond)
            {
                elapsedTime += Time.deltaTime;
                if (AdmobAdsManager.Instance.appopenad != null)
                {
                    AdmobAdsManager.Instance.showAppOpenOnStart();
                    spleshImageClid.DOKill();
                    StopCoroutine(coroHold);
                    Invoke(nameof(showLoadingIfApOpenNotavaible),2f);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
    void showLoadingIfApOpenNotavaible()
    {
        spleshImageClid.DOFillAmount(1f, .5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            spleshImageClid.fillAmount = 1f;
            SceneLoad();
        });
    }

    void MoveToZeroX()
    {
        spleshImageClid.DOFillAmount(1f, LoadingTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            
            spleshImageClid.fillAmount = 1f;
            SceneLoad();
        });
    }

    void MoveToZeroXAferAddCalling()
    {
        spleshImageClid.DOFillAmount(1f, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            spleshImageClid.fillAmount = 1f;
            SceneLoad();
        });
    }
    public void StartFillBar()
    {
        MoveToZeroX();
        coroHold = MoveOverSeconds();
        StartCoroutine(coroHold);
    }

    void SceneLoad()
    {
        PlayerPrefs.SetInt("FirsTimeAppOpen", 1);
        gameObject.SetActive(false);
       // Debug.LogError("FirsTimeAppOpen");
        MainPage.SetActive(true);
        FirebaseInitialize.instance.LogEventGame("loading_end");//sohail
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("loading_end");//sohail 
    }
}
  

