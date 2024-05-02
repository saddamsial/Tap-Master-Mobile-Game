using Core.Data;
using Core.GamePlay.Block;
using Core.GamePlay.BlockPool;
using Core.SystemGame;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.GamePlay
{
    public class _GamePlayManager
    {
        private static _GamePlayManager _instance;
        public static _GamePlayManager Instance => _instance ?? (_instance = new _GamePlayManager());

        private Camera _gamePlayCamera;
        private _BlockPool _blockPool;

        private int _totalBlocks;
        private int _remainBlocksToHaveSpecialBlock;
        private int _remainingWrongMoves;


        public void InitGamePlayManager()
        {
            _totalBlocks = 0;
            _blockPool = new _BlockPool();
            IsGameplayInteractable = true;
            IsSpawnCollectionBlock = false;
        }

        public void StartLevel(LevelData level)
        {
            _blockPool?.InitPool(level);
            _totalBlocks = level.numOfBlocks;
            _remainBlocksToHaveSpecialBlock = _totalBlocks / 10;
            _remainBlocksToHaveSpecialBlock = Mathf.Max(_ConstantGameplayConfig.MIN_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
            _remainBlocksToHaveSpecialBlock = Mathf.Min(_ConstantGameplayConfig.MAX_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
            _remainingWrongMoves = _totalBlocks / 10;
            _remainingWrongMoves = Mathf.Max(_ConstantGameplayConfig.MIN_REMAINING_WRONG_MOVES, _remainingWrongMoves);
            _remainingWrongMoves = Mathf.Min(_ConstantGameplayConfig.MAX_REMAINING_WRONG_MOVES, _remainingWrongMoves);
            _remainingWrongMoves += _totalBlocks;
            GlobalEventManager.Instance.OnLevelPlay(level.levelIndex);
        }

        private void WinGame()
        {
            _PlayerData.UserData.UpdateWinGameUserDataValue();
            _GameEvent.OnGameWin?.Invoke();
            // await UniTask.Delay(1500);
            // _blockPool?.DeSpawnAllBlocks();
            // _GameManager.Instance.NextLevel();
        }

        public bool OnBlockSelected(_BlockController block, bool isBlockCanMove = true, bool isSpecialBlock = false, int blocks = 1)
        {
            _remainingWrongMoves -= 1;
            _MySoundManager.Instance.Vibrate();
            if (isBlockCanMove)
            {
                _MySoundManager.Instance.PlaySound(SoundType.Tap);
                _totalBlocks -= blocks;
                _GameManager.Instance.CurrentCollectedBlock -= blocks;
                _GameEvent.OnSelectIdleBlock?.Invoke();
                if (_totalBlocks <= 0)
                {
                    block.IsLastBlock = true;
                    if (!isSpecialBlock){
                        _GameManager.Instance.WinGame();
                    }
                    
                    return true;
                }
                if (!isSpecialBlock)
                    _remainBlocksToHaveSpecialBlock -= 1;
                if (_remainBlocksToHaveSpecialBlock == 0)
                {
                    _blockPool.SpawnSpecialBlockInCameraView(_gamePlayCamera);
                    _remainBlocksToHaveSpecialBlock = _totalBlocks / 10;
                    _remainBlocksToHaveSpecialBlock = Mathf.Max(_ConstantGameplayConfig.MIN_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
                    _remainBlocksToHaveSpecialBlock = Mathf.Min(_ConstantGameplayConfig.MAX_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
                }
            }
            else
            {
                _MySoundManager.Instance.PlaySound(SoundType.TapFail);
                _GameEvent.OnSelectIdleBlock?.Invoke();
            }
            if (_remainingWrongMoves <= 0)
            {
                _GameManager.Instance.LoseGame();
                return true;
            }
            if (_GameManager.Instance.CurrentCollectedBlock <= 0)
            {
                AdsManager.Instance.ShowInter( () => {
                    GlobalEventManager.Instance.OnCloseInterstitial();
                });
                _GameManager.Instance.CurrentCollectedBlock = 100;
                return false;
            }
            return false;
        }

        // Only hint idle block
        public void OnBlockSelectedByHint(int blockNums)
        {
            Debug.Log(_totalBlocks + " - " + blockNums + " = " + (_totalBlocks - blockNums));
            _totalBlocks -= blockNums;
            _GameManager.Instance.CurrentCollectedBlock -= blockNums;
            _GameEvent.OnSelectIdleBlock?.Invoke();
            if (_totalBlocks <= 0)
            {
                _GameManager.Instance.WinGame();
                return;
            }
            if (_GameManager.Instance.CurrentCollectedBlock <= 0)
            {
                AdsManager.Instance.ShowInter(
                    () => { GlobalEventManager.Instance.OnCloseInterstitial();}
                );
                _GameManager.Instance.CurrentCollectedBlock = 100;
                return;
            }
        }

        public void OnContinueGame()
        {
            if (_remainingWrongMoves > 0)
            {
                Debug.LogError("Can't continue game, wrong moves <= 0: " + _remainingWrongMoves);
                return; // chuwa thua, k can continue
            }
            else
            {
                IsGameplayInteractable = true;
                _remainingWrongMoves = BlockPool.BlockObjectPool.Count + 10;
            }
        }

        public _BlockPool BlockPool => _blockPool;
        public Camera GamePlayCamera
        {
            get => _gamePlayCamera;
            set => _gamePlayCamera = value;
        }

        public bool IsGameplayInteractable { get; set; }
        public bool IsSpawnCollectionBlock { get; set; }
        public int RemainingWrongMoves
        {
            get
            {
                return _remainingWrongMoves;
            }
        }
    }
}