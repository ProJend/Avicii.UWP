using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImagesPage : Page
    {
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public ImagesPage()
        {
            this.InitializeComponent();
            if ((bool)localSettings.Values["SetBackgroundColor"]) Main.Background =new SolidColorBrush(Colors.Black);
            else Main.Background = new SolidColorBrush(Color.FromArgb(0xFF, 38, 38, 38));
        }
    }
}
