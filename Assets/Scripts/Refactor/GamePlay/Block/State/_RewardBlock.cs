using System;
using System.ComponentModel.Design.Serialization;
using Core.Data;
using Core.SystemGame;
using DG.Tweening;
using UnityEngine;

namespace Core.GamePlay.Block{
    public class _RewardBlock : _BlockState{
        private int _rewardCoin = 0;
        
        public _RewardBlock(_BlockController blockController) : base(blockController){
        }

        public override void Init(bool isSetColor = false, Vector3 color = default, Mesh _specialMesh = null, Material specialMaterial = null){
            base.Init();
            _rewardCoin = 0;
        }

        public override void SetUp(){
            base.SetUp();
            _blockController.gameObject.layer = _LayerConstant.GOLD_BLOCK;
            _meshRenderer.material.SetInt(_ConstantBlockSetting.KEY_IS_IDLE_BLOCK, 0);
            _rewardCoin = 10;
        }

        public override void OnSelect(){
            base.OnSelect();
            AnimatedCollectRewardBlock();
            _GameManager.Instance.BlockPool.SetStateElementBlockInPool(_blockController.LogicPos.x, _blockController.LogicPos.y, _blockController.LogicPos.z, false);
            bool isEndGame = _GamePlayManager.Instance.OnBlockSelected(_blockController ,true, true);
//            Debug.Log(isEndGame);
            //_PlayerData.UserData.Coin += _rewardCoin;
            _PlayerData.UserData.CurrentCollectCoin += _rewardCoin;
            if(!_blockController.IsLastBlock && !isEndGame)
                _GameEvent.OnSelectRewardBlock?.Invoke(_BlockTypeEnum.GoldReward, _rewardCoin);
            else if(isEndGame && _blockController.IsLastBlock){
                _GameEvent.OnGameEnd?.Invoke();
                _GameEvent.OnSelectRewardBlockToWin?.Invoke(_BlockTypeEnum.GoldReward, _rewardCoin);
            }
        }

        private void AnimatedCollectRewardBlock(){
            _meshRenderer.material.DOFade(0, _ConstantBlockSetting.KEY_IS_SPECIAL_COLOR, 0.5f).OnComplete(() => {
                _meshRenderer.material.DOFade(1, _ConstantBlockSetting.KEY_IS_SPECIAL_COLOR, 0f);
                _GameManager.Instance.BlockPool.DespawnBlock(_blockController);});
            // }).OnKill(() => {
            //     _meshRenderer.material.DOFade(1, _ConstantBlockSetting.KEY_IS_SPECIAL_COLOR, 0f);
            //     _GameManager.Instance.BlockPool.DespawnBlock(_blockController);
            // });
        }

        public override void OnBlockReturnToPool(){
            base.OnBlockReturnToPool();
            _meshRenderer.material.DOFade(1, _ConstantBlockSetting.KEY_IS_SPECIAL_COLOR, 0f);
            _meshRenderer.material.SetInt(_ConstantBlockSetting.KEY_IS_IDLE_BLOCK, 1);
        }
    }
}