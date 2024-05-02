using System.Collections.Generic;
using Core.GamePlay.LevelSystem;
using Core.GamePlay.Shop;
using Core.SystemGame;
using UnityEngine;

namespace Core.Data{
    public class _UserData{
        
        public int CurrentLevel;
        public int Coin;
        public int CurrentCollectCoin;
        public bool IsTurnOnSound;
        public bool IsTurnOnMusic;
        public bool IsTurnOnVibration;
        public KeyValuePair<int , int> CurrentCollectionPuzzlePiece;
        public Dictionary<int, List<int>> RuntimeCollectionData;
        public Dictionary<_ShopPage, List<int>> RuntimePurchasedShopData;
        public Dictionary<_ShopPage, int> RuntimeSelectedShopData;
        public Dictionary<_LevelType, int> HighestLevelInMode;

        public void InitUserData(){
            CurrentLevel = 0;
            Coin = 0;
            CurrentCollectCoin = 0;
            HighestLevelInMode = new Dictionary<_LevelType, int>(){
                {_LevelType.Easy, 1},
                {_LevelType.Medium, _ConstantGameplayConfig.LEVEL_EASY+1},
                {_LevelType.Master, _ConstantGameplayConfig.LEVEL_EASY + _ConstantGameplayConfig.LEVEL_MEDIUM + 1}
            };
            IsTurnOnSound = true;
            IsTurnOnMusic = true;
            IsTurnOnVibration = true;
#region Collection and Shop Data
            RuntimeCollectionData = new Dictionary<int, List<int>>{
                {0, new List<int>()}
            };
            RuntimePurchasedShopData = new Dictionary<_ShopPage, List<int>>();
            RuntimeSelectedShopData = new Dictionary<_ShopPage, int>();
            UpdatePurchasedData(_ShopPage.Arrow, 0);
            UpdatePurchasedData(_ShopPage.Block, 0);
            UpdatePurchasedData(_ShopPage.Color, 0);
            UpdatePurchasedData(_ShopPage.Color, 1);
            UpdatePurchasedData(_ShopPage.Color, 2);
            UpdatePurchasedData(_ShopPage.Color, 3);
            UpdateSelectedData(_ShopPage.Arrow, 0);
            UpdateSelectedData(_ShopPage.Block, 0);
            UpdateSelectedData(_ShopPage.Color, 0);
#endregion
        }

        public void UpdateWinGameUserDataValue(){
            CurrentLevel++;
            if(CurrentLevel < _ConstantGameplayConfig.LEVEL_EASY)
            {
                HighestLevelInMode[_LevelType.Easy] = Mathf.Max(CurrentLevel + 1, HighestLevelInMode[_LevelType.Easy]);
            }
            else if(CurrentLevel < _ConstantGameplayConfig.LEVEL_EASY + _ConstantGameplayConfig.LEVEL_MEDIUM)
            {
                HighestLevelInMode[_LevelType.Medium] = Mathf.Max(CurrentLevel + 1,HighestLevelInMode[_LevelType.Medium]);
            }
            else
            {
                HighestLevelInMode[_LevelType.Master] = Mathf.Max(CurrentLevel + 1,HighestLevelInMode[_LevelType.Master]);
            }
            Coin += CurrentCollectCoin;
            if(CurrentCollectionPuzzlePiece.Key != - 1)
                UpdateCollectionData(CurrentCollectionPuzzlePiece.Key, CurrentCollectionPuzzlePiece.Value);
        }

        public void UpdateCollectionData(int colelctionId, int puzzlePieceId){
            if(RuntimeCollectionData.ContainsKey(colelctionId)){
                if(!RuntimeCollectionData[colelctionId].Contains(puzzlePieceId)){
                    RuntimeCollectionData[colelctionId].Add(puzzlePieceId);
                }
            }
            else{
                RuntimeCollectionData.Add(colelctionId, new System.Collections.Generic.List<int>(){puzzlePieceId});
            }
        }

        public void UpdatePurchasedData(_ShopPage type, int elementId){
            if(RuntimePurchasedShopData.ContainsKey(type)){
                if(!RuntimePurchasedShopData[type].Contains(elementId)){
                    RuntimePurchasedShopData[type].Add(elementId);
                }
            }
            else{
                RuntimePurchasedShopData.Add(type, new System.Collections.Generic.List<int>(){elementId});
            }
        }

        public void UpdateSelectedData(_ShopPage type, int elementId){
            if(RuntimeSelectedShopData.ContainsKey(type)){
                RuntimeSelectedShopData[type] = elementId;
            }
            else{
                RuntimeSelectedShopData.Add(type, elementId);
            }
        }

        public int GetCurrentTimePurchaseItem(_ShopPage type){
            return type switch
            {
                _ShopPage.Arrow => RuntimePurchasedShopData[_ShopPage.Arrow].Count - 1,
                _ShopPage.Block => RuntimePurchasedShopData[_ShopPage.Block].Count - 1,
                _ShopPage.Color => RuntimePurchasedShopData[_ShopPage.Color].Count - 4,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
    }
}