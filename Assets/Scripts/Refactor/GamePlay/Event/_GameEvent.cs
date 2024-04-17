using System;
using Core.GamePlay.Shop;

namespace Core.GamePlay{
    public class _GameEvent{
        public static Action OnGamePlayReset;
        public static Action OnGameWin;
        public static Action OnGameLose;
        //public static Action OnGamePlayWin;
    
        public static Action OnUseBoosterOpenFace;
        public static Action OnUseBoosterHint;

        public static Action<int, _ShopPage> OnSelectShopElement;

    }
}