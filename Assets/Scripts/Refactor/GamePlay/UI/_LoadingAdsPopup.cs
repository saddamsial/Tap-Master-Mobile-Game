using UnityEngine;
using PopupSystem;
using DG.Tweening;
using System;

namespace Core.UI{
    public class _LoadingAdsPopup : BasePopup
    {
        [SerializeField] private Transform _loadingIcon;
        public void Show(Action complete = null)
        {
            base.Show();
            _loadingIcon.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetLoops(3).OnComplete(() => {
                Exit();
                complete?.Invoke();
            });
        }

        private void Exit(){
            Debug.Log("Exit");
            base.Hide();
        }
    }
}