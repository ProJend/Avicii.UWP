using Microsoft.Toolkit.Uwp.Connectivity;
using TrueLove.Lib.Models.Code;
using TrueLove.Lib.Notification;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            CompositionTarget.Rendered += CompositionTarget_Rendered; //订阅页面加载后事件
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Window.Current.Activated -= OnWindowActivated;
        private void CompositionTarget_Rendered(object sender, RenderedEventArgs e)
        {
            ImageCollection.LoadMoreItemsManually();
            CompositionTarget.Rendered -= CompositionTarget_Rendered;
        }

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e) => VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);

        //private async void MenuFlyout_Click(object sender, RoutedEventArgs e)
        //{
        //    var menuFlyoutItem = sender as FrameworkElement;
        //    switch (menuFlyoutItem.Tag as string)
        //    {
        //        case "show":
        //            var options = new FolderLauncherOptions();
        //            var folder = ApplicationData.Current.LocalFolder;
        //            await Launcher.LaunchFolderAsync(folder, options);
        //            break;

        //        case "save":
        //            break;
        //    }
        //}

        private async void Scroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (Scroller.ScrollableHeight - Scroller.VerticalOffset <= 200)
            {
                var isInternetAvailable = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
                if (isInternetAvailable)
                {
                    if (_isLoading) return;
                    _isLoading = true;
                    _isLoading = await ImageCollection.LoadMoreItemsManuallyAsync();
                }
                else
                {
                    Assembly.Toast();
                }
            }
        }
        bool _isLoading;
        private ImageCollection ImageCollection = [];
    }
}