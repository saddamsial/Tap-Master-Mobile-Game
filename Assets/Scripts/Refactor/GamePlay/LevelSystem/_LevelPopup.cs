using Extensions.InfinityScroll;
using PopupSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GamePlay.LevelSystem{
    public class _LevelPopup : BasePopup{
        private const int _numberEasyLevels = 10;
        private const int _numberMediumLevels = 10;
        private const int _numberMasterLevels = 10;

        [Header("Resource")]
        [SerializeField] private GameObject _levelElementPrefab;
        [SerializeField] private LevelDatas _levelDatas;

        [Header("Elements")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private RectTransform _rect;

        private _InfinityScroll _infinityScroll;

        public void Show(){
            base.Show();
            _infinityScroll ??= new _InfinityScroll(_scrollRect, _levelDatas.numberOfLevels, _gridLayoutGroup.constraintCount, _gridLayoutGroup.cellSize.y, _gridLayoutGroup.cellSize.x, _rect.rect.height, _levelElementPrefab);
            _infinityScroll.Init();
        }

        public void Exit(){
            base.Hide();
        }

        public void OnScroll(){
            _infinityScroll.OnScrollValueChange(_scrollRect.verticalNormalizedPosition);
        }
    }
}