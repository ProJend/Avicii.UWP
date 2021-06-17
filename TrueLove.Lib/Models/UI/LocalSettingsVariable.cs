using Windows.Storage;

namespace TrueLove.Lib.Models.UI
{
    public class LocalSettingsVariable
    {
        public static bool setLiveTiles
        {
            get => (bool)localSettings.Values["SetLiveTiles"];
            set => localSettings.Values["SetLiveTiles"] = value;
        }

        public static bool setHideBottomBar
        {
            get => (bool)localSettings.Values["SetHideBottomBar"];
            set => localSettings.Values["SetHideBottomBar"] = value;
        }

        public static bool setPageBackgroundColor
        {
            get => (bool)localSettings.Values["SetPageBackgroundColor"];
            set => localSettings.Values["SetPageBackgroundColor"] = value;
        }

        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
    }
}
