using System.Collections.Generic;
using TrueLove.Lib.Models.Code;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImagesPage : Page
    {
        public ImagesPage()
        {
            this.InitializeComponent();
            Window.Current.Activated += OnWindowActivated; // 订阅窗口活动事件
        }

        /// <summary>
        /// 返回顶部按钮。
        /// </summary>
        private void BackToTop_Click(object sender, RoutedEventArgs e) => Show_ScrollViewer.ChangeView(null, 0, null);

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e) => VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);
    }
}
