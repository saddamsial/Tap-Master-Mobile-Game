using System;
using Core.Data;
using Core.GamePlay;
using Core.GamePlay.Block;
using Core.GamePlay.Collection;
using Core.GamePlay.LevelSystem;
using Core.GamePlay.Shop;
using Core.UI.ExtendPopup;
using DG.Tweening;
using MyTools.ScreenSystem;
using PopupSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class _GameplayScreen : _BaseScreen
    {
        [Header("Gameplay Screen Elements")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private GameObject _openFrontFaceBoosterButton;
        [SerializeField] private TMP_Text _remainingWrongMovesText;
        [SerializeField] private RectTransform _puzzlePieces;

        private void Awake()
        {
            _GameEvent.OnGamePlayReset += SetupScreen;
            _GameEvent.OnGamePlayReset += UpdateScreen;
            _GameEvent.OnSelectIdleBlock += UpdateScreen;
            _GameEvent.OnGamePlayContinue += UpdateScreen;
            _GameEvent.OnSelectRewardBlock += NotifyCollectPuzzlePiece;
            _GameEvent.OnSelectRewardBlockToWin += NotifyCollectPuzzlePiece;
        }

        private void OnDestroy()
        {
            _GameEvent.OnGamePlayReset -= SetupScreen;
            _GameEvent.OnGamePlayReset -= UpdateScreen;
            _GameEvent.OnSelectIdleBlock -= UpdateScreen;
            _GameEvent.OnGamePlayContinue -= UpdateScreen;
            _GameEvent.OnSelectRewardBlock -= NotifyCollectPuzzlePiece;
            _GameEvent.OnSelectRewardBlockToWin -= NotifyCollectPuzzlePiece;
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
            _puzzlePieces.gameObject.SetActive(false);
            _puzzlePieces.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        private void NotifyCollectPuzzlePiece(_BlockTypeEnum typeEnum, int count){
            if (typeEnum == _BlockTypeEnum.PuzzleReward)
            {
                _puzzlePieces.localPosition = _puzzlePieces.localPosition + new Vector3(0, -100, 0);
                _puzzlePieces.gameObject.SetActive(true);
                _puzzlePieces.DOLocalMoveY(_puzzlePieces.localPosition.y + 100, 0.5f).SetEase(Ease.OutBack);
                _puzzlePieces.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() =>
                {
                    if(count == -1)
                        _GameManager.Instance.WinGame();
                });
            }
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
                    GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel, "open_front_face_booster");
                    if (x)
                    {
                        _GameEvent.OnUseBoosterOpenFace?.Invoke();
                        _openFrontFaceBoosterButton.SetActive(false);
                    }
                }, null, location: "open_front_face_booster"
            );
        }

        public void OnClickUseHintBooster()
        {
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            AdsManager.Instance.ShowRewarded(
                (x) =>
                {
                    GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel, "hint_booster");
                    if (x)
                        _GameEvent.OnUseBoosterHint?.Invoke();
                }, null, location: "hint_booster"
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