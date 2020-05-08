using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private LiveTileService liveTileService;
        private string source;

        public SettingsPage()
        {
            liveTileService = new LiveTileService();
            source = "ms-appx:///Assets/Background/BG1.jpg";
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

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

        private async void weibo_Click(object sender, RoutedEventArgs e)

        {
            var a = await Launcher.LaunchUriAsync(new Uri("https://weibo.com/6081786829"));

        }

        private void WPHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WPPage));
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            bool OnOrOff;
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    liveTileService.AddTile("adad","dadd",source);//添加新磁贴
                    OnOrOff = true;
                }
                else
                {
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear(); // 清空队列
                    OnOrOff = false;
                }
            }
        }
    }
}
