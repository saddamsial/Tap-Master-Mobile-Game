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
        }

        public override void OnSelect(){
            base.OnSelect();
        }
    }
}