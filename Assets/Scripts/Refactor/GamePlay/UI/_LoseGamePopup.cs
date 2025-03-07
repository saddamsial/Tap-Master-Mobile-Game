using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Spine.Unity;
using Core.Data;
using MyTools.ScreenSystem;
using UnityEngine.UI;

namespace Core.UI.ExtendPopup{
    public class _LoseGamePopup : BasePopup{
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] Image _nativeAdImage;

        public void Show(){
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            base.Show(
                () => {
                    AnimLoseGame();
                }
            );
            AddNativeAd();
        }

        public void OnClickClose(){
            base.Hide();
        }

        public void OnClickWatchAds(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            AdsManager.Instance.ShowRewarded(
                (x) => {
                    GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel, "lose_game_watch_ads_to_continue");
                    _GameManager.Instance.GamePlayManager.OnContinueGame();
                    _GameEvent.OnGamePlayContinue?.Invoke();
                    _ScreenManager.Instance.ShowScreen(_ScreenTypeEnum.GamePlay);
                    base.Hide();
                }, null, location: "lose_game_watch_ads_to_continue"
            );
        }

        public void OnClickRetry(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            // base.Hide(() => {
            //     _GameManager.Instance.ReTry();
            // }); 
            this.gameObject.SetActive(false);
            _GameManager.Instance.ReTry();
        }

        private void AnimLoseGame(){
            _skeletonAnimation.initialSkinName = "default";
            _skeletonAnimation.AnimationState.SetAnimation(0, "Lose-Appear", false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "Lose-Idle", true, 0);
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