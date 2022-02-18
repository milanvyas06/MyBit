using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ADState
{
    Null,
    Requested,
    Failed,
    Loaded
}

public class GoogleADHandlers
{
    private BannerView bannerView;

    private InterstitialAd interstitial;

    private RewardBasedVideoAd rewardBasedVideo;

    public bool isInterstitialLoaded = false;
    public bool isRewardAdLoaded = false;

    public ADState rewardAdState = ADState.Null;

    private string appId = "ca-app-pub-3940256099942544~3347511713";

    private string adUnitIdBanner = "ca-app-pub-3940256099942544/6300978111";
    private string adUnitIdInterstitial = "ca-app-pub-3940256099942544/1033173712";
    private string adUnitIdRewarded = "ca-app-pub-3940256099942544/5224354917";

    public event Action DoRewardByUnlockingEffect;
    public event Action DoRewardToUserWaterMark;

    public event Action onRewardAdLoaded;
    public event Action onRewardAdFailedToLoad;
    public event Action onRewardAdClosed;

    public event Action OnInterAdClosed;

    public GoogleADHandlers()
    {
        MobileAds.Initialize(appId);
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
    }

    public void RequestBanner()
    {
        Debug.Log("Google Banner Loading...");
        bannerView = new BannerView(adUnitIdBanner, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().AddTestDevice("D3CF4354F5269944092A66C9FA3DE361").AddTestDevice("ea319f9d-9b9d-4b4f-8a0d-755e16b3787f").AddTestDevice("0434B91D833247ADCA076C0951BD9DC6").Build();
        bannerView.LoadAd(request);
        bannerView.OnAdLoaded += HanlderBannerAdLoaded;
    }

    public void RequestInterstitial()
    {
        interstitial = new InterstitialAd(adUnitIdInterstitial);
        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleInterstitialOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HanlderInterADFailToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnInterAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnInterAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        AdRequest request = new AdRequest.Builder().AddTestDevice("D3CF4354F5269944092A66C9FA3DE361").AddTestDevice("ea319f9d-9b9d-4b4f-8a0d-755e16b3787f").AddTestDevice("0434B91D833247ADCA076C0951BD9DC6").Build();
        interstitial.LoadAd(request);
    }

    public void RequestRewardBasedVideo()
    {
        rewardAdState = ADState.Requested;
        AdRequest request = new AdRequest.Builder().AddTestDevice("D3CF4354F5269944092A66C9FA3DE361").AddTestDevice("ea319f9d-9b9d-4b4f-8a0d-755e16b3787f").AddTestDevice("0434B91D833247ADCA076C0951BD9DC6").Build();
        this.rewardBasedVideo.LoadAd(request, adUnitIdRewarded);
        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    }

    #region RewardedAd callback handlers

    private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        rewardAdState = ADState.Null;
        onRewardAdClosed?.Invoke();
        isRewardAdLoaded = false;

        //REMOVE SUBSCRIBER
        ADManager.Instance.googleHanlder.DoRewardToUserWaterMark -= ADManager.Instance.RemoveWaterMark;
        ADManager.Instance.googleHanlder.DoRewardByUnlockingEffect -= ADManager.Instance.RewardToUserByUnlockAnEffect;
        //this.RequestRewardBasedVideo();
    }

    private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs e)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    private void HandleRewardBasedVideoOpened(object sender, EventArgs e)
    {

    }

    private void HandleRewardBasedVideoRewarded(object sender, Reward e)
    {
        rewardAdState = ADState.Null;
        Debug.Log("User is rewarded.!");
        DoRewardByUnlockingEffect?.Invoke();
        DoRewardToUserWaterMark?.Invoke();
    }

    private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
    {

    }

    private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        rewardAdState = ADState.Failed;
        isRewardAdLoaded = false;
        onRewardAdFailedToLoad?.Invoke();
        MonoBehaviour.print("HandleRewardedAdFailedToLoad event received with message: " + e.Message);
    }

    private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
    {
        rewardAdState = ADState.Loaded;
        isRewardAdLoaded = true;
        onRewardAdLoaded?.Invoke();
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    #endregion


    #region Banner callback handlers
    public void HanlderBannerAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }
    #endregion

    #region Interstitial callback handlers
    public void HandleInterstitialOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        isInterstitialLoaded = true;
    }

    public void HanlderInterADFailToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleOnInterAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnInterAdClosed(object sender, EventArgs args)
    {
        isInterstitialLoaded = false;
        OnInterAdClosed?.Invoke();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion

    #region DisplayRegion
    public bool ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            Debug.Log("Interstital ad is loaded.!");
            interstitial.Show();
            return true;
        }
        Debug.Log("Interstital ad is not loaded.!");
        return false;
    }

    public bool ShowRewardAd()
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
            return true;
        }
        return false;

    }
    #endregion

    #region DESTROY
    public void DestoryBannerAds()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    public void DestoryIntertitialAds()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
    }
    #endregion
}
