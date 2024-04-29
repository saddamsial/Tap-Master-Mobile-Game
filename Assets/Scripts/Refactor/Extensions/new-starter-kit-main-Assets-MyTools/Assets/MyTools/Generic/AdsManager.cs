using Core.Data;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : SingletonMonoBehaviour<AdsManager>
{
    [SerializeField] ApplovinManager _applovinManager;
    [SerializeField] AdMobManager _admnobManager;
    public bool CanLoadAds;
    private bool _isShowRemoveAds = true;
    public bool IsShowRemoveAds
    {
        get
        {
            return _isShowRemoveAds;
        }
        set
        {
            _isShowRemoveAds = value;
        }
    }

    public override void Awake()
    {
        base.Awake();
        ActionEvent.OnShowBanner += ShowBanner;
        ActionEvent.OnHideBanner += HideBanner;
    }

    private void OnEnable()
    {
        _applovinManager?.InitAdsEvent();
        _admnobManager?.InitSDK();
    }

    private void Start()
    {
        _applovinManager?.InitAds();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.I))
    //     {
    //         Debug.Log("Show Interstitial");
    //         ShowInter(null);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         Debug.Log("Show Rewarded");
    //         ShowRewarded(null);
    //     }
    // }

    private void OnDestroy()
    {
        ActionEvent.OnShowBanner -= ShowBanner;
        ActionEvent.OnHideBanner -= HideBanner;
    }

    public void ShowBanner()
    {
        //if (!PlayerData.UserData.IsNotRemoveAds) return;
        //if (!GlobalSetting.NetWorkRequirements() || PlayerData.UserData.HighestLevel + 1 < AppConfig.Instance.BannerAdLevel) return;

        _admnobManager?.LoadBannerAd();
    }

    public void HideBanner()
    {
        _admnobManager?.DestroyBannerView();
    }

    //     public void SwichMREC()
    //     {
    //         // _ironSourceManager.SwitchMREC();
    //     }

    //     public void SwichBanner()
    //     {
    //         // _ironSourceManager.SwitchBANNER();
    //     }

    public void ShowInter(Action callBack, string localtion = "")
    {
        _applovinManager?.ShowInterstitial(callBack, localtion);
        GlobalEventManager.Instance.OnShowInterstitial();
    }

    public void ShowRewarded(Action<bool> closeCallBack, Action onClick = null, string localtion = "")
    {
        _applovinManager?.ShowRewardedAd(closeCallBack, onClick, localtion);
        GlobalEventManager.Instance.OnShowRewarded(_PlayerData.UserData.CurrentLevel);
    }

    public void ShowNativeOverlay(){
        _admnobManager?.RenderAd();
    }
}
