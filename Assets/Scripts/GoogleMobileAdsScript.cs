using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using GoogleMobileAds.Api;

public class GoogleMobileAdsScript : MonoBehaviour
{
    public static GoogleMobileAdsScript instance;
    private RewardedAd energyRewardedAd;
    private RewardedAd gameOverRewardedAd;

    //test ad
    private string appId = "ca-app-pub-8471432327502017~5389357621";
    private List<string> deviceIds;
    public static string rewardedAdTest = "ca-app-pub-3940256099942544/5224354917";
    public static string interstitialAdTest = "ca-app-pub-3940256099942544/1033173712";
    public static string energyAdId = "ca-app-pub-8471432327502017/5292907285";
    public static string itemsAdId = "ca-app-pub-8471432327502017/8437945377";
    public static string interstitialAd = "ca-app-pub-8471432327502017/3269404829";
    void Start()
    {
        deviceIds = new List<string>();
        deviceIds.Add(SystemInfo.deviceUniqueIdentifier);
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        //deviceIds.Add();
        MobileAds.Initialize(initStatus => { });
        MobileAds.SetRequestConfiguration(requestConfiguration);
        instance = this;
    }
}
