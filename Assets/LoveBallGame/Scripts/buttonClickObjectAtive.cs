using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonClickObjectAtive : MonoBehaviour
{

    public GameObject ObjectToOpen;
    public GameObject[] CloseOtherObjects;

    public void ShowObject() 
    {
        SoundManager.Instance.PlaypopUpOpen();
        ObjectToOpen.SetActive(true);
        if (CloseOtherObjects.Length > 0)
        {
            for (int i = 0; i < CloseOtherObjects.Length; i++)
            {
                CloseOtherObjects[i].SetActive(false);
            }
        }
    }

    public void CloseObject()
    {
        SoundManager.Instance.PlaypopupClose();
        ObjectToOpen.SetActive(false);
        if (CloseOtherObjects.Length > 0)
        {
            for (int i = 0; i < CloseOtherObjects.Length; i++)
            {
                CloseOtherObjects[i].SetActive(false);
            }
        }
    }
}
