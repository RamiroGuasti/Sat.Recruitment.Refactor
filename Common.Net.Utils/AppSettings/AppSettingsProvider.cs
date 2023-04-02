using System.Configuration;

namespace Common.Net.Utils.AppSettings
{
    public static class AppSettingsProvider
    {
        #region Fixed AppSettings Fijas

        public static int MaxSupportRequestTimeDurationInMinutes => 30;

        #endregion


        #region Config AppSettings

        #region Users

        public static decimal MinMoneyToGif => 100m; // decimal.Parse(GetAppSettingData("MinMoneyToGif"));

        public static decimal MinMoneyToGifNormalUser => 10m; // decimal.Parse(GetAppSettingData("MinMoneyToGifNormalUser"));

        public static decimal GiftPercentToNormalUserMax  => 0.8m; // decimal.Parse(GetAppSettingData("GiftPercentToNormalUserMax"));

        public static decimal GiftPercentToNormalUserMin  => 0.12m; // decimal.Parse(GetAppSettingData("GiftPercentToNormalUserMin"));

        public static decimal GiftPercentToSuperUser      => 0.20m; // decimal.Parse(GetAppSettingData("GiftPercentToSuperUser"));

        public static decimal GiftPercentToPremiumUser    => 2m; // decimal.Parse(GetAppSettingData("GiftPercentToPremiumUser"));

        #endregion

        private static string GetAppSettingData(string appSettingName) => ConfigurationManager.AppSettings[appSettingName];

        #endregion

        #region DB AppSettings 

        #endregion
    }
}