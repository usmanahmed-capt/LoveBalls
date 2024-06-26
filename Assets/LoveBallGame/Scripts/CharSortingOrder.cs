using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharSortingOrder : MonoBehaviour
{

    public SpriteRenderer head;
    public SpriteRenderer wing;
    public SpriteRenderer wing2;
    public SpriteRenderer facesad;
    public SpriteRenderer facehappy;
    public SpriteRenderer facenormal;
    public SpriteRenderer eyebrow;
    public SpriteRenderer eyebrow2;
    public SpriteRenderer eyeouter;
    public SpriteRenderer eyeinnter;
    public SpriteRenderer eyeouter2;
    public SpriteRenderer eyeinnter2;
    public Animator WingAnim;
    public Transform Wingtrns;
    public Transform Wingtrns2;
    private Vector3 WinInitialPos;
    private Vector3 WinInitialPos2;
    public GameObject FacesadObj;
    public GameObject FacehappyObj;
    public GameObject FaceNormalObj;
    public Animator EyeAnim;
    public Animator EyeAnimleft;

    public Animator eyeAnimator; // Assuming you have an Animator component
    public float minBlinkRate = 1.5f;
    public float maxBlinkRate =3f;

    private float nextBlinkTime;

    private bool IsCloseEyeAnimPlayed;
    private void Start()
    {
        WinInitialPos = Wingtrns.localPosition;
        WinInitialPos2 = Wingtrns2.localPosition;
        nextBlinkTime = Time.time + Random.Range(minBlinkRate, maxBlinkRate);
    }

    void Update()
    {
        if (Time.time >= nextBlinkTime && !IsCloseEyeAnimPlayed)
        {
            EyeAnimPlay();
            nextBlinkTime = Time.time + Random.Range(minBlinkRate, maxBlinkRate);
        }
        if(!GamePlayController.Instance.IsWin)
            IsCloseEyeAnimPlayed =false;

        if (!IsCloseEyeAnimPlayed &&GamePlayController.Instance.IsWin) 
        {
            IsCloseEyeAnimPlayed = true;
            EyeAnimClose();
        }

    }

    public void AssingSortingOrder(int headInt, int wintint, int faceint,int eyebrowint
        , int eyeouterint, int eyeinnterint) 
    {
        head.sortingOrder = headInt;
        wing.sortingOrder = wintint;
        wing2.sortingOrder = wintint;
        facesad.sortingOrder = faceint;
        facehappy.sortingOrder = faceint;
        facenormal.sortingOrder = faceint;
        eyebrow.sortingOrder = eyebrowint;
        eyebrow2.sortingOrder = eyebrowint;
        eyeouter.sortingOrder = eyeouterint;
        eyeouter2.sortingOrder = eyeouterint;
        eyeinnter.sortingOrder = eyeinnterint;
        eyeinnter2.sortingOrder = eyeinnterint;
    }

    public void WingInterOter(bool IsWing) 
    {
        if (IsWing) 
        {
            Wingtrns.DOLocalMoveX(0f, .25f).SetEase(Ease.Linear);
            Wingtrns2.DOLocalMoveX(0f, .25f).SetEase(Ease.Linear);
            
        }
        else
        {
            Wingtrns.DOLocalMove(WinInitialPos, .25f).SetEase(Ease.Linear);
            Wingtrns2.DOLocalMove(WinInitialPos2, .25f).SetEase(Ease.Linear);
        }
      
    }
    public void ResetWing()
    {
        Wingtrns.DOLocalMove(WinInitialPos, 0f).SetEase(Ease.Linear);
        Wingtrns2.DOLocalMove(WinInitialPos2, 0f).SetEase(Ease.Linear);
      //  EyeAnimPlay();
        NormalExpressionofBird();
      

    }

    public void WingAnimOnOf(bool IsWing)
    {
        if (IsWing)
        {
            WingAnim.SetInteger("Wing", 1);
         
        }
        else 
        {
            WingAnim.SetInteger("Wing", 2);
        }
        
    }

    public void WinExpressionOfBird()
    {
        FacehappyObj.SetActive(true);
        FaceNormalObj.SetActive(false);
        FacesadObj.SetActive(false);
    }
    public void LoseExpressionofBird()
    {
        FacehappyObj.SetActive(false);
        FaceNormalObj.SetActive(false);
        FacesadObj.SetActive(true);
    }
    public void NormalExpressionofBird()
    {
        FacehappyObj.SetActive(false);
        FaceNormalObj.SetActive(true);
        FacesadObj.SetActive(false);
    }
    public void EyeAnimPlay()
    {
        EyeAnim.SetTrigger("Eye");
        EyeAnimleft.SetTrigger("Eye");
    }

    public void EyeAnimClose()
    {
        EyeAnim.SetTrigger("EyeClose");
        EyeAnimleft.SetTrigger("EyeClose");
    }


}
