using System.Collections.Generic;
using UnityEngine;
namespace Core.GamePlay.Collection{
    [CreateAssetMenu(fileName = "CollectionElementDatas", menuName = "CollectionElementDatas")]
    public class _CollectionElementDatas : ScriptableObject{
        public List<_CollectionElementData> collectionElementDatas;
    }
}