using System.Drawing;
using MyTools.Generic;
using UnityEngine;

namespace Core.GamePlay.Shop{
    public class _ShopElements{
        private GameObject _elementObject;
        private TwoStateElement _stateElement;
        private Image _icon;
        private Transform _selectedIcon;

        public _ShopElements(Transform rootObject){
            _elementObject = rootObject.gameObject;
            _stateElement = new TwoStateElement(rootObject);
            _icon = rootObject.GetChild(0).GetComponent<Image>();
            _selectedIcon = rootObject.GetChild(3);
        }

        public void SetUpShopElement(){
            
        }

        private void SetState(bool isPurchased, bool isSelectd = false){
            _stateElement.SetState(isPurchased);
            _selectedIcon.gameObject.SetActive(isSelectd);
        }
    }
}