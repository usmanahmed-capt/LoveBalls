using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobBase : MonoBehaviour
{


  
    public bool IsValidSDK()
    {
#if UNITY_ANDROID

        string info = SystemInfo.operatingSystem;

        string sdkversion = info.Substring(0, 16);

        if (sdkversion.Equals("Android OS 8.1.0") && SystemInfo.systemMemorySize < 1024)
        {
            return false;
        }
        else
        {
            return true;
        }
#endif

        return true;

    }


}
