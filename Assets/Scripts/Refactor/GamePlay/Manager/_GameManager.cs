using Core.Data;
using Core.GamePlay.BlockPool;
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
        private int _currentCollectedLevelCoin;

        public void InitGame(LevelDatas levelData, Camera cameraGameplay)
        {
            _LevelSystem = _LevelSystem.Instance;
            _LevelSystem.InitLevelSystem(levelData);
            _gamePlayManager = _GamePlayManager.Instance;
            _gamePlayManager.InitGamePlayManager();
            _gamePlayManager.GamePlayCamera = cameraGameplay;
        }

        public void StartLevel()
        {
            Level = _LevelSystem.GetLevelData();
            _gamePlayManager.StartLevel(Level);
            _GameEvent.OnGamePlayReset?.Invoke();
        }
#if UNITY_EDITOR
        public void StartLevelByTool(){
            _gamePlayManager.BlockPool.DeSpawnAllBlocks();
            StartLevel();
        }
#endif

        public void WinGame()
        {
            //_GameEvent.OnGamePlayWin?.Invoke();
        }

        public void NextLevel()
        {
            var currentLevel = Level.levelIndex - 1;
            StartLevel(currentLevel + 1);
        }

        public void StartLevel(int level){
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
        public int CurrentCollectedLevelCoin
        {
            get => _currentCollectedLevelCoin;
            set
            {
                _currentCollectedLevelCoin = value;
            }
        }
    }
}