
using MyTools.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GamePlay.Shop
{
    public class _ShopElements : MonoBehaviour
    {
        private TwoStateElement _stateElement;
        private Image _icon;
        private int _elementId;
        private Transform _selectedIcon;
        private bool _isInit = false;
        private bool _isInteractable = false;
        private _ShopPage _currentElementType;
        private Image _boderFrame;

        public void InitElements(_ShopPage type)
        {
            _currentElementType = type;
            if (_isInit) return;
            _isInit = true;
            _stateElement = new TwoStateElement(this.transform);
            _icon = this.transform.GetChild(0).GetComponent<Image>();
            _selectedIcon = this.transform.GetChild(2);
            _boderFrame = this.GetComponent<Image>();
        }

        public void SetUpShopElement(Sprite icon, int id, bool isPurchased, bool isSelected = false)
        {
            InitElements(_currentElementType);
            _icon.sprite = icon;
            _elementId = id;
            SetState(isPurchased, isSelected);

        }

        public void SetState(bool isPurchased, bool isSelectd = false)
        {
            _stateElement.SetState(isPurchased);
            _selectedIcon.gameObject.SetActive(isSelectd);
            _isInteractable = isPurchased && !isSelectd;
        }

        public void OnClickElement()
        {
            if(!_isInteractable) return;
            _GameEvent.OnSelectShopElement?.Invoke(_elementId, _currentElementType);
            //_GameEvent.OnSelectArrow?.Invoke(_elementId);
        }

        public void DisplayHighlightElement(bool isHighlight){
            //_boderFrame.color = isHighlight ? Color.yellow : Color.white;
            _selectedIcon.gameObject.SetActive(isHighlight);
        }
    }
}