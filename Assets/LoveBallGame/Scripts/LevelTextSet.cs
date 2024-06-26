using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelTextSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "L e v e l  " + SceneManager.GetActiveScene().name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
