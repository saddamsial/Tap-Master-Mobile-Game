using System.Collections.Generic;
using Core.GamePlay.Shop;

namespace Core.Data{
    public class _UserData{
        
        public int HighestLevel;
        public Dictionary<int, List<int>> RuntimeCollectionData;
        public Dictionary<_ShopPage, List<int>> RuntimePurchasedShopData;
        public Dictionary<_ShopPage, int> RuntimeSelectedShopData;


        public void InitUserData(){
            HighestLevel = 0;
#region Collection and Shop Data
            RuntimeCollectionData = new Dictionary<int, List<int>>();
            RuntimePurchasedShopData = new Dictionary<_ShopPage, List<int>>();
            RuntimeSelectedShopData = new Dictionary<_ShopPage, int>();
            UpdateCollectionData(0, 3);
            UpdatePurchasedData(_ShopPage.Arrow, 0);
            UpdatePurchasedData(_ShopPage.Arrow, 1);
            UpdatePurchasedData(_ShopPage.Block, 0);
            UpdatePurchasedData(_ShopPage.Color, 0);
            UpdateSelectedData(_ShopPage.Arrow, 0);
            UpdateSelectedData(_ShopPage.Block, 0);
            UpdateSelectedData(_ShopPage.Color, 0);
#endregion
        }

        public void UpdateWinGameUserDataValue(){
            HighestLevel++;
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
    }
}