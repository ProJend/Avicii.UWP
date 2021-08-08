using System;
using TrueLove.Lib.Helpers;
using TrueLove.Lib.Models.Enum;
using TrueLove.Lib.Models.UI;
using TrueLove.Lib.Notification;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Windows.System.Launcher;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage() => this.InitializeComponent();

        /// <summary>
        /// 切换开关。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            FrameworkElement toggleSwitch = sender as FrameworkElement;
            switch (toggleSwitch.Tag as string)
            {
                case "enableLiveTileSwitch":
                    if (EnableLiveTileSwitch.IsOn)
                        await Register.RegisterBackgroundTask("BackgroundTask.BackgroundTask", "LiveTile", new TimeTrigger(30, false), null);
                    else
                    {
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear(); // 清空队列

                        foreach (var task in BackgroundTaskRegistration.AllTasks)
                        {
                            if (task.Value.Name == "LiveTile")
                                task.Value.Unregister(true);
                        }
                    }
                    LocalSettings.isLiveTiles = EnableLiveTileSwitch.IsOn;
                    break;

                case "hideToolBarSwitch":
                    LocalSettings.isBottomBarHidden = HideToolBarSwitch.IsOn;
                    break;
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        private void Release_Click(object sender, RoutedEventArgs e) => Assembly.Dialog(DialogType.ReleaseNotes);

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
                case "gitHub": await LaunchUriAsync(new Uri("https://github.com/ProJend/TrueLove-UWP/issues/new")); break;
                case "mail": await LaunchUriAsync(new Uri(@"mailto:projend@outlook.com")); break;
                case "weibo": await LaunchUriAsync(new Uri("https://weibo.com/6081786829")); break;


                // 社群
                case "telegram": await LaunchUriAsync(new Uri("https://t.me/TrueAvicii")); break;

                // Avicii 的音乐
                case "spotify": await LaunchUriAsync(new Uri("spotify:artist:1vCWHaC5f2uS3yhpwWbIA6")); break;
                case "youTube": await LaunchUriAsync(new Uri("https://www.youtube.com/user/AviciiOfficialVEVO")); break;
                case "apple": await LaunchUriAsync(new Uri("https://itunes.apple.com/ca/artist/avicii/298496035")); break;
                case "netease": await LaunchUriAsync(new Uri("https://music.163.com/#/artist?id=45236")); break;
                case "QQ": await LaunchUriAsync(new Uri("https://y.qq.com/n/yqq/singer/001jgAtj3LtJnE.html")); break;
                case "kugou": await LaunchUriAsync(new Uri("https://www.kugou.com/singer/86133.html")); break;

                // Avicii 的个人故事
                case "instagram": await LaunchUriAsync(new Uri("https://www.instagram.com/avicii/")); break;
                case "facebook": await LaunchUriAsync(new Uri("https://www.facebook.com/avicii.t.berg")); break;
                case "twitter": await LaunchUriAsync(new Uri("https://www.twitter.com/Avicii")); break;

                // Avicii 的个人网站
                case "shop": await LaunchUriAsync(new Uri("https://shop.avicii.com")); break;
                case "memory": await LaunchUriAsync(new Uri("http://www.avicii.com")); break;
                case "foundation": await LaunchUriAsync(new Uri("https://www.timberglingfoundation.org")); break;
                case "quora": await LaunchUriAsync(new Uri("https://www.quora.com/profile/Tim-Bergling-2/answers")); break;

                // 粉丝们的个人小站
                case "projctOne": await LaunchUriAsync(new Uri("https://avicii.one")); break;
            }
        }

        /// <summary>
        /// 留给 App 第一次运行加载设置
        /// </summary>
        public void AppFirstRun()
        {
            EnableLiveTileSwitch.IsOn = LocalSettings.isLiveTiles = true;
            HideToolBarSwitch.IsOn = LocalSettings.isBottomBarHidden = false;
        }

        string versionInfo
        {
            get
            {
                var version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }
        string installedDate => $"{Package.Current.InstalledDate.ToLocalTime().DateTime}";
    }
}
