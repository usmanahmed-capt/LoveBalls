using System.Net.NetworkInformation;
using UnityEngine;

public class InternetConnectionChecker: MonoBehaviour
{
    public static InternetConnectionChecker instance;
    public GameObject noInternetPanel;
    public 
    void Awake()
    {
        instance = this;
    }
   

    private bool IsInternetAvailable()
    {
        if((Application.internetReachability != NetworkReachability.NotReachable))
        {
            if (CheckInternetConnection())
            {
                return true;

            }
            else
            {
                noInternetPanel.SetActive(true);

                return false;

            }

        }
        else
        {
            noInternetPanel.SetActive(true);
            return false;
        } 
    }
    
    public void LoadWifiIntent()
    {
        #if UNITY_EDITOR
                Debug.Log("Wi-Fi Settings Please manually open Wi-Fi settings in the Unity Editor.");
                #elif UNITY_ANDROID
                                using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                                {
                                    using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.WIFI_SETTINGS"))
                                    {
                                        activity.Call("startActivity", intent);
                                    }
                                }
        #elif UNITY_IOS
                                // iOS implementation to open Wi-Fi settings
                                UnityEngine.iOS.Device.OpenURL("App-Prefs:root=WIFI"); 
        #endif
    }
   
    public void OpenWifiSettings()
    { 
        noInternetPanel.SetActive(false); 
        LoadWifiIntent();
    }

    public static bool CheckInternetConnection()
    {
        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        PingOptions options = new PingOptions();
        options.DontFragment = true;
        options.Ttl = 128;
        byte[] buffer = new byte[32];

        PingReply reply = ping.Send("google.com", 1000, buffer, options);

        if (reply.Status == IPStatus.Success)
        {
            Debug.Log("Ping successful");

            return true;
        }
        else
        {
            Debug.Log("Ping failed");

            return false;
        }
    }


}
