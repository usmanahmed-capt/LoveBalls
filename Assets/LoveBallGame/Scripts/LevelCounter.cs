using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelCounter : MonoBehaviour {

	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = (i + 1).ToString();
        }
    }
}
