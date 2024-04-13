
using MyTools.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GamePlay.Shop{
    public class _ShopElements : MonoBehaviour{
        private TwoStateElement _stateElement;
        private Image _icon;
        private Transform _selectedIcon;
        private bool _isInit = false;

        public void InitElements(){
            if(_isInit) return;
            _isInit = true;
            _stateElement = new TwoStateElement(this.transform);
            _icon = this.transform.GetChild(0).GetComponent<Image>();
            _selectedIcon = this.transform.GetChild(2);
        }

        public void SetUpShopElement(Sprite icon, bool isPurchased, bool isSelected = false){
            InitElements();
            _icon.sprite = icon;
            SetState(isPurchased, isSelected);

        }

        public void SetState(bool isPurchased, bool isSelectd = false){
            _stateElement.SetState(isPurchased);
            _selectedIcon.gameObject.SetActive(isSelectd);
        }
    }
}