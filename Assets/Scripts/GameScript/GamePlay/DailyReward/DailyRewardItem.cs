using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [SerializeField] protected Image icon;
    [SerializeField] protected Button button;
    [SerializeField] protected int amount;
    [SerializeField] protected TMP_Text amountText;
    [SerializeField] protected RewardType rewardType;
    [SerializeField] protected Sprite isCollectedIcon;
    [SerializeField] protected DailyRewardListController listController;

    protected bool isCollected;
    protected DailyRewardPanelController panel;

    public bool IsCollected
    {
        get { return isCollected; }
        set { isCollected = value; }
    }

    private void Awake()
    {
        panel = UIManager.instance.dailyRewardPanel.GetComponent<DailyRewardPanelController>();
    }

    public DailyRewardListController ListController { get { return listController; } set { listController = value; } }

    public Button Button
    {
        get { return button; }
        set { button = value; }
    }

    public void InitializeReward(DailyRewardData data)
    {
        amount = data.amount;
        amountText.SetText(data.amount.ToString());
        rewardType = data.rewardType;
        if (isCollected)
        {
            icon.sprite = isCollectedIcon;
            button.interactable = false;
        }
        else
            icon.sprite = data.icon;
    }

    public void CollectReward()
    {
        DailyRewardSystem.isDailyCollected = true;
        switch (rewardType)
        {
            case RewardType.Coin:
                CollectCoin();
                break;
        }
    }

    public virtual void CollectCoin()
    {
        this.panel.ExitPanel();
        isCollected = true;
        PlayerPrefs.SetInt("Collected Daily Reward", 1);
        listController.IsDailyCollected = true;
        button.interactable = false;
        int coin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", coin + this.amount);

        int collectIndex = PlayerPrefs.GetInt("Collect daily reward at index: ", 0);
        PlayerPrefs.SetInt("Collect daily reward at index: ", (collectIndex + 1));
        listController.GetIndex();
        listController.UpdateDailyRewardList();

        UIManager.instance.dailyRewardPopUp.PreActive(this.amount);
        UIManager.instance.dailyRewardPopUp.gameObject.SetActive(true);
        GameManager.Instance.isOnMenu = true;
        GameManager.Instance.camMoving.CanRotate = false;
    }
}
