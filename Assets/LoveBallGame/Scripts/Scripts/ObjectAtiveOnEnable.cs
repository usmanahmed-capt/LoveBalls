using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectAtiveOnEnable : MonoBehaviour
{

    public GameObject ActiveObject;
    public GameObject[] DisableObject;

    private void OnEnable()
    {
        ActiveObject.SetActive(true);
        if (DisableObject.Length > 0)
        {
            for (int i = 0; i < DisableObject.Length; i++)
            {
                DisableObject[i].SetActive(false);
            }
        }

      
    }
}
