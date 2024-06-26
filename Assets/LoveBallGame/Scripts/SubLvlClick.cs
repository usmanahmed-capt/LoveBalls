using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.IO;
using GameAnalyticsSDK;
using DG.Tweening;

public class SubLvlClick : MonoBehaviour
{

    public int LvlNumber;
    public GameObject[] StarObject;
    public TextMeshProUGUI Currentlvl;
   
    public Button MyLvlBtn;
    public Transform ScaleObject;
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
    public TextMeshProUGUI RequiredStarText;
    private int ballInduxPre;
    public Ball Ballfemale;
    public Ball Ballmale;
    public SpriteRenderer bg;
    public bool IsAnim;
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

                if (GameController.Instance.currenLvlClick == LvlNumber)
                    YouAreHare.SetActive(true);

                if (GameController.Instance.CurrentPlayedIndux.Count > 0)
                {
                    if (GameController.Instance.CurrentPlayedIndux.Contains(LvlNumber)) 
                    {
                        Invoke(nameof(ShowOnEnable), .0f);
                        GameController.Instance.CurrentPlayedIndux.Remove(LvlNumber);
                    }
                }

                int starCount = PlayerPrefs.GetInt("lvl" + LvlNumber, 0);
                if (starCount > 0)
                {
                    for (int i = 0; i < starCount; i++)
                    {
                        StarObject[i].SetActive(true);
                    }
                }
            }
        }
    }

    public void AppliedFirstTimeImages()
    {
        IsFirstTimeDone = true;
        isFirstTimeApplied = true;
        YouAreHare.SetActive(false);
        CheckIsLevelUnlock();
        if (IsFirstTimeDone)
        {
            if (GameController.Instance.currenLvlClick == LvlNumber)
                YouAreHare.SetActive(true);
            Invoke(nameof(ShowOnEnable), .1f);


        }
        else
        {
            Invoke(nameof(ShowOnEnable), .1f);
        }


    }

    private void Update()
    {
        CheckIsLevelUnlock();
    }
    void CheckIsLevelUnlock()
    {
        if (RequiredStar <= GameController.Instance.Stars)
        {
            IsLvllock = false;
            Lock.SetActive(false);
        }
    }

    void ShowOnEnable()
    {
        tick.SetActive(false);
        Currentlvl.text = LvlNumber.ToString();
       
        string filePath = Application.persistentDataPath + "/lvl" + LvlNumber + ".png";

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

    void ShowPlayedImageifPlaydAgain()
    {
        Currentlvl.text = LvlNumber.ToString();
        int starCount = PlayerPrefs.GetInt("lvl" + LvlNumber, 0);
        if (starCount > 0)
        {
            for (int i = 0; i < starCount; i++)
            {
                StarObject[i].SetActive(true);
            }
        }
        string filePath = Application.persistentDataPath + "/lvl" + LvlNumber + ".png";

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

    void ShowUiOnly()
    {
        Currentlvl.text = LvlNumber.ToString();
        
    }

    void Start()
    {
        IsFirstTimeDone = true;
        MyLvlBtn.onClick.AddListener(() => CurrentlvlClickFun());
        if (IsLvllock)
        {
            Lock.SetActive(true);
            RequiredStarText.text = "Required " + RequiredStar + " stars to unlock";

        }
        else
        {
            Lock.SetActive(false);
        }

        //  Udpateball();
        this.Invoke(nameof(Udpateball),.1f);

        if (IsAnim)
        {
          // Infinite loop with Yoyo effect
            if (PlayerPrefs.GetInt("toturial") == 0)
                transform.DOScale(Vector3.one * 1.1f, .5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); ;
        }

        int starCount = PlayerPrefs.GetInt("lvl" + LvlNumber, 0);
        if (starCount > 0)
        {
            for (int i = 0; i < starCount; i++)
            {
                StarObject[i].SetActive(true);
            }
        }
    }

   public  void CurrentlvlClickFun()
    {
        if (IsLvllock)
        {
            LockBtnClick();
        }
        else 
        {
            SoundManager.Instance.PlayPageOpen();

            if (IsAnim)
            {
                transform.DOKill();
                transform.localScale = Vector3.one;
                IsAnim = false;
            }
            GameController.Instance.currenLvlClick = LvlNumber;
            tick.SetActive(false);
            GamePlayController.Instance.ShowCurrentlvl();

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_"+ GameController.Instance.currenLvlClick);//sohail
            FirebaseInitialize.instance.LogEventGame("level_" + GameController.Instance.currenLvlClick+"_start");//sohail
        }
      
    }

    public void LockBtnClick()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.RequirdStartext.text = "" + RequiredStar + " Stars required to unlock these levels";
        GamePlayController.Instance.MoreStarPanal.SetActive(true);
        GamePlayController.Instance.MoreStarPanal.transform.parent.gameObject.SetActive(true);
        //RequiredStarText.enabled = true;
        //LockAnim.Rebind();
        //if (!LockAnim.enabled)
        //    LockAnim.enabled = true;

    }

    //public IEnumerator MoveOverSpeed(Transform objectToMove, Vector3 end, float speed)
    //{
    //    ScaleObject.GetComponent<SortingGroup>().enabled = true;
    //    while (objectToMove.position != end)
    //    {
    //        objectToMove.position = Vector3.MoveTowards(objectToMove.position, end, speed * Time.deltaTime);
    //        objectToMove.localScale = Vector3.MoveTowards(objectToMove.localScale, targerPos.localScale, speed * Time.deltaTime);
    //        yield return new WaitForEndOfFrame();
    //    }
    //    objectToMove.position = end;
    //    GamePlayController.Instance.ShowCurrentLvl(objectToMove);
    //    Debug.LogError("MoveOverSpeed");
    //}

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
          //  Debug.LogError("Failed to load female prefab: " + malePrefab);
        }

        //    GameObject Female = Resources.Load<GameObject>("female/"+PlayerPrefs.GetInt("ballCurrentUsing").ToString());
        //GameObject Male = Resources.Load<GameObject>("Male/" + PlayerPrefs.GetInt("ballCurrentUsing").ToString());
        //Female.SetActive(false);
        //Male.SetActive(false);
        //  Ballmale
        Male.transform.SetParent(Ballmale.transform,true);
        Male.transform.localPosition = Ballmale.charSorting.transform.localPosition;
        Male.transform.rotation = Ballmale.charSorting.transform.rotation;
        Male.transform.localScale = Ballmale.charSorting.transform.localScale;
        Male.transform.SetAsFirstSibling();
        Destroy(Ballmale.charSorting.gameObject);
        Ballmale.GetcharSorting();
        Male.SetActive(true);
        Female.transform.SetParent(Ballfemale.transform, true);
        Female.transform.localPosition = Ballfemale.charSorting.transform.localPosition;
        Female.transform.rotation = Ballfemale.charSorting.transform.rotation;
        Female.transform.localScale = Ballfemale.charSorting.transform.localScale;
        Female.transform.SetAsFirstSibling();
        Destroy(Ballfemale.charSorting.gameObject);
        Ballfemale.GetcharSorting();
        Female.SetActive(true);
    }

    public void Udpatebg()
    {
        bg.sprite = GamePlayController.Instance.bgSp[GamePlayController.Instance.CurrentbgUsing];
    }
}
