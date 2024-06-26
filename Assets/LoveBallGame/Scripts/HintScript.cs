using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HintScript : MonoBehaviour {

    LineRenderer line;
    public Transform Pen;
    private bool CanRunOnlyOnes;
    public List<Transform> ChildTransfrom = new List<Transform>();

    public IEnumerator enumeratorHold;
    private bool CanPenAssing;
    public float RunSpeed;
    private void OnEnable()
    {
        CanRunOnlyOnes = true;
        if (CanPenAssing) 
        {
            if (enumeratorHold != null)
                StopCoroutine(MoveOverSpeed());

            Pen.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            Pen.position = ChildTransfrom[0].position;
            enumeratorHold = MoveOverSpeed();
            StartCoroutine(enumeratorHold);
        }

    }
    // Use this for initialization
    void Start ()
    {
        if (Pen) 
        {
            if (ChildTransfrom.Count == 0)
            {
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    ChildTransfrom.Add(transform.GetChild(i));
                }
            }
            Pen.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            Pen.position = ChildTransfrom[0].position;
            enumeratorHold = MoveOverSpeed();

            StartCoroutine(enumeratorHold);
            CanPenAssing = true;
        }
    }

    
  public IEnumerator MoveOverSpeed()
    {
        // speed should be 1 unit per second
        if (ChildTransfrom.Count > 0)
        {
            //Debug.Log("MoveOverSpeed");
            for (int i = 0; i < ChildTransfrom.Count; i++)
            {
                while (Pen.position != ChildTransfrom[i].position)
                {
                    Pen.position = Vector3.MoveTowards(Pen.position, ChildTransfrom[i].position, RunSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
            //  Debug.Log("MoveOverSpeed2");
            Pen.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
              yield return new WaitForSeconds(1.5f);
            Pen.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            Pen.position = ChildTransfrom[0].position;
            enumeratorHold = MoveOverSpeed();
            StartCoroutine(enumeratorHold);
        }
    }

    // Update is called once per frame
    void Update () {
        if (CanRunOnlyOnes)
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = transform.childCount - 1;
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                line.SetPosition(i, transform.GetChild(i).position);
            }
            CanRunOnlyOnes = false;
        }
	}

}
