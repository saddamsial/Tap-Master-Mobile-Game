using PopupSystem;

namespace Core.GamePlay.Shop{
    public class _ShopPopup : BasePopup{
        public void Show(){
            base.ActivePopup();
        }

        public void Exit(){
            base.Hide();
        }
    }
}