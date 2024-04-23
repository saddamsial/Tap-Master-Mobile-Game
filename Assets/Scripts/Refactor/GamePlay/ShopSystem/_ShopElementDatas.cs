using SKUnityToolkit.SerializableDictionary;
using UnityEngine;

namespace Core.GamePlay.Shop{
    [CreateAssetMenu(fileName = "ShopElementDatas", menuName = "ShopElementDatas", order = 0)]
    public class _ShopElementDatas : ScriptableObject{
        public SerializableDictionary<Sprite, Texture2D> arrowData;
        public SerializableDictionary<Sprite, Texture2D> blockData;
        public SerializableDictionary<Sprite, _ColorPurchased> colorData;
    }

    [System.Serializable]
    public class _ColorPurchased{
        public Color blockColor;
        public Color backgroundColor;

        public _ColorPurchased(Color blockColor, Color backgroundColor){
            this.blockColor = blockColor;
            this.backgroundColor = backgroundColor;
        }    
    }
}   