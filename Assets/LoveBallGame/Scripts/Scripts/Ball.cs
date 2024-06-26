using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Ball : MonoBehaviour {

	// Use this for initialization
	public Rigidbody2D Rb2d;
    public Collider2D collider2D;
    public Animator Anim;
    public CharSortingOrder charSorting;
    public bool CanBallAnimPlay;
    public bool CanBallFlyInitialy;
    private float storePos;
    private float movementHeight = 0.1f;
    private float movementSpeed = 1.3f;
    private Ease easeType = Ease.Linear;

    void OnEnable()
    {
      //  GameController.OnClicked += UpdateBall;
    }


    void OnDisable()
    {
       // GameController.OnClicked -= UpdateBall;
    }

    void Start ()
    {
        GetcharSorting();
        storePos = transform.localPosition.y;
        PlayAnimOfBirds();
    
    }

    internal void GetcharSorting() 
    {
        if (transform.childCount > 0)
        {
            charSorting = transform.GetChild(0).GetComponent<CharSortingOrder>();
        }
    }

    internal void PlayAnimOfBirds()
    {
        if (CanBallAnimPlay)
            Anim.enabled = true;

      
        if (CanBallFlyInitialy)
        {
            movementSpeed = Random.Range(0.5f, 0.8f);
            // Start an up and down tween animation
            transform.DOLocalMoveY(storePos + movementHeight, movementSpeed)
                   .SetEase(easeType)
                   .SetLoops(-1, LoopType.Yoyo)
                   .Play();
        }
    }


    internal void DisableBirdsAnimAndFlyingEffects() 
    {
            Anim.enabled = false;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.z, transform.localScale.z);
        if (CanBallFlyInitialy)
        {
            // Start an up and down tween animation
            transform.DOKill();
        }
    }
    void Onco(Collider2D collider)
    {
        if (collider.tag == "Target")
        {
            GameObject.Find("YouWonText").GetComponent<Text>().enabled = true;
        }

		//Vector2 dragDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - currentBall.transform.position;
		//currentBallRb.AddForce(dragDirection * forceMultiplier, ForceMode2D.Impulse);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (!GamePlayController.Instance.IsLose)
            {
                Rb2d.isKinematic = true;
                Rb2d.velocity = Vector2.zero;
                //   collider2D.enabled = false;
                Rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                Ball otherBallScripts = collision.gameObject.GetComponent<Ball>();
                otherBallScripts.Rb2d.isKinematic = true;
                //  otherBallScripts.collider2D.enabled = false;
                otherBallScripts.Rb2d.velocity = Vector2.zero;
                otherBallScripts.Rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                GamePlayController.Instance.WinFun();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FailPre"))
        {
            // Debug.LogError("Fail");
            GamePlayController.Instance.IsLose=true;
        }
        if (collision.gameObject.CompareTag("Fail"))
        {
           // Debug.LogError("Fail");
            GamePlayController.Instance.LoseFun();
        }
        
    }

}
