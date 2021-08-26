using Windows.Storage;

namespace TrueLove.Lib.Models.UI
{
    public class LocalSettings
    {
        public static bool isLiveTiles
        {
            get => (bool)localSettings.Values["isLiveTiles"];
            set => localSettings.Values["isLiveTiles"] = value;
        }

        public static bool isBottomBarHidden
        {
            get => (bool)localSettings.Values["isBottomBarHidden"];
            set => localSettings.Values["isBottomBarHidden"] = value;
        }
        public static bool isMidnightTheme
        {
            get => (bool)localSettings.Values["isMidnightTheme"];
            set => localSettings.Values["isMidnightTheme"] = value;
        }

        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
    }
}
