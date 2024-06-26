using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TotatorActive : MonoBehaviour {

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name== "Collider")
        {
          //  Debug.LogError("Collider");
            if(!GamePlayController.Instance.CanTotatorActive)
                GamePlayController.Instance.CanTotatorActive = true;
        }
    }

}
