using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public bool IsFistTimeClickOnMainPage;

    public Transform StorySnapingImg;
    public RenderTexture renderedTexture;
    public Camera snapshotCam;
    public int currenLvlClick;
    public int currenLvlMain;
    public float SfxSoundLevel;
    public bool CanPlayOn;
    public bool isRewraDoneRecent;
    public List<int> CurrentPlayedIndux = new List<int>();
    public List<int> CurrentPlayedInduxMain = new List<int>();
    [SerializeField]
    int _coins;
    public int Coins
    {
        get { return _coins; }
        private set { _coins = value; }
    }

    [SerializeField]
    int _star;
    internal string rewardPlacement;
    /// <summary>
    /// rewardIndux=1 spinWheel: rewardIndux=2 randomreward: rewardIndux=3 bonus skin:rewardIndux=4 spinweel skin: rewardIndux=5 spinweel pen:rewardIndux=6 spinweel pen: rewardIndux=7 coins with ad: rewardIndux=8 hint ad: rewardIndux=9 pen buy ad: rewardIndux=10 ball buy ad:
    /// rewardIndux=11 ball bg ad: rewardIndux=12 more star ad:
    /// </summary>
    [Header("RewardIndux")]
    internal int rewardIndux;


    public int Stars
    {
        get { return _star; }
        private set { _star = value; }
    }

    public int CallingIndux { get; internal set; }
    public int AfterCallingInterstatialads { get; internal set; }

    public int SkinReward;
    internal int vibrationValue;

    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    internal GameObject InterRefObj;

    public  bool GiftAvalible(string PrfesName)
    {

        return RemendingTimeSpanForGift(PrfesName).TotalSeconds <= 0;
    }

    public  TimeSpan RemendingTimeSpanForGift(string PrfesName)
    {
        return (NextGiftTimeGet(PrfesName) - DateTime.Now);
        //Debug.LogError("RemendingTimeSpanForGift");
    }
    public  DateTime NextGiftTimeGet(string PrfesName)
    {
        return DateTime.FromFileTime(long.Parse(PlayerPrefs.GetString(PrfesName, DateTime.Now.ToFileTime() + "")));
    }

    public  void NextGiftTimeSet(string PrfesName, DateTime dateTime)
    {
        PlayerPrefs.SetString(PrfesName, dateTime.ToFileTime() + "");
    }



    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
           // DontDestroyOnLoad(this.gameObject);
        }
       

      //  PlayerPrefs.SetInt("coins", 30000);
        Coins = PlayerPrefs.GetInt("coins");
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#elif UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_ANDROID
                        Debug.unityLogger.logEnabled = false;
#endif
    }

    private void Start()
    {
        GamePlayController.Instance.UpdateCoins();
    }
    public void AddCoins(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetInt("coins", Coins);
        GamePlayController.Instance.UpdateCoins();
    }

    public void RemoveCoins(int amount)
    {
        Coins -= amount;
        // Store new coin value
        PlayerPrefs.SetInt("coins", Coins);
        GamePlayController.Instance.UpdateCoins();
    }

    public void AddStar(int amount)
    {
        Stars += amount;
        PlayerPrefs.SetInt("Star", Stars);
    }

    public void UpadetStar(int amount)
    {
        Stars = amount;
    }

    public void RemoveStar(int amount)
    {
        Stars -= amount;
        // Store new coin value
        PlayerPrefs.SetInt("Star", Stars);
    }
}
