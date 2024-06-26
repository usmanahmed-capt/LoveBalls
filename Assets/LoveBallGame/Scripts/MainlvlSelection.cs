using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.IO;
using DG.Tweening;
public class MainlvlSelection : MonoBehaviour
{
    public int LvlNumber;
    public int LvlNumberOfStage;
    public TextMeshProUGUI Currentlvl;
    public TextMeshProUGUI GainStarprlvl;
    public TextMeshProUGUI RequiredStarText;
    public SpriteRenderer ScreenImage;
    public GameObject ScreenImageObject;
    public GameObject YouAreHare;
    private bool IsFirstTimeDone;
    public GameObject tick;
    private bool isFirstTimeApplied;
    public bool IsLvllock;
    public int RequiredStar;
    public GameObject Lock;
    public Animator LockAnim;
    public Button LvlClickBtn;
    public Transform Ballfemale;
    public Transform Ballmale;
    public SpriteRenderer bg;
    public bool IsAnim;
    void Start()
    {
        IsFirstTimeDone = true;
        if (IsLvllock)
        {
            LvlClickBtn.enabled = false;
            Lock.SetActive(true);
            RequiredStarText.text = "Required " + RequiredStar + " stars to unlock";

        }
        else
        {
            LvlClickBtn.enabled = true;
            Lock.SetActive(false);
        }

        Udpateball();
        if (IsAnim) 
        {
            if (PlayerPrefs.GetInt("toturial") == 0)
                transform.DOScale(Vector3.one * 1.1f, .5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); ;
        }
    }

   
    private void OnEnable()
    {
        if (isFirstTimeApplied)
        {
            YouAreHare.SetActive(false);

            if (IsFirstTimeDone)
            {
                RequiredStarText.enabled = false;
                LockAnim.enabled = false;
                CheckIsLevelUnlock();
                Invoke(nameof(YouAreHere),.1f);
                if (GameController.Instance.CurrentPlayedInduxMain.Count > 0)
                {
                    if (GameController.Instance.CurrentPlayedInduxMain.Contains(LvlNumberOfStage))
                    {
                        Invoke(nameof(ShowOnEnable), .0f);
                        GameController.Instance.CurrentPlayedIndux.Remove(LvlNumberOfStage);
                    }
                }
            }
        }
    }

    void YouAreHere() 
    {
        if (GameController.Instance.currenLvlMain == LvlNumber)
            YouAreHare.SetActive(true);
    }

    public void AppliedFirstTimeImages()
    {
        IsFirstTimeDone = true;
        isFirstTimeApplied = true;
        YouAreHare.SetActive(false);

        CheckIsLevelUnlock();

        if (IsFirstTimeDone)
        {
            if (GameController.Instance.currenLvlMain == LvlNumber)
                YouAreHare.SetActive(true);
            Invoke(nameof(ShowOnEnable), .1f);
        }
        else
        {
            Invoke(nameof(ShowOnEnable), .1f);
        }
    }


    void CheckIsLevelUnlock() 
    {
        if (RequiredStar <= GameController.Instance.Stars)
        {
            LvlClickBtn.enabled = true;
            IsLvllock = false;
            Lock.SetActive(false);
        }
    }

    public void YouAreHareindux()
    {
        GameController.Instance.currenLvlMain = LvlNumber;
        FirebaseInitialize.instance.LogEventGame("mainslection_to_lvlslection");//sohail
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent("mainslection_to_lvlslection");//sohail 
        AdsManager.Instance.ShowBannerBottom();
        if (IsAnim)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            IsAnim = false;
        }
    }

    void ShowOnEnable()
    {
        tick.SetActive(false);
        Currentlvl.text = LvlNumber.ToString();
      //  GainStarprlvl.text=
        string filePath = Application.persistentDataPath + "/lvl" + LvlNumberOfStage + ".png";
        GainStarprlvl.text =PlayerPrefs.GetInt("lvlMain" + LvlNumber).ToString()+"/15";
        if (File.Exists(filePath))
        {
            byte[] bytess = File.ReadAllBytes(filePath);
            Texture2D texture2D = new Texture2D(GameController.Instance.renderedTexture.width, GameController.Instance.renderedTexture.height, TextureFormat.ARGB32, false);
            texture2D.LoadImage(bytess);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, GameController.Instance.renderedTexture.width, GameController.Instance.renderedTexture.height), new Vector2(0.5f, 0.5f));
            ScreenImage.sprite = sprite;
            ScreenImageObject.SetActive(true);
            tick.SetActive(true);
        }
    }
    public void LockBtnClick() 
    {
        SoundManager.Instance.PlayButtonClickSound();
        RequiredStarText.enabled = true;
        GamePlayController.Instance.RequirdStartext.text = "" + RequiredStar + " Stars required to unlock these levels";
        GamePlayController.Instance.MoreStarPanal.SetActive(true);
        GamePlayController.Instance.MoreStarPanal.transform.parent.gameObject.SetActive(true);
        //LockAnim.Rebind();
        //if (!LockAnim.enabled)
        //    LockAnim.enabled = true;
      
    }

    private GameObject Female;
    private GameObject Male;
    public void Udpateball()
    {
        int ballCurrentUsing = PlayerPrefs.GetInt("ballCurrentUsing", 0); // Use a default value in case the key is missing
        ballCurrentUsing++;
        string femalePath = "female/" + ballCurrentUsing;
        string malePath = "Male/" + ballCurrentUsing;

        // Check if resources exist before loading
        GameObject femalePrefab = Resources.Load<GameObject>(femalePath);
        GameObject malePrefab = Resources.Load<GameObject>(malePath);

        if (femalePrefab != null)
        {
            Female = Instantiate(femalePrefab); // Create an instance from the prefab
            Female.SetActive(false);
        }
        else
        {
           // Debug.LogError("Failed to load female prefab: " + femalePath);
        }

        if (malePrefab != null)
        {
            Male = Instantiate(malePrefab);
            Male.SetActive(false);
        }
        else
        {
           // Debug.LogError("Failed to load female prefab: " + malePrefab);
        }
        //  Ballmale
        Male.transform.SetParent(Ballmale.transform, true);
        Male.transform.localPosition = Ballmale.GetChild(0).transform.localPosition;
        Male.transform.rotation = Ballmale.GetChild(0).transform.rotation;
        Male.transform.localScale = Ballmale.GetChild(0).transform.localScale;
        Destroy(Ballmale.GetChild(0).gameObject);
        Male.name = "Male";
        Male.SetActive(true);
        Male.transform.SetAsFirstSibling();
        Female.transform.SetParent(Ballfemale.transform, true);
        Female.transform.localPosition = Ballfemale.GetChild(0).transform.localPosition;
        Female.transform.rotation = Ballfemale.GetChild(0).transform.rotation;
        Female.transform.localScale = Ballfemale.GetChild(0).transform.localScale;
        Destroy(Ballfemale.GetChild(0).gameObject);
        Female.transform.SetAsFirstSibling();
        Female.SetActive(true);
    }
    public void Udpatebg()
    {
        bg.sprite = GamePlayController.Instance.bgSp[GamePlayController.Instance.CurrentbgUsing];
    }

}
