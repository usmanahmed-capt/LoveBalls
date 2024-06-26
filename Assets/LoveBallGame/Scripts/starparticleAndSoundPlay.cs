using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starparticleAndSoundPlay : MonoBehaviour
{

    public ParticleSystem starParticele;
    public void PlayStarParticeleAndSound() 
    {
        starParticele.Play();
        SoundManager.Instance.PlayStarSound();
    }
}
