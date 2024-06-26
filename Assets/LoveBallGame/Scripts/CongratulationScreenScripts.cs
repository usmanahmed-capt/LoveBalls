using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CongratulationScreenScripts : MonoBehaviour
{
    public TextMeshProUGUI youGotNewtext;
    public GameObject skinHold;
    public GameObject penHold;
    public GameObject[] SkinObject;
    public string gotskin;
    public string gotpen;
    private int RandReward;
    public Image PenRedere;
    public ParticleSystem WinParticleball1;
    public ParticleSystem WinParticleball2;
    public ParticleSystem StarPenParticel;


    private void OnEnable()
    {
        for (int i = 0; i < SkinObject.Length; i++)
        {
            SkinObject[i].SetActive(false);
        }
        if (GameController.Instance.rewardIndux == 2)
        {
            RandReward = Random.Range(0, 2);
            if (RandReward == 0)
            {
                youGotNewtext.text = gotpen;
                skinHold.SetActive(false);
                penHold.SetActive(true);
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, GamePlayController.Instance.IsPenBuy.Length);
                } while (GamePlayController.Instance.IsPenBuy[randomIndex]);
                PenRedere.sprite = GamePlayController.Instance.PensSp[randomIndex];
                GamePlayController.Instance.BuyPen(randomIndex);
                StarPenParticel.Play();
            }
            else
            {
                youGotNewtext.text = gotskin;
                skinHold.SetActive(true);
                penHold.SetActive(false);
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, GamePlayController.Instance.IsPBallBuy.Length);
                } while (GamePlayController.Instance.IsPBallBuy[randomIndex]);
                for (int i = 0; i < SkinObject.Length; i++)
                {
                    SkinObject[i].SetActive(false);
                }
                SkinObject[randomIndex].SetActive(true);
                GamePlayController.Instance.BuyBall(randomIndex);
                WinParticleball1.Play();
                WinParticleball2.Play();
            }
        }
        else if (GameController.Instance.rewardIndux == 3) 
        {
            youGotNewtext.text = gotskin;
            skinHold.SetActive(true);
            penHold.SetActive(false);
            if (GameController.Instance.SkinReward == 0)
            {
                SkinObject[3].SetActive(true);
                GamePlayController.Instance.BuyBall(3);

            } else if (GameController.Instance.SkinReward == 1) 
            {
                SkinObject[6].SetActive(true);
                GamePlayController.Instance.BuyBall(6);
            }
            else if (GameController.Instance.SkinReward == 2)
            {
                SkinObject[4].SetActive(true);
                GamePlayController.Instance.BuyBall(4);
            }
            WinParticleball1.Play();
            WinParticleball2.Play();
            
        }

        else if (GameController.Instance.rewardIndux == 4)
        {
            youGotNewtext.text = gotskin;
            skinHold.SetActive(true);
            penHold.SetActive(false);
            SkinObject[1].SetActive(true);
            GamePlayController.Instance.BuyBall(1);
            WinParticleball1.Play();
            WinParticleball2.Play();
        }
        else if (GameController.Instance.rewardIndux == 5)
        {
            youGotNewtext.text = gotpen;
            skinHold.SetActive(false);
            penHold.SetActive(true);
            PenRedere.sprite = GamePlayController.Instance.PensSp[5];
            GamePlayController.Instance.BuyPen(5);
            StarPenParticel.Play();
        }
    }

    public void CloseContragulationPoupup() 
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.AfterCloseCongratulationPop();
        skinHold.SetActive(false);
        penHold.SetActive(false);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        WinParticleball1.Stop();
        WinParticleball2.Stop();
        StarPenParticel.Stop();
    }
}
