using Microsoft.Toolkit.Uwp.Connectivity;
using TrueLove.Lib.Models.Code;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommentsPage : Page
    {
        public CommentsPage()
        {
            this.InitializeComponent();
            Window.Current.Activated += OnWindowActivated; // 订阅窗口活动事件
            CompositionTarget.Rendered += CompositionTarget_Rendered; //订阅页面加载后事件
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Window.Current.Activated -= OnWindowActivated;
        private void CompositionTarget_Rendered(object sender, RenderedEventArgs e)
        {
            CommentCollection.LoadMoreItemsManually();
            CompositionTarget.Rendered -= CompositionTarget_Rendered;
        }

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e) => VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);


        /// <summary>
        /// 检查工具栏相关的按钮可用状态。
        /// 下滑隐藏工具栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private async void Scroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            scrlocation = Scroller.VerticalOffset;

            if (ForebodyExtendHeight == 0)
            {
                ForebodyExtendHeight = Forebody.ActualHeight;
            }
            if (ForebodyExtendHeight - scrlocation <= 85)
                Forebody.Height = 85;
            else
            {
                BackSubTitle.Opacity = 0;
                Forebody.Height = ForebodyExtendHeight - scrlocation;
            }
            if (Scroller.VerticalOffset == 0)
            {
                BackSubTitle.Opacity = 1;
            }

            if (Scroller.ScrollableHeight - Scroller.VerticalOffset <= 500)
            {
                if (_isLoading) return;
                _isLoading = true;
                _isLoading = await CommentCollection.LoadMoreItemsManuallyAsync();
            }
        }
        bool _isLoading;

        CommentCollection CommentCollection = new CommentCollection();

        // 滚动条位置变量
        double scrlocation = 0;

        double ForebodyExtendHeight;

        private void Forebody_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) => Scroller.ChangeView(null, 0, null);
    }
}
