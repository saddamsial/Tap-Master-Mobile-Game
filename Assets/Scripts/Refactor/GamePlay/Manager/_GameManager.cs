using System.Collections.Generic;
using Core.Data;
using Core.GamePlay.BlockPool;
using Core.GamePlay.Collection;
using Core.GamePlay.Shop;
using Core.SystemGame;
using UnityEngine;

namespace Core.GamePlay
{
    public class _GameManager
    {
        private static _GameManager _instance;
        public static _GameManager Instance => _instance ?? (_instance = new _GameManager());

        private _LevelSystem _LevelSystem;
        private _GamePlayManager _gamePlayManager;
        private int _currentCollectedBlock = 100;

        public void InitGame(LevelDatas levelData, Camera cameraGameplay)
        {
            _LevelSystem = _LevelSystem.Instance;
            _LevelSystem.InitLevelSystem(levelData);
            _gamePlayManager = _GamePlayManager.Instance;
            _gamePlayManager.InitGamePlayManager();
            _gamePlayManager.GamePlayCamera = cameraGameplay;
            _currentCollectedBlock = 100;
        }

        public void StartLevel()
        {
            _PlayerData.UserData.CurrentCollectCoin = 0;
            _PlayerData.UserData.CurrentCollectionPuzzlePiece = new KeyValuePair<int, int>(-1, -1);
            Level = _LevelSystem.GetLevelData();
            _gamePlayManager.StartLevel(Level);
            _GameEvent.OnGamePlayReset?.Invoke();
        }
#if UNITY_EDITOR
        public void StartLevelByTool()
        {
            _gamePlayManager.BlockPool.DeSpawnAllBlocks();
            StartLevel();
        }
#endif

        public void WinGame()
        {
            //_GameEvent.OnGamePlayWin?.Invoke();
            GlobalEventManager.Instance.OnLevelComplete(Level.levelIndex);
            _MySoundManager.Instance.PlaySound(SoundType.Win);
            if (_PlayerData.UserData.CurrentCollectionPuzzlePiece.Value != -1)
            {
                PopupSystem.PopupManager.CreateNewInstance<_ReceiveCollectionPopup>().Show(_PlayerData.UserData.CurrentCollectionPuzzlePiece.Key, _PlayerData.UserData.CurrentCollectionPuzzlePiece.Value);
            }
            else
                _GameEvent.OnGameWin?.Invoke();
            _PlayerData.UserData.UpdateWinGameUserDataValue();
        }

        public void LoseGame()
        {
            _MySoundManager.Instance.PlaySound(SoundType.Lose);
            _GameEvent.OnGameLose?.Invoke();
            GlobalEventManager.Instance.OnLevelLose(Level.levelIndex);
        }

        public void NextLevel()
        {
            AdsManager.Instance.ShowInter(
                () =>
                {
                    var currentLevel = Level.levelIndex + 1;
                    StartLevel(currentLevel);
                    GlobalEventManager.Instance.OnCloseInterstitial();
                    GlobalEventManager.Instance.OnLevelComplete(currentLevel);
                }
            );
        }

        public void ReTry()
        {
            AdsManager.Instance.ShowInter(
                () =>
                {
                    var currentLevel = Level.levelIndex - 1;
                    StartLevel(currentLevel);
                    GlobalEventManager.Instance.OnCloseInterstitial();
                    GlobalEventManager.Instance.OnLevelReplay(currentLevel);
                }
            );
        }

        public void StartLevel(int level)
        {
            _gamePlayManager.IsGameplayInteractable = true;
            _PlayerData.UserData.CurrentLevel = level;
            _gamePlayManager.BlockPool.DeSpawnAllBlocks();
            StartLevel();
        }

        public _BlockPool BlockPool => _gamePlayManager.BlockPool;
        public LevelData Level { get; set; }

        public _CameraController CameraController { get; set; }
        public _GamePlayManager GamePlayManager => _gamePlayManager;
        public _LevelSystem LevelSystem => _LevelSystem;

        public _ShopElementDatas BlockElementDatas { get; set; }
        public _ItemPriceDatas ItemPriceDatas { get; set; }
        public _CollectionElementDatas CollectionElementDatas { get; set; }
        public int CurrentCollectedBlock
        {
            get => _currentCollectedBlock;
            set => _currentCollectedBlock = value;
        }
    }
}