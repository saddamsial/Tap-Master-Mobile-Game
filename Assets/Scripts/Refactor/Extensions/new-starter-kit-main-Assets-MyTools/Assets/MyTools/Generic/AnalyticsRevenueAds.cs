using com.adjust.sdk;

public class AnalyticsRevenueAds{
       public static void SendRevAdmobToAdjust(double rev)
    {
        AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
        adjustAdRevenue.setRevenue(rev, "USD");
        // optional fields
        adjustAdRevenue.setAdRevenueNetwork("appopenads");
        //adjustAdRevenue.setAdRevenueUnit(data.adUnit);
        //adjustAdRevenue.setAdRevenuePlacement(data.placement);
        // track Adjust ad revenue
        Adjust.trackAdRevenue(adjustAdRevenue);
    }

}