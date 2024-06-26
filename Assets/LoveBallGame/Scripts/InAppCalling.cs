using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InAppCalling : MonoBehaviour
{
    public void BuyInApp(int value)
    {
        UnityIAPInitialization.instance.BuyProductID(AdsDefination._instance.InAppIds[value].Id, value);
    }
    public void SpecailDesign(int value)
    {
        UnityIAPInitialization.instance.BuyProductID(AdsDefination._instance.InAppIds[value].Id, value);
    }
}