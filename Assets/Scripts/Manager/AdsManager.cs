using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public static Action onRewardedAddSucces;
    public static Action onRewardedAddFailed;
    
    void Start()
    {
        Advertisement.Initialize("4917547");
        Advertisement.AddListener(this);
    }

    public static void PlayRewardedAdd(Action onSucces,Action onFail)
    {
        onRewardedAddSucces = onSucces;
        onRewardedAddFailed = onFail;
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Rewarded_Android");
        }
    }


    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Ads ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        onRewardedAddSucces?.Invoke();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ads started");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            onRewardedAddSucces?.Invoke();
        }
        
    }
}
