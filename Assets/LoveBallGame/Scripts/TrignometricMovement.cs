using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrignometricMovement : MonoBehaviour
{

    public Vector3 Distance;
    public Vector3 MovementFrequency;
    Vector3 Moveposition;
    Vector3 startPosition;
	public bool circular;

    void Start()
    {
        startPosition = transform.localPosition;
    }
    void Update()
    {
        Moveposition.x = startPosition.x + Mathf.Sin(Time.timeSinceLevelLoad * MovementFrequency.x) * Distance.x;
			if (circular) {
				Moveposition.y = startPosition.y + Mathf.Cos(Time.timeSinceLevelLoad * MovementFrequency.y) * Distance.y;
			} else {
				Moveposition.y = startPosition.y + Mathf.Sin(Time.timeSinceLevelLoad * MovementFrequency.y) * Distance.y;
			}     
	
        Moveposition.z = startPosition.z + Mathf.Sin(Time.timeSinceLevelLoad * MovementFrequency.z) * Distance.z;       
        transform.localPosition = new Vector3(Moveposition.x, Moveposition.y, Moveposition.z);
     
        
    }
}
