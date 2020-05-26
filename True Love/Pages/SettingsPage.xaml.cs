using System;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private LiveTileService liveTileService;
        private string source;
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public SettingsPage()
        {
            liveTileService = new LiveTileService();
            source = "ms-appx:///Assets/Background/BG1.jpg";
            this.InitializeComponent();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                if (toggleSwitch.IsOn == true)
                {
                    liveTileService.AddTile("adad", "dadd", source);//添加新磁贴
                    localSettings.Values["LiveTile"] = true;
                }
                else
                {
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear(); // 清空队列
                    localSettings.Values["LiveTile"] = false;
                }
            }
        }

        #region Links
        /// <summary>
        /// 发送邮件 https://blog.csdn.net/weixin_34128534/article/details/94255782
        /// </summary>
        private async void Mail_Click(object sender, RoutedEventArgs e)
        {
            //收件人            
            EmailRecipient emailRecipient1 = new EmailRecipient("projend@outlook.com");

            //具体的一封email
            EmailMessage emailMessage = new EmailMessage();

            //给邮件添加收件人，可以添加多个
            emailMessage.To.Add(emailRecipient1);

            //通过邮件管理类，生成一个邮件 简单来说  帮你唤起设备里的邮件软
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        private async void Weibo_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://weibo.com/6081786829"));
        }
        private async void Github_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/ProJend/TrueLove-UWP/issues/new"));
        }
        private async void Spotify_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://open.spotify.com/artist/1vCWHaC5f2uS3yhpwWbIA6?si=IYzh3XGLTo2t_CzCMTro0g"));
        }
        private async void YouTube_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/user/AviciiOfficialVEVO"));
        }
        private async void Apple_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://itunes.apple.com/ca/artist/avicii/298496035"));
        }
        private async void Netease_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://music.163.com/#/artist?id=45236"));
        }
        private async void QQ_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://y.qq.com/n/yqq/singer/001jgAtj3LtJnE.html"));
        }
        private async void Kugou_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.kugou.com/singer/86133.html"));
        }
        private async void Instagram_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.instagram.com/avicii/"));
        }
        private async void Facebook_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/avicii.t.berg"));
        }
        private async void Memory_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.avicii.com"));
        }
        private async void Shop_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://shop.avicii.com"));
        }
        private async void Quora_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.quora.com/profile/Tim-Bergling-2/answers"));
        }
        #endregion 
    }
}
