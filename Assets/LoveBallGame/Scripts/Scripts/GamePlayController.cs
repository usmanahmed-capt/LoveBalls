using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
using UnityEngine.Rendering;
using GameAnalyticsSDK;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance;
    public bool shadowLineCollides;
    public bool shadowWithCollision;
    public Animator PenAnim;
    public bool IsWin;
    public bool IsLose;
    public Image fillBar;
    public GameObject[] stars;
    public Animator[] StarsAnim;

    public ParticleSystem[] Particle;
    public int StarCount;
    public GameObject PlayUi;



    public GameObject LevComp;
    public TextMeshProUGUI LvlNumber;
    public GameObject[] RewardOnLevComp;
    public GameObject[] starsWinLvl;
    public GameObject[] bonusSkin;
    public GameObject SideBoxPanal;
    public RectTransform SideimagePanal;
    public RectTransform FullBg;
    public RectTransform HalfBg;
    public Animator AnimatorLvlComp;
    public CanvasGroup WinCanvaluseGroup;
    public Transform RePlayBtnPosToCenter;
    public Animator RePlayBtnPosToCenterAnim;
    public Transform CenterPos;
    public Transform ReplayConerPos;
    private bool CheckFalse;
    private int CounterToMoveValue;
    public Animator RplyBtnAim;
    public Animator HintBtnAnim;
    public GameObject WinStarTick;
    public Image WinPopupImage;
    public RawImage CpatuerImage;
    public Slider PenCapacity;
    public Image FillBarOfCapacity;
    public TextMeshProUGUI PenPercent;
    public Animator BackBtnAnim;

    public Image Star1;
    public Image Star2;
    public Image Star3;
    public ParticleSystem Star1part;
    public ParticleSystem Star2part;
    public ParticleSystem Star3part;

    public List<SubLvlClick> sublvlScrolls = new List<SubLvlClick>();
    public List<MainlvlSelection> mainlvlSelections = new List<MainlvlSelection>();
    private GameObject currentScaleObject;
    public GameObject CanvasGamePlay;
    public GameObject lvlSelection;
    private SubLvlClick sublvlClick;
    public GameManager MyCurrentGamemanager;
    public Transform CurrentParent;
    public GameObject Background;
    public RenderTexture renderedTexture;
    private bool AssingStarFirstTime;
    public TextMeshProUGUI StarCountTotalStarOnSelection;
    public bool CanTotatorActive;
    public TextMeshProUGUI[] CoinsText;
    [Header("pen shop Data")]
    public Sprite[] PensSp;
    public int[] penPrice;
    public bool[] IsPenBuy;
    public bool[] IsPenreward;
    public GameObject UsingBtn;
    public GameObject Use;
    public GameObject buyBtn;
    public GameObject buyBtnreward;
    internal int CurrentPenUsing;
    public TextMeshProUGUI PenPriceTxt;
    public TextMeshProUGUI PenBuyTime;
    [Header("Ball shop Data")]
    public GameObject[] BallsObject;
    public int[] BallPrice;
    public bool[] IsPBallBuy;
    public bool[] IsPBallreward;
    public GameObject UsingBtnBall;
    public GameObject UseBtnBall;
    public GameObject buyBtnBall;
    public GameObject buyBtnBallreward;
    internal int CurrentBallUsing;
    public TextMeshProUGUI BallPriceTxt;
    public TextMeshProUGUI ballBuyTime;

    [Header("Ball background Shop Data")]
    public Sprite[] bgSp;
    public int[] bgPrice;
    public bool[] bgreward;
    public bool[] IsbgBuy;
    public GameObject UsingBtnbg;
    public GameObject UseBtnbg;
    public GameObject buyBtnbg;
    public GameObject buyBtnbgreward;
    internal int CurrentbgUsing;
    public TextMeshProUGUI bgPriceTxt;
   

    public GameObject SpinWheel;
    public GameObject rewardWinPop;
    public PrizeWheel prizeWheel;
    public ParticleSystem bird1part;
    public ParticleSystem bird2part;
    public ParticleSystem heartpart;
    public ParticleSystem bird1partstart;
    public ParticleSystem bird2partstart;
    private string fileSharePath;
    string shareBody ;
    private const string AndroidRatingURI = "http://play.google.com/store/apps/details?id={0}";
    public Button hintBtn;
    public Image hintbtnimage;
    public Sprite hintSp;
    public Sprite HintGreenSp;
    public Animator requiredStarToUnlockobj;
    public Color[] PencileColor;

    private bool IsRewardAnimPlayed;
    public ScrollSnapRect scrollSnapUpScroller;
    float accumolateTime2ball;
    float accumolateTime2pen;
    public GameObject MoreStarPanal;
    public TextMeshProUGUI RequirdStartext;
    private void Awake()
    {
        Input.multiTouchEnabled = false;
        Instance = this;
        for (int i = 0; i < sublvlScrolls.Count; i++)
        {
            sublvlScrolls[i].LvlNumber = i + 1;
        }
    }

    void Start()
    {
        PlayerPrefs.SetInt("pen" + 0, 1);
        if (!PlayerPrefs.HasKey("penCurrentUsing"))
            PlayerPrefs.SetInt("penCurrentUsing", 0);
        PlayerPrefs.SetInt("ball" + 0, 1);
        if (!PlayerPrefs.HasKey("ballCurrentUsing"))
            PlayerPrefs.SetInt("ballCurrentUsing", 0);


        PlayerPrefs.SetInt("bg" + 0, 1);
        if (!PlayerPrefs.HasKey("bgCurrentUsing"))
            PlayerPrefs.SetInt("bgCurrentUsing", 0);

        CheckIsPenBuy();
        CurrentPenUsing = PlayerPrefs.GetInt("penCurrentUsing", 0);
        ChangeBtnStateOfPen(true, CurrentPenUsing);
        CheckIsBallBuy();
        CurrentBallUsing = PlayerPrefs.GetInt("ballCurrentUsing", 0);
        ChangeBtnStateOfBall(true, CurrentBallUsing);
        CheckIsbgBuy();
        CurrentbgUsing = PlayerPrefs.GetInt("bgCurrentUsing", 0);
        ChangeBtnStateOfbg(true, CurrentbgUsing);
        StarUpdateFun();
        AppliedImagesIfPlayed();
        shareBody = AndroidRatingURI.Replace("{0}", Application.identifier);

        SoundManager.Instance.BgSoundHigh();
    }

    void AppliedImagesIfPlayed() 
    {
        for (int i = 0; i < sublvlScrolls.Count; i++)
        {
            sublvlScrolls[i].AppliedFirstTimeImages();
        }
        for (int i = 0; i < mainlvlSelections.Count; i++)
        {
            mainlvlSelections[i].AppliedFirstTimeImages();
        }
    }
    #region Penshop

    internal void CheckIsPenBuy()
    {
        for (int i = 0; i < IsPenBuy.Length; i++)
        {
            if (PlayerPrefs.GetInt("pen" + i) == 1)
            {
                IsPenBuy[i] = true;
            }
            else
            {
                if (i != 0)
                    IsPenBuy[i] = false;
            }
        }
    }
    internal void RestPenState(int PenRestIndux)
    {
        PlayerPrefs.SetInt("pen" + PenRestIndux, 0);
        CheckIsPenBuy();
    }
    internal void BuyPen(int BuyAbleIndux)
    {
        PlayerPrefs.SetInt("pen" + BuyAbleIndux, 1);
        PlayerPrefs.SetInt("penCurrentUsing", BuyAbleIndux);
        CurrentPenUsing = BuyAbleIndux;
        CheckIsPenBuy();
        TimerSatOfCurrentItemBuy("penTimer" + BuyAbleIndux);
        ChangeBtnStateOfPen(true, BuyAbleIndux);

    }

    internal void ChangeBtnStateOfPen(bool isUsing,int indux)
    {
        if (isUsing)
        {
            if (CurrentPenUsing == indux)
            {
                Use.SetActive(false);
                buyBtn.SetActive(false);
                UsingBtn.SetActive(true);
                buyBtnreward.SetActive(false);

                accumolateTime2pen = (float)GameController.Instance.RemendingTimeSpanForGift("penTimer" + indux).TotalSeconds;
                if (accumolateTime2pen > 0)
                {
                    currentIndux = indux;
                    PenBuyTime.gameObject.SetActive(true);
                    if (IsInvoking("TimeIncrease2"))
                        CancelInvoke("TimeIncrease2");

                    InvokeRepeating("TimeIncrease2", 0f, 1f);
                }
            }
            else 
            {
                Use.SetActive(true);
                buyBtn.SetActive(false);
                UsingBtn.SetActive(false);
                buyBtnreward.SetActive(false);

                accumolateTime2pen = (float)GameController.Instance.RemendingTimeSpanForGift("penTimer" + indux).TotalSeconds;
                if (accumolateTime2pen > 0)
                {
                    currentIndux = indux;
                    PenBuyTime.gameObject.SetActive(true);
                    if (IsInvoking("TimeIncrease2"))
                        CancelInvoke("TimeIncrease2");

                    InvokeRepeating("TimeIncrease2", 0f, 1f);
                }
            }
           
        }
        else
        {
            PenPriceTxt.text = penPrice[indux].ToString();
            if (IsPenreward[indux])
            {
                buyBtn.SetActive(false);
                buyBtnreward.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(true);
                buyBtnreward.SetActive(false);
            }
           // buyBtn.SetActive(true);
            UsingBtn.SetActive(false);
            Use.SetActive(false); 
            PenBuyTime.gameObject.SetActive(false);

        }

    }
    internal void BuyPenWithCoins(int BuyAablePriceIndux)
    {
        if (penPrice[BuyAablePriceIndux] <= GameController.Instance.Coins) 
        {
            GameController.Instance.RemoveCoins(penPrice[BuyAablePriceIndux]);
            BuyPen(BuyAablePriceIndux);
        }
    }
    public void UsingPen(int BuyAablePriceIndux)
    {
         CurrentPenUsing = BuyAablePriceIndux;
        ChangeBtnStateOfPen(true, CurrentPenUsing);
    }

    #endregion


    #region Ballshop

    internal void CheckIsBallBuy()
    {
        for (int i = 0; i < IsPBallBuy.Length; i++)
        {
            if (PlayerPrefs.GetInt("ball" + i) == 1)
            {
                IsPBallBuy[i] = true;
            }
            else 
            {
                if(i!=0)
                    IsPBallBuy[i] = false;
            }
        }
    }
    internal void RestBallState(int PenRestIndux)
    {
        PlayerPrefs.SetInt("ball" + PenRestIndux, 0);
        CheckIsBallBuy();
    }
    internal void BuyBall(int BuyAbleIndux)
    {
        PlayerPrefs.SetInt("ball" + BuyAbleIndux, 1);
        PlayerPrefs.SetInt("ballCurrentUsing", BuyAbleIndux);
        CurrentBallUsing = BuyAbleIndux;
        CheckIsBallBuy();
        TimerSatOfCurrentItemBuy("ballTimer" + BuyAbleIndux);
        ChangeBtnStateOfBall(true, BuyAbleIndux);
        for (int i = 0; i < sublvlScrolls.Count; i++)
        {
            sublvlScrolls[i].Udpateball();
        }
        for (int i = 0; i < mainlvlSelections.Count; i++)
        {
            mainlvlSelections[i].Udpateball();
        }
    }
    private int currentIndux;
    internal void ChangeBtnStateOfBall(bool isUsing, int indux)
    {
        if (isUsing)
        {
            if (CurrentBallUsing == indux)
            {
                UseBtnBall.SetActive(false);
                buyBtnBall.SetActive(false);
                UsingBtnBall.SetActive(true);
                buyBtnBallreward.SetActive(false);
                accumolateTime2ball = (float)GameController.Instance.RemendingTimeSpanForGift("ballTimer" + indux).TotalSeconds;
                   if (accumolateTime2ball > 0)
                    {
                        currentIndux = indux;
                        ballBuyTime.gameObject.SetActive(true);
                        if (IsInvoking("TimeIncrease"))
                            CancelInvoke("TimeIncrease");

                        InvokeRepeating("TimeIncrease", 0f, 1f);
                    }
            }
            else 
            {
                UseBtnBall.SetActive(true);
                buyBtnBall.SetActive(false);
                UsingBtnBall.SetActive(false);
                buyBtnBallreward.SetActive(false);
                accumolateTime2ball = (float)GameController.Instance.RemendingTimeSpanForGift("ballTimer" + indux).TotalSeconds;
                if (accumolateTime2ball > 0)
                {
                    currentIndux = indux;
                    ballBuyTime.gameObject.SetActive(true);
                    if (IsInvoking("TimeIncrease"))
                        CancelInvoke("TimeIncrease");
                    InvokeRepeating("TimeIncrease", 0f, 1f);
                }
            }
        }
        else
        {
            BallPriceTxt.text = BallPrice[indux].ToString();
            if (IsPBallreward[indux])
            {
                buyBtnBall.SetActive(false);
                buyBtnBallreward.SetActive(true);
            }
            else
            {
                buyBtnBall.SetActive(true);
                buyBtnBallreward.SetActive(false);
            }
           // buyBtnBall.SetActive(true);
            UsingBtnBall.SetActive(false);
            UseBtnBall.SetActive(false);
            ballBuyTime.gameObject.SetActive(false);
        }
    }

    void TimerSatOfCurrentItemBuy(string Name) 
    {
        DateTime startTimeoo = System.DateTime.Now;
        GameController.Instance.NextGiftTimeSet(Name, startTimeoo.AddDays(1));
    }

    internal void UpdateTimer(string PrefeForTime) 
    {
        accumolateTime2ball = (float)GameController.Instance.RemendingTimeSpanForGift(PrefeForTime).TotalSeconds;
    }

    void TimeIncrease() 
    {
        accumolateTime2ball--;
        if (accumolateTime2ball<=0) 
        {
            PlayerPrefs.SetInt("ball" + currentIndux, 0);
            CheckIsBallBuy();
            ChangeBtnStateOfBall(false, currentIndux);
            CancelInvoke("TimeIncrease");
        }
        // Calculate hours, minutes, and seconds
        float hours = Mathf.FloorToInt(accumolateTime2ball / 3600);
        float minutes = Mathf.FloorToInt((accumolateTime2ball % 3600) / 60);
        float seconds = accumolateTime2ball % 60; // Calculate seconds directly
        // Display time with leading zeros
        ballBuyTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    void TimeIncrease2()
    {
        accumolateTime2pen--;
        if (accumolateTime2pen <= 0)
        {
            PlayerPrefs.SetInt("pen" + currentIndux, 0);
            CheckIsPenBuy();
            ChangeBtnStateOfPen(false, currentIndux);
            CancelInvoke("TimeIncrease2");
        }
        // Calculate hours, minutes, and seconds
        float hours = Mathf.FloorToInt(accumolateTime2pen / 3600);
        float minutes = Mathf.FloorToInt((accumolateTime2pen % 3600) / 60);
        float seconds = accumolateTime2pen % 60; // Calculate seconds directly
        // Display time with leading zeros
        PenBuyTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    internal void BuyBallWithCoins(int BuyAablePriceIndux)
    {
        if (BallPrice[BuyAablePriceIndux] <= GameController.Instance.Coins)
        {
            GameController.Instance.RemoveCoins(BallPrice[BuyAablePriceIndux]);
            BuyBall(BuyAablePriceIndux);
        }
    }
    public void UsingBall(int BuyAablePriceIndux)
    {
        CurrentBallUsing = BuyAablePriceIndux;
        ChangeBtnStateOfBall(true, CurrentBallUsing);
    }

    #endregion


    #region bgshop

    internal void CheckIsbgBuy()
    {
        for (int i = 0; i < IsbgBuy.Length; i++)
        {
            if (PlayerPrefs.GetInt("bg" + i) == 1)
            {
                IsbgBuy[i] = true;
            }
        }
    }
    internal void RestbgState(int PenRestIndux)
    {
        PlayerPrefs.SetInt("bg" + PenRestIndux, 0);
        CheckIsbgBuy();
    }
    internal void Buybg(int BuyAbleIndux)
    {
        PlayerPrefs.SetInt("bg" + BuyAbleIndux, 1);
        PlayerPrefs.SetInt("bgCurrentUsing", BuyAbleIndux);
        CurrentbgUsing = BuyAbleIndux;
        CheckIsbgBuy();
        ChangeBtnStateOfbg(true, BuyAbleIndux);
        for (int i = 0; i < mainlvlSelections.Count; i++)
        {
            mainlvlSelections[i].Udpatebg();
        }

        for (int i = 0; i < sublvlScrolls.Count; i++)
        {
            sublvlScrolls[i].Udpatebg();
        }
    }

    internal void ChangeBtnStateOfbg(bool isUsing, int indux)
    {
        if (isUsing)
        {
            if (CurrentBallUsing == indux)
            {
                UseBtnbg.SetActive(false);
                buyBtnbg.SetActive(false);
                UsingBtnbg.SetActive(true);
                buyBtnbgreward.SetActive(false);
            }
            else 
            {
                UseBtnbg.SetActive(true);
                buyBtnbg.SetActive(false);
                UsingBtnbg.SetActive(false);
                buyBtnbgreward.SetActive(false);
            }
        }
        else
        {
            bgPriceTxt.text = bgPrice[indux].ToString();
            if (bgreward[indux])
            {
                buyBtnbg.SetActive(false);
                buyBtnbgreward.SetActive(true);
            }
            else 
            {
                buyBtnbg.SetActive(true);
                buyBtnbgreward.SetActive(false);
            }
            UseBtnbg.SetActive(false);
            UsingBtnbg.SetActive(false);

        }

    }
    internal void BuybgWithCoins(int BuyAablePriceIndux)
    {
        if (bgPrice[BuyAablePriceIndux] <= GameController.Instance.Coins)
        {
            GameController.Instance.RemoveCoins(bgPrice[BuyAablePriceIndux]);
            Buybg(BuyAablePriceIndux);
        }
    }
    public void Usingbg(int BuyAablePriceIndux)
    {
        CurrentbgUsing = BuyAablePriceIndux;
        ChangeBtnStateOfbg(true, CurrentbgUsing);
    }

    #endregion

    public void StarUpdateFun() 
    {
        int TotalLvlPrStar = sublvlScrolls.Count;
        TotalLvlPrStar = TotalLvlPrStar * 3;
        StarCountTotalStarOnSelection.text = PlayerPrefs.GetInt("Stars").ToString() + "/"+ TotalLvlPrStar.ToString();
        GameController.Instance.UpadetStar(PlayerPrefs.GetInt("Stars"));

    }

    public void StarUpdateAfterAds()
    {
        PlayerPrefs.SetInt("Stars", PlayerPrefs.GetInt("Stars") + 3);
        int TotalLvlPrStar = sublvlScrolls.Count;
        TotalLvlPrStar = TotalLvlPrStar * 3;
        StarCountTotalStarOnSelection.text = PlayerPrefs.GetInt("Stars").ToString() + "/" + TotalLvlPrStar.ToString();
        GameController.Instance.UpadetStar(PlayerPrefs.GetInt("Stars"));
    }

    public void AfterAddStarWithRewardVideoCallBack() 
    {
        StarUpdateAfterAds();
        MoreStarPanal.SetActive(false);
    }

    public void DisableStarOfBar(int indux) 
    {
        if (indux == 3)
        {
            if (Star3.gameObject.activeSelf)
            {
                if(GameController.Instance.vibrationValue==0)
                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

                Star3.gameObject.SetActive(false);
                Star3part.Play();
            }
        }
        if (indux == 2)
        {
            if (Star2.gameObject.activeSelf)
            {
                if (GameController.Instance.vibrationValue == 0)
                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                Star2.gameObject.SetActive(false);
                Star2part.Play();
            }
        }
        if (indux == 1)
        {
            if (Star1.gameObject.activeSelf)
            {
                if (GameController.Instance.vibrationValue == 0)
                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                Star1.gameObject.SetActive(false);
                Star1part.Play();
            }
        }
    }
    public void StartFormNewlvl() 
    {
        IsWin = false;
        IsLose = false;
        MyCurrentGamemanager.penRendere.sprite = PensSp[CurrentPenUsing];
        MyCurrentGamemanager.PaperBg.sprite = bgSp[CurrentbgUsing];
        for (int i = 0; i < MyCurrentGamemanager.BallsLineRendere.Length; i++)
        {
            MyCurrentGamemanager.BallsLineRendere[i].enabled = true;
        }
        FillBarOfCapacity.fillAmount = 1f;
        PenCapacity.value = 1f;
        StarCount = 3;
        PenPercent.text ="100 %";
        Star3.gameObject.SetActive(true);
        Star2.gameObject.SetActive(true);
        Star1.gameObject.SetActive(true);
        MyCurrentGamemanager.ActiveAll();
        MyCurrentGamemanager.CanDrawPen = true;
        GameController.Instance.CanPlayOn = true;
        HintBtnAnim.enabled = false;
        CancelInvoke("ReplayMoveToCenterCeckFun");
        RplyBtnAim.enabled = false;
        StarCheckingReplay();
        if (MyCurrentGamemanager.IsBannerOnThislvl) 
        {
            AdsManager.Instance.ShowBannerBottom();
        }

        if (GameController.Instance.currenLvlClick == 1)
        {
            if (PlayerPrefs.GetInt("toturial") == 0)
            {
                hintBtn.interactable = false;
                hintbtnimage.sprite = hintSp;
                HintFun(true);
            }
            else
            {
                hintBtn.interactable = true;
                hintbtnimage.sprite = HintGreenSp;
            }
        }
        else 
        {
            hintBtn.interactable = true;
            hintbtnimage.sprite = HintGreenSp;
        }
       
        bird1partstart.transform.position = new Vector3(MyCurrentGamemanager.Balls[0].transform.localPosition.x,
    MyCurrentGamemanager.Balls[0].transform.localPosition.y + 0.28f, 0f);
        bird2partstart.transform.position = new Vector3(MyCurrentGamemanager.Balls[1].transform.localPosition.x,
           MyCurrentGamemanager.Balls[1].transform.localPosition.y + 0.28f, 0f);
        bird1partstart.gameObject.SetActive(true);
        bird2partstart.gameObject.SetActive(true);
        SoundManager.Instance.PlayParticleSound();
        bird1partstart.Play();
        bird2partstart.Play();
        Invoke("HintBtnAimPlayed",1f);
    }
    void HintBtnAimPlayed() 
    {
        if (IsRewardAnimPlayed)
        {
            if (!HintBtnAnim.enabled)
                HintBtnAnim.enabled = true;
            IsRewardAnimPlayed = false;
        }
    }

    public  void StarCheckingReplay() 
    {
        InvokeRepeating("ReplayMoveToCenterCeckFun", 0, 1f);
    }
    void ReplayMoveToCenterCeckFun() 
    {
        if (!IsWin && !IsLose)
        {
            //if (!CheckFalse)
            //    CounterToMoveValue = 0;

            if (PenCapacity.value <= 0)
            {
                if (!RplyBtnAim.enabled)
                    RplyBtnAim.enabled = true;
            }

            if (PenCapacity.value <= 0 /*&& CheckFalse*/)
            {

                CounterToMoveValue++;
                if (CounterToMoveValue >= 5)
                {
                    CounterToMoveValue = 0;
                    RePlayBtnMoveToCenter(true);
                    RePlayBtnPosToCenterAnim.enabled = false;
                    CancelInvoke("ReplayMoveToCenterCeckFun");
                }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            CheckFalse = false;

        if (Input.GetMouseButtonUp(0))
            CheckFalse = true;

    }

    public void ReducedStar(float FillBarValue) 
    {
        if (FillBarValue < .71f && FillBarValue > .69f)
        {
            if(!StarsAnim[2].enabled)
                StarsAnim[2].enabled = true;
        }

        if (FillBarValue < .41f && FillBarValue > .39f)
        {
            if (!StarsAnim[1].enabled)
                StarsAnim[1].enabled = true;
        }
    }
    public void RestartFun()
    {
        LevComp.SetActive(false);
        requiredStarToUnlockobj.gameObject.SetActive(false);
        HintFun(false);
        MyCurrentGamemanager.ResetAll();
        CanvasGamePlay.SetActive(false);
        Background.SetActive(true);
        MyCurrentGamemanager.ShowBackgroundImage(true);
        lvlSelection.SetActive(false);
        CanvasGamePlay.SetActive(true);
        PlayUi.SetActive(true);
        PenCapacity.value = 1f;
        PenCapacity.value = 1f;
        RePlayBtnPosToCenter.gameObject.SetActive(false);
        Invoke(nameof(StartFormNewlvl), .1f);


        //for (int i = 0; i < sublvlScrolls.Count; i++)
        //{
        //    if (GameController.Instance.currenLvlClick == sublvlScrolls[i].LvlNumber)
        //    {
        //        sublvlClick = sublvlScrolls[i];
        //        ShowLvlFun_2(sublvlScrolls[i].ScaleObject);
        //        break;
        //    }
        //}
    }


    public void WinFun()
    {
        if (!IsWin)
        {
            HintFun(false);
            IsWin = true;
            CancelInvoke("ReplayMoveToCenterCeckFun");
            //  CamerAClicActionBtn.onClick.Invoke();
            GameManager.Instance.StopAllPhysics();
            PlayUi.SetActive(false);
            for (int i = 0; i < starsWinLvl.Length; i++)
            {
                starsWinLvl[i].SetActive(false);
            }
            bird1part.transform.position= MyCurrentGamemanager.Balls[0].transform.localPosition;
            bird2part.transform.position = MyCurrentGamemanager.Balls[1].transform.localPosition;
            bird1part.Play();
            bird2part.Play();
            for (int i = 0; i < MyCurrentGamemanager.Balls.Length; i++)
            {
                MyCurrentGamemanager.ballsScripts[i].charSorting.WinExpressionOfBird();
            }
            SoundManager.Instance.PlayBirtMeetSound();
            if (PlayerPrefs.GetInt("lvl" + GameController.Instance.currenLvlClick, 0) == 0)
            {
                AssingStarFirstTime = true;
                PlayerPrefs.SetInt("lvl" + GameController.Instance.currenLvlClick, StarCount);
                PlayerPrefs.SetInt("Stars", PlayerPrefs.GetInt("Stars") + StarCount);
                GameController.Instance.AddCoins(15);
                StarUpdateFun();
            }
            else if(PlayerPrefs.GetInt("lvl" + GameController.Instance.currenLvlClick, 0)< StarCount)
            {
                AssingStarFirstTime = true;
                int NewStarGain = StarCount - PlayerPrefs.GetInt("lvl" + GameController.Instance.currenLvlClick, 0);
                PlayerPrefs.SetInt("lvl" + GameController.Instance.currenLvlClick, StarCount);
                PlayerPrefs.SetInt("Stars", PlayerPrefs.GetInt("Stars") + NewStarGain);
                GameController.Instance.AddCoins(15);
                StarUpdateFun();
            }

            //  PlayerPrefs.SetInt((SceneManager.GetActiveScene().buildIndex).ToString(), StarCount);
           
            StartCoroutine(CaptureCollisionMoment());
        }
    }
    
 
    private IEnumerator CaptureCollisionMoment()
    {
        yield return new WaitForSeconds(.1f);
        GameController.Instance.snapshotCam.Render();
        RenderTexture.active = GameController.Instance.snapshotCam.targetTexture;
        Texture2D texture2D = new Texture2D(GameController.Instance.snapshotCam.targetTexture.width, GameController.Instance.snapshotCam.targetTexture.height, TextureFormat.ARGB32, false,true);
        texture2D.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0,false);
        texture2D.Apply();
        RenderTexture.active = null;
        //  Assume areaWidth and areaHeight are set beforehand
        string filePath = Application.persistentDataPath + "/lvl" + GameController.Instance.currenLvlClick + ".png";
        fileSharePath = filePath;
        yield return null;
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        //if (File.Exists(filePath))
        //{
        //    byte[] bytess = File.ReadAllBytes(filePath);
        //    capturedImage2 = new Texture2D(GameController.Instance.renderedTexture.width, GameController.Instance.renderedTexture.height, TextureFormat.ARGB32, false);
        //    capturedImage2.LoadImage(bytes);
        //}

        // Convert Texture2D to Sprite
        //   Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        CpatuerImage.texture = texture2D;
        StartCoroutine(WinShowWithDelay());
    }
 
    public void ShareFun()
    {
        AdmobAdsManager.blockAppOpen = true;
        new NativeShare().AddFile(fileSharePath).SetSubject(shareBody).SetText(shareBody).Share();
        Invoke(nameof(ResetAppOpen), 1);
    }
    void ResetAppOpen()
    {
        AdmobAdsManager.blockAppOpen = false;
    }
    IEnumerator WinShowWithDelay()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < Particle.Length; i++)
        {
            Particle[i].Play();
        }
        SoundManager.Instance.PlayGameWinSound();
        yield return new WaitForSeconds(.5f);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_" + GameController.Instance.currenLvlClick);//sohail
        FirebaseInitialize.instance.LogEventGame("level_" + GameController.Instance.currenLvlClick + "_complete");//sohail
        yield return new WaitForSeconds(.5f);//1.8f
        for (int i = 0; i < RewardOnLevComp.Length; i++)
        {
            RewardOnLevComp[i].SetActive(false);
        }
        WinStarTick.SetActive(false);
        WinCanvaluseGroup.alpha = 1f;
        LvlNumber.text = "Level "+GameController.Instance.currenLvlClick;
        IsaddApperaOnceOnNextBnt = false;
        SoundManager.Instance.BgSoundLow();
       
        LevComp.SetActive(true);
        BackBtnAnim.enabled = false;
        AnimatorLvlComp.SetTrigger("lvl");
        FullBg.gameObject.SetActive(true);
        HalfBg.gameObject.SetActive(false);
        HalfBg.anchoredPosition = new Vector3(-173.0501f, 7.880005f, 0f);
        SideimagePanal.anchoredPosition = new Vector3(-176f, 7.880005f, 0f);
        SideBoxPanal.SetActive(true);
        PlayerPrefs.SetInt("toturial", 1);
        if (AssingStarFirstTime)
        {
            // Assuming you have a variable to store your value
            int yourValue = GameController.Instance.currenLvlClick;
            // Call the function to get the corresponding number based on the value
            int result = GetNumber(yourValue);
            PlayerPrefs.SetInt("lvlMain" + result, PlayerPrefs.GetInt("lvlMain" + result)+StarCount);
            // Print the result to the console (you can use it as needed in your game)
            Debug.Log("ResultMainStar: " + result);
            AssingStarFirstTime = false;
        }

        int RewardIndux = UnityEngine.Random.Range(0, RewardOnLevComp.Length);
        RewardOnLevComp[RewardIndux].SetActive(true);

      
        if (RewardIndux == 2) 
        {
            for (int i = 0; i < bonusSkin.Length; i++)
            {
                bonusSkin[i].SetActive(false);
            }

            GameController.Instance.SkinReward = UnityEngine.Random.Range(0, 3);

            bonusSkin[GameController.Instance.SkinReward].SetActive(true);
        }
        yield return new WaitForSeconds(.5f);
        SoundManager.Instance.PlayPopupWinSound();
        if (StarCount > 2)
        {
            for (int i = 0; i < starsWinLvl.Length; i++)
            {
                starsWinLvl[i].SetActive(true);
                yield return new WaitForSeconds(.4f);
            }
        }
        else if (StarCount > 1)
        {
            for (int i = 0; i < starsWinLvl.Length - 1; i++)
            {
                starsWinLvl[i].SetActive(true);
                yield return new WaitForSeconds(.4f);
            }
        }
        else if (StarCount > 0)
        {
            for (int i = 0; i < starsWinLvl.Length - 2; i++)
            {
                starsWinLvl[i].SetActive(true);
            }
        }
       
        yield return new WaitForSeconds(.4f);
        WinStarTick.SetActive(true);
    }
    // Function to get the corresponding number based on the value
    int GetNumber(int value)
    {
        // Define the range size
        int rangeSize = 5;

        // Calculate the corresponding number based on the value and range size
        int result = Mathf.CeilToInt((float)value / rangeSize);

        return result;
    }
    public void LoseFun()
    {
        HintFun(false);
        IsLose = true;
        SoundManager.Instance.PlayGameOverSound();
        CancelInvoke("ReplayMoveToCenterCeckFun");
        for (int i = 0; i < MyCurrentGamemanager.Balls.Length; i++)
        {
            MyCurrentGamemanager.Balls[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            MyCurrentGamemanager.ballsScripts[i].charSorting.LoseExpressionofBird();
        }
        MyCurrentGamemanager.StopAllPhysics();
        IsRewardAnimPlayed = true;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level_" + GameController.Instance.currenLvlClick);//sohail
        FirebaseInitialize.instance.LogEventGame("level_" + GameController.Instance.currenLvlClick + "_failed");//sohail
        RePlayBtnPosToCenter.gameObject.SetActive(false);
        CanvasGamePlay.SetActive(false);
        if (AdsManager.ShowAd)
        {
            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
            {
                Invoke(nameof(AdsonRestart), .5f);
            }
            else
            {
                Invoke(nameof(RestartFun), .75f);
            }
        }
        else
        {
            Invoke(nameof(RestartFun), .75f);
        }
    }
      void AdsonRestart() 
        {
        GameController.Instance.AfterCallingInterstatialads = 1;
        AdsManager.Instance.ShowVideoAdOnTimer_2("replay");
        }

    internal void RestarAfterAdsFunCalling()
    {
        RestartFun();
    }
    private bool IsaddApperaOnceOnNextBnt;
    public void nextSceneAddCheck()
    {
        CancelInvoke("ReplayMoveToCenterCeckFun");
        if (GameController.Instance.currenLvlClick > 1)
        {
            //if (GameController.Instance.currenLvlClick % 2 != 0)
            //{
                if (!GameController.Instance.isRewraDoneRecent)
                {
                    if (!IsaddApperaOnceOnNextBnt)
                    {
                        if (AdsManager.ShowAd)
                        {
                            if (AdmobAdsManager.Instance.HasInterstitialAvailable())
                            {
                                if (GameController.Instance.currenLvlClick == 2)
                                {
                                    GameController.Instance.AfterCallingInterstatialads = 5;
                                    AdsManager.Instance.ShowVideoAdOnTimer_2AdmobFirst("levelWin");
                                }
                                else 
                                {
                                    GameController.Instance.AfterCallingInterstatialads = 5;
                                    AdsManager.Instance.ShowVideoAdOnTimer_2("levelWin");
                                }
                            }
                            else
                            {
                                NextSeneFun();
                            }
                        //if ((AdmobAdsManager.Instance.HasInterstitialAvailable() || MediationInterstitial.Instance.IsInterstitialAvailable()))
                        //    {
                        //        AdsManager.Instance.ShowInterstitialFixedInGameOutercheck("levelWin");
                        //        Invoke(nameof(NextSeneFun), .5f);
                        //    }
                        //    else 
                        //    {
                        //        NextSeneFun();
                        //    }
                        }
                        else
                        {
                            NextSeneFun();
                        }
                    }
                    else
                    {
                        NextSeneFun();
                    }
                }
                else 
                {
                    NextSeneFun();
                }
        }
        else 
        {
            NextSeneFun();
        }
        GameController.Instance.isRewraDoneRecent = false;
    }
    public void NextSeneFun()
    {
        IsaddApperaOnceOnNextBnt = true;
        CancelInvoke("ReplayMoveToCenterCeckFun");
        int lvl = GameController.Instance.currenLvlClick;
        lvl++;

        if (lvl < 21)
        {
            for (int i = 0; i < sublvlScrolls.Count; i++)
            {
                if (lvl == sublvlScrolls[i].LvlNumber)
                {
                    sublvlClick = sublvlScrolls[i];
                    if (sublvlClick.RequiredStar <= GameController.Instance.Stars)
                    {

                    }
                    else
                    {
                        requiredStarToUnlockobj.gameObject.SetActive(true);
                        requiredStarToUnlockobj.Rebind();
                        if (!requiredStarToUnlockobj.enabled)
                            requiredStarToUnlockobj.enabled = true;

                        BackBtnAnim.enabled = true;
                        return;
                    }
                    break;
                }
            }
            LevComp.SetActive(false);
            requiredStarToUnlockobj.gameObject.SetActive(false);
            GameController.Instance.CurrentPlayedIndux.Add(GameController.Instance.currenLvlClick);
            GameController.Instance.CurrentPlayedInduxMain.Add(GameController.Instance.currenLvlClick);

            for (int i = 0; i < sublvlScrolls.Count; i++)
            {
                if (GameController.Instance.currenLvlClick == sublvlScrolls[i].LvlNumber)
                {
                        HideCurrentLvlFun(sublvlScrolls[i].ScaleObject, sublvlScrolls[i]);
                    break;
                }
            }
            GameController.Instance.currenLvlClick++;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_" + GameController.Instance.currenLvlClick);//sohail
            FirebaseInitialize.instance.LogEventGame("level_" + GameController.Instance.currenLvlClick + "_start");//sohail
            for (int i = 0; i < sublvlScrolls.Count; i++)
            {
                if (GameController.Instance.currenLvlClick == sublvlScrolls[i].LvlNumber)
                {
                    sublvlClick = sublvlScrolls[i];
                if (sublvlClick.RequiredStar <= GameController.Instance.Stars)
                {
                    ShowLvlFun_2(sublvlScrolls[i].ScaleObject);
                }
                else
                    {
                        
                        requiredStarToUnlockobj.gameObject.SetActive(true);
                        requiredStarToUnlockobj.Rebind();
                        if (!requiredStarToUnlockobj.enabled)
                            requiredStarToUnlockobj.enabled = true;

                    }
                    break;
                }
            }
        }
        else
        {
            LevComp.SetActive(false);
            GameController.Instance.currenLvlClick = 20;
           // Debug.LogError("HidCurrentLvl");
            CanvasGamePlay.SetActive(false);
            GamePlayController.Instance.HidCurrentLvl();
            FirebaseInitialize.instance.LogEventGame("level_" + (GameController.Instance.currenLvlClick-1) + "to_lvlslection");//sohail
            GameAnalyticsSDK.GameAnalytics.NewDesignEvent("level_" + (GameController.Instance.currenLvlClick - 1) + "to_lvlslection");//sohail 
        }
        //Page Swipe Animation
    }

    public void HintFun(bool CanHintTrue)
    {
        if (!IsWin && !IsLose)
        {
            if (CanHintTrue)
            {
                for (int i = 0; i < MyCurrentGamemanager.Hint.Length; i++)
                {
                    MyCurrentGamemanager.Hint[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < MyCurrentGamemanager.Hint.Length; i++)
                {
                    MyCurrentGamemanager.Hint[i].SetActive(false);
                }
            }
        }
    }
    public void RePlayBtnMoveToCenter(bool ISCenterMove) 
    {
        if (ISCenterMove)
        {
            RePlayBtnPosToCenter.gameObject.SetActive(true);
            RePlayBtnPosToCenter.position = ReplayConerPos.position;
            StartCoroutine(MoveOverSpeed(RePlayBtnPosToCenter, CenterPos.position, 15f,false));
        }
        else 
        {
            RePlayBtnPosToCenter.position = CenterPos.position;
            StartCoroutine(MoveOverSpeed(RePlayBtnPosToCenter, ReplayConerPos.position, 15f,true));
        }
    }
   

    public IEnumerator MoveOverSpeed(Transform objectToMove, Vector3 end, float speed,bool CanEndDisable)
    {
         while (objectToMove.position != end)
               {
                objectToMove.position = Vector3.MoveTowards(objectToMove.position, end, speed * Time.deltaTime);
                   yield return new WaitForEndOfFrame();
               }
                objectToMove.position = end;
        if (CanEndDisable)
        {
            RePlayBtnPosToCenterAnim.enabled = false;
            objectToMove.gameObject.SetActive(false);
            StarCheckingReplay();
        }
        else 
        {
            // Debug.LogError("RebindAnim");
            RePlayBtnPosToCenterAnim.enabled = true;
            RePlayBtnPosToCenterAnim.Rebind();

        }
    }


    //public void ShowCurrentLvl(Transform) 
    //{

    //}

    public void ShowCurrentlvl()
    {
        for (int i = 0; i < sublvlScrolls.Count; i++)
        {
            if (GameController.Instance.currenLvlClick == sublvlScrolls[i].LvlNumber) 
            {
                sublvlClick = sublvlScrolls[i];
                //   ShowLvlFun_2(sublvlScrolls[i].ScaleObject);
                StartCoroutine(ShowLvlFun(sublvlScrolls[i].ScaleObject, .2f));
                break;
            }
        }
    }
    public void BackSnapPos ()
    {
        RePlayBtnPosToCenterAnim.gameObject.SetActive(false);
        RePlayBtnPosToCenterAnim.enabled = false;
        scrollSnapUpScroller.PageInduxCallingFun =(int)(GameController.Instance.currenLvlClick / 5f);
    }
    public void HidCurrentLvl()
    {
        GameController.Instance.CanPlayOn = false;
        LevComp.SetActive(false);
        requiredStarToUnlockobj.gameObject.SetActive(false);
        HintFun(false);
        for (int i = 0; i < sublvlScrolls.Count; i++)
        {
            if (GameController.Instance.currenLvlClick == sublvlScrolls[i].LvlNumber)
            {
                sublvlClick = sublvlScrolls[i];
                StartCoroutine(HideCurrentLvl(sublvlScrolls[i].ScaleObject, .2f));
                break;
            }
        }
    }
    public IEnumerator ShowLvlFun(Transform objectToMove, float speed)
    {
        objectToMove.GetComponent<SortingGroup>().enabled = false;
        MyCurrentGamemanager = objectToMove.transform.GetChild(0).GetComponent<GameManager>();
        currentScaleObject = objectToMove.gameObject;
        CurrentParent = currentScaleObject.transform.parent;
        currentScaleObject.transform.parent = null;
        sublvlClick.ScreenImageObject.SetActive(false);
        // Get the initial distance to move and scale proportionally
        float initialDistance = Vector3.Distance(objectToMove.position, Vector3.zero);
        float initialScale = objectToMove.localScale.x; // Assuming uniform scaling

        //while (objectToMove.position != Vector3.zero)
        //{
        //    // Calculate the proportion of movement and scaling to apply
        //    float currentDistance = Vector3.Distance(objectToMove.position, Vector3.zero);
        //    float proportion = 1 - (currentDistance / initialDistance); // Adjusted proportion calculation

        //    // Apply movement and scaling based on the proportion
        //    Vector3 moveDelta = Vector3.MoveTowards(objectToMove.position, Vector3.zero, speed * Time.deltaTime * proportion);
        //    objectToMove.position -= moveDelta;
        //    objectToMove.localScale = Vector3.Lerp(objectToMove.localScale, Vector3.one, proportion);

        //    // Show background image if scale is above threshold
        //    if (objectToMove.localScale.x > .8f)
        //    {
        //        Background.SetActive(true);
        //        MyCurrentGamemanager.ShowBackgroundImage(true);
        //    }

        //    yield return new WaitForEndOfFrame();
        //}
        Background.SetActive(true);
        MyCurrentGamemanager.ShowBackgroundImage(true);
        objectToMove.position = Vector3.zero;
        objectToMove.localScale = Vector3.one; // Ensure final scale is 1
        lvlSelection.SetActive(false);
        CanvasGamePlay.SetActive(true);
        PlayUi.SetActive(true);
        PenCapacity.value = 1f;
        PenCapacity.value = 1f;
        StartFormNewlvl();
        yield return new WaitForEndOfFrame();
    }
   
    public IEnumerator HideCurrentLvl(Transform objectToMove, float speed)
    {
        GameController.Instance.CanPlayOn = false;
        CancelInvoke("ReplayMoveToCenterCeckFun");
        MyCurrentGamemanager.ResetAll();
        CanvasGamePlay.SetActive(false);
        lvlSelection.SetActive(true);
        currentScaleObject.transform.SetParent(CurrentParent, true);
        bird1partstart.gameObject.SetActive(false);
        bird2partstart.gameObject.SetActive(false);
        // Get the initial distance to move and scale proportionally
        float initialDistance = Vector3.Distance(objectToMove.position, Vector3.zero);
        float initialScale = objectToMove.localScale.x; // Assuming uniform scaling
        Background.SetActive(false);
        MyCurrentGamemanager.ShowBackgroundImage(false);
        //while (currentScaleObject.transform.localPosition != Vector3.zero)
        //{
        //    // Calculate the proportion of movement and scaling to apply
        //    float currentDistance = Vector3.Distance(currentScaleObject.transform.position, Vector3.zero);
        //    float proportion = 1 - (currentDistance / initialDistance); // Adjusted proportion calculation

        //    // Apply movement and scaling based on the proportion
        //    Vector3 moveDelta = Vector3.MoveTowards(currentScaleObject.transform.localPosition, Vector3.zero, speed * Time.deltaTime * proportion);
        //    currentScaleObject.transform.localPosition -= moveDelta;
        //    currentScaleObject.transform.localScale = Vector3.Lerp(currentScaleObject.transform.localScale, Vector3.one, proportion);

        //    // Condition checks for background image and sorting group
        //    if (currentScaleObject.transform.localScale.x < 4f)
        //    {
        //        MyCurrentGamemanager.ShowBackgroundImage(false);
        //        Background.SetActive(false);
        //    }
        //    if (currentScaleObject.transform.localScale.x < 1.3f)
        //        currentScaleObject.GetComponent<SortingGroup>().enabled = false;

        //    yield return new WaitForEndOfFrame();
        //}
        sublvlClick.ScreenImageObject.SetActive(true);
        currentScaleObject.GetComponent<SortingGroup>().enabled = true;
        currentScaleObject.transform.localPosition = Vector3.zero;
        currentScaleObject.transform.localScale = Vector3.one; // Ensure final scale is 1
        yield return new WaitForEndOfFrame();
    }
    public void HideCurrentLvlFun(Transform objectToMove, SubLvlClick CurrentSubLvl)
    {
        CancelInvoke("ReplayMoveToCenterCeckFun");
        GameManager CurrentGameManager = objectToMove.transform.GetChild(0).GetComponent<GameManager>();
        GameController.Instance.CanPlayOn = false;
        CurrentGameManager.ResetAll();
        CanvasGamePlay.SetActive(false);
        objectToMove.transform.SetParent(CurrentParent, true);
        Background.SetActive(false);
        CurrentGameManager.ShowBackgroundImage(false);
        CurrentSubLvl.ScreenImageObject.SetActive(true);
        objectToMove.GetComponent<SortingGroup>().enabled = true;
        objectToMove.transform.localPosition = Vector3.zero;
        objectToMove.transform.localScale = Vector3.one; // Ensure final scale is 1
    }

    public void ShowLvlFun_2(Transform objectToMove)
    {
        objectToMove.GetComponent<SortingGroup>().enabled = false;
        MyCurrentGamemanager = objectToMove.transform.GetChild(0).GetComponent<GameManager>();
        currentScaleObject = objectToMove.gameObject;
        CurrentParent = currentScaleObject.transform.parent;
        currentScaleObject.transform.parent = null;
        sublvlClick.ScreenImageObject.SetActive(false);
        // Get the initial distance to move and scale proportionally
        Background.SetActive(true);
        MyCurrentGamemanager.ShowBackgroundImage(true);
        objectToMove.position = Vector3.zero;
        objectToMove.localScale = Vector3.one; // Ensure final scale is 1
        lvlSelection.SetActive(false);
        CanvasGamePlay.SetActive(true);
        PlayUi.SetActive(true);
        PenCapacity.value = 1f;
        PenCapacity.value = 1f;
        Invoke(nameof(StartFormNewlvl),.1f);
    //    StartFormNewlvl();
        
    }
    internal void AfterCloseCongratulationPop()
    {
        if(!SpinWheel.activeSelf)
            WinCanvaluseGroup.alpha=1f;
        // AnimatorLvlComp.SetTrigger("lvl");
    }
    internal void AfterCloseSpinWheelPop()
    {
        WinCanvaluseGroup.alpha = 1f;
        // AnimatorLvlComp.SetTrigger("lvl");
    }
    public void UpdateCoins()
    {
        for (int i = 0; i < CoinsText.Length; i++)
        {
            CoinsText[i].text = GameController.Instance.Coins.ToString();
        }
    }
  
    public void SpinWheelShow()
    {
        SideBoxPanal.SetActive(false);
        SideimagePanal.anchoredPosition = new Vector3(0f, 7.880005f, 0f);
        HalfBg.anchoredPosition = new Vector3(5f, 7.880005f, 0f);
        FullBg.gameObject.SetActive(false);
        HalfBg.gameObject.SetActive(true);
        WinCanvaluseGroup.alpha = 0f;
        SpinWheel.SetActive(true);
    }
    public void ShowRandomWinPopup()
    {
        SideBoxPanal.SetActive(false);
        FullBg.gameObject.SetActive(false);
        HalfBg.gameObject.SetActive(true);
        SideimagePanal.anchoredPosition = new Vector3(0f, 7.880005f, 0f);
        HalfBg.anchoredPosition = new Vector3(5f, 7.880005f, 0f);
        rewardWinPop.SetActive(true);
        WinCanvaluseGroup.alpha = 0f;
        SoundManager.Instance.PlayRewardWinSound();
    }
    public void SpinAfterReward()
    {
        prizeWheel.StartSpinning();
    }

    public void GetCoinsReward()
    {
        GameController.Instance.AddCoins(100);
        SoundManager.Instance.PlayCoinsSound();
    }

    public void SetBtnState(int ShopIndux, int _currentPage)
    {
        GameController.Instance.CallingIndux = _currentPage;
        if (ShopIndux == 0)
        {
            if (GamePlayController.Instance.IsPenBuy[_currentPage])
            {
                GamePlayController.Instance.ChangeBtnStateOfPen(true, _currentPage);
            }
            else
            {
                GamePlayController.Instance.ChangeBtnStateOfPen(false, _currentPage);
            }
        }
        else if (ShopIndux == 1)
        {
            if (GamePlayController.Instance.IsPBallBuy[_currentPage])
            {
                GamePlayController.Instance.ChangeBtnStateOfBall(true, _currentPage);
            }
            else
            {
                GamePlayController.Instance.ChangeBtnStateOfBall(false, _currentPage);
            }
        }
        else if (ShopIndux == 2)
        {

            if (GamePlayController.Instance.IsbgBuy[_currentPage])
            {
                GamePlayController.Instance.ChangeBtnStateOfbg(true, _currentPage);
            }
            else
            {
                GamePlayController.Instance.ChangeBtnStateOfbg(false, _currentPage);
            }
        }
    }
}
