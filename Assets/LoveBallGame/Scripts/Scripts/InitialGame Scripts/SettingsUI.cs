using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
public class SettingsUI : MonoBehaviour
{
    
    public Sprite SONSp,SOffSp;
    public Sprite MONSp, MOffSp;
    public Sprite vibONSp, vibOffSp;
    public Image soundImage;
    public Image MusicImage;
    public Image VibarationImage;

    //public anim
    private void Start()
    {
        PlayerPrefs.SetInt("music", 0);
        PlayerPrefs.SetInt("sound", 0);
        PlayerPrefs.SetInt("vibration", 0);
        GameController.Instance.vibrationValue = PlayerPrefs.GetInt("vibration", 0);
        MusicImage.sprite = PlayerPrefs.GetInt("music") == 0 ? MONSp : MOffSp;
        soundImage.sprite = PlayerPrefs.GetInt("sound") == 0 ? SONSp : SOffSp;
        VibarationImage.sprite = PlayerPrefs.GetInt("vibration") == 0 ? vibONSp : vibOffSp;

        SoundManager.Instance.AudioSoundOfOn();
        SoundManager.Instance.AudioMusicOfOn();
    }
   
    public void onMusicButton()
    {
        PlayerPrefs.SetInt("music", PlayerPrefs.GetInt("music") == 0 ? 1 : 0);
        MusicImage.sprite = PlayerPrefs.GetInt("music") == 0 ? MONSp : MOffSp;
        SoundManager.Instance.AudioMusicOfOn();
        SoundManager.Instance.PlayButtonClickSound();
      //  print(PlayerPrefs.GetInt("music"));
    }

    public void onSoundButton ()
    {

        PlayerPrefs.SetInt("sound", PlayerPrefs.GetInt("sound")== 0? 1:0);
        soundImage.sprite = PlayerPrefs.GetInt("sound") == 0 ? SONSp : SOffSp;
        SoundManager.Instance.AudioSoundOfOn();
        SoundManager.Instance.PlayButtonClickSound();
      
    }
    public void onVibButton()
    {
        PlayerPrefs.SetInt("vibration", PlayerPrefs.GetInt("vibration") == 0 ? 1 : 0);
        VibarationImage.sprite = PlayerPrefs.GetInt("vibration") == 0 ? vibONSp : vibOffSp;
        GameController.Instance.vibrationValue= PlayerPrefs.GetInt("vibration", 0);
        SoundManager.Instance.PlayButtonClickSound();
      
    }

    public void onBackButton ()
    {

       // settingPanal.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 1500f), .25f).SetEase(Ease.Linear);
        SoundManager.Instance.PlayButtonClickSound();
    }

    

  

}
