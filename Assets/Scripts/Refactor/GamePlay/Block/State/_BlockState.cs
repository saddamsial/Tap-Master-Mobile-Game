using UnityEngine;

namespace Core.GamePlay.Block{
    public abstract class _BlockState{

        protected _BlockController _blockController;
        protected MeshRenderer _meshRenderer;

        public _BlockState(_BlockController blockController){
            _blockController = blockController;
            _meshRenderer = _blockController.GetComponent<MeshRenderer>();
        }

        public virtual void Init(bool isSetColor = false, Vector3 color = default, Mesh specialMesh = null, Material specialMaterial = null){
        }

        public virtual void SetUp(){
        }


        public virtual void OnSelect(){
        }

        public bool IsCanMove {get; protected set;}
    }
}