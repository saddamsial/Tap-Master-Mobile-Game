using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Core.Data;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;

namespace Core.UI.ExtendPopup{
    public class _WinGamePopup : BasePopup{
        [SerializeField] private TMPro.TMP_Text _coinText;
        [SerializeField] private TMP_Text _multiCoinText;
        [SerializeField] private GameObject _watchAdsButton;
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] Image _nativeAdImage;
        
        public void Show(){
            base.Show(
                () => {
                    AnimWinGame();
                }
            );
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            _coinText.gameObject.SetActive(false);
            int currentCoin = _PlayerData.UserData.CurrentCollectCoin;
            int coin = currentCoin;
            _coinText.text = "+" + coin.ToString();
            _multiCoinText.text =  "+" + (coin * 2).ToString();
            if (coin > 0){
                _coinText.gameObject.SetActive(true);
                _watchAdsButton.SetActive(true);
            }
            else{
                _watchAdsButton.SetActive(false);
            }
            AddNativeAd();
        }

        public void OnClickClose(){
            base.Hide();
        }

        public void OnClickWatchAds(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            AdsManager.Instance.ShowRewarded(
                (x) => {
                    if(x){
                        GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel, "win_game_watch_ads_to_multi_coin");
                        _PlayerData.UserData.Coin += _PlayerData.UserData.CurrentCollectCoin;
                        _coinText.text = "+" + _PlayerData.UserData.CurrentCollectCoin * 2;
                        _watchAdsButton.SetActive(false);
                    }
                }, null, location: "win_game_watch_ads_to_multi_coin"
            );
        }

        public void OnClickToContinue(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            //base.Hide(() => {_GameManager.Instance.NextLevel();}); 
            this.gameObject.SetActive(false);
            _GameManager.Instance.NextLevel();
        }

        private void AnimWinGame(){
            _skeletonAnimation.initialSkinName = "default";
            _skeletonAnimation.AnimationState.SetAnimation(0, "Win-Appear", false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "Win-Idle", true, 0);
        }

        private void AddNativeAd(){
            var nativeAd = AdsManager.Instance.GetNativeAd();
            if(nativeAd != null){
                _nativeAdImage.sprite = Sprite.Create(nativeAd.GetIconTexture(), new Rect(0, 0, nativeAd.GetIconTexture().width, nativeAd.GetIconTexture().height), new Vector2(0.5f, 0.5f));
            }
            else{
                _nativeAdImage.gameObject.SetActive(false);
            }
        }
    }
}