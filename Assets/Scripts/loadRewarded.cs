using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class loadRewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string androidAdUnitId;
    public string iosAdUnitId;

    public GameObject endGameScreen;
    string adUnitId;

    void Awake()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif

    }

    public void LoadAd()
    {
        print("Loading Rewarded!!");
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adUnitId))
        {
            print("Rewarded loaded!!");
            ShowAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("Rewarded failed to load");
    }



    public void ShowAd()
    {
        print("showing Rewarded ad!!");
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Rewarded clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        print("Rewarded show complete");
        if (UiDeadEnd.revivead == false){
            Debug.Log("revive ads");
            GameObject.FindWithTag("EndGameScreen").GetComponent<UiDeadEnd>().ReviveCharacter();   
        }else if (UiDeadEnd.doubleCoins == false){
            GameObject.FindWithTag("EndGameScreen").GetComponent<UiDeadEnd>().DoubleCoins();
        }
             
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("Rewarded show failure");

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("Rewarded show start");

    }
}