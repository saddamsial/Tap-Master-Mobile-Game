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

        private _BlockPool _blockPool;

        private int _totalBlocks;
        private int _remainBlocksToHaveSpecialBlock;


        public void InitGamePlayManager()
        {
            _totalBlocks = 0;
            _blockPool = new _BlockPool();
        }

        public void StartLevel(LevelData level)
        {
            _blockPool?.InitPool(level);
            _totalBlocks = level.numOfBlocks;
            _remainBlocksToHaveSpecialBlock = _totalBlocks / 10;
            _remainBlocksToHaveSpecialBlock = Mathf.Max(_ConstantGameplayConfig.MIN_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
            _remainBlocksToHaveSpecialBlock = Mathf.Min(_ConstantGameplayConfig.MAX_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
        }

        private async void WinGame()
        {
            Debug.Log("WinGame");
            _PlayerData.UserData.UpdateWinGameUserDataValue();
            await UniTask.Delay(1500);
            _blockPool?.DeSpawnAllBlocks();
            _GameManager.Instance.NextLevel();
        }

        public void OnBlockSelected(bool isBlockCanMove = true, bool isSpecialBlock = false)
        {
            if (isBlockCanMove)
            {
                _totalBlocks -= 1;
                if (_totalBlocks == 0)
                {
                    WinGame();
                    return;
                }
                if(!isSpecialBlock)
                    _remainBlocksToHaveSpecialBlock -= 1;
                if (_remainBlocksToHaveSpecialBlock == 0)
                {
                    _blockPool.SpawnSpecialBlock();
                    _remainBlocksToHaveSpecialBlock = _totalBlocks / 10;
                    _remainBlocksToHaveSpecialBlock = Mathf.Max(_ConstantGameplayConfig.MIN_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
                    _remainBlocksToHaveSpecialBlock = Mathf.Min(_ConstantGameplayConfig.MAX_BLOCKS_TO_SPECIAL, _remainBlocksToHaveSpecialBlock);
                }
            }
        }

        public _BlockPool BlockPool => _blockPool;
    }
}