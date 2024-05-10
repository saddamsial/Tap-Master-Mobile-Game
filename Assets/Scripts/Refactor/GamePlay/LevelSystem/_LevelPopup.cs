using System.Collections.Generic;
using Core.Data;
using Core.SystemGame;
using Core.UI.ExtendPopup;
using MyTools.Generic;
using PopupSystem;
using TMPro;
using UIS;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GamePlay.LevelSystem
{
    public enum _LevelType
    {
        None = -1,
        Easy,
        Medium,
        Master
    }

    public class _LevelPopup : BasePopup
    {
        /// <summary>
        /// Link to list
        /// </summary>
        [SerializeField]
        Scroller List = null;

        /// <summary>
        /// Items count
        /// </summary>
        [SerializeField]
        int Count = 100;

        [SerializeField] private Transform _navigationBar;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Image _bodyFrame;
        [Header("Element Resources")]
        [SerializeField] private Sprite _easyFrameSprite;
        [SerializeField] private Sprite _mediumFrameSprite;
        [SerializeField] private Sprite _masterFrameSprite;
        [SerializeField] private Sprite _easyElementSprite;
        [SerializeField] private Sprite _mediumElementSprite;
        [SerializeField] private Sprite _masterElementSprite;

        private int _maxDataLevelInMode = 30;
        private Dictionary<_LevelType, TwoStateElement> _gotoPageButton;
        private _LevelType _currentLevelType = _LevelType.None;
        private int _gotoLevel;
        private bool _isCanGoToLevel = false;

        private bool _isInit = false;

        public override void Awake()
        {
            base.Awake();
            _inputField.onEndEdit.AddListener(OnEndEditHanlder);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _inputField.onEndEdit.RemoveListener(OnEndEditHanlder);
        }

        public void Show()
        {
            base.Show();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            if (!_isInit)
            {
                _isInit = true;
                InitInfinityScroll();
                InitPopupElement();
            }
            //List.RecycleAll();
            GotoLevelPage(0);
            _isCanGoToLevel = false;
            _inputField.text = "";
        }

        public void Exit()
        {
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            base.Hide(
                () => { _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;}
            );
            _gotoPageButton[_currentLevelType].SetState(false);
            _currentLevelType = _LevelType.None;
        }

        #region Infinity Scroll Action
        /// <summary>
        /// Init
        /// </summary>
        void InitInfinityScroll()
        {
            //Debug.Log("Start");
            List.OnFill += OnFillItem;
            List.OnHeight += OnHeightItem;
            //List.InitData(10);
        }

        /// <summary>
        /// Callback on fill item
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="item">Item object</param>
        void OnFillItem(int index, GameObject item)
        {
            //item.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
            item.GetComponent<_LevelElementsContainer>().SetLevelInLine(index, GetStartGroupLevel(_currentLevelType), _maxDataLevelInMode);
            item.GetComponent<_LevelElementsContainer>().SetLevelElementsBackground(_currentLevelType switch
            {
                _LevelType.Easy => _easyElementSprite,
                _LevelType.Medium => _mediumElementSprite,
                _LevelType.Master => _masterElementSprite,
                _ => null
            });
        }

        /// <summary>
        /// Callback on request item height
        /// </summary>
        /// <param name="index">Item index</param>
        /// <returns>Current item height</returns>
        int OnHeightItem(int index)
        {
            return (int)List.Prefab.GetComponent<RectTransform>().rect.height;
        }
        #endregion

        public void OnEndEditHanlder(string value){
            if(int.TryParse(value, out int result)){
                if(CheckValidLevel(result - 1)){
                    //List.InitData(result);
                    _isCanGoToLevel = true;
                    _gotoLevel = result - 1;
                    //Debug.Log("Can go to level: " + _gotoLevel);
                    return;
                }
            }
        }

        public void OnGoToLevelClick(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            if(_isCanGoToLevel){
                _GameManager.Instance.StartLevel(_gotoLevel);
                PopupManager.Instance.CloseAllPopup();
            }
            else{
                PopupManager.CreateNewInstance<_NotificationPopup>().Show("Invalid value", true);
            }
        }

        private void InitPopupElement()
        {
            _gotoPageButton = new Dictionary<_LevelType, TwoStateElement>(){
                {_LevelType.Easy, new TwoStateElement(_navigationBar.GetChild(0))},
                {_LevelType.Medium, new TwoStateElement(_navigationBar.GetChild(1))},
                {_LevelType.Master, new TwoStateElement(_navigationBar.GetChild(2))}
            };
        }

        public void GotoLevelPage(int i)
        {
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            var nextLevelType = (_LevelType)i;
            if (_currentLevelType == nextLevelType) return;
            if (_currentLevelType != _LevelType.None)
                _gotoPageButton[_currentLevelType].SetState(false);
            _currentLevelType = nextLevelType;
            _gotoPageButton[_currentLevelType].SetState(true);
            List.RecycleAll();
            switch (_currentLevelType)
            {
                case _LevelType.Easy:
                    _maxDataLevelInMode = _ConstantGameplayConfig.LEVEL_EASY;
                    _bodyFrame.sprite = _easyFrameSprite;
                    break;
                case _LevelType.Medium:
                    _maxDataLevelInMode = _ConstantGameplayConfig.LEVEL_MEDIUM;
                    _bodyFrame.sprite = _mediumFrameSprite;
                    break;
                case _LevelType.Master:
                    _maxDataLevelInMode = _ConstantGameplayConfig.LEVEL_MASTER;
                    _bodyFrame.sprite = _masterFrameSprite;
                    break;
                default:
                    break;
            }
            List.InitData(Mathf.CeilToInt(_maxDataLevelInMode / 3.0f));
        }

        private int GetStartGroupLevel(_LevelType type)
        {
            return type switch
            {
                _LevelType.Easy => 0,
                _LevelType.Medium => _ConstantGameplayConfig.LEVEL_EASY,
                _LevelType.Master => _ConstantGameplayConfig.LEVEL_MEDIUM + _ConstantGameplayConfig.LEVEL_EASY,
                _ => 0
            };
        }

        private bool CheckValidLevel(int level){
            if(level >= GetStartGroupLevel(_LevelType.Easy) && level <= _PlayerData.UserData.HighestLevelInMode[_LevelType.Easy]) return true;
            if(level >= GetStartGroupLevel(_LevelType.Medium) && level <= _PlayerData.UserData.HighestLevelInMode[_LevelType.Medium]) return true;
            if(level >= GetStartGroupLevel(_LevelType.Master) && level <= _PlayerData.UserData.HighestLevelInMode[_LevelType.Master]) return true;
            return false;
        }
    }
}