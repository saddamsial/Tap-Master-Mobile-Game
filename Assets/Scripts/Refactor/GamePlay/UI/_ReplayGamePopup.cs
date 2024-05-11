using Core.GamePlay;
using PopupSystem;

namespace Core.UI.ExtendPopup
{
    public class _ReplayGamePopup : _NotificationPopup
    {
        public void OnClickAccept()
        {
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            PopupManager.Instance.CloseAllPopup();
            _GameManager.Instance.ReTry();
        }
    }
}