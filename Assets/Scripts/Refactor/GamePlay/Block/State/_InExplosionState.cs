namespace Core.GamePlay.Block{

    public class _InExplosionState : _BlockState{
        public _InExplosionState(_BlockController blockController) : base(blockController){
            
        }

        public override void SetUp(){
            //_meshRenderer.material = _blockController._blockModel._inExplosionMaterial;
        }

        public override void OnBlockReturnToPool(){
            //_blockController._blockModel._blockType = _BlockTypeEnum.Moving;
        }

        public void AnimExplosion(){
            
        }
    }
}