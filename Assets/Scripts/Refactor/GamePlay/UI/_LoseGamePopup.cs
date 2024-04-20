using UnityEngine;
using PopupSystem;
using Core.GamePlay;

namespace Core.UI.ExtendPopup{
    public class _LoseGamePopup : BasePopup{
        
        public void Show(){
            base.Show();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
        }

        public void OnClickClose(){
            base.Hide();
        }

        public void OnClickWatchAds(){
            
        }

        public void OnClickRetry(){
            base.Hide(() => {_GameManager.Instance.ReTry();}); 
        }
    }
}