using System.Collections.Generic;
using Core.Data;
using PopupSystem;
using UnityEngine;

namespace Core.GamePlay.Collection{
    public class _CollectionPopup : BasePopup{
        [Header("Collection Resources")] 
        [SerializeField] private GameObject _collectionElementPrefab;

        [Header("Collection Elements")]
        [SerializeField] private Transform _collectionContainers;
        
        private List<_CollectionElements> _listCollections;
        _CollectionElementDatas _collectionElementDatas;
        private bool _isInit = false;

        private void Init(){
            if(_isInit) return;
            _collectionElementDatas = _GameManager.Instance.CollectionElementDatas;
            _isInit = true;
            _listCollections = new List<_CollectionElements>();
            for(int i = 0; i < _collectionElementDatas.collectionElementDatas.Count; i++){
                var gameObject = SimplePool.Spawn(_collectionElementPrefab,Vector3.zero, Quaternion.identity);
                gameObject.transform.SetParent(_collectionContainers);
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = new Vector3(0,0,-100);
                var collectionElement = new _CollectionElements(gameObject.transform);
                _listCollections.Add(collectionElement);
            }
        }
        
        public void SetupCollection(){
            for (int i = 0; i < _listCollections.Count; i++){
                _listCollections[i].SetupPuzzle(_collectionElementDatas.collectionElementDatas[i]);
            }
        }

        public void SetupCurrentStateCollection(){
            foreach(var data in _PlayerData.UserData.RuntimeCollectionData){
                foreach(var puzzlePieceId in data.Value){
                    _listCollections[data.Key].SetupPuzzleState(puzzlePieceId);
                }
            }
        }

        public void Show(){
            base.Show();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            Init();
            SetupCollection();
            SetupCurrentStateCollection();
        }

        public void Exit(){
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            base.Hide(
                () => {
                    _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
                }
            );
        }

    }
}