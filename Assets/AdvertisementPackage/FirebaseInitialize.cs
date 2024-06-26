using UnityEngine;

public class FirebaseInitialize : MonoBehaviour
{
    public static FirebaseInitialize instance;
    private void Awake()
    {
        instance = this;
    }

    [HideInInspector]
    public bool firebaseInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SendFirebaseConsentDetail(char ad_Personalization)
    {
        if (!firebaseInitialized)
            return;
    }
    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {

    }

   
    public void AnalyticsLogin()
    {
        // Log an event with no parameters.
    }  

    public void LogEvent(string evt)
    {
        if (!firebaseInitialized)
            return;

        // Log an event with a float.
        Debug.Log("Logging a progress event.");
    }

    public void LogEventGame(string evt)
    {
        if (!firebaseInitialized)
            return;
    }

    public void AnalyticsScore()
    {
        // Log an event with an int parameter.
        Debug.Log("Logging a post-score event.");
    }

    public void AnalyticsGroupJoin()
    {
        // Log an event with a string parameter.
        Debug.Log("Logging a group join event.");
    }

    public void AnalyticsLevelUp()
    {
        // Log an event with multiple parameters.
        Debug.Log("Logging a level up event.");
    }

    // Reset analytics data for this app instance.
    public void ResetAnalyticsData()
    {
        Debug.Log("Reset analytics data.");
    }

    public void CustomAdEvent(string evt , string placement )
    {
        if (!firebaseInitialized)
            return;
    }
}
