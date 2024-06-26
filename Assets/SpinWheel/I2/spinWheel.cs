using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class spinWheel : MonoBehaviour
{

    public static spinWheel Instance;

    public GameObject GameWinPanal;
    // Start is called before the first frame update
    public Text earnCoinsText;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BackToSLotMachine()
    {
        //Sound.instance.PlayButton();
        PlayerPrefs.SetInt("EarnCoins", DataManager.Instance.currentBettForBouce);
        //CUtils.LoadScene(8, false);


    }


    public void BackToSLotMachineWithOutCoins()
    {
        //Sound.instance.PlayButton();
       
     //   CUtils.LoadScene(8, false);


    }
}
