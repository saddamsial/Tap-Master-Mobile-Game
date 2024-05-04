using DG.Tweening;
using UnityEngine;

namespace Core.GamePlay.Block{
    public class _SpecialMovingBlock : _BlockState
    {
        public _SpecialMovingBlock(_BlockController blockController) : base(blockController)
        {
        }

        public override void Init(bool isSetColor = false, Vector3 color = default, Mesh specialMesh = null, Material specialMaterial = null)
        {
            base.Init(isSetColor, color);
        }

        public override void SetUp()
        {
            base.SetUp();
            MoveSpecial();
        }

        private void MoveSpecial(){
            _GameManager.Instance.BlockPool.SetStateElementBlockInPool(_blockController.LogicPos.x, _blockController.LogicPos.y, _blockController.LogicPos.z, false);
            _blockController.transform.DOLocalMove(_blockController.transform.localPosition + -_blockController.transform.right, 0.08f).SetLoops(30, LoopType.Incremental);
            _meshRenderer.material.DOFade(0,_ConstantBlockSetting.KEY_CORLOR_SETTING, 1.2f).OnComplete(() => {
                _meshRenderer.material.DOFade(1,_ConstantBlockSetting.KEY_CORLOR_SETTING, 0f);
                _GameManager.Instance.BlockPool.DespawnBlock(_blockController);
            }).OnKill(() => {
                _meshRenderer.material.DOFade(1,_ConstantBlockSetting.KEY_CORLOR_SETTING, 0f);
                _GameManager.Instance.BlockPool.DespawnBlock(_blockController);
            });
        }

        public override void OnBlockReturnToPool()
        {
            base.OnBlockReturnToPool();
            _meshRenderer.material.DOFade(1,_ConstantBlockSetting.KEY_CORLOR_SETTING, 0f);
        }
    }
}