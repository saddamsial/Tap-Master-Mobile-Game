using Core.GamePlay;
using PopupSystem;
using UnityEngine;

namespace Core.UI.ExtendPopup{
    public class _NotificationPopup : BasePopup{
        
        [SerializeField] private TMPro.TMP_Text _messageText;
        private bool _isTopOfGroupPopup;

        public void Show(string message, bool isTopOfGroupPopup = false){
            base.Show();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            _messageText.text = message;
            _isTopOfGroupPopup = isTopOfGroupPopup;
        }

        public void OnClickClose(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            base.Hide(
                () => {
                    if(!_isTopOfGroupPopup)
                        _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
                }
            );
        }
    }
}