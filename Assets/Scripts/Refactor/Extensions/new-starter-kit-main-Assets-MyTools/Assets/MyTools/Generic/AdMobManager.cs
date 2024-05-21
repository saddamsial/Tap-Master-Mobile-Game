using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{
    [Header("---AD UNIT ID---")]
    [Header("ANDROID")]
    public string androidBannerID;
    public string androidInterstitialID;
    public string androidrewardID;
    public string androidOpenAdsID;
    public string androidNativeID;
    [Header("IOS")]
    public string iosBannerID;
    public string iosInterstitialID;
    public string iosRewardID;
    public string iosOpenAdsID;
    public string iosNativeID;

    private string _bannerID;
    private string _interID;
    private string _rewardID;
    private string _openAdsID;
    private string _nativeID;

    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    private AppOpenAd _appOpenAd;
    private NativeOverlayAd _nativeOverlayAd;
    private NativeAd _nativeAd;


    private AdLoader _adLoader;


    private Action closeInter;
    private Action closeVideo;
    private Action<bool> closeVideoWithEarnReward;


    private void Awake()
    {
#if UNITY_ANDROID
        _bannerID = androidBannerID;
        _interID = androidInterstitialID;
        _rewardID = androidrewardID;
        _openAdsID = androidOpenAdsID;
        _nativeID = androidNativeID;
#elif UNITY_IOS
        _bannerID = iosBannerID;
        _interID = iosInterstitialID;
        _rewardID = iosRewardID;
        _openAdsID = iosOpenAdsID;
        _nativeID = iosNativeID;
#else
        _bannerID = "unexpected_platform";
        _interID = "unexpected_platform";
        _rewardID = "unexpected_platform";
        _openAdsID = "unexpected_platform";
        _nativeID = "unexpected_platform";
#endif

        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    public void InitAdEvent()
    {
        //  Banner
        ListenToAdEventsBanner();

        //Inter
        //RegisterEventHandlersInter(_interstitialAd);
        //RegisterReloadHandlerInter(_interstitialAd);

        ////Rewarded
        //RegisterEventHandlersRewarded(_rewardedAd);
        //RegisterReloadHandlerRewarded(_rewardedAd);

        //AppOpen
        RegisterEventHandlers(_appOpenAd);
    }

    public void InitSDK()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            CreateBannerView();
            //LoadBannerAd();
            //LoadInterstitialAd();
            //LoadRewardedAd();
            LoadAppOpenAd();
            LoadNativeOverLayAd();
            InitAdEvent();
            //RequestNativeAd();
        });
    }

    #region BANNER
    private bool _bannerAdsAvailable;
    /// <summary>
    /// listen to events the banner view may raise.
    /// </summary>
    private void ListenToAdEventsBanner()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
            _bannerAdsAvailable = true;
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
            _bannerAdsAvailable = false;
            RetryLoadBannerAds();
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));

            //AnalyticsRevenueAds.SendRevAdmobToAdjust(adValue);
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyBannerView();
        }

        // Create a 320x50 banner at top of the screen
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        // AdSize adSize = new AdSize(250, 250);
        _bannerView = new BannerView(_bannerID, adSize, AdPosition.Bottom);
        // Debug.Log(_bannerView.);
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadBannerAd()
    {
        //if (!PlayerData.UserData.IsNotRemoveAds) return;
        //if (!GlobalSetting.NetWorkRequirements() || PlayerData.UserData.HighestLevel + 1 < AppConfig.Instance.BannerAdLevel) return;

        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // Create an extra parameter that aligns the bottom of the expanded ad to the
        // bottom of the bannerView.
        adRequest.Extras.Add("collapsible", "bottom");
        
        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    void RetryLoadBannerAds()
    {
        if (!_bannerAdsAvailable)
        {
            Debug.Log("Banner Ad Load Failed. Try again");
            LoadBannerAd();
        }
    }

    public void ShowBanner()
    {
        if (_bannerView != null)
        {
            _bannerView.Show();
        }
    }

    public void HideBanner()
    {
        if (_bannerView != null)
        {
            _bannerView.Hide();
        }
    }

    #endregion

    #region INTERSTITIAL
    private void RegisterEventHandlersInter(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
    }

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            DestroyInterstitialAd();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_interID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
                RegisterEventHandlersInter(_interstitialAd);
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void DestroyInterstitialAd()
    {
        _interstitialAd.Destroy();
    }
    #endregion

    #region VIDEO REWARDS
    private void RegisterEventHandlersRewarded(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            DestroyRewardedAd();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_rewardID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterEventHandlersRewarded(_rewardedAd);
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((GoogleMobileAds.Api.Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    private void DestroyRewardedAd()
    {
        _rewardedAd.Destroy();
    }
    #endregion

    #region AppOpenAd
    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd()
    {
        // Clean up the old ad before loading a new one.
        if (_appOpenAd != null)
        {
            DestroyAppOpen();
            _appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_openAdsID, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                _appOpenAd = ad;
                RegisterEventHandlers(_appOpenAd);
            });
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            AnalyticsRevenueAds.SendRevAdmobToAdjust(adValue.Value);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            LoadAppOpenAd();
        };
    }

    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        if (_appOpenAd != null && _appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            _appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }

    private void DestroyAppOpen()
    {
        _appOpenAd.Destroy();
    }

    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }

    public bool IsAdAvailable
    {
        get
        {
            return _appOpenAd != null;
            //&& _appOpenAd.IsLoaded()
            //&& DateTime.Now < _expireTime;
        }
    }
    #endregion

    #region  NATIVE OVERLAY


    /// <summary>
    /// Loads the ad.
    /// </summary>
    public void LoadNativeOverLayAd()
    {
        // Clean up the old ad before loading a new one.
        if (_nativeOverlayAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading native overlay ad.");

        // Create a request used to load the ad.
        var adRequest = new AdRequest();

        // Optional: Define native ad options.
        var options = new NativeAdOptions(
            new NativeAdOptions()
            {
                MediaAspectRatio = MediaAspectRatio.Landscape,
                AdChoicesPlacement = AdChoicesPlacement.TopLeftCorner,
                VideoOptions = new VideoOptions()
            }
        );

        Debug.Log("Loading native overlay ad with options : " + options);

        // Send the request to load the ad.
        NativeOverlayAd.Load(_nativeID, adRequest, options,
            (NativeOverlayAd ad, LoadAdError error) =>
                {
                    if (error != null)
                    {
                        Debug.LogError("Native Overlay ad failed to load an ad " +
                                    " with error: " + error);
                        return;
                    }

                    // The ad should always be non-null if the error is null, but
                    // double-check to avoid a crash.
                    if (ad == null)
                    {
                        Debug.LogError("Unexpected error: Native Overlay ad load event " +
                                    " fired with null ad and null error.");
                        return;
                    }

                    // The operation completed successfully.
                    Debug.Log("Native Overlay ad loaded with response : " + ad.GetResponseInfo());
                    _nativeOverlayAd = ad;

                    // Register to ad events to extend functionality.
                    RegisterEventHandlers(ad);
                });
    }

    private void RegisterEventHandlers(NativeOverlayAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            //AnalyticsRevenueAds.SendRevAdmobToAdjust(adValue);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            LoadAppOpenAd();
        };
    }

    /// <summary>
    /// Renders the ad.
    /// </summary>
    public void RenderAd()
    {
        if (_nativeOverlayAd != null)
        {
            Debug.Log("Rendering Native Overlay ad.");

            // Define a native template style with a custom style.
            var style = new NativeTemplateStyle
            {
                TemplateId = NativeTemplateId.Medium,
                MainBackgroundColor = Color.red,
                CallToActionText = new NativeTemplateTextStyle
                {
                    BackgroundColor = Color.green,
                    TextColor = Color.white,
                    FontSize = 9,
                    Style = NativeTemplateFontStyle.Bold
                }
            };

            // Renders a native overlay ad at the default size
            // and anchored to the bottom of the screne.
            _nativeOverlayAd.RenderTemplate(style, AdPosition.Bottom);
        }
        else
        {
            Debug.LogError("Native Overlay ad is not ready yet.");
        }
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAd()
    {
        if (_nativeOverlayAd != null)
        {
            Debug.Log("Showing Native Overlay ad.");
            _nativeOverlayAd.Show();
        }
        else
        {
            Debug.Log("Native Overlay ad is not ready yet.");
        }
    }

    /// <summary>
    /// Destroys the native overlay ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_nativeOverlayAd != null)
        {
            Debug.Log("Destroying native overlay ad.");
            _nativeOverlayAd.Destroy();
            _nativeOverlayAd = null;
        }
    }
    #endregion

//     #region NATIVE

//         #region Native Ad
//     private bool _nativeAdLoaded;

//     public void RequestNativeAd()
//     {
//         AdLoader adLoader = new AdLoader.Builder(_nativeAdId)
//             .ForNativeAd()
//             .Build();
//         adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
//         adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
//         adLoader.OnNativeAdImpression += this
//             .HandleNativeImpression;
//         adLoader.OnNativeAdClicked += this.HandleNativeAdClicked;
//         adLoader.LoadAd(new AdRequest());
//     }

//     private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//     {
//         Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
//         _nativeAdLoaded = false;
//         RequestNativeAd();
//     }

//     private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
//     {
//         Debug.Log("Native ad loaded.");

//         this._nativeAd = args.nativeAd;

//         if (CreateNativeAd())
//         {
//             _nativeAdLoaded = true;
//             Debug.Log("Native Ad Show");
//         }
//         else
//         {
//             Debug.Log("Native Ad Show Fail");
//             _nativeAdLoaded = false;
//             RequestNativeAd();
//         }
//     }

//     private void HandleNativeImpression(object sender, EventArgs args)
//     {
//         Debug.Log("Handle native ad Impression!");
//     }

//     private void HandleNativeAdClicked(object sender, EventArgs args)
//     {
//         Debug.Log("Handle native ad clicked!");
//     }

//     public bool CreateNativeAd()
//     {
//         Debug.Log($"===>set object native <===");
// #if UNITY_EDITOR

//         _adNativeObj.body.text = "<color=blue>" +   + "</color>\n";
//         _adNativeObj.setAdBG(new Texture2D[3].ToList());
//         return true;
// #endif

//         List<Texture2D> imagetexture = _nativeAd.GetImageTextures();
//         if (imagetexture.Any())
//         {
//             List<GameObject> Bgs = _adNativeObj.setAdBG(imagetexture);

//             _nativeAd.RegisterImageGameObjects(Bgs);
//         }


//         Texture2D iconTexture = _nativeAd.GetIconTexture();
//         if (iconTexture)
//         {
//             _adNativeObj.adIcon.texture = iconTexture;

//             if (!_nativeAd.RegisterIconImageGameObject(_adNativeObj.adIcon.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register adIcon error <====");
//                 return false;
//             }
//         }
//         else
//         {
//             _adNativeObj.adIcon.gameObject.SetActive(false);
//         }

//         string headLineText = _nativeAd.GetHeadlineText();
//         if (!string.IsNullOrEmpty(headLineText))
//         {
//             _adNativeObj.headLine.text = headLineText;
//             if (!_nativeAd.RegisterHeadlineTextGameObject(_adNativeObj.headLine.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register adHeadline error <====");
//                 return false;
//             }
//         }
//         else
//         {
//             _adNativeObj.headLine.gameObject.SetActive(false);
//         }

//         Texture2D iconChoice = _nativeAd.GetAdChoicesLogoTexture();
//         if (iconChoice != null)
//         {
//             _adNativeObj.adChoice.texture = iconChoice;
//             if (!_nativeAd.RegisterAdChoicesLogoGameObject(_adNativeObj.adChoice.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register adChoiceIcon error <====");
//                 return false;
//             }
//         }
//         else
//         {
//             _adNativeObj.adChoice.gameObject.SetActive(false);
//         }

//         string CTAText = _nativeAd.GetCallToActionText();
//         if (!string.IsNullOrEmpty(CTAText))
//         {
//             _adNativeObj.callToAction.text = CTAText;
//             if (!_nativeAd.RegisterCallToActionGameObject(_adNativeObj.callToAction.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register CTA error <====");
//                 return false;
//             }
//         }

//         string advertiseText = _nativeAd.GetAdvertiserText();
//         if (!string.IsNullOrEmpty(advertiseText))
//         {
//             _adNativeObj.advertiser.text = advertiseText;
//             if (!_nativeAd.RegisterAdvertiserTextGameObject(_adNativeObj.advertiser.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register advertise text error!<====");
//                 return false;
//             }
//         }
//         else
//         {
//             _adNativeObj.advertiser.gameObject.SetActive(false);
//         }

//         string bodyText = _nativeAd.GetBodyText();
//         if (!string.IsNullOrEmpty(bodyText))
//         {
//             _adNativeObj.body.text = bodyText;
//             if (!_nativeAd.RegisterBodyTextGameObject(_adNativeObj.body.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register body text error!<====");
//                 return false;
//             }
//         }
//         else
//         {
//             _adNativeObj.body.gameObject.SetActive(false);
//         }

//         string priceText = _nativeAd.GetPrice();
//         if (!string.IsNullOrEmpty(priceText))
//         {
//             _adNativeObj.price.text = priceText;
//             if (!_nativeAd.RegisterPriceGameObject(_adNativeObj.price.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register price text error!<====");
//                 _adNativeObj.price.gameObject.SetActive(false);
//                 return false;
//             }
//         }
//         else
//         {
//             _adNativeObj.price.gameObject.SetActive(false);
//         }

//         string storeText = _nativeAd.GetStore();
//         if (!string.IsNullOrEmpty(storeText))
//         {
//             _adNativeObj.store.text = storeText;
//             if (!_nativeAd.RegisterStoreGameObject(_adNativeObj.store.gameObject))
//             {
//                 Debug.LogError($"===> Native Ad register store text error!<====");
//                 _adNativeObj.store.gameObject.SetActive(false);
//                 return false;
//             }
//         }
//         else
//         {
//             _adNativeObj.store.gameObject.SetActive(false);
//         }
//         return true;
//     }

//     public void ShowNative(Action<AdNativeObject> callBack = null)
//     {
//         StartCoroutine(WaitShowNative(callBack));
//     }
//     /// <summary>
//     /// ID is index of ID native AD
//     /// Actually, Native AD object already create right when native AD success!
//     /// Show function using for assign Native AD object into right canvas
//     /// <code>
//     /// _= AdMHighFather.Instant.ShowNative(0,(nativePanel)=>{
//     ///       nativePanel.transform.SetParent(canvas.transform);
//     ///       nativePanel.transform.localScale = Vector3.one;
//     ///       nativePanel.transform.localPosition = Vector3.zero;
//     ///       nativePanel.rectTransform.sizeDelta = Vector2.zero;
//     ///       nativePanel.rectTransform.anchorMax = new Vector2(1, 0.4f);
//     /// 
//     /// })
//     /// </code>
//     /// </summary> 
//     /// <param name="callback">Callback using for assign Native AD object into right canvas</param>
//     private IEnumerator WaitShowNative(Action<AdNativeObject> callBack = null)
//     {
//         if (!_nativeAdLoaded)
//             yield break;
//         Debug.Log("Show Native");
// #if UNITY_EDITOR
//         yield return new WaitForSeconds(0.5f);
// #else
//         while (!_nativeAdLoaded)
//         {
//             yield return new WaitForSeconds(5f);
//         }
// #endif
//         _adNativeObj.gameObject.SetActive(true);
//         try
//         {
//             callBack?.Invoke(_adNativeObj);
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"===>Error on callback show native! error: " + e.ToString() + "<====");
//         }
//         _adNativeObj.FitCollider();
//     }

//     public void HideNative(Action<AdNativeObject> callBack = null)
//     {
//         StartCoroutine(WaitHideNative(callBack));
//     }

//     public IEnumerator WaitHideNative(Action<AdNativeObject> callBack = null)
//     {
//         if (!_nativeAdLoaded)
//         {
//             Debug.LogError("===> item ad native doesnt exist! ");
//             callBack?.Invoke(null);
//             yield break;
//         }

// #if UNITY_EDITOR
//         yield return new WaitForSeconds(0.5f);
// #else
//         while (!_nativeAdLoaded)
//         {
//             yield return new WaitForSeconds(5f);
//         }
// #endif
//         _adNativeObj.gameObject.SetActive(false);
//         _adNativeObj.transform.SetParent(this.transform);

//         try
//         {
//             callBack?.Invoke(_adNativeObj);
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"===>Error on callback show native! error: " + e.ToString() + "<====");
//         }
//     }
//     #endregion

//     #endregion
}


