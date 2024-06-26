using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBgSound : MonoBehaviour
{

    private void OnDisable()
    {
        SoundManager.Instance.BgSoundHigh();
    }
}
