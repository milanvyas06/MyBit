using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADManager : MonoBehaviour
{
    public static ADManager Instance;

    public GoogleADHandlers googleHanlder;

    public event Action OnInterAdClosed;


    public ADState rewardADState { get { return googleHanlder.rewardAdState; } }

    public bool isInterGoogleAdLoaded { get { return googleHanlder.isInterstitialLoaded; } }

    public bool isRewardAdLoaded { get { return googleHanlder.isRewardAdLoaded; } }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    private void Start()
    {
        googleHanlder = new GoogleADHandlers();
        try
        {
            googleHanlder.OnInterAdClosed += OnGoogleInerAdClosed;
            LoadGoogleInterAd();
            //LoadRewardAd();
        }
        catch (Exception arg)
        {
            UnityEngine.Debug.Log("Exception Load Intertitial Ad " + arg);
        }
    }

    private void OnGoogleInerAdClosed()
    {
        OnInterAdClosed?.Invoke();
    }

    public void RewardToUserByUnlockAnEffect()
    {

        Debug.Log("Rewarding to user.!");
        if (ExportManager.instance.themeToUnlock == 0 && ExportManager.instance.particleToUnlock == 0)
        {
            return;
        }

        if (ExportManager.instance.themeToUnlock != 0)
        {
            if (ExportManager.instance.themeToUnlock > 50)
            {
                UnlockTheme.instance.UnlockAllTheme(ExportManager.instance.themeToUnlock);
            }
            else
            {
                UnlockTransaction.instance.UnlockTransactionFromAndroid(ExportManager.instance.themeToUnlock);
            }
        }

        if (ExportManager.instance.particleToUnlock != 0)
        {
            UnlockParticleManager.instance.UnlockParticalsFromAndroid(ExportManager.instance.particleToUnlock);
        }

        ExportManager.instance.themeToUnlock = 0;
        ExportManager.instance.particleToUnlock = 0;
        googleHanlder.DoRewardByUnlockingEffect -= RewardToUserByUnlockAnEffect;
    }

    public void LoadBannerAds()
    {
        UnityEngine.Debug.Log("Loading Banner.!");
        googleHanlder.RequestBanner();
    }

    public void LoadGoogleBannerAd()
    {
        googleHanlder.RequestBanner();
    }

    public void LoadRewardAd()
    {
        Debug.Log(" Requesting for reward ad.!");
        googleHanlder.RequestRewardBasedVideo();
    }

    public void LoadGoogleInterstitialAd()
    {
        googleHanlder.RequestInterstitial();
    }

    public void LoadGoogleInterAd()
    {
        googleHanlder.RequestInterstitial();
    }

    public bool ShowGoogleInterstitialAds()
    {
        try
        {
            if (googleHanlder.ShowInterstitial())
            {
                UnityEngine.Debug.Log("Load google inter ad");

                return true;
            }
        }
        catch (Exception arg)
        {
            UnityEngine.Debug.Log("Exception show Intertitial Ad " + arg);
        }
        return false;
    }
    public bool ShowRewardAds()
    {
        try
        {
            if (googleHanlder.ShowRewardAd())
            {
                UnityEngine.Debug.Log("Load google reward ad");
                return true;
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Exception show reward Ad " + ex.Message);
        }
        return false;
    }
    public void DestoryBannerAds()
    {
        try
        {
            googleHanlder.DestoryBannerAds();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    internal void RemoveWaterMark()
    {
        SettingManager.instance.waterMarkButton.SetActive(false);
        ExportManager.instance.shouldRemoveWatermark = true;
        ADManager.Instance.googleHanlder.DoRewardToUserWaterMark -= ADManager.Instance.RemoveWaterMark;
    }
}
