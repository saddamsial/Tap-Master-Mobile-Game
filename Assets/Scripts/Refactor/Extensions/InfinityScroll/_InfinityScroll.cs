using System.Collections.Generic;
using Core.GamePlay.LevelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions.InfinityScroll{
    public class _InfinityScroll {
        private readonly int _numberOfItems;
        private readonly int _numberOfItemsPerRow;
        private readonly float _itemsHeight;
        private readonly float _itemsWidth;
        private readonly ScrollRect _scrollRect;
        private readonly float _viewPortHeight;
        private readonly GameObject _itemPrefab;

        public _InfinityScroll(){}
        public _InfinityScroll(ScrollRect rect, int numberOfItems, int numberOfItemsPerRow, float itemsHeight, float itemsWidth, float viewPortHeight, GameObject itemPrefab){
            _scrollRect = rect;
            _numberOfItems = numberOfItems;
            _numberOfItemsPerRow = numberOfItemsPerRow;
            _itemsHeight = itemsHeight;
            _itemsWidth = itemsWidth;
            _viewPortHeight = viewPortHeight;
            _itemPrefab = itemPrefab;
        }

        private int _numOfViewedRows;
        private int _currentTopLine;
        private float _loopIncreaseNormalizePostion;
        private float _normalizedPivot;
        private Queue<GameObject> elementQueue = new Queue<GameObject>();

        public void Init(){
            CalCulateNumberOfRows();
            InitScrollElements();
            _currentTopLine = 0;
        }

        public void OnScrollValueChange(float value){
            //Debug.Log("OnScrollValueChange: " + value);
            if(value < _normalizedPivot){
                UpdateInfinityLoop();
            }
        }

        private void CalCulateNumberOfRows(){
            //Debug.Log(_viewPortHeight + " " + _itemsHeight + " " + _numberOfItemsPerRow);
            _numOfViewedRows = Mathf.FloorToInt(_viewPortHeight /_itemsHeight) + 4;
            var spacing = _scrollRect.content.GetComponent<GridLayoutGroup>().spacing.y;
            float totalHeight = _numOfViewedRows * _itemsHeight + (_numOfViewedRows - 1) * spacing;
            _loopIncreaseNormalizePostion = _itemsHeight / (totalHeight - _viewPortHeight) + spacing / totalHeight;
            _normalizedPivot = 1 - (_loopIncreaseNormalizePostion * 2);
        }

        private void InitScrollElements(){
            for(int i = 0; i < _numOfViewedRows; i++){
                for(int j = 0; j < _numberOfItemsPerRow; j++){
                    var item = Object.Instantiate(_itemPrefab, _scrollRect.content);
                    item.GetComponent<_LevelElements>().SetLevel(i * _numberOfItemsPerRow + j);
                    elementQueue.Enqueue(item);
                }
            }
        }

        private void UpdateInfinityLoop(){
            for(int i = 0; i < _numberOfItemsPerRow; i++){
                var item = elementQueue.Dequeue();
                item.transform.SetParent(null);
                // var pos = item.transform.localPosition;
                // if(pos.y > _itemsHeight * _numOfViewedRows){
                //     pos.y -= _itemsHeight * _numOfViewedRows;
                //     item.transform.localPosition = pos;
                //     elementQueue.Enqueue(item);
                // }
                item.transform.SetParent(_scrollRect.content);
                elementQueue.Enqueue(item);
            }
            Vector2 normalizedPosition = _scrollRect.normalizedPosition;
            normalizedPosition.y +=  _loopIncreaseNormalizePostion;
            _scrollRect.normalizedPosition = normalizedPosition;
        }
    }
}