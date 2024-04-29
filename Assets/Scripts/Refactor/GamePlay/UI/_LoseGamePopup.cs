using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Spine.Unity;

namespace Core.UI.ExtendPopup{
    public class _LoseGamePopup : BasePopup{
        [SerializeField] private SkeletonAnimation _skeletonAnimation;

        public void Show(){
            base.Show(
                () => {
                    AnimLoseGame();
                    _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
                }
            );
            AdsManager.Instance.ShowNativeOverlay();
        }

        public void OnClickClose(){
            base.Hide();
        }

        public void OnClickWatchAds(){
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            AdsManager.Instance.ShowRewarded(
                (x) => {
                    GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel);
                    _GameManager.Instance.GamePlayManager.OnContinueGame();
                    _GameEvent.OnGamePlayContinue?.Invoke();
                    base.Hide();
                }
            );
        }

        public void OnClickRetry(){
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            base.Hide(() => {_GameManager.Instance.ReTry();}); 
        }

        private void AnimLoseGame(){
            _skeletonAnimation.initialSkinName = "default";
            _skeletonAnimation.AnimationState.SetAnimation(0, "Lose-Appear", false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "Lose-Idle", true, 0);
        }
    }
}