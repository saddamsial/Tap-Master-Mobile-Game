using UnityEngine;

namespace Core.GamePlay.Block{
    public class _CollectionRewardBlock : _BlockState{
        public _CollectionRewardBlock(_BlockController block) : base(block) {}

        private Mesh _specialMesh;
        private Material _specialMaterial;

        public override void Init(bool isSetColor = false, Vector3 color = default, Mesh specialMesh = null, Material specialMaterial = null)
        {
            base.Init();
            _specialMesh = specialMesh;
            _specialMaterial = specialMaterial;
        }

        public override void SetUp()
        {
            base.SetUp();
            _blockController.GetComponent<MeshFilter>().mesh = _specialMesh;
            _meshRenderer.material = _specialMaterial;
            _blockController.transform.GetComponent<BoxCollider>().center += _ConstantBlockSetting.colliderOffSet;
            _blockController.transform.localPosition = _blockController.transform.localPosition - _blockController.transform.forward * 0.5f;
        }

        public override void OnSelect()
        {
            base.OnSelect();
            _GameEvent.OnSelectRewardBlock?.Invoke(_BlockTypeEnum.PuzzleReward, 1);
        }
    }
}