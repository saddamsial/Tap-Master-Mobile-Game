using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplovinManager : MonoBehaviour
{
    [Header("---SDK ID ANDROID---")]
    public string andoidSdkID;
    [Header("---SDK ID IOS---")]
    public string iosSdkID;
    [Header("---AD UNIT ID ANDROID---")]
    public string androidBannerID;
    public string androidInterstitialID;
    public string androidrewardID;
    public string androidOpenAdsID;
    [Header("---AD UNIT ID IOS---")]
    public string iosBannerID;
    public string iosInterstitialID;
    public string iosRewardID;
    public string iosOpenAdsId;

    private string _sdkID;
    private string _bannerID;
    private string _interID;
    private string _rewardID;
    private string _openAdsID;

    int retryAttempt;

    private Action actionCloseInter;
    private Action<bool> actVideoClosedCallback;
    private Action actVideoClickedCallback;

    private DateTime? lastTime = null;
//     bool timeShowAds => lastTime == null ? true : (DateTime.Now - lastTime)?.TotalSeconds > AppConfig.Instance.InterFrequencyTime;/* inter ad tần suất(s)*/
    private bool _canClaimReward;

    private void Awake()
    {
#if UNITY_ANDROID
        _sdkID = andoidSdkID;
        _bannerID = androidBannerID;
        _interID = androidInterstitialID;
        _rewardID = androidrewardID;
        _openAdsID = androidOpenAdsID;
#elif UNITY_IOS
        _sdkID = iosSdkID;
        _bannerID = iosBannerID;
        _interID = iosInterstitialID;
        _rewardID = iosRewardID;
        _openAdsID = iosOpenAdsId;
#else
        _sdkID = "unexpected_platform";
        _bannerID = "unexpected_platform";
        _interID = "unexpected_platform";
        _rewardID = "unexpected_platform";
        _openAdsID ="unexpected_platform";
#endif
    }

    // public int GetBannerHeight()
    // {
    //    if (MaxSdkUtils.IsTablet())
    //    {
    //        return Mathf.RoundToInt(90 * Screen.dpi / 160);
    //    }
    //    else
    //    {
    //        if (Screen.height <= 400 * Mathf.RoundToInt(Screen.dpi / 160))
    //        {
    //            return 32 * Mathf.RoundToInt(Screen.dpi / 160);
    //        }
    //        else if (Screen.height <= 720 * Mathf.RoundToInt(Screen.dpi / 160))
    //        {
    //            return 42 * Mathf.RoundToInt(Screen.dpi / 160);
    //        }
    //        else
    //        {
    //            return 50 * Mathf.RoundToInt(Screen.dpi / 160);
    //        }
    //    }
    // }

    public void InitAdsEvent()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads   

            // Load the first interstitial
            LoadInterstitial();

            // Load the first rewarded ad
            LoadRewardedAd();

            AdsManager.Instance.CanLoadAds = true;
        };

        //InitializeAd_impressionEvent();
        // InitializeBannerAdsEvent();
        InitializeInterstitialAdsEvent();
        InitializeRewardedAdsEvent();
    }

    public void InitAds()
    {
        MaxSdk.SetSdkKey(_sdkID);
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        //   Debug.Log("Ads ");
    }
//     #region InitBanner
//     public void InitializeBannerAdsEvent()
//     {
//         MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
//         MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
//         MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
//         MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
//         MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
//         MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

//         // var height = MaxSdk.GetBannerLayout(bannerID);
//         //  height.position.y;
//     }


//     private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         //GlobalEventManager.Instance.OnAds_Banner_times();
//     }

//     private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

//     private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {

//     }

//     private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         //MaxSdk.HideBanner(adUnitId);
//     }

//     private void LoadBanner()
//     {
//         // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
//         // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
//         MaxSdk.CreateBanner(_bannerID, MaxSdkBase.BannerPosition.TopCenter);

//         // Set background or background color for banners to be fully functional
//         //    MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);
//     }

//     public void ShowBanner()
//     {
//         MaxSdk.ShowBanner(_bannerID);
//         // GlobalEventManager.Instance.AdBannerTimes();
//     }

//     #endregion

    #region InitInter
    public void InitializeInterstitialAdsEvent()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(_interID);
        //  Debug.Log("Init Intern");
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'     
        // Reset retry attempt    
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //Debug.Log("Inter run");
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        actionCloseInter?.Invoke();
        actionCloseInter = null;
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
        lastTime = DateTime.Now;

        // GlobalEventManager.Instance.OnCloseInterstitial();
    }

    public void ShowInterstitial(Action Close_CallBack = null, string location = "")
    {
        if (!GlobalSetting.NetWorkRequirements() /* || !timeShowAds || PlayerData.UserData.HighestLevel < AppConfig.Instance.InterAdLevel || !PlayerData.UserData.IsNotRemoveAds*/)
        {
            // Debug.LogError("Dont show ads");
            Close_CallBack?.Invoke();
            return;
        }
        // if (AdsManager.Instance.IsShowRemoveAds)
        // {
        //     AdsManager.Instance.IsShowRemoveAds = false;
        //     // PopupAd_Remove.Instance.Show((n) =>
        //     // {
        //     //     // Debug.Log(n);
        //     //     if (n)
        //     //     {
        //     //         Close_CallBack?.Invoke();
        //     //         return;
        //     //     }
        //     //     else
        //     //     {
        //     //         WaitShowInter(Close_CallBack);
        //     //     }
        //     // });
        //     return;
        // }
        WaitShowInter(Close_CallBack);
    }

    private void WaitShowInter(Action Close_CallBack = null)
    {
        if (MaxSdk.IsInterstitialReady(_interID))
        {
            Debug.Log("Show Interstitial 1");
            actionCloseInter = Close_CallBack;
            MaxSdk.ShowInterstitial(_interID);
            // PopupLoadingAd.Instance.Show(() =>
            // {
            //     actionCloseInter = Close_CallBack;
            //     MaxSdk.ShowInterstitial(_interID);
            // });
        }
        else
        {
            Close_CallBack?.Invoke();
        }
    }
    #endregion

    #region Init Rewarded
    public void InitializeRewardedAdsEvent()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(_rewardID);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }
    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        actVideoClosedCallback?.Invoke(_canClaimReward);
        actVideoClosedCallback = null;
        LoadRewardedAd();
        lastTime = DateTime.Now;
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        _canClaimReward = true;
        actVideoClosedCallback?.Invoke(_canClaimReward);
        actVideoClosedCallback = null;
        _canClaimReward = false;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

    public void ShowRewardedAd(Action<bool> closeCallBack, Action onClick = null, string localtion = "")
    {
        if (!GlobalSetting.NetWorkRequirements())
        {
            //ActionEvent.OnShowToast?.Invoke(Const.KEY_NO_INTERNET);
            return;
        }

        if (MaxSdk.IsRewardedAdReady(_rewardID))
        {
            MaxSdk.ShowRewardedAd(_rewardID);
            actVideoClosedCallback = closeCallBack;
            actVideoClickedCallback = onClick;
        }
        else
        {
            LoadRewardedAd();
            //ActionEvent.OnShowToast?.Invoke(Const.KEY_CANNOT_LOAD_ADS);
        }
    }
    #endregion

//     public void InitializeAd_impressionEvent()
//     {
//         MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
//         MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
//         //  MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
//         MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
//     }

//     // private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
//     // {
//     //     double revenue = impressionData.Revenue;
//     //     var impressionParameters = new[] {
//     //             new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
//     //             new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
//     //             new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
//     //             new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
//     //             new Firebase.Analytics.Parameter("value", revenue),
//     //             new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
//     //     };
//     //     Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
//     // }
}
