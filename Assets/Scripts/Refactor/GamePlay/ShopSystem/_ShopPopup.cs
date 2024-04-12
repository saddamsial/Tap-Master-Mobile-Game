using DG.Tweening;
using MyTools.Generic;
using PopupSystem;
using UnityEngine;

namespace Core.GamePlay.Shop{
    public enum _ShopPage{
        Block,
        Color,
        Arrow
    }


    public class _ShopPopup : BasePopup{
        [Header("Shop Data")]
        [SerializeField] private _ShopElementDatas _shopElementDatas;
        [SerializeField] private GameObject _shopElementPrefab;

        [Header("Shop Elements")]
        [SerializeField] private Transform _previewBlock;
        [SerializeField] private Transform _navigationBar;

        private TwoStateElement _gotoBlockPage;
        private TwoStateElement _gotoColorPage;
        private TwoStateElement _gotoArrowPage;

        public override void Awake()
        {
            base.Awake();
            SetupNavigationButton();
        }

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

        public void OnClickGotoArrowPage(){
            _gotoArrowPage.SetState(true);
            _gotoBlockPage.SetState(false);
            _gotoColorPage.SetState(false);
        }

        public void OnClickGotoBlockPage(){
            _gotoArrowPage.SetState(false);
            _gotoBlockPage.SetState(true);
            _gotoColorPage.SetState(false);
        }

        public void OnClickGotoColorPage(){
            _gotoArrowPage.SetState(false);
            _gotoBlockPage.SetState(false);
            _gotoColorPage.SetState(true);
        }

        private void SetStateGamePlayCamera(bool state){
            _GameManager.Instance.CameraController.IsInteractable = state;
        }

        private void RotateBlock(){
            _previewBlock.DOLocalRotate(new Vector3(0, 360, 0), 10f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        private void SetupNavigationButton(){
            _gotoBlockPage = new TwoStateElement(_navigationBar.GetChild(1));
            _gotoColorPage = new TwoStateElement(_navigationBar.GetChild(2));
            _gotoArrowPage = new TwoStateElement(_navigationBar.GetChild(0));

            _gotoArrowPage.SetState(true);
            _gotoBlockPage.SetState(false);
            _gotoColorPage.SetState(false);
        }
    }
}