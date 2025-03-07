using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDailyRewardItem : DailyRewardItem
{
    public override void CollectCoin()
    {
        this.panel.ExitPanel();
        this.isCollected = true;
        button.interactable = false;
        int coin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", coin + this.amount);

        PlayerPrefs.SetInt("Collect daily reward at index: ", 0);
        this.icon.sprite = isCollectedIcon;
        listController.ResetList();

        UIManager.instance.dailyRewardPopUp.PreActive(this.amount);
        UIManager.instance.dailyRewardPopUp.gameObject.SetActive(true);
        GameManager.Instance.isOnMenu = true;
        GameManager.Instance.camMoving.CanRotate = false;
    }
}
