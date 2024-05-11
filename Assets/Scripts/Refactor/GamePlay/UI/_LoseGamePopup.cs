using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Spine.Unity;
using Core.Data;

namespace Core.UI.ExtendPopup{
    public class _LoseGamePopup : BasePopup{
        [SerializeField] private SkeletonAnimation _skeletonAnimation;

        public void Show(){
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            base.Show(
                () => {
                    AnimLoseGame();
                }
            );
            AdsManager.Instance.ShowNativeOverlay();
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
                    base.Hide();
                }, null, location: "lose_game_watch_ads_to_continue"
            );
        }

        public void OnClickRetry(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            base.Hide(() => {_GameManager.Instance.ReTry();}); 
        }

        private void AnimLoseGame(){
            _skeletonAnimation.initialSkinName = "default";
            _skeletonAnimation.AnimationState.SetAnimation(0, "Lose-Appear", false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "Lose-Idle", true, 0);
        }
    }
}