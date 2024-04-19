using System.Collections.Generic;
using UnityEngine;

namespace Core.GamePlay.Shop{
    [CreateAssetMenu(fileName = "ItemPriceDatas", menuName = "Shop/ItemPriceDatas")]
    public class _ItemPriceDatas : ScriptableObject{
        public List<int> ArrowPrices;
        public List<int> BlockPrices;
        public List<int> ColorPrices;

        public int GetPrice(_ShopPage shopPage, int index){
            switch (shopPage){
                case _ShopPage.Arrow:
                    return ArrowPrices[index > ArrowPrices.Count - 1 ? ArrowPrices.Count - 1 : index];
                case _ShopPage.Block:
                    return BlockPrices[index > BlockPrices.Count - 1 ? BlockPrices.Count - 1 : index];
                case _ShopPage.Color:
                    return ColorPrices[index > ColorPrices.Count - 1 ? ColorPrices.Count - 1 : index];
                default:
                    return 0;
            }
        }
    }
}