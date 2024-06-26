using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInCamera : MonoBehaviour
{
    public Transform linePosition;          // Reference to the position where the line is being drawn
    public float zoomedCircleRadius = 2.0f; // Adjust this value based on the desired circle size
    public Camera MainCam;
    private void Update()
    {
        // Check if the line position is valid
        if (linePosition != null)
        {
            // Update the position of the secondary camera to follow the line
            transform.position = linePosition.position;

            // Adjust the orthographic size based on the desired circle size
            MainCam.orthographicSize = zoomedCircleRadius;
        }
    }
}
