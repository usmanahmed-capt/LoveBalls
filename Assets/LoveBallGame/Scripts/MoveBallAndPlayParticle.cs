using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveBallAndPlayParticle : MonoBehaviour
{

    public Transform LeftBall;
    public Transform RightBall;
    private Vector3 leftballOrigionalPos;
    private Vector3 RigthballOrigionalPos;
    private bool isPositionAssing;
    public ParticleSystem ParticleSystem;
    private void OnEnable()
    {
        if (isPositionAssing) 
        {
            LeftBall.localPosition = leftballOrigionalPos;
            RightBall.localPosition = RigthballOrigionalPos;
        }
    }
    void Start()
    {
        leftballOrigionalPos = LeftBall.localPosition;
        RigthballOrigionalPos = RightBall.localPosition;
        isPositionAssing = true;
    }

    public void CloseAndPlayParticle() 
    {
        LeftBall.DOLocalMoveX(-75F, .2F).SetEase(Ease.Linear);
        RightBall.DOLocalMoveX(75F, .2F).SetEase(Ease.Linear);
        Invoke(nameof(Playparticle),.2f);
    }
    void Playparticle() 
    {
        ParticleSystem.Play();
    }
}
