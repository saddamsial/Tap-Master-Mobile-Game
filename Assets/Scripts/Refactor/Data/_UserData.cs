using System.Collections.Generic;
using Core.GamePlay.Collection;
using Unity.VisualScripting;

namespace Core.Data{
    public class _UserData{
        
        public int HighestLevel;
        public Dictionary<int, List<int>> RuntimeCollectionData;


        public void InitUserData(){
            HighestLevel = 0;
            RuntimeCollectionData = new Dictionary<int, List<int>>();
            UpdateCollectionData(0, 3);
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
    }
}