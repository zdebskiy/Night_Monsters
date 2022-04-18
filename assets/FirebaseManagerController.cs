using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase.Analytics;

public class FirebaseManagerController : MonoBehaviour
{
    public static FirebaseManagerController instance = null;

    public static bool flagFirebaseStatus;

    private void Awake() {
        InitSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckFirebaseAnalitics();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitSingleton(){
        if (instance == null) {
	        instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            if(instance == this) {
	            Destroy(gameObject);
            }
        }        
    }
    private void CheckFirebaseAnalitics() {
        flagFirebaseStatus = false;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("Firebase - OK!!!");
                InitFirebaseAnalitics();
                flagFirebaseStatus = true;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }    

    private void InitFirebaseAnalitics() {
        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Debug.Log("Set user properties.");
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
        // Set the user ID.
        FirebaseAnalytics.SetUserId("uber_user_510");
        // Set default session duration values.
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));        
    }

    public static void FBA_EventScreenView (string nameScreen) {        
        Parameter[] paramEvent = {
			new Parameter ("ParameterScreenName", nameScreen)
		};
        if (flagFirebaseStatus) {
		    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, paramEvent);
        }
	}

    public static void FBA_EventLevelStart (string levelName, int iDifficulty) {
        Parameter[] paramEvent = {
			new Parameter ("ParameterLevelName", levelName)
		};
        if (flagFirebaseStatus) {
		    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, paramEvent);        
            FirebaseAnalytics.LogEvent("Start_"+levelName+"_Dif_"+iDifficulty.ToString());
        }
	}    
    public static void FBA_EventLevelEnd (string levelName, int iDifficulty, int success) {
        string s = "WIN";
        if (success == 0){
            s = "FAIL";
        }
        Parameter[] paramEvent = {
			new Parameter ("ParameterLevelName", levelName),
            new Parameter ("ParameterSuccess", success)
		};
        if (flagFirebaseStatus) {
		    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, paramEvent);
            FirebaseAnalytics.LogEvent("End_"+levelName+"_Dif_"+iDifficulty.ToString()+"_"+s);
        }
	}

    public static void FBA_EventMonsterDie (int iMonster, int nMonster) {
        if (flagFirebaseStatus) {
            FirebaseAnalytics.LogEvent("MonsterDie_"+iMonster.ToString()+"_of_"+nMonster.ToString());
        }
	}
    public static void FBA_EventMonsterAdd () {
        if (flagFirebaseStatus) {
            FirebaseAnalytics.LogEvent("MonsterAdd");
        }
	}    

}
