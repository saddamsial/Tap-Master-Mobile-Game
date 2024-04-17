using System;
using System.Collections.Generic;
using MyTools.Generic;
using PopupSystem;
using UIS;
using UnityEngine;

namespace Core.GamePlay.LevelSystem
{
    public enum _LevelType
    {
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

        private int _maxDataLevelInMode = 30;
        private Dictionary<_LevelType, TwoStateElement> _gotoPageButton;
        private _LevelType _currentLevelType = _LevelType.Easy;

        private bool _isInit = false;

        public void Show()
        {
            base.Show();
            if (_isInit) return;
            _isInit = true;
            InitInfinityScroll();
            InitPopupElement();
            
        }

        public void Exit()
        {
            base.Hide();
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
            List.InitData(10);
        }

        /// <summary>
        /// Callback on fill item
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="item">Item object</param>
        void OnFillItem(int index, GameObject item)
        {
            //item.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
            item.GetComponent<_LevelElementsContainer>().SetLevelInLine(index, _maxDataLevelInMode);
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
        
        private void InitPopupElement(){
            _gotoPageButton = new Dictionary<_LevelType, TwoStateElement>(){
                {_LevelType.Easy, new TwoStateElement(_navigationBar.GetChild(0))},
                {_LevelType.Medium, new TwoStateElement(_navigationBar.GetChild(1))},
                {_LevelType.Master, new TwoStateElement(_navigationBar.GetChild(2))}
            };
            GotoLevelPage(0);
        }
        
        public void GotoLevelPage(int i)
        {
            var nextLevelType = (_LevelType)i;
            if (_currentLevelType == nextLevelType) return;
            _gotoPageButton[_currentLevelType].SetState(false);
            _currentLevelType = nextLevelType;
            _gotoPageButton[_currentLevelType].SetState(true);
            List.RecycleAll();
            switch (_currentLevelType)
            {
                case _LevelType.Easy:
                    _maxDataLevelInMode = 30;
                    break;
                case _LevelType.Medium:
                    _maxDataLevelInMode = 60;
                    break;
                case _LevelType.Master:
                    _maxDataLevelInMode = 89;
                    break;
                default:
                    break;
            }
            List.InitData(Mathf.CeilToInt(_maxDataLevelInMode / 3.0f));
        }
    }
}