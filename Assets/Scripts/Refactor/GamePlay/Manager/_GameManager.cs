using System.Collections.Generic;
using Core.Data;
using Core.GamePlay.BlockPool;
using Core.GamePlay.Collection;
using Core.GamePlay.Shop;
using Core.SystemGame;
using MyTools.ParticleSystem;
using MyTools.ScreenSystem;
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
            PopupSystem.PopupManager.Instance.CloseAllPopup();
            Level = _LevelSystem.GetLevelData();
            _gamePlayManager.StartLevel(Level);
            _GameEvent.OnGamePlayReset?.Invoke();
            if (_PlayerData.UserData.CurrentLevel == 0)
            {
                _ScreenManager.Instance.ShowScreen(_ScreenTypeEnum.Tutorial1);
            }
            else if (_PlayerData.UserData.CurrentLevel == 1)
            {
                _ScreenManager.Instance.ShowScreen(_ScreenTypeEnum.Tutorial2);
            }
            else
            {
                _ScreenManager.Instance.ShowScreen(_ScreenTypeEnum.GamePlay);
            }
            AdsManager.Instance.ShowBanner();
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
            _GameEvent.OnGameEnd?.Invoke();
            _ScreenManager.Instance.HideCurrentScreen();
            _ParticleSystemManager.Instance.ShowParticle(_ParticleTypeEnum.WinGame, new Vector3(0, 0, _ParticleSystemManager.Instance.UICamera.nearClipPlane), true,
                () =>
                {
                    if (_PlayerData.UserData.CurrentCollectionPuzzlePiece.Value != -1)
                    {
                        PopupSystem.PopupManager.CreateNewInstance<_ReceiveCollectionPopup>().Show(_PlayerData.UserData.CurrentCollectionPuzzlePiece.Key, _PlayerData.UserData.CurrentCollectionPuzzlePiece.Value);
                    }
                    else
                    {
                        _GameEvent.OnGameWin?.Invoke();
                    }
                }
            );
            _MySoundManager.Instance.PlaySound(_SoundType.Win);
            _PlayerData.UserData.UpdateWinGameUserDataValue();
        }

        public void LoseGame()
        {
            _ScreenManager.Instance.HideCurrentScreen();
            _MySoundManager.Instance.PlaySound(_SoundType.Lose);
            _GameEvent.OnGameLose?.Invoke();
            GlobalEventManager.Instance.OnLevelLose(Level.levelIndex);
        }

        public void NextLevel()
        {
            AdsManager.Instance.ShowInter(
                () =>
                {
                    var currentLevel = Level.levelIndex;
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