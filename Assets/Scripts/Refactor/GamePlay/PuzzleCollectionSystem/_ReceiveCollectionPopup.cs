using System;
using System.Collections;
using Core.Data;
using PopupSystem;
using UnityEngine;

namespace Core.GamePlay.Collection{
    public class _ReceiveCollectionPopup : BasePopup{
        [SerializeField] private Transform _collectionElementRoot;
        private _CollectionElements _collectionElement;
        private _CollectionElementDatas _collectionDatas;

        private bool _isInit = false;

        private void Init(){
            if(_isInit) return;
            if(_collectionDatas == null){
                _collectionDatas = _GameManager.Instance.CollectionElementDatas;
            }
            _collectionElement = new _CollectionElements(_collectionElementRoot);
            _isInit = true;
        }

        public override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Show(int type, int index){
            base.Show(
                () => {
                    StartCoroutine(OpenReceivedPiece(0.75f, index));
                }
            );
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            _collectionElement.SetupPuzzle(_collectionDatas.collectionElementDatas[type]);
            SetupCurrentStateCollection(type);
        }

        public void Exit(){
            base.Hide(() => {
                _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
                _GameEvent.OnGameWin?.Invoke();
                //PopupManager.Instance.ShowFade();
            });
        }
        private IEnumerator OpenReceivedPiece(float time, int index){
            //if (index < 0 || index >= _collectionDatas.collectionElementDatas.Count) throw new System.Exception("Index out of range");
            yield return new WaitForSeconds(time);
            _collectionElement.FadeOpenPuzzlePiece(index);
            yield return new WaitForSeconds(1);
            base.Hide();
            _GameEvent.OnGameWin?.Invoke();
        }

        public void SetupCurrentStateCollection(int type){
            foreach(var puzzlePieceId in _PlayerData.UserData.RuntimeCollectionData[type]){
                _collectionElement.SetupPuzzleState(puzzlePieceId);
            }
        }
    }
}