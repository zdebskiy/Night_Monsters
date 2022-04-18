using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdMobManagerController : MonoBehaviour
{
    public static AdMobManagerController instance = null;
    private InterstitialAd interstitialAd;

    private bool AD_TEST_TIME = false;    
    private string adUnitId_TEST = "ca-app-pub-3940256099942544/1033173712";
    private string adUnitId_REAL = "ca-app-pub-1339618403883604/8266727048";

    private void Awake() {
        InitSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitAdMob();
    }

    private void OnEnable() {
        EventManager.OnGameLost_GM += ShowInterstitialAd;
        EventManager.OnGameWon_GM += ShowInterstitialAd;
    }

    private void OnDisable() {
        EventManager.OnGameLost_GM -= ShowInterstitialAd;
        EventManager.OnGameWon_GM -= ShowInterstitialAd;
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

    void InitAdMob(){
        MobileAds.Initialize(initStatus => {RequestAndLoadInterstitialAd();}
             /*
             foreach (var item in initStatus.getAdapterStatusMap.Keys){
                 var value = initStatus.getAdapterStatusMap.Get(item);
                 Debug.Log("("+item+") = "+value);
             }
             */
         );
    }

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        string adUnitId = adUnitId_TEST;
        Debug.Log("AdMob.status: Requesting Interstitial Ad.");

//    #if UNITY_EDITOR
//        string adUnitId = "unused";
//    #elif UNITY_ANDROID
        if (!AD_TEST_TIME) {
            adUnitId = adUnitId_REAL;
        }
        
//    #elif UNITY_IPHONE
//        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
//    #else
//        string adUnitId = "unexpected_platform";
//    #endif

        // Clean up interstitial before using it
        DestroyInterstitialAd();
        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        // Called when an ad request has successfully loaded.
        interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitialAd.OnAdClosed += HandleOnAdClosed;


        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("AdMob.status: Interstitial ad is not ready yet");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("AdMob.status: HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("AdMob.status: HandleFailedToReceiveAd event received with message: " + args.LoadAdError);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("AdMob.status: HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("AdMob.status: HandleAdClosed event received");
        RequestAndLoadInterstitialAd();   
    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    #endregion



}
