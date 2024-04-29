using System;
using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : SingletonMonoBehaviour<GlobalEventManager>
{

    public Action<string, Parameter[]> EvtSendEvent;
    //  public Action<string> EvtUpdateUserProperties;

    #region Tracking Ads
    // public static int interAdCount
    // {
    //     get => PlayerPrefs.GetInt("INTER_AD_COUNT", 0);
    //     set => PlayerPrefs.SetInt("INTER_AD_COUNT", value);
    // }

    // public static int rewardedAdCount
    // {
    //     get => PlayerPrefs.GetInt("REWARDED_AD_COUNT", 0);
    //     set => PlayerPrefs.SetInt("REWARED_AD_COUNT", value);
    // }

    //#region tracking Banner
    // public int bannerAdCount
    // {
    //     get => PlayerPrefs.GetInt("BANNER_AD_COUNT", 0);
    //     set => PlayerPrefs.SetInt("BANNER_AD_COUNT", value);
    // }

    // public void AdBannerTimes()
    // {
    //     bannerAdCount += 1;
    //     Parameter[] parameter = new Parameter[]
    //     {
    //         new Parameter("amount", bannerAdCount.ToString()),
    //     };
    //     FirebaseAnalytics.LogEvent("ads_banner_times", parameter);
    // }
    // #endregion

    // public void AdIntertitialTimes()
    // {
    //     interAdCount += 1;
    //     Parameter[] parameter = new Parameter[]
    //     {
    //         new Parameter("amount", interAdCount.ToString()),
    //     };
    //     FirebaseAnalytics.LogEvent("ads_interstitial_times", parameter);
    // }

    // public void AdRewardedTimes()
    // {
    //     rewardedAdCount += 1;
    //     Parameter[] parameter = new Parameter[]
    //     {
    //         new Parameter("amount", rewardedAdCount.ToString()),
    //     };
    //     FirebaseAnalytics.LogEvent("ads_rewarded_times", parameter);
    // }

    public void OnShowInterstitial()
    {
        FirebaseAnalytics.LogEvent("inter_show");
    }

    public void OnCloseInterstitial()
    {
        FirebaseAnalytics.LogEvent("inter_close");
    }

    public void OnShowRewarded()
    {
        FirebaseAnalytics.LogEvent("reward_show");
    }


    public void OnRewardedComplete()
    {
        FirebaseAnalytics.LogEvent("reward_complete");
    }

    public void OnShowRewarded(int level)
    {
        Parameter[] parameter = new Parameter[]
        {
                new Parameter("level", level.ToString()),
        };
        FirebaseAnalytics.LogEvent("reward_show", parameter);
    }

    public void OnRewardedComplete(int level)
    {
        Parameter[] parameter = new Parameter[]
        {
                new Parameter("level", level.ToString()),
        };
        FirebaseAnalytics.LogEvent("reward_complete", parameter);
    }

    public void OnShowRewarded(int level, string adLocation){
        Parameter[] parameter = new Parameter[]
        {
                new Parameter("level", level.ToString()),
                new Parameter("ad_location", adLocation),
        };
        FirebaseAnalytics.LogEvent("reward_show", parameter);
    }

    public void OnRewardedComplete(int level, string adLocation){
        Parameter[] parameter = new Parameter[]
        {
                new Parameter("level", level.ToString()),
                new Parameter("ad_location", adLocation),
        };
        FirebaseAnalytics.LogEvent("reward_complete", parameter);
    }
    #endregion

    #region Tracking GamePlay
    public void OnLevelPlay(int _level)
    {
        Parameter[] parameter = new Parameter[]
       {
            new Parameter("level", _level.ToString()),
       };
        EvtSendEvent?.Invoke($"level_play", parameter);
    }

    public void OnLevelComplete(int _level)
    {
        Parameter[] parameter = new Parameter[]
      {
            new Parameter("level", _level.ToString()),
      };
        EvtSendEvent?.Invoke($"level_win", null);
    }

    public void OnLevelReplay(int _level)
    {
        Parameter[] parameter = new Parameter[]
      {
            new Parameter("level", _level.ToString()),
      };
        EvtSendEvent?.Invoke($"level_replay", null);
    }

    public void OnLevelLose(int _level)
    {
        Parameter[] parameter = new Parameter[]
      {
            new Parameter("level", _level.ToString()),
      };
        EvtSendEvent?.Invoke($"level_lose", null);
    }

    public void ChapterOnComplete(int _chapterIndex)
    {
        EvtSendEvent?.Invoke($"chapter_{_chapterIndex}_completed", null);
    }
    #endregion
}
