using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Windows.System.Launcher;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 链接按钮
        /// 应用于从外部浏览器打开各种网址
        /// </summary>
        private async void Links_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = sender as FrameworkElement;
            switch (button.Tag as string)
            {
                // Avicii 的音乐
                case "spotify": await LaunchUriAsync(new Uri("spotify:artist:1vCWHaC5f2uS3yhpwWbIA6")); break;
                case "youTube": await LaunchUriAsync(new Uri("https://www.youtube.com/user/AviciiOfficialVEVO")); break;
                case "apple": await LaunchUriAsync(new Uri("https://itunes.apple.com/ca/artist/avicii/298496035")); break;
            }
        }
    }
}