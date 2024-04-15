using System;
using Core.GamePlay.Shop;

namespace Core.GamePlay{
    public class _GameEvent{
        public static Action OnGamePlayReset;
        //public static Action OnGamePlayWin;
    
        public static Action OnUseBoosterOpenFace;
        public static Action OnUseBoosterHint;

        // public static Action<int> OnSelectArrow;
        // public static Action<int> OnSelectBlock;
        // public static Action<int> OnSelectColor;

        public static Action<int, _ShopPage> OnSelectShopElement;

    }
}