using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrignometricScale : MonoBehaviour
{

    public Vector3 ScaleLimit;
    public Vector3 ScaleFrequency;
    Vector3 FinalScale;
    Vector3 StartRotation;
    void Start()
    {
        StartRotation = transform.localScale;
    }
    void Update()
    {
        FinalScale.x = StartRotation.x + Mathf.Sin(Time.timeSinceLevelLoad * ScaleFrequency.x) * ScaleLimit.x;
        FinalScale.y = StartRotation.y + Mathf.Sin(Time.timeSinceLevelLoad * ScaleFrequency.y) * ScaleLimit.y;
        FinalScale.z = StartRotation.z + Mathf.Sin(Time.timeSinceLevelLoad * ScaleFrequency.z) * ScaleLimit.z;
        transform.localScale = new Vector3(FinalScale.x, FinalScale.y, FinalScale.z);
    }
}
