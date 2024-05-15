using DG.Tweening;
using UnityEngine;

namespace Core.GamePlay.Block{

    public class _InExplosionState : _BlockState{
        public _InExplosionState(_BlockController blockController) : base(blockController){
            
        }

        public override void SetUp(){
            //_meshRenderer.material = _blockController._blockModel._inExplosionMaterial;
            AnimExplosion();
        }

        public override void OnBlockReturnToPool(){
            //_blockController._blockModel._blockType = _BlockTypeEnum.Moving;
        }

        public override void OnSelect()
        {
            base.OnSelect();
            //AnimExplosion();
        }

        private void AnimExplosion(){
            _GameManager.Instance.GamePlayManager.BlockPool.SetStateElementBlockInPool(_blockController.LogicPos.x, _blockController.LogicPos.y, _blockController.LogicPos.z, false);
            //_GameManager.Instance.BlockPool.DespawnBlock(_blockController);
            //_GameManager.Instance.GamePlayManager.OnBlockSelected(_blockController);
            _blockController.transform.DOLocalMove(_blockController.transform.localPosition + GetRandomDirectionOfBlock() *5, 0.08f * 10).SetEase(Ease.OutCubic)
                .OnComplete(() => {
                    _blockController.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutSine).OnComplete(() => {
                        _GameManager.Instance.GamePlayManager.BlockPool.DespawnBlock(_blockController);
                    });
                });

            _blockController.transform.DORotate(Vector3.one * 360 + _blockController.transform.eulerAngles, 1.5f, RotateMode.FastBeyond360);
        }

        private Vector3 GetRandomDirectionOfBlock(){
            return new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        }
    }
}