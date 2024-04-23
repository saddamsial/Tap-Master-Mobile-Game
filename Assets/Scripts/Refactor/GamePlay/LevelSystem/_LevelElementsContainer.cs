using System.Collections.Generic;
using UnityEngine;

namespace Core.GamePlay.LevelSystem{
    public class _LevelElementsContainer : MonoBehaviour{
        [SerializeField] private GameObject _levelItemPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _numberOfItemsPerRow = 3;
        private bool _isInit = false;

        private List<_LevelElements> _listContainedLevelElements = new List<_LevelElements>();

        private void Awake(){
            Init(_numberOfItemsPerRow);
        }

        public void Init(int numberOfItems){
            if(_isInit) return;
            _isInit = true;
            for(int i = 0; i < 3; i++){
                var item = SimplePool.Spawn(_levelItemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_container);
                item.transform.localScale = Vector3.one;
                item.transform.position = Vector3.zero;
                _listContainedLevelElements.Add(new _LevelElements(item.transform));
            }
            Debug.Log(_listContainedLevelElements.Count);
        }

        public void SetLevelInLine(int line, int startGroupLevel = 0,int maxLevel = -1){
            // int isHaveMaxLevel = maxLevel == -1 ? 0 : 1;
            for(int i = 0; i < _numberOfItemsPerRow; i++){
                int level = line * _numberOfItemsPerRow + i + 1 + startGroupLevel;
                if(level <= maxLevel + startGroupLevel){
                    _listContainedLevelElements[i].SetLevel(level);
                }else{
                    _listContainedLevelElements[i].SetLevel(-1);
                }
            }
        }

        public void SetLevelElementsBackground(Sprite sprite){
            if(sprite == null) throw new System.Exception("Sprite is null");
            for(int i = 0; i < _listContainedLevelElements.Count; i++){
                _listContainedLevelElements[i].SetBackgroundElement(sprite);
            }
        }
    }
}