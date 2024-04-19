using Core.Data;
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
        }

        private void WinGame()
        {
            Debug.Log("WinGame");
            _PlayerData.UserData.UpdateWinGameUserDataValue();
            _GameEvent.OnGameWin?.Invoke();
            // await UniTask.Delay(1500);
            // _blockPool?.DeSpawnAllBlocks();
            // _GameManager.Instance.NextLevel();
        }

        public void OnBlockSelected(bool isBlockCanMove = true, bool isSpecialBlock = false, int blocks = 1)
        {
            if (isBlockCanMove)
            {
                _totalBlocks -= blocks;
                _GameEvent.OnSelectIdleBlock?.Invoke();
                if (_totalBlocks == 0)
                {
                    _GameManager.Instance.WinGame();
                    return;
                }
                if (blocks > 1) // only spawn special block when player dont use hint booster by check number of selected blokcs < 2
                    return;
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
                _remainingWrongMoves -= 1;
                _GameEvent.OnSelectIdleBlock?.Invoke();
                if (_remainingWrongMoves == 0)
                {
                    _GameManager.Instance.LoseGame();
                }
            }
        }

        public _BlockPool BlockPool => _blockPool;
        public Camera GamePlayCamera
        {
            get => _gamePlayCamera;
            set => _gamePlayCamera = value;
        }

        public bool IsGameplayInteractable { get; set; }
        public int RemainingWrongMoves => _remainingWrongMoves;
    }
}