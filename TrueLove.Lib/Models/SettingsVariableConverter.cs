using Windows.Storage;

namespace TrueLove.Lib.Models
{
    public class SettingsVariableConverter
    {
        public static bool setLiveTiles
        {
            get => (bool)localSettings.Values["SetLiveTiles"];
            set => localSettings.Values["SetLiveTiles"] = value;
        }

        public static bool setHideBottonBar
        {
            get => (bool)localSettings.Values["SetHideBottonBar"];
            set => localSettings.Values["SetHideBottonBar"] = value;
        }

        public static bool setPageBackgroundColor
        {
            get => (bool)localSettings.Values["SetPageBackgroundColor"];
            set => localSettings.Values["SetPageBackgroundColor"] = value;
        }

        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
    }
}
