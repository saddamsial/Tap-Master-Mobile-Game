using DG.Tweening;
using PopupSystem;
using UnityEngine;

namespace Core.GamePlay.Shop{
    public class _ShopPopup : BasePopup{
        [Header("Shop Elements")]
        [SerializeField] private Transform _previewBlock;

        public void Show(){
            base.ActivePopup();
            SetStateGamePlayCamera(false);
            RotateBlock();
        }

        public void Exit(){
            base.Hide();
            SetStateGamePlayCamera(true);
            _previewBlock.DOKill(); 
        }

        private void SetStateGamePlayCamera(bool state){
            _GameManager.Instance.CameraController.IsInteractable = state;
        }

        private void RotateBlock(){
            _previewBlock.DOLocalRotate(new Vector3(0, 360, 0), 10f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }
    }
}