using System.Xml.Serialization;
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

        public static bool isPageBackgroundColorSwitched
        {
            get => (bool)localSettings.Values["isPageBackgroundColorSwitched"];
            set => localSettings.Values["isPageBackgroundColorSwitched"] = value;
        }

        [XmlIgnore]
        public static string strPageBackgroundColor
        {
            get => (string)localSettings.Values["strPageBackgroundColor"];
            set => localSettings.Values["strPageBackgroundColor"] = value;
        }

        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
    }
}
