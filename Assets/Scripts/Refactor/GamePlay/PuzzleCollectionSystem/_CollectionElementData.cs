using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GamePlay.Collection{
    [Serializable]
    public class _CollectionElementData{
        public string name;
        public Sprite rewardImage;
        public Sprite[] mainImages = new Sprite[6];
    }

    public class _RuntimeCollectionData{
        public Dictionary<int, List<int>> collectionPuzzlesData;

        public _RuntimeCollectionData(){
            collectionPuzzlesData = new Dictionary<int, List<int>>();
        }
    }
}