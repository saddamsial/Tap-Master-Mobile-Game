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

namespace Core.UI{
    public class _GameplayScreen : _BaseScreen{
        [Header("Gameplay Screen Elements")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private GameObject _openFrontFaceBoosterButton;
        [SerializeField] private TMP_Text _remainingWrongMovesText;

        private void Awake(){
            _GameEvent.OnGamePlayReset += SetupScreen;
            _GameEvent.OnGamePlayReset += UpdateScreen;
            _GameEvent.OnSelectedBlock +=  UpdateScreen;
        }

        private void OnDestroy(){
            _GameEvent.OnGamePlayReset -= SetupScreen;
            _GameEvent.OnGamePlayReset -= UpdateScreen;
            _GameEvent.OnSelectedBlock -= UpdateScreen;
        }

        protected override void OnStartShowItSelf()
        {
            base.OnCompleteShowItSelf();
            SetupScreen();
        }


        private void SetupScreen(){
            _levelText.text ="Level " + (_PlayerData.UserData.CurrentLevel + 1); 
            _openFrontFaceBoosterButton.SetActive(true);
        }

        private void UpdateScreen(){
            _remainingWrongMovesText.text = _GameManager.Instance.GamePlayManager.RemainingWrongMoves.ToString() + " Left";
        }

        public void OnClickUseOpenFrontFaceBooster(){
            _GameEvent.OnUseBoosterOpenFace?.Invoke();
            _openFrontFaceBoosterButton.SetActive(false);
        }

        public void OnClickUseHintBooster(){
            _GameEvent.OnUseBoosterHint?.Invoke();
        }

        public void OnClickReplayGame(){
            PopupManager.CreateNewInstance<_NotificationPopup>().Show("Are you sure you want to replay this level?", true);
        }

        public void OnClickOpenCollection(){
            PopupManager.CreateNewInstance<_CollectionPopup>().Show();
        }

        public void OnClickOpenShop(){
            PopupManager.CreateNewInstance<_ShopPopup>().Show();
        }

        public void OnClickOpenLevel(){
            PopupManager.CreateNewInstance<_LevelPopup>().Show();
        }

        public void OnClickPauseGame(){
            PopupManager.CreateNewInstance<_NotificationPopup>().Show("Coming soon!");
        }

        public void OnClickAchievement(){
            PopupManager.CreateNewInstance<_NotificationPopup>().Show("Coming soon!");
        }
    }
}