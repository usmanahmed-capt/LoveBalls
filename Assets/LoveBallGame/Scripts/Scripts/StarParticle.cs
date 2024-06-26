using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticle : MonoBehaviour
{

    public GameObject Particel;
    public Animator StarAnim;
    public void PlarticelPlay() 
    {
        Particel.SetActive(true);
    }
   
}
