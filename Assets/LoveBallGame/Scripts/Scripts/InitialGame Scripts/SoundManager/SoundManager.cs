using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

	public AudioSource BgAudioSource;
    public AudioSource ClickAudioSource;
    public AudioSource GameOverAudioSource;
    public AudioSource CoinsHitAudio;
    public AudioSource PlayWithClipAudioSource;
    public AudioSource LoopAudioSource;
    public bool isMultipleSounds = false;
	public bool isSound = true;
    public AudioSource[] bgSounds;
    public AudioSource[] ClickAudioSources;

    #region OneShotSoundClips

    /*
	 * Audio Clip Files for One Shot
	 */

    public AudioClip buttonClickSound;
    public AudioClip GameOverClip;
    public AudioClip CoinsClip;
    public AudioClip GameWinClip;
    public AudioClip BirdMeatClip;
    public AudioClip ScrollSoundclip;
    public AudioClip RewardWinClip;
    public AudioClip popupopen;
    public AudioClip popupClose;
    public AudioClip PageOpen;
    public AudioClip StarClip;
    public AudioClip WinPopUpClip;
    public AudioClip BirdInitialparticelClip;

    public void SoundGameObject(int buildIndux) 
    {
        //for (int i = 0; i < bgSounds.Length; i++)
        //{
        //    bgSounds[i].gameObject.SetActive(false);
        //}
        if (buildIndux == 1)
        {
            bgSounds[0].gameObject.SetActive(false);
            if (!bgSounds[buildIndux].gameObject.activeSelf)
                bgSounds[buildIndux].gameObject.SetActive(true);
        }
        else
        {
            bgSounds[1].gameObject.SetActive(false);
            if (!bgSounds[buildIndux].gameObject.activeSelf)
                bgSounds[buildIndux].gameObject.SetActive(true);
        }
       

    }
    #endregion

    #region InGameLoopSoundClips

    /*
	 * Audio Clip Files for In Game Loop Sound
	 */

    #endregion

    #region BackGroundSoundClips

    /*
	 * Audio Clip Files for BG Loop Sound
	 */



    #endregion


    //public AudioClip[] mensterPattrenSound;

    #region DefaultMethods

    //TODO: Make sure you override your awake by override keyword
    /*override*/
    void Awake ()
	{
        if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
	}

	void Start ()
	{

		//PlayerPrefs.DeleteAll ();
		/*
         * If Sound is mute previously Mute the sound.
         */
		//TODO : Need to change issound with your player prefrence
		if (!isSound) {
			GetComponent<AudioSource> ().mute = true;
		} else {
			GetComponent<AudioSource> ().mute = false;
		}

		/*
		 * Checking whether dual sound enable or not.
		 * If Enable setting MenuBGSound and GameBGSound Accourding.
		 * Else always set GameBGSound.
		 */

        

		/*
		 * If Sound is not playing Play the musicAudioSource
		 */

		if (!GetComponent<AudioSource> ().isPlaying) {
			GetComponent<AudioSource> ().Play ();
		}
	}

	#endregion



	#region InGameLoopSoundMethods

	/*
	 * Stop Playing previous in game loop sound
     * checking is new in game loop sound available playing it.
	 */
    
	

	public void StopInGameLoop ()
	{
        BgAudioSource.Stop ();
	}

	#endregion

	#region BGSoundMethods

	/*
	 * Playing Different Background Sounds
	 */


    public void BackgroundSoundOn()
    {

        for (int i = 0; i < bgSounds.Length; i++)
        {
            bgSounds[i].mute = false;
        }

    }
    public void BackgroundSoundOf()
    {
        for (int i = 0; i < bgSounds.Length; i++)
        {
            bgSounds[i].mute = true;
        }

    }

   

	

	#endregion

	#region OneShotSoundMethods

	/*
	 * Playing one shot for each OneShotSound.
     * Muting and UnMuting sound Accordingly.
	 */
	

	public void PlayButtonClickSound ()
	{
		if (buttonClickSound) {
            Utilities.PlaySFX(ClickAudioSource, buttonClickSound, GameController.Instance.SfxSoundLevel, false);
        }
	}
    public void PlayCoinsSound()
    {
        if (CoinsClip)
        {
            Utilities.PlaySFX(ClickAudioSource, CoinsClip, GameController.Instance.SfxSoundLevel, false);
        }
    }

    //  public AudioClip popupopen;
    //  public AudioClip popupClose;
    //  public AudioClip PageOpen;

    public void PlayPageOpen()
    {
        if (buttonClickSound)
        {
            Utilities.PlaySFX(PlayWithClipAudioSource, PageOpen, GameController.Instance.SfxSoundLevel, false);
        }
    }

    public void PlaypopUpOpen()
    {
        if (buttonClickSound)
        {
            Utilities.PlaySFX(PlayWithClipAudioSource, popupopen, GameController.Instance.SfxSoundLevel, false);
        }
    }
    public void PlaypopupClose()
    {
        if (buttonClickSound)
        {
            Utilities.PlaySFX(PlayWithClipAudioSource, popupClose, GameController.Instance.SfxSoundLevel, false);
        }
    }
    public void PlayScrollSound()
    {
        if (buttonClickSound)
        {
          //  Utilities.PlaySFX(ClickAudioSource, ScrollSoundclip, GameController.Instance.SfxSoundLevel, false);
        }
    }
    public void PlayRewardWinSound()
    {
        if (buttonClickSound)
        {
            Utilities.PlaySFX(ClickAudioSource, RewardWinClip, GameController.Instance.SfxSoundLevel, false);
        }
    }

    public void PlayPopupWinSound() {
        if (WinPopUpClip)
        {
            Utilities.PlaySFX(ClickAudioSource, WinPopUpClip, GameController.Instance.SfxSoundLevel, false);
        }
            }
    public void PlayGameWinSound()
    {
        if (GameWinClip)
        {
            Utilities.PlaySFX(GameOverAudioSource, GameWinClip, GameController.Instance.SfxSoundLevel, false);
        }
    }
    public void PlayBirtMeetSound()
    {
        if (BirdMeatClip)
        {
            Utilities.PlaySFX(PlayWithClipAudioSource, BirdMeatClip, GameController.Instance.SfxSoundLevel, false);
        }
    }
    public void PlayParticleSound()
    {
        if (BirdInitialparticelClip)
        {
            Utilities.PlaySFX(PlayWithClipAudioSource, BirdInitialparticelClip, GameController.Instance.SfxSoundLevel, false);
        }
    }
    public void PlayStarSound()
    {
        if (StarClip)
        {
            Utilities.PlaySFX(PlayWithClipAudioSource, StarClip, GameController.Instance.SfxSoundLevel, false);
        }
    }

    public void PlayGameOverSound()
    {
        if (GameOverClip)
        {
            Utilities.PlaySFX(GameOverAudioSource, GameOverClip, GameController.Instance.SfxSoundLevel, false);
        }
    }




    public void PlaySoundWithClip2(AudioClip clip)
    {
        if (clip && !PlayWithClipAudioSource.isPlaying)
        {
            Utilities.PlaySFX(PlayWithClipAudioSource, clip, GameController.Instance.SfxSoundLevel, false);
        }
    }


    public void stopSound() 
    {
        LoopAudioSource.Stop();
    }
    public void stopSoundTimerEnd()
    {
        LoopAudioSource.Stop();
    }


    public void MuteSound ()
	{
		isSound = false;
        for (int i = 0; i < ClickAudioSources.Length; i++)
        {
            ClickAudioSources[i].mute = true;
        }

        //TODO : Need to save the sound states 
    }

	public void UnMuteSound ()
	{
		isSound = true;
        for (int i = 0; i < ClickAudioSources.Length; i++)
        {
            ClickAudioSources[i].mute = false;
        }
        //TODO : Need to save the sound states 
    }

    public void BgSoundLow()
    {
        BgAudioSource.volume = .3f;
    }

    public void BgSoundHigh()
    {
        BgAudioSource.volume =1f;
    }

    public void AudioSoundOfOn() 
    {

    ClickAudioSource.mute = PlayerPrefs.GetInt("sound") == 0 ? ClickAudioSource.mute = false : ClickAudioSource.mute = true;
        GameOverAudioSource.mute = PlayerPrefs.GetInt("sound") == 0 ? GameOverAudioSource.mute = false : GameOverAudioSource.mute = true;
        CoinsHitAudio.mute = PlayerPrefs.GetInt("sound") == 0 ? CoinsHitAudio.mute = false : CoinsHitAudio.mute = true;
        PlayWithClipAudioSource.mute = PlayerPrefs.GetInt("sound") == 0 ? PlayWithClipAudioSource.mute = false : PlayWithClipAudioSource.mute = true;
        LoopAudioSource.mute = PlayerPrefs.GetInt("sound") == 0 ? LoopAudioSource.mute = false : LoopAudioSource.mute = true;
    }
    public void AudioMusicOfOn()
    {
        BgAudioSource.mute = PlayerPrefs.GetInt("music") == 0 ? BgAudioSource.mute = false : BgAudioSource.mute = true;
    }

    #endregion
}