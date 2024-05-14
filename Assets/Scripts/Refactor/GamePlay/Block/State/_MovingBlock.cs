using UnityEngine;
using DG.Tweening;
using ObjectPooling;
using Extensions;
using Core.SystemGame;

namespace Core.GamePlay.Block
{
    public class _MovingBlock : _BlockState
    {

        private Material _movingMaterial;
        private Material _blockedMaterial;
        private Material _currentMaterial;
        private bool _isMoving;
        private Vector3 _color;
        public _MovingBlock(_BlockController blockController, Material movingMaterial, Material blockedMaterial) : base(blockController)
        {
            _blockController = blockController;
            _meshRenderer = _blockController.GetComponent<MeshRenderer>();
            _isMoving = false;
            _currentMaterial = _blockController.GetComponent<MeshRenderer>().material;
            _movingMaterial = movingMaterial;
            _blockedMaterial = blockedMaterial;
        }

        public override void Init(bool isSetColor = false, Vector3 color = default,Mesh specialMesh = null, Material specialMaterial = null)
        {
            base.Init();
            if (isSetColor)
            {
                _color = color;
                _meshRenderer.material.SetColor(_ConstantBlockSetting.KEY_CORLOR_SETTING, new Color(_color.x / 255, _color.y / 255, _color.z / 255));
            }
            else
            {
                _color = _ConstantBlockSetting.defaultColor;
                _meshRenderer.material.SetColor(_ConstantBlockSetting.KEY_CORLOR_SETTING, new Color(_color.x / 255, _color.y / 255, _color.z / 255));
            }
            SetUp();
        }

        public override void SetUp()
        {
            base.SetUp();
            _blockController.gameObject.layer = _LayerConstant.IDLE_BLOCK;
            _meshRenderer.material.SetInt(_ConstantBlockSetting.KEY_IS_IDLE_BLOCK, 1);
            _meshRenderer.material.SetInt(_ConstantBlockSetting.KEY_IS_OPEN_FRONT_FACE, 1);
            //_GameEvent.OnUseBoosterOpenFace -= () => {_meshRenderer.material.SetInt(_ConstantBlockSetting.KEY_IS_OPEN_FRONT_FACE, 1);};
            //_GameEvent.OnUseBoosterOpenFace += () => {_meshRenderer.material.SetInt(_ConstantBlockSetting.KEY_IS_OPEN_FRONT_FACE, 1);};
            _isMoving = false;
        }

        public override void OnSelect()
        {
            base.OnSelect();
            if (_isMoving)
            {
                IsCanMove = false;
                return;
            }
            _isMoving = true;
            _blockController.IsMoving = true;
            if (_GameManager.Instance.BlockPool.CheckCanEscape(_blockController))
            {
                IsCanMove = true;
                _GameManager.Instance.BlockPool.BlockObjectPool.Remove(_blockController);
                _blockController.transform.DOLocalMove(_blockController.transform.localPosition + -_blockController.transform.right, 0.04f)
                    .SetLoops(30, LoopType.Incremental)
                    .SetEase(Ease.Linear)
                    .OnStart(() =>
                    {
                        _blockController.SetMaterial(_movingMaterial);
                        _GameManager.Instance.BlockPool.SetStateElementBlockInPool(_blockController.LogicPos.x, _blockController.LogicPos.y, _blockController.LogicPos.z, false);
                    })
                    .OnComplete(() =>
                    {
                        //_ObjectPooling.Instance.ReturnToPool(_TypeGameObjectEnum.Block, _blockController.gameObject);
                        SimplePool.Despawn(_blockController.gameObject);
                    });
            }
            else
            {
                IsCanMove = false;
                //Debug.Log("Can't move");
                var obstacle = _GameManager.Instance.BlockPool.GetBlock(_blockController.ObstacleLogicPos);
                var t = _blockController.transform.DOMove(obstacle.transform.position - -_blockController.transform.right * 0.9f, 0.1f * _NormalizingVector3.GetDistanceBetweenVector3(_blockController.LogicPos, obstacle.LogicPos))
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.InSine)
                    .OnStart(() =>
                    {
                        _MySoundManager.Instance.PlaySound(_SoundType.TapFail);
                        _blockController.SetMaterial(_blockedMaterial);
                    })
                    .OnComplete(() =>
                    {
                        _isMoving = false;
                        _blockController.IsMoving = false;
                        _blockController.SetMaterial(_currentMaterial);
                        _meshRenderer.material.SetColor(_ConstantBlockSetting.KEY_CORLOR_SETTING, new Color(_color.x / 255, _color.y / 255, _color.z / 255));
                    });

                t.OnStepComplete(() =>
                    {
                        if (t.ElapsedPercentage() == 1) return;
                        if (obstacle.IsMoving) return;
                        obstacle.HittedByMovingBlock(-_blockController.transform.right);
                    });
            }
            _GamePlayManager.Instance.OnBlockSelected(_blockController, IsCanMove);
        }

        private void ObstacleHitted(Vector3Int logicPos, Vector3 direction)
        {
            var obstacle = _GameManager.Instance.BlockPool.GetBlock(_blockController.ObstacleLogicPos);
            if (obstacle == null) return;
            obstacle.transform.DOMove(obstacle.transform.position + -_blockController.transform.right * 0.1f, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InSine)
                .OnStart(() =>
                {
                    obstacle.SetMaterial(_blockedMaterial);
                })
                .OnComplete(() =>
                {
                    obstacle?.SetMaterial(_currentMaterial);
                })
                .OnStepComplete(() =>
                {
                    obstacle = _GameManager.Instance.BlockPool.GetBlock(obstacle.LogicPos + _NormalizingVector3.IgnoreDecimalPart(-_blockController.transform.right));
                });
        }

        public void OnUseBoosterOpenFace(){
            _meshRenderer.material.SetInt(_ConstantBlockSetting.KEY_IS_OPEN_FRONT_FACE, 1);
        }

        public Vector3 Color {
            get => _color;
            set => _color = value;
        }
    }
}