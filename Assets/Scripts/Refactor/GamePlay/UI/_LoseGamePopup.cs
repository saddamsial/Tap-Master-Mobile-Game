using UnityEngine;
using PopupSystem;
using Core.GamePlay;
using Spine.Unity;

namespace Core.UI.ExtendPopup{
    public class _LoseGamePopup : BasePopup{
        [SerializeField] private SkeletonAnimation _skeletonAnimation;

        public void Show(){
            base.Show(AnimLoseGame);
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

        private void AnimLoseGame(){
            _skeletonAnimation.initialSkinName = "default";
            _skeletonAnimation.AnimationState.SetAnimation(0, "Lose-Appear", false);
            _skeletonAnimation.AnimationState.AddAnimation(0, "Lose-Idle", true, 0);
        }
    }
}