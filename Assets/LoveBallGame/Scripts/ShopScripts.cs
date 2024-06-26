using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScripts : MonoBehaviour
{
    public void BuyPen()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.BuyPenWithCoins(GameController.Instance.CallingIndux);
    }
    public void UsingPen()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.UsingPen(GameController.Instance.CallingIndux);
    }

    public void BuyBall()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.BuyBallWithCoins(GameController.Instance.CallingIndux);
    }
    public void UsingBall()
    {
        SoundManager.Instance.PlayButtonClickSound();
       // ContenClid[_currentPage].GetComponent<MoveBallAndPlayParticle>().CloseAndPlayParticle();
        GamePlayController.Instance.UsingBall(GameController.Instance.CallingIndux);
    }

    public void BuyBg()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.BuybgWithCoins(GameController.Instance.CallingIndux);
    }
    public void UsingBg()
    {
        SoundManager.Instance.PlayButtonClickSound();
        GamePlayController.Instance.Usingbg(GameController.Instance.CallingIndux);
    }


    public void SetBtnState( int ShopIndux, int _currentPage)
    {
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
