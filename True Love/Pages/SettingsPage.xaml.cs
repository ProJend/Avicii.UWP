using System;
using True_Love.Helpers;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using static Windows.System.Launcher;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel;
using True_Love.Pages.XAML_ContentDialog;
using static True_Love.Helpers.Generic;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private LiveTileService liveTileService;
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private string closeText, titleText; // 声明更新记录字符串

        public SettingsPage()
        {
            this.InitializeComponent();           
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            // 判定状态
            if (Language != "zh-Hans-CN") FAQ_CN.Visibility = Visibility.Collapsed;
            if (!ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5)) HotKeys.Visibility = Visibility.Collapsed;
            LiveTiles.IsOn = (bool)localSettings.Values["SetLiveTiles"];
            HideCommandbar.IsOn = (bool)localSettings.Values["SetHideCommandBar"];
            BackgroundColor.IsOn = (bool)localSettings.Values["SetBackgroundColor"];
            var version = Package.Current.Id.Version;
            VersionInfo.Text = $"Version : {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            releasedDate.Text = $"Installation Date : {Package.Current.InstalledDate.ToLocalTime().DateTime}";
            Settings.Loaded -= Settings_Loaded;
        }

        /// <summary>
        /// 切换开关。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            FrameworkElement toggleSwitch = sender as FrameworkElement;
            switch (toggleSwitch.Tag as string)
            {
                case "liveTiles":
                    liveTileService = new LiveTileService();
                    if (LiveTiles.IsOn == true)
                    {
                        liveTileService.AddTile(); // 添加新磁贴
                        localSettings.Values["SetLiveTiles"] = true;
                    }
                    else
                    {
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear(); // 清空队列
                        localSettings.Values["SetLiveTiles"] = false;
                        localSettings.Values["OnlyLiveTiles"] = true;
                    }
                    break;

                case "hideCommandbar":
                    localSettings.Values["SetHideCommandBar"] = HideCommandbar.IsOn == true ? true : false;
                    break;

                case "backgroundColor":
                    if (BackgroundColor.IsOn == true)
                    {
                        localSettings.Values["SetBackgroundColor"] = true;
                        Main.Background = new SolidColorBrush(Colors.Black);
                        MainPage.Current.PageBackgroundChange();
                    }
                    else
                    {
                        localSettings.Values["SetBackgroundColor"] = false;
                        Main.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
                        MainPage.Current.PageBackgroundChange();
                    }
                    break;
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        private async void Release_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Language)) // 判断对传的值进行是否为空值
            {
                switch (Language) // 匹对语种
                {
                    case "en-US":
                        closeText = "Get it!";
                        titleText = "Release Notes";
                        //Application.Current.Resources["Key"];
                        break;
                    case "zh-Hans-CN":
                        closeText = "好哒！";
                        titleText = "更新记录";
                        break;
                    case "zh-Hant-HK":
                        closeText = "好嘅！";
                        titleText = "更新日志";
                        break;
                    default:
                        closeText = "OK";
                        titleText = "Release Notes";
                        break;
                }
            }

            var release = new ContentDialog()
            {
                Title = titleText,
                CloseButtonText = closeText,
                Content = new ReleaseNotes(),
                Background = new SolidColorBrush(Colors.Black),
            };
            if (!IdentifyDeviceFamily("mobile"))
            {
                release.CloseButtonStyle = (Style)Resources["ButtonRevealStyle"];
                release.BorderBrush = (Brush)Resources["SystemControlBackgroundListMediumRevealBorderBrush"];
            }
            try
            {
                await release.ShowAsync();
            }
            catch (System.Runtime.InteropServices.COMException) { } //Nothing todo.
        }

        #region Links
        /// <summary>
        /// 发送邮件 https://blog.csdn.net/weixin_34128534/article/details/94255782
        /// </summary>
        private async void Mail_Click(object sender, RoutedEventArgs e)
        {
            // 收件人 
            EmailRecipient emailRecipient1 = new EmailRecipient("projend@outlook.com");

            // 具体的一封 Email
            EmailMessage emailMessage = new EmailMessage();

            // 给邮件添加收件人，可以添加多个
            emailMessage.To.Add(emailRecipient1);

            // 通过邮件管理类，生成一个邮件。简单来说，帮你唤起设备里的邮件 UWP
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
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
                // 汇报 Bugs
                case "Weibo": await LaunchUriAsync(new Uri("https://weibo.com/6081786829")); break;
                case "GitHub": await LaunchUriAsync(new Uri("https://github.com/ProJend/TrueLove-UWP/issues/new")); break;

                // 社区群
                case "Telegram": await LaunchUriAsync(new Uri("https://t.me/TrueAvicii")); break;
                //case "Skype": await LaunchUriAsync(new Uri("")); break;

                // Avicii 的音乐
                case "Spotify": await LaunchUriAsync(new Uri("spotify:artist:1vCWHaC5f2uS3yhpwWbIA6")); break;
                case "YouTube": await LaunchUriAsync(new Uri("https://www.youtube.com/user/AviciiOfficialVEVO")); break;
                case "Apple": await LaunchUriAsync(new Uri("https://itunes.apple.com/ca/artist/avicii/298496035")); break;
                case "Netease": await LaunchUriAsync(new Uri("https://music.163.com/#/artist?id=45236")); break;
                case "QQ": await LaunchUriAsync(new Uri("https://y.qq.com/n/yqq/singer/001jgAtj3LtJnE.html")); break;
                case "Kugou": await LaunchUriAsync(new Uri("https://www.kugou.com/singer/86133.html")); break;

                // Avicii 的个人故事
                case "Instagram": await LaunchUriAsync(new Uri("https://www.instagram.com/avicii/")); break;
                case "Facebook": await LaunchUriAsync(new Uri("https://www.facebook.com/avicii.t.berg")); break;
                case "Twitter": await LaunchUriAsync(new Uri("https://www.twitter.com/Avicii")); break;

                // Avicii 的个人网站
                case "Shop": await LaunchUriAsync(new Uri("https://shop.avicii.com")); break;
                case "Memory": await LaunchUriAsync(new Uri("http://www.avicii.com")); break;
                case "Foundation": await LaunchUriAsync(new Uri("https://www.timberglingfoundation.org")); break;
                case "Quora": await LaunchUriAsync(new Uri("https://www.quora.com/profile/Tim-Bergling-2/answers")); break;

                // 粉丝们的个人小站
                case "One": await LaunchUriAsync(new Uri("https://avicii.one")); break;
            }
        }
        #endregion 
    }
}
