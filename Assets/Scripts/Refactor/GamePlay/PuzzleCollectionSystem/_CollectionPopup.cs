using System.Collections.Generic;
using PopupSystem;
using UnityEngine;

namespace Core.GamePlay.Collection{
    public class _CollectionPopup : BasePopup{
        [Header("Collection Resources")]
        [SerializeField] private _CollectionElementDatas _collectionElementDatas;
        [SerializeField] private GameObject _collectionElementPrefab;

        [Header("Collection Elements")]
        [SerializeField] private Transform _collectionContainers;
        
        private List<_CollectionElements> _listCollections;

        private bool _isInit = false;

        private void Init(){
            if(_isInit) return;
            _isInit = true;
            _listCollections = new List<_CollectionElements>();
            for(int i = 0; i < _collectionElementDatas.collectionElementDatas.Count; i++){
                var gameObject = SimplePool.Spawn(_collectionElementPrefab,Vector3.zero, Quaternion.identity);
                gameObject.transform.SetParent(_collectionContainers);
                var collectionElement = new _CollectionElements(gameObject.transform);
                _listCollections.Add(collectionElement);
            }
        }
        
        public void SetupCollection(){
            for (int i = 0; i < _listCollections.Count; i++){
                _listCollections[i].SetupPuzzle(_collectionElementDatas.collectionElementDatas[i]);
            }
        }
        
        public void Show(){
            base.Show();
            Init();
            SetupCollection();
        }

        public void Exit(){
            base.Hide();
        }

    }
}