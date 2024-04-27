using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.GamePlay.Block;
using DG.Tweening;
using MyTools.Generic;
using PopupSystem;
using TMPro;
using UnityEditor;
using UnityEngine;

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
        [SerializeField] private TMP_Text _coinText;
        [SerializeField] private Transform _purchaseButton;

        private Dictionary<_ShopPage, TwoStateElement> _gotoPageButtons;
        private _ItemPriceDatas _itemPriceDatas;
        private List<_ShopElements> _shopElements;
        private bool _isInit = false;
        private _ShopPage _currentPage;
        private MeshRenderer _previewBlockRenderer;
        private _PurchaseItemButton _purchaseItemButton;
        private List<int> _listUnPurchased;


        public override void Awake()
        {
            base.Awake();

            _previewBlockRenderer = _previewBlock.GetComponent<MeshRenderer>();
            _GameEvent.OnSelectShopElement += OnClickShopElement();
            _GameEvent.OnSelectRewardBlock += UpdateCoinText;
            _GameEvent.OnSelectRewardBlockToWin += UpdateCoinText;
            _GameEvent.OnReceivedRewardByAds += UpdateCoinText;
            _GameEvent.OnGameWin += () => {UpdateCoinText(_BlockTypeEnum.GoldReward, 0);};
            _purchaseItemButton = new _PurchaseItemButton(_purchaseButton);
            _itemPriceDatas = _GameManager.Instance.ItemPriceDatas;
            UpdateCoinText(_BlockTypeEnum.GoldReward, 0);
            SetupNavigationButton();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            // _GameEvent.OnSelectArrow -= OnClickElement(_ShopPage.Arrow);
            // _GameEvent.OnSelectBlock -= OnClickElement(_ShopPage.Block);
            // _GameEvent.OnSelectColor -= OnClickElement(_ShopPage.Color);

            _GameEvent.OnSelectShopElement -= OnClickShopElement();
            _GameEvent.OnSelectRewardBlock -= UpdateCoinText;
            _GameEvent.OnSelectRewardBlockToWin -= UpdateCoinText;
            _GameEvent.OnReceivedRewardByAds -= UpdateCoinText;
            _GameEvent.OnGameWin -= () => {UpdateCoinText(_BlockTypeEnum.GoldReward, 0);};
        }

        public void Show()
        {
            base.Show(
                () => {
                    _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
                }
            );
            //SetStateGamePlayCamera(false);
            RotateBlock();
        }

        public void Exit()
        {
            base.Hide(() => {
                _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
            });
            //SetStateGamePlayCamera(true);
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

        public void OnClickPurchaseButton()
        {
            int purchasedIndex = UnityEngine.Random.Range(0, _shopElements.Count);
            while (_PlayerData.UserData.RuntimePurchasedShopData[_currentPage].Contains(purchasedIndex))
            {
                purchasedIndex = (purchasedIndex + 1) % _shopElements.Count;
            }
            StartCoroutine(RandomPurchasedElement(2f, purchasedIndex));
            _PlayerData.UserData.Coin -= _itemPriceDatas.GetPrice(_currentPage, _PlayerData.UserData.GetCurrentTimePurchaseItem(_currentPage));
            _PlayerData.UserData.UpdatePurchasedData(_currentPage, purchasedIndex);
            UpdateCoinText(_BlockTypeEnum.GoldReward, 0);
        }

        public void OnClickWatchAdButton()
        {

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
            UpdatePreviewBlock(_PlayerData.UserData.RuntimeSelectedShopData[_ShopPage.Arrow], _ShopPage.Arrow);
            UpdatePreviewBlock(_PlayerData.UserData.RuntimeSelectedShopData[_ShopPage.Color], _ShopPage.Color);
            UpdatePreviewBlock(_PlayerData.UserData.RuntimeSelectedShopData[_ShopPage.Block], _ShopPage.Block);
        }

        private void SetupNavigationButton()
        {
            _gotoPageButtons = new Dictionary<_ShopPage, TwoStateElement>
            {
                { _ShopPage.Block, new TwoStateElement(_navigationBar.GetChild(1)) },
                { _ShopPage.Color, new TwoStateElement(_navigationBar.GetChild(2)) },
                { _ShopPage.Arrow, new TwoStateElement(_navigationBar.GetChild(0)) }
            };

            OnClickGotoArrowPage();
        }

        private void UpdatePurchasedButton(bool isInit = true)
        {
            if (!isInit) return;
            int coin = _PlayerData.UserData.Coin;
            int price = _itemPriceDatas.GetPrice(_currentPage, _PlayerData.UserData.GetCurrentTimePurchaseItem(_currentPage));
            _purchaseItemButton.SetUpPurchaseItemButton(price, coin < price);
        }

        private void UpdateCoinText(_BlockTypeEnum type, int tmp)
        {
            if (type == _BlockTypeEnum.GoldReward)
            {
                _coinText.text = _PlayerData.UserData.Coin.ToString();
                UpdatePurchasedButton();
            }
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
            UpdatePurchasedButton();
        }

        private IEnumerator RandomPurchasedElement(float timer, int purchasedIndex)
        {
            var tmp = timer;
            var listPurchased = _PlayerData.UserData.RuntimePurchasedShopData[_currentPage];
            int currentElement = UnityEngine.Random.Range(0, _shopElements.Count);
            int lastElment = 0;
            _listUnPurchased ??= new List<int>();
            _listUnPurchased.Clear();
            for (int i = 0; i < _shopElements.Count; i++)
            {
                if (!listPurchased.Contains(i))
                {
                    _listUnPurchased.Add(i);
                }
            }
            if (_listUnPurchased.Count == 0)
            {
                yield return null;
            }
            else if (_listUnPurchased.Count == 1)
            {
                _shopElements[purchasedIndex].DisplayHighlightElement(true);
                _shopElements[_listUnPurchased[0]].SetState(true);
                yield return new WaitForSeconds(0.2f);
                _shopElements[_listUnPurchased[0]].DisplayHighlightElement(false);
            }
            else
            {
                while (tmp > 0)
                {
                    tmp -= 0.2f;
                    yield return new WaitForSeconds(0.2f);
                    currentElement = UnityEngine.Random.Range(0, _listUnPurchased.Count);
                    if (currentElement == lastElment)
                    {
                        currentElement = (currentElement + 1) % _listUnPurchased.Count;
                    }
                    _shopElements[_listUnPurchased[currentElement]].DisplayHighlightElement(true);
                    _shopElements[_listUnPurchased[lastElment]].DisplayHighlightElement(false);
                    lastElment = currentElement;
                }
                _shopElements[_listUnPurchased[currentElement]].DisplayHighlightElement(false);
                _shopElements[purchasedIndex].DisplayHighlightElement(true);
                yield return new WaitForSeconds(0.2f);
                _shopElements[purchasedIndex].SetState(true);
                _shopElements[purchasedIndex].DisplayHighlightElement(false);
            }
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

        private Action<int, _ShopPage> OnClickShopElement()
        {
            return (int id, _ShopPage type) =>
            {
                _shopElements[_PlayerData.UserData.RuntimeSelectedShopData[type]].SetState(true, false);
                _PlayerData.UserData.UpdateSelectedData(type, id);
                _shopElements[id].SetState(true, true);

                UpdatePreviewBlock(id, type);
            };
        }

        private Action<int> OnClickElement(_ShopPage type)
        {
            return (int id) =>
            {
                _shopElements[_PlayerData.UserData.RuntimeSelectedShopData[type]].SetState(true, false);
                _PlayerData.UserData.UpdateSelectedData(type, id);
                _shopElements[id].SetState(true, true);

                UpdatePreviewBlock(id, type);
            };
        }

        private void UpdatePreviewBlock(int id, _ShopPage type)
        {
            switch (type)
            {
                case _ShopPage.Block:
                    _previewBlockRenderer.material.SetTexture(_ConstantBlockSetting.KEY_IDLE_NORMALMAP_TEXTURE, _shopElementDatas.blockData.ElementAt(id).Value);
                    break;
                case _ShopPage.Color:
                    _previewBlockRenderer.material.SetColor(_ConstantBlockSetting.KEY_CORLOR_SETTING, _shopElementDatas.colorData.ElementAt(id).Value.blockColor);
                    break;
                case _ShopPage.Arrow:
                    _previewBlockRenderer.material.SetTexture(_ConstantBlockSetting.KEY_ARROW_TEXTTURE, _shopElementDatas.arrowData.ElementAt(id).Value);
                    break;
            }
        }

#if UNITY_EDITOR // Load data tools
        [Header("Data Resources Path")]
        [SerializeField] private string _arrowSpriteIcon;
        [SerializeField] private string _arrowTexture;
        [SerializeField] private string _blockSpriteICon;
        [SerializeField] private string _blockNormalMap;
        [SerializeField] private string _colorSpriteIcon;
        [SerializeField] private string _colorTexture;

        [ContextMenu("Load Data")]
        private void LoadData()
        {
            var listArrowSpriteIcon = System.IO.Directory.GetFiles(_arrowSpriteIcon, "*.png", System.IO.SearchOption.AllDirectories);
            List<Sprite> listSprite = new List<Sprite>();
            foreach (var name in listArrowSpriteIcon)
            {
                listSprite.Add(AssetDatabase.LoadAssetAtPath<Sprite>(name));
            }

            var listArrowSpriteTexture = System.IO.Directory.GetFiles(_arrowTexture, "*.png", System.IO.SearchOption.AllDirectories);
            List<Texture2D> listTexture = new List<Texture2D>();
            foreach (var name in listArrowSpriteTexture)
            {
                listTexture.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(name));
            }

            var listBlockSpriteIcon = System.IO.Directory.GetFiles(_blockSpriteICon, "*.png", System.IO.SearchOption.AllDirectories);
            List<Sprite> listBlockSprite = new List<Sprite>();
            foreach (var name in listBlockSpriteIcon)
            {
                listBlockSprite.Add(AssetDatabase.LoadAssetAtPath<Sprite>(name));
            }

            var listBlockNormalMap = System.IO.Directory.GetFiles(_blockNormalMap, "*.png", System.IO.SearchOption.AllDirectories);
            List<Texture2D> listBlockTexture = new List<Texture2D>();
            foreach (var name in listBlockNormalMap)
            {
                listBlockTexture.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(name));
            }

            var listColorSpriteIcon = System.IO.Directory.GetFiles(_colorSpriteIcon, "*.png", System.IO.SearchOption.AllDirectories);
            List<Sprite> listColorSprite = new List<Sprite>();
            foreach (var name in listColorSpriteIcon)
            {
                listColorSprite.Add(AssetDatabase.LoadAssetAtPath<Sprite>(name));
            }

            var listColorTxt = System.IO.Directory.GetFiles(_colorTexture, "*.txt", System.IO.SearchOption.AllDirectories);
            char[] splitChar = { '\n', '\r' };
            var colorString = System.IO.File.ReadAllText(listColorTxt[0]).Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            List<_ColorPurchased> listColor = new List<_ColorPurchased>();
            char splitColor = ',';
            foreach (var color in colorString)
            {
                var colorData = color.Split(splitColor);
                if (ColorUtility.TryParseHtmlString(colorData[0], out Color blockColor))
                {
                    if (ColorUtility.TryParseHtmlString(colorData[1], out Color backgroundColor))
                    {
                        listColor.Add(new _ColorPurchased(blockColor, backgroundColor));
                    }
                    else Debug.LogError("Color not valid");
                }
            }

            _shopElementDatas.arrowData = new SKUnityToolkit.SerializableDictionary.SerializableDictionary<Sprite, Texture2D>();
            if (listArrowSpriteIcon.Length == listArrowSpriteTexture.Length)
            {
                for (int i = 0; i < listArrowSpriteIcon.Length; i++)
                {
                    _shopElementDatas.arrowData.Add(listSprite[i], listTexture[i]);
                }
            }
            else
            {
                Debug.LogError("Arrow data not match");
            }

            _shopElementDatas.blockData = new SKUnityToolkit.SerializableDictionary.SerializableDictionary<Sprite, Texture2D>();
            if (listBlockSprite.Count == listBlockTexture.Count)
            {
                for (int i = 0; i < listBlockSprite.Count; i++)
                {
                    _shopElementDatas.blockData.Add(listBlockSprite[i], listBlockTexture[i]);
                }
            }
            else
            {
                Debug.LogError("Block data not match");
            }

            _shopElementDatas.colorData = new SKUnityToolkit.SerializableDictionary.SerializableDictionary<Sprite, _ColorPurchased>();
            if (listColorSprite.Count == listColor.Count)
            {
                for (int i = 0; i < listColorSprite.Count; i++)
                {
                    _shopElementDatas.colorData.Add(listColorSprite[i], listColor[i]);
                }
            }
            else
            {
                Debug.LogError("Color data not match " + listColorSprite.Count + " " + listColor.Count);
            }

            UnityEditor.EditorUtility.SetDirty(_shopElementDatas);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
