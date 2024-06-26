using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMute : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource WheelAudio;
    void Start()
    {
        WheelAudio.mute = PlayerPrefs.GetInt("sound") == 0 ? WheelAudio.mute = false : WheelAudio.mute = true;
    }

    private void OnEnable()
    {
        WheelAudio.mute = PlayerPrefs.GetInt("sound") == 0 ? WheelAudio.mute = false : WheelAudio.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
