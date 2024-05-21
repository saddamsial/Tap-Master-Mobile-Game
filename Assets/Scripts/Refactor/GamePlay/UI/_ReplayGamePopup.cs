using Core.GamePlay;
using PopupSystem;

namespace Core.UI.ExtendPopup
{
    public class _ReplayGamePopup : _NotificationPopup
    {
        public void OnClickAccept()
        {
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            // this.gameObject.SetActive(false);
            //PopupManager.Instance.CloseAllPopup();
            base.Hide();
            _GameManager.Instance.ReTry();
        }
    }
}