using System;
using TrueLove.Lib.Models.Enum;
using TrueLove.Lib.Models.UI;
using TrueLove.Lib.Notification.ContentDialog;
using TrueLove.Lib.Notification.LiveTile;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Email;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static Windows.System.Launcher;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();           
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            // 判定状态
            if (Language != "zh-Hans-CN") FAQ_CN.Visibility = Visibility.Collapsed;
            preventLoad = false;
            LiveTiles.IsOn = LocalSettingsVariable.setLiveTiles;
            HideCommandbar.IsOn = LocalSettingsVariable.setHideBottonBar;
            BackgroundColor.IsOn = LocalSettingsVariable.setPageBackgroundColor;
            preventLoad = true;
            var version = Package.Current.Id.Version;
            VersionInfo.Text = $"Version : {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            releasedDate.Text = $"Installed Date : {Package.Current.InstalledDate.ToLocalTime().DateTime}";
            Settings.Loaded -= Settings_Loaded;
        }

        /// <summary>
        /// 切换开关。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (preventLoad)
            {
                FrameworkElement toggleSwitch = sender as FrameworkElement;
                switch (toggleSwitch.Tag as string)
                {
                    case "liveTiles":
                        if (LiveTiles.IsOn)
                        {
                            TileSetup.SetupTile(); // 添加新磁贴

                            //await Register.RegisterBackgroundTask("BackgroundTask.BackgroundTask", "LiveTile", new TimeTrigger(30, false), null);
                        }

                        else
                        {
                            TileUpdateManager.CreateTileUpdaterForApplication().Clear(); // 清空队列
                        }
                        LocalSettingsVariable.setLiveTiles = LiveTiles.IsOn == true ? true : false;
                        break;

                    case "hideCommandbar":
                        LocalSettingsVariable.setHideBottonBar = HideCommandbar.IsOn == true ? true : false;
                        break;

                    case "backgroundColor":
                        if (BackgroundColor.IsOn)
                        {
                            LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                            MainPage.Current.PageBackgroundChange();
                        }
                        else
                        {
                            LayoutRoot.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
                            MainPage.Current.PageBackgroundChange();
                        }
                        LocalSettingsVariable.setPageBackgroundColor = BackgroundColor.IsOn == true ? true : false;
                        break;
                }
            }          
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        private void Release_Click(object sender, RoutedEventArgs e)
        {
            DialogSetup.SetupDialog(GetDialogInfo.ReleaseNotes);
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

        /// <summary>
        /// 留给 App 第一次运行加载设置
        /// </summary>
        public void AppFirstRun()
        {
            LocalSettingsVariable.setLiveTiles = true;
            LocalSettingsVariable.setHideBottonBar = false;
            LocalSettingsVariable.setPageBackgroundColor = true;

            preventLoad = true;
            LiveTiles.IsOn = LocalSettingsVariable.setLiveTiles;
            HideCommandbar.IsOn = LocalSettingsVariable.setHideBottonBar;
            BackgroundColor.IsOn = LocalSettingsVariable.setPageBackgroundColor;         
        }

        public bool preventLoad;
    }
}
