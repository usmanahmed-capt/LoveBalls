using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsSelf : MonoBehaviour
{

    public string DesignEventStart;
    public string DesignEventComplete;
    public bool isDesingEventComplateOnDisable;
    public bool isDesignEventShowingOnStart;
    private string MyStartFinger;
    private string Lvl;

    private void OnEnable()
    {
        DesignEventStartFun();
    }
    void DesignEventStartFun()
    {
        if (isDesignEventShowingOnStart)
        {
            FirebaseInitialize.instance.LogEventGame(DesignEventStart);//sohail
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent(DesignEventStart);//sohail
          //  Debug.LogError(DesignEventStart);
        }
    }


    private void OnDisable()
    {
        if (isDesingEventComplateOnDisable)
        {
            FirebaseInitialize.instance.LogEventGame(DesignEventComplete);//sohail
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent(DesignEventComplete);//sohail
          //  Debug.LogError(DesignEventComplete);
        }
    }
   
}
