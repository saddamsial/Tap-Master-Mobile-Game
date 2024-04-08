using System.Collections;
using System.Collections.Generic;
using Core.ResourceGamePlay;
using Core.SystemGame;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Core.GamePlay.Block
{
    public class _BlockController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Sprite texture2D;
        [SerializeField] private Vector3 _defaultScale;
        [SerializeField] public _BlockTypeEnum _blockType;

        private Dictionary<_BlockTypeEnum, _BlockState> _blockStates = new Dictionary<_BlockTypeEnum, _BlockState>();
        private _BlockTypeEnum _currentType;
        private Vector3Int _logicPos;
        private Vector3Int _obstacleLogicPos;
        private Vector3 _color;
        private bool _isInit;

        private void Awake()
        {
            //_GameEvent.OnGamePlayReset += ResetBlock;
        }

        private void OnDestroy()
        {
            //   _GameEvent.OnGamePlayReset -= ResetBlock;
        }

        public void InitBlock(Material idleMaterial, Material movingMaterial, Material blockedMaterial, Vector3 rotation, Vector3 color, bool isSetColor = false)
        {
            _meshRenderer.material = idleMaterial;
            AnimationInitBlock(rotation);
            SetUpTypeBlock(movingMaterial, blockedMaterial);
            InitBlockStates(color, isSetColor);
            SetCurrentTypeBlock(_BlockTypeEnum.Moving);
            IsMoving = false;
        }

        private void ResetBlock()
        {
            this.transform.DOKill();
        }

        private void SetUpTypeBlock(Material movingMaterial, Material blockedMaterial)
        {
            if (_isInit) return;
            _blockStates.Add(_BlockTypeEnum.Moving, new _MovingBlock(this, movingMaterial, blockedMaterial));
            _blockStates.Add(_BlockTypeEnum.Reward, new _RewardBlock(this));
            _isInit = true;
        }

        private void InitBlockStates(Vector3 color, bool isSetColor = false)
        {
            _blockStates[_BlockTypeEnum.Moving].Init(isSetColor, color);
            _blockStates[_BlockTypeEnum.Reward].Init();
        }

        public void SetCurrentTypeBlock(_BlockTypeEnum blockType)
        {
            _currentType = blockType;
            _blockStates[_currentType].SetUp();
        }

        public void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }

        public void SetTexture(string typeTexture, Sprite texture)
        {
            _meshRenderer.sharedMaterial.SetTexture(typeTexture, texture.texture);
        }

        public void HittedByMovingBlock(Vector3 direction)
        {
            var t = transform.DOMove(transform.position + direction * 0.1f, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InSine);
            t.OnStepComplete(() =>
            {
                if (t.ElapsedPercentage() == 1) return;
                var thisObstacle = _GameManager.Instance.BlockPool.GetBlock(_logicPos + _NormalizingVector3.ConvertToVector3Int(direction));
                if (thisObstacle != null)
                    thisObstacle.HittedByMovingBlock(direction);
            });
        }

        public bool CheckObjectVisible(Plane[] planes){
            return GeometryUtility.TestPlanesAABB(planes, _meshRenderer.bounds);
        }

        private void AnimationInitBlock(Vector3 rotation)
        {
            transform.rotation = Quaternion.Euler(rotation);
            transform.localScale = Vector3.zero;
            transform.DOScale(_defaultScale, 1.5f);
            transform.DORotate(Vector3.one * 360 + rotation, 1.5f, RotateMode.FastBeyond360);
        }

        private void OnMouseDown()
        {
            StopAllCoroutines();
            StartCoroutine("CaculateHodingTime");
        }

        private void OnMouseUp()
        {
            StopCoroutine("CaculateHodingTime");
            if (_InputSystem.Instance.Timer > 0.15f)
                return;
            OnSelected();
        }

        private IEnumerator CaculateHodingTime()
        {
            _InputSystem.Instance.Timer = 0;
            while (true)
            {
                _InputSystem.Instance.Timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnSelected()
        {
            _blockStates[_currentType].OnSelect();
            _GamePlayManager.Instance.OnBlockSelected(_blockStates[_currentType].IsCanMove);
        }

        public Vector3Int LogicPos
        {
            get => _logicPos;
            set => _logicPos = value;
        }

        public Vector3Int ObstacleLogicPos
        {
            get => _obstacleLogicPos;
            set => _obstacleLogicPos = value;
        }

        public MeshRenderer MeshRenderer => _meshRenderer;

        public bool IsMoving { get; set; }
        public _BlockTypeEnum CurrentType => _currentType;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(_BlockController))]
    public class _BlockControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var blockController = target as _BlockController;
            if (GUILayout.Button("Set Type Block"))
            {
                blockController.SetCurrentTypeBlock(blockController._blockType);
            }
        }
    }
#endif
}