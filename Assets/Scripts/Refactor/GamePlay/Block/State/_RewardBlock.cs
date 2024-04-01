using Core.SystemGame;
using UnityEngine;

namespace Core.GamePlay.Block{
    public class _RewardBlock : _BlockState{
        public _RewardBlock(_BlockController blockController) : base(blockController){
        }

        public override void Init(bool isSetColor = false, Vector3 color = default){
            base.Init();
        }

        public override void SetUp(){
            base.SetUp();
            _blockController.gameObject.layer = _LayerConstant.GOLD_BLOCK;
            _meshRenderer.material.SetInt("_IsIdleBlock", 0);
        }

        public override void OnSelect(){
            base.OnSelect();
            Debug.Log("Reward Block");
        }
    }
}