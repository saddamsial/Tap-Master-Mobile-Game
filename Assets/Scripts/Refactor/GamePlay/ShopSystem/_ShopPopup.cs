using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.GamePlay.Block;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using MyTools.Generic;
using PopupSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core.GamePlay.Shop
{
    public enum _ShopPage
    {
        Block,
        Color,
        Arrow
    }


    public class _ShopPopup : BasePopup
    {
        [Header("Shop Data")]
        [SerializeField] private _ShopElementDatas _shopElementDatas;
        [SerializeField] private GameObject _shopElementPrefab;

        [Header("Shop Elements")]
        [SerializeField] private Transform _previewBlock;
        [SerializeField] private Transform _navigationBar;
        [SerializeField] private Transform _elementContainer;

        private Dictionary<_ShopPage, TwoStateElement> _gotoPageButtons;

        private List<_ShopElements> _shopElements;
        private bool _isInit = false;
        private _ShopPage _currentPage;
        private MeshRenderer _previewBlockRenderer;

        public override void Awake()
        {
            base.Awake();
            SetupNavigationButton();
            _previewBlockRenderer = _previewBlock.GetComponent<MeshRenderer>();
            _GameEvent.OnSelectArrow += (int para) => { OnClickElement(para, _ShopPage.Arrow); };
            _GameEvent.OnSelectBlock += (int para) => { OnClickElement(para, _ShopPage.Block); };
            _GameEvent.OnSelectColor += (int para) => { OnClickElement(para, _ShopPage.Color); };
        }

        public void Show()
        {
            base.ActivePopup();
            SetStateGamePlayCamera(false);
            RotateBlock();
        }

        public void Exit()
        {
            base.Hide();
            SetStateGamePlayCamera(true);
            _previewBlock.DOKill();
        }

        public void OnClickGotoArrowPage()
        {
            _gotoPageButtons[_currentPage].SetState(false);
            _currentPage = _ShopPage.Arrow;
            _gotoPageButtons[_currentPage].SetState(true);

            LoadPage();
        }

        public void OnClickGotoBlockPage()
        {
            _gotoPageButtons[_currentPage].SetState(false);
            _currentPage = _ShopPage.Block;
            _gotoPageButtons[_currentPage].SetState(true);

            LoadPage();
        }

        public void OnClickGotoColorPage()
        {
            _gotoPageButtons[_currentPage].SetState(false);
            _currentPage = _ShopPage.Color;
            _gotoPageButtons[_currentPage].SetState(true);

            LoadPage();
        }

        private void SetStateGamePlayCamera(bool state)
        {
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = state;
        }

        private void RotateBlock()
        {
            _previewBlock.DOLocalRotate(new Vector3(0, 360, 0), 10f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        private void SetupNavigationButton()
        {
            _gotoPageButtons = new Dictionary<_ShopPage, TwoStateElement>();
            _gotoPageButtons.Add(_ShopPage.Block, new TwoStateElement(_navigationBar.GetChild(1)));
            _gotoPageButtons.Add(_ShopPage.Color, new TwoStateElement(_navigationBar.GetChild(2)));
            _gotoPageButtons.Add(_ShopPage.Arrow, new TwoStateElement(_navigationBar.GetChild(0)));

            OnClickGotoArrowPage();
        }

        private void LoadPage()
        {
            InitShopElements();
            DespawnShopElements();
            switch (_currentPage)
            {
                case _ShopPage.Block:
                    var blockData = _shopElementDatas.blockData;
                    foreach (var data in blockData)
                    {
                        var shopElement = SimplePool.Spawn(_shopElementPrefab, _previewBlock.position, Quaternion.identity).GetComponent<_ShopElements>();
                        shopElement.transform.SetParent(_elementContainer);
                        shopElement.transform.localScale = Vector3.one;
                        shopElement.transform.localPosition = Vector3.zero;
                        _shopElements.Add(shopElement);
                        shopElement.InitElements(_currentPage);
                        shopElement.SetUpShopElement(data.Key, _shopElements.Count - 1, false);
                    }
                    break;
                case _ShopPage.Color:
                    var colorData = _shopElementDatas.colorData;
                    foreach (var data in colorData)
                    {
                        var shopElement = SimplePool.Spawn(_shopElementPrefab, _previewBlock.position, Quaternion.identity).GetComponent<_ShopElements>();
                        shopElement.transform.SetParent(_elementContainer);
                        shopElement.transform.localScale = Vector3.one;
                        shopElement.transform.localPosition = Vector3.zero;
                        _shopElements.Add(shopElement);
                        shopElement.InitElements(_currentPage);
                        shopElement.SetUpShopElement(data.Key, _shopElements.Count - 1, false);
                    }
                    break;
                case _ShopPage.Arrow:
                    var arrowData = _shopElementDatas.arrowData;
                    foreach (var data in arrowData)
                    {
                        var shopElement = SimplePool.Spawn(_shopElementPrefab, _previewBlock.position, Quaternion.identity).GetComponent<_ShopElements>();
                        shopElement.transform.SetParent(_elementContainer);
                        shopElement.transform.localScale = Vector3.one;
                        shopElement.transform.localPosition = Vector3.zero;
                        _shopElements.Add(shopElement);
                        shopElement.InitElements(_currentPage);
                        shopElement.SetUpShopElement(data.Key, _shopElements.Count - 1, false);
                    }
                    break;
            }
            SetupElementState();
        }

        private void InitShopElements()
        {
            if (_isInit) return;
            _isInit = true;
            _shopElements = new List<_ShopElements>();
            //SimplePool.Preload(_shopElementPrefab, 9);
        }

        private void DespawnShopElements()
        {
            foreach (var shopElement in _shopElements)
            {
                shopElement.transform.SetParent(null);
                SimplePool.Despawn(shopElement.gameObject);
            }
            _shopElements.Clear();
        }

        private void SetupElementState()
        {
            var listData = _PlayerData.UserData.RuntimePurchasedShopData[_currentPage];
            foreach (int id in listData)
            {
                _shopElements[id].SetState(true);
            }
            _shopElements[_PlayerData.UserData.RuntimeSelectedShopData[_currentPage]].SetState(true, true);
            //OnClickElement(_PlayerData.UserData.RuntimeSelectedShopData[_currentPage], _currentPage);
        }
 
        private void OnClickElement(int id, _ShopPage type)
        {
            _shopElements[_PlayerData.UserData.RuntimeSelectedShopData[type]].SetState(true, false);
            _PlayerData.UserData.UpdateSelectedData(type, id);
            _shopElements[id].SetState(true, true);

            switch (type)
            {
                case _ShopPage.Block:
                    _previewBlockRenderer.material.SetTexture(_ConstantBlockSetting.KEY_IDLE_NORMALMAP_TEXTURE, _shopElementDatas.blockData.ElementAt(id).Value);
                    break;
                case _ShopPage.Color:
                    _previewBlockRenderer.material.SetColor(_ConstantBlockSetting.KEY_CORLOR_SETTING, _shopElementDatas.colorData.ElementAt(id).Value);
                    break;
                case _ShopPage.Arrow:
                    _previewBlockRenderer.material.SetTexture(_ConstantBlockSetting.KEY_ARROW_TEXTTURE, _shopElementDatas.arrowData.ElementAt(id).Value);
                    break;
            }
        }
    }
}