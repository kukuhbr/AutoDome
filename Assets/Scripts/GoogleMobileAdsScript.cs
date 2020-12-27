using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using GoogleMobileAds.Api;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private RewardedAd energyRewardedAd;
    private RewardedAd gameOverRewardedAd;

    //test ad
    private string appId = "";
    private string adUnitTest = "ca-app-pub-3940256099942544/5224354917";
    void Start()
    {
        //MobileAds.Initialize(initStatus => { });
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

    void HandleUserEarnedReward(object sender, EventArgs args) {

    }

    void HandleRewardedAdClosed(object sender, EventArgs args)
    {

    }
}
