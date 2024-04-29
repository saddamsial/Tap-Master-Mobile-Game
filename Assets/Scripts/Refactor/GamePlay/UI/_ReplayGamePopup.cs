using Core.GamePlay;
using PopupSystem;

namespace Core.UI.ExtendPopup
{
    public class _ReplayGamePopup : _NotificationPopup
    {
        public void OnClickAccept()
        {
            PopupManager.Instance.CloseAllPopup();
            _GameManager.Instance.ReTry();
        }
    }
}