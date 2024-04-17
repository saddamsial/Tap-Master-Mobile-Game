using Core.Data;
using Core.GamePlay;
using Core.GamePlay.Collection;
using Core.GamePlay.LevelSystem;
using Core.GamePlay.Shop;
using MyTools.ScreenSystem;
using PopupSystem;
using TMPro;
using UnityEngine;

namespace Core.UI{
    public class _GameplayScreen : _BaseScreen{
        [Header("Gameplay Screen Elements")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private GameObject _openFrontFaceBoosterButton;

        private void Awake(){
            _GameEvent.OnGamePlayReset += SetupScreen;
        }

        private void OnDestroy(){
            _GameEvent.OnGamePlayReset -= SetupScreen;
        }

        protected override void OnStartShowItSelf()
        {
            base.OnCompleteShowItSelf();
            SetupScreen();
        }



        public void SetupScreen(){
            _levelText.text ="Level " + (_PlayerData.UserData.CurrentLevel + 1); 
            _openFrontFaceBoosterButton.SetActive(true);
        }

        public void OnClickUseOpenFrontFaceBooster(){
            _GameEvent.OnUseBoosterOpenFace?.Invoke();
            _openFrontFaceBoosterButton.SetActive(false);
        }

        public void OnClickUseHintBooster(){
            _GameEvent.OnUseBoosterHint?.Invoke();
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
    }
}