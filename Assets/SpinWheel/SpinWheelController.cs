using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SpinWheelController : MonoBehaviour 
{
	public GameObject SpinWithReward;
	public GameObject SpinOver;
	public GameObject SpinCross;
	public int Coin_1,Coin_2, Coin_3, Coin_4, Coin_5, Coin_6, Coin_7, Coin_8, Coin_9, Coin_10;
	public Example CoinsExample;
	public RectTransform CenterPoint;
	private int SpinCount;
	public Animator SpinAnim;
	public TextMeshProUGUI CoinsText;
	void RewardBtnActive(int NumberItemToCoins) 
	{
		CoinsExample.mItemNumber = NumberItemToCoins;
		CoinsExample.OnClickMoney(CenterPoint);
		SpinCount++;
		Invoke(nameof(BtnActive), .5f);
	}

	void RewardBtnActive()
	{
		SpinCount++;
		Invoke(nameof(BtnActive), .5f);
	}

	void BtnActive() 
	{
		if (SpinCount == 1)
		{
			SpinWithReward.SetActive(true);
			SpinCross.SetActive(true);
		}

		if (SpinCount == 2)
		{
			SpinWithReward.SetActive(false);
			SpinOver.SetActive(true);
			SpinCross.SetActive(true);
		}
	}

    private void OnEnable()
    {
		SpinWithReward.SetActive(false);
		SpinOver.SetActive(false);
		SpinCross.SetActive(false);
		SpinAnim.Rebind();
		   SpinCount = 0;

	}

    private void Start()
    {
        
    }

    private void Update()
    {
		CoinsText.text = GameController.Instance.Coins.ToString();
    }
    public void SpinResult1()
	{
		RewardBtnActive();
		Debug.Log ("1");
		//GameManager.Instance.AddCoins(Coin_1);
		GameController.Instance.rewardIndux = 4;
		GamePlayController.Instance.ShowRandomWinPopup();

	}
	public void SpinResult2()
	{
		RewardBtnActive();
		//RewardBtnActive(12);
		Debug.Log ("2");
		//	GameManager.Instance.AddCoins(Coin_2);
		GameController.Instance.rewardIndux = 5;
		GamePlayController.Instance.ShowRandomWinPopup();
	}
	public void SpinResult3()
	{
		RewardBtnActive(14);
		Debug.Log ("3"+ Coin_3);
        GameController.Instance.AddCoins(Coin_3);
		SoundManager.Instance.PlayCoinsSound();
    }
	public void SpinResult4()
	{
		RewardBtnActive(16);
		Debug.Log("4" + Coin_3);
		GameController.Instance.AddCoins(Coin_4);
		SoundManager.Instance.PlayCoinsSound();
	}
	public void SpinResult5()
	{
		RewardBtnActive(18);
		Debug.Log("5" + Coin_5);
		GameController.Instance.AddCoins(Coin_5);
		SoundManager.Instance.PlayCoinsSound();
	}
	public void SpinResult6()
	{
		RewardBtnActive(20);
		Debug.Log("5" + Coin_6);
		GameController.Instance.AddCoins(Coin_6);
		SoundManager.Instance.PlayCoinsSound();
	}
	public void SpinResult7()
	{
		RewardBtnActive(20);
		Debug.Log ("7");
	//	GameManager.Instance.AddCoins(Coin_7);
	}
	public void SpinResult8()
	{
		RewardBtnActive(20);
		Debug.Log ("8");
		//GameManager.Instance.AddCoins(Coin_8);
	}
	public void SpinResult9()
	{
		RewardBtnActive(20);
		Debug.Log ("9");
		//GameManager.Instance.AddCoins(Coin_9);
	}
	public void SpinResult10()
	{
		RewardBtnActive(20);
		Debug.Log ("10");
		//GameManager.Instance.AddCoins(Coin_10);
	}

}
