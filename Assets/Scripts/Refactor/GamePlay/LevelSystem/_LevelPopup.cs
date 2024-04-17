using System;
using System.Collections.Generic;
using Core.SystemGame;
using MyTools.Generic;
using PopupSystem;
using TMPro;
using UIS;
using UnityEngine;

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
            base.Hide();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
            _gotoPageButton[_currentLevelType].SetState(false);
            _currentLevelType = _LevelType.None;
        }

        #region Infinity Scroll Action
        /// <summary>
        /// Init
        /// </summary>
        void InitInfinityScroll()
        {
            Debug.Log("Start");
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
            item.GetComponent<_LevelElementsContainer>().SetLevelInLine(index, GetStartGroupLevel(), _maxDataLevelInMode);
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
                if(result > 0 && result <= Count){
                    //List.InitData(result);
                    _isCanGoToLevel = true;
                    _gotoLevel = result;
                }
                Debug.Log("Value: " + result);
            }
            else{
                Debug.Log("Invalid value");
            }
        }

        public void OnGoToLevelClick(){
            if(_isCanGoToLevel){
                _GameManager.Instance.StartLevel(_gotoLevel - 1);
                PopupManager.Instance.CloseAllPopup();
            }
            else{
                Debug.Log("Invalid value");
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
                    break;
                case _LevelType.Medium:
                    _maxDataLevelInMode = _ConstantGameplayConfig.LEVEL_MEDIUM;
                    break;
                case _LevelType.Master:
                    _maxDataLevelInMode = _ConstantGameplayConfig.LEVEL_MASTER;
                    break;
                default:
                    break;
            }
            List.InitData(Mathf.CeilToInt(_maxDataLevelInMode / 3.0f));
        }

        private int GetStartGroupLevel()
        {
            return _currentLevelType switch
            {
                _LevelType.Easy => 0,
                _LevelType.Medium => _ConstantGameplayConfig.LEVEL_EASY,
                _LevelType.Master => _ConstantGameplayConfig.LEVEL_MEDIUM + _ConstantGameplayConfig.LEVEL_EASY,
                _ => 0
            };
        }
    }
}