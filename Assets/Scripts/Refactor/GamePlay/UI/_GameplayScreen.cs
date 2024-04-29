using System;
using Core.Data;
using Core.GamePlay;
using Core.GamePlay.Collection;
using Core.GamePlay.LevelSystem;
using Core.GamePlay.Shop;
using Core.UI.ExtendPopup;
using MyTools.ScreenSystem;
using PopupSystem;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class _GameplayScreen : _BaseScreen
    {
        [Header("Gameplay Screen Elements")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private GameObject _openFrontFaceBoosterButton;
        [SerializeField] private TMP_Text _remainingWrongMovesText;

        private void Awake()
        {
            _GameEvent.OnGamePlayReset += SetupScreen;
            _GameEvent.OnGamePlayReset += UpdateScreen;
            _GameEvent.OnSelectIdleBlock += UpdateScreen;
            _GameEvent.OnGamePlayContinue += UpdateScreen;
        }

        private void OnDestroy()
        {
            _GameEvent.OnGamePlayReset -= SetupScreen;
            _GameEvent.OnGamePlayReset -= UpdateScreen;
            _GameEvent.OnSelectIdleBlock -= UpdateScreen;
            _GameEvent.OnGamePlayContinue -= UpdateScreen;
        }

        protected override void OnStartShowItSelf()
        {
            base.OnCompleteShowItSelf();
            SetupScreen();
        }


        private void SetupScreen()
        {
            _levelText.text = "Level " + (_PlayerData.UserData.CurrentLevel + 1);
            _openFrontFaceBoosterButton.SetActive(true);
        }

        private void UpdateScreen()
        {
            _remainingWrongMovesText.text = _GameManager.Instance.GamePlayManager.RemainingWrongMoves.ToString() + " Moves";
        }

        public void OnClickUseOpenFrontFaceBooster()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            AdsManager.Instance.ShowRewarded(
                (x) =>
                {
                    GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel);
                    if (x)
                    {
                        _GameEvent.OnUseBoosterOpenFace?.Invoke();
                        _openFrontFaceBoosterButton.SetActive(false);
                    }
                }
            );
        }

        public void OnClickUseHintBooster()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            AdsManager.Instance.ShowRewarded(
                (x) =>
                {
                    GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel);
                    if (x)
                        _GameEvent.OnUseBoosterHint?.Invoke();
                }
            );
        }

        public void OnClickReplayGame()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            PopupManager.CreateNewInstance<_ReplayGamePopup>().Show("Are you sure you want to replay this level?", false);
        }

        public void OnClickOpenCollection()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            PopupManager.CreateNewInstance<_CollectionPopup>().Show();
        }

        public void OnClickOpenShop()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            PopupManager.CreateNewInstance<_ShopPopup>().Show();
        }

        public void OnClickOpenLevel()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            PopupManager.CreateNewInstance<_LevelPopup>().Show();
        }

        public void OnClickPauseGame()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            PopupManager.CreateNewInstance<_SettingPopup>().Show();
        }

        public void OnClickAchievement()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            PopupManager.CreateNewInstance<_NotificationPopup>().Show("Coming soon!");
        }
    }
}