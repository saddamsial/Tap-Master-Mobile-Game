using System;
using UnityEngine;

namespace Core.GamePlay.Collection{
    [Serializable]
    public class _CollectionElementData{
        public string name;
        public Sprite rewardImage;
        public Sprite[] mainImages = new Sprite[6];
    }
}