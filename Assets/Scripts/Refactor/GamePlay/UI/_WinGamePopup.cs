using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Core.Data;

namespace Core.UI.ExtendPopup{
    public class _WinGamePopup : BasePopup{
        [SerializeField] private TMPro.TMP_Text _coinText;
        [SerializeField] private GameObject _watchAdsButton;
        
        public void Show(){
            base.Show();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            _coinText.gameObject.SetActive(false);
            int coin = _PlayerData.UserData.CurrentCollectCoin;
            _coinText.text = "+" + coin.ToString();
            if (coin > 0){
                _coinText.gameObject.SetActive(true);
                _watchAdsButton.SetActive(true);
            }
            else{
                _watchAdsButton.SetActive(false);
            }
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