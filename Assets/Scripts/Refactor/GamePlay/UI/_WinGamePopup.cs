using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Core.Data;
using Spine.Unity;
using TMPro;

namespace Core.UI.ExtendPopup{
    public class _WinGamePopup : BasePopup{
        [SerializeField] private TMPro.TMP_Text _coinText;
        [SerializeField] private GameObject _watchAdsButton;
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        
        public void Show(){
            base.Show(
                () => {
                    AnimWinGame();
                    _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
                }
            );
            _coinText.gameObject.SetActive(false);
            int currentCoin = _PlayerData.UserData.CurrentCollectCoin;
            int coin = currentCoin;
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
            AdsManager.Instance.ShowRewarded(
                (x) => {
                    if(x){
                        _PlayerData.UserData.Coin += _PlayerData.UserData.CurrentCollectCoin;
                        _coinText.text = "+" + _PlayerData.UserData.CurrentCollectCoin * 2;
                        _watchAdsButton.SetActive(false);
                    }
                }
            );
        }

        public void OnClickToContinue(){
            base.Hide(() => {_GameManager.Instance.NextLevel();}); 
        }

        private void AnimWinGame(){
            _skeletonAnimation.initialSkinName = "default";
            _skeletonAnimation.AnimationState.SetAnimation(0, "Win-Appear", false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "Win-Idle", true, 0);
        }
    }
}