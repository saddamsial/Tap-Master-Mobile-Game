
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

        public void InitElements(_ShopPage type)
        {
            _currentElementType = type;
            if (_isInit) return;
            _isInit = true;
            _stateElement = new TwoStateElement(this.transform);
            _icon = this.transform.GetChild(0).GetComponent<Image>();
            _selectedIcon = this.transform.GetChild(2);
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
            switch (_currentElementType)
            {
                case _ShopPage.Arrow:
                    _GameEvent.OnSelectArrow?.Invoke(_elementId);
                    break;
                case _ShopPage.Block:
                    _GameEvent.OnSelectBlock?.Invoke(_elementId);
                    break;
                case _ShopPage.Color:
                    _GameEvent.OnSelectColor?.Invoke(_elementId);
                    break;
            }
            //_GameEvent.OnSelectArrow?.Invoke(_elementId);
        }
    }
}