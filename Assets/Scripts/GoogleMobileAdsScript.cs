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
    public static string adUnitTest = "ca-app-pub-3940256099942544/5224354917";
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        instance = this;
    }

    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request =  new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        return rewardedAd;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HandleRewardedAdLoaded(object sender, EventArgs args)
    {

    }

    void HandleUserEarnedReward(object sender, EventArgs args)
    {

    }

    void HandleRewardedAdClosed(object sender, EventArgs args)
    {

    }
}
