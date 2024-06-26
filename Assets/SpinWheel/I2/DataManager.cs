using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour 
{

	public static DataManager Instance;
  
	public int Coins
	{ 
		get { return _coins; }
		private set { _coins = value; }
	}
	public int FreeAdNumber
	{ 
		get { return _freeadnum; }
		private set { _freeadnum = value; }
	}

    public int CurrentLine;
    public int CurrentBetN;

    public int currentBettForBouce;
    public static event Action<int> CoinsUpdated = delegate {};
	public static event Action<int> FreeAdNumberUpdated = delegate {};

  
    public int mybetIndux;
    
	[SerializeField]
	int initialCoins = 0;
	// Show the current coins value in editor for easy testing
	[SerializeField]
	int _coins;
	// key name to store high score in PlayerPrefs
	//	const string COINSGAMESTRING = "coins";
	[SerializeField]
	int initialFreeAdNumber = 10;
	[SerializeField]
	int _freeadnum;
	void Awake()
	{
//		PlayerPrefs.DeleteAll ();

		if (Instance)
		{
			DestroyImmediate(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}



	void Start()
	{
		Reset();
	}

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            CurrentLine = 0;
            mybetIndux = 0;
            currentBettForBouce = 0;
            CurrentBetN = 0;
        }
       
      

    }

    public void Reset()
	{

        // Initialize coins
        print(initialCoins);
      
        Coins = PlayerPrefs.GetInt("coins", initialCoins);
		ResetFreeAds ();
	}

	public void AddCoins(int amount)
	{
		Coins += amount;
		// Store new coin value
		PlayerPrefs.SetInt("coins", Coins);
		// Fire event
		CoinsUpdated(Coins);
	}

	public void RemoveCoins(int amount)
	{
		Coins -= amount;
		// Store new coin value
		PlayerPrefs.SetInt("coins", Coins);
		// Fire event
		CoinsUpdated(Coins);
	}
	//----------------------
	public void ResetFreeAds()
	{
		FreeAdNumber = PlayerPrefs.GetInt("FreeAdsNumber", initialFreeAdNumber);
	}

	public void AddFreeAdNumber(int amount)
	{
		FreeAdNumber += amount;
		PlayerPrefs.SetInt("FreeAdsNumber", FreeAdNumber);
		FreeAdNumberUpdated(FreeAdNumber);
	}

	public void RemoveFreeAdNumber(int amount)
	{
		FreeAdNumber -= amount;
		PlayerPrefs.SetInt("FreeAdsNumber", FreeAdNumber);
		FreeAdNumberUpdated(FreeAdNumber);
	}





    //----------------------
}
