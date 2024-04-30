using System.Collections.Generic;
using Core.Data;
using ObjectPooling;
using UnityEngine;

namespace Core.GamePlay.Block
{
    public class _CollectionRewardBlock : _BlockState
    {
        public _CollectionRewardBlock(_BlockController block) : base(block) { }

        private Mesh _specialMesh;
        private Mesh _defaultMesh;
        private Material _specialMaterial;
        private Material _defaultMaterial;

        public override void Init(bool isSetColor = false, Vector3 color = default, Mesh specialMesh = null, Material specialMaterial = null)
        {
            base.Init();
            _specialMesh = specialMesh;
            _specialMaterial = specialMaterial;
        }

        public override void SetUp()
        {
            base.SetUp();
            _defaultMaterial = _blockController.GetComponent<MeshRenderer>().material;
            _defaultMesh = _blockController.GetComponent<MeshFilter>().mesh;
            _blockController.GetComponent<MeshFilter>().mesh = _specialMesh;
            _meshRenderer.material = _specialMaterial;
            _blockController.transform.GetComponent<BoxCollider>().center += _ConstantBlockSetting.colliderOffSet;
            _blockController.transform.localPosition = _blockController.transform.localPosition - _blockController.transform.forward * 0.5f;
        }

        public override void OnSelect()
        {
            base.OnSelect();
            //_GameEvent.On?.Invoke(_BlockTypeEnum.PuzzleReward, 1);
            _GameManager.Instance.GamePlayManager.BlockPool.SetStateElementBlockInPool(_blockController.LogicPos.x, _blockController.LogicPos.y, _blockController.LogicPos.z, false);
            _GameManager.Instance.GamePlayManager.BlockPool.BlockObjectPool.Remove(_blockController);
            _GameManager.Instance.GamePlayManager.OnBlockSelected(_blockController, true, true);
            if (_PlayerData.UserData.CurrentCollectionPuzzlePiece.Value == -1)
            {
                int length = _GameManager.Instance.CollectionElementDatas.collectionElementDatas.Count;
                int randomType = Random.Range(0, length);
                int randomIndex = Random.Range(0, 36);
                var listCollected = _PlayerData.UserData.RuntimeCollectionData[randomType];
                int count = 0;

                while (listCollected.Contains(randomIndex))
                {
                    randomIndex = (randomIndex + 1) % 36;
                    count += 1;
                    if(count >= 36){
                        randomType = (randomType + 1) % length;
                        count = 0;
                    }
                }
                _GameEvent.OnSelectRewardBlock?.Invoke(_BlockTypeEnum.PuzzleReward, 1);
                _PlayerData.UserData.CurrentCollectionPuzzlePiece = new KeyValuePair<int, int>(randomType, randomIndex);
            }
            _blockController.gameObject.SetActive(false);
            _blockController.GetComponent<MeshFilter>().mesh = _defaultMesh;
            _blockController.GetComponent<MeshRenderer>().material = _defaultMaterial;
            _ObjectPooling.Instance.ReturnToPool(_TypeGameObjectEnum.Block, _blockController.gameObject);
        }
    }
}