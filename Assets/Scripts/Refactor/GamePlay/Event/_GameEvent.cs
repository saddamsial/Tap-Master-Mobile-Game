using System;
using Core.GamePlay.Block;
using Core.GamePlay.Shop;

namespace Core.GamePlay{
    public class _GameEvent{
        public static Action OnGameEnd;
        public static Action OnGamePlayReset;
        public static Action OnGameWin;
        public static Action OnGameLose;
        public static Action OnGamePlayContinue;

/// <summary>
///    isBlockCanMove: true if block can move, false if block can't move
/// </summary>
        public static Action OnSelectIdleBlock;
        public static Action<_BlockTypeEnum, int> OnSelectRewardBlock;
        public static Action<_BlockTypeEnum, int> OnSelectRewardBlockToWin; 
        //public static Action OnGamePlayWin;
    
        public static Action OnUseBoosterOpenFace;
        public static Action OnUseBoosterHint;

        public static Action<int, _ShopPage> OnSelectShopElement;
        public static Action<_BlockTypeEnum, int> OnReceivedRewardByAds;

    }
}