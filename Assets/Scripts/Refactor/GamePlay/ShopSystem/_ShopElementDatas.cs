using SKUnityToolkit.SerializableDictionary;
using UnityEngine;

namespace Core.GamePlay.Shop{
    [CreateAssetMenu(fileName = "ShopElementDatas", menuName = "ShopElementDatas", order = 0)]
    public class _ShopElementDatas : ScriptableObject{
        public SerializableDictionary<Sprite, Sprite> arrowData;
        public SerializableDictionary<Sprite, Mesh> blockData;
        public SerializableDictionary<Sprite, Color> colorData;
    }
}