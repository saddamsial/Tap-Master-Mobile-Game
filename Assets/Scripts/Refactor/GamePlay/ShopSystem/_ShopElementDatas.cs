using SKUnityToolkit.SerializableDictionary;
using UnityEngine;

namespace Core.GamePlay.Shop{
    [CreateAssetMenu(fileName = "ShopElementDatas", menuName = "ShopElementDatas", order = 0)]
    public class _ShopElementDatas : ScriptableObject{
        public SerializableDictionary<Sprite, Texture2D> arrowData;
        public SerializableDictionary<Sprite, Texture2D> blockData;
        public SerializableDictionary<Sprite, Color> colorData;
    }
}