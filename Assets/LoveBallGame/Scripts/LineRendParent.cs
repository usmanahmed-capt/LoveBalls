using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendParent : MonoBehaviour {
    LineRenderer lines;
    // Use this for initialization
    void Start () {
        lines = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        lines.positionCount = 2;
        if (transform.name == "PreDictLine")
        {
            lines.SetPosition(0, transform.position);
            lines.SetPosition(1, (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
        else
        {
            lines.SetPosition(0, transform.position);
            lines.SetPosition(1, transform.parent.GetChild(transform.GetSiblingIndex() - 1).position);
            lines.sortingOrder = 167;
            lines.enabled = true;
        }
    }
}
