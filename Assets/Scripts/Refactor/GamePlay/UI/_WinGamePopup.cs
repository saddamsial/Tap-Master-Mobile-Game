using UnityEngine;
using PopupSystem;
using Core.GamePlay;

namespace Core.UI.ExtendPopup{
    public class _WinGamePopup : BasePopup{
        [SerializeField] private TMPro.TMP_Text _coinText;
        
        public void Show(){
            base.Show();
            _coinText.text = "+" + _GameManager.Instance.CurrentCollectedLevelCoin;
        }

        public void OnClickClose(){
            base.Hide();
        }

        public void OnClickWatchAds(){
            
        }

        public void OnClickToContinue(){
            base.Hide(() => {_GameManager.Instance.NextLevel();}); 
        }
    }
}