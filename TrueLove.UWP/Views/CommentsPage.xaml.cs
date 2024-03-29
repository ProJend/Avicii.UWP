﻿using Microsoft.Toolkit.Uwp.Connectivity;
using TrueLove.Lib.Models.Code;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            CommentCollection.LoadMoreItemsManuallyAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Window.Current.Activated -= OnWindowActivated;

        /// <summary>
        /// 返回顶部按钮。
        /// </summary>
        private void BackToTop_Click(object sender, RoutedEventArgs e) => Scroller.ChangeView(null, 0, null);

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            isInternetAvailable = !NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            CommentCollection.Clear();
            Scroller.ChangeView(null, 0, null);
            CommentCollection.LoadMoreItemsManuallyAsync(true);
        }

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e) => VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);

        /// <summary>
        /// 检查工具栏相关的按钮可用状态。
        /// 下滑隐藏工具栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private void Scroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            // 滚动条当前位置小于存储的变量值时代表往上滑，即不可以下滑隐藏
            // 增加额外距离以防误触
            if (Scroller.VerticalOffset > scrlocation + 1 && canMinimize)
            {
                AfterbodySlideDownStoryboard.Begin();
                canMinimize = false;
            }
            else if (Scroller.VerticalOffset < scrlocation - 18 && !canMinimize)
            {
                AfterbodySlideUpStoryboard.Begin();
                canMinimize = true;
            }
            else if (Scroller.VerticalOffset == 0 && !canMinimize)
            {
                AfterbodySlideUpStoryboard.Begin();
                canMinimize = true;
            }

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

            if ((Scroller.ScrollableHeight - scrlocation) is >= 750 and <= 760)
            {
                CommentCollection.LoadMoreItemsManuallyAsync();
            }
        }

        CommentCollection CommentCollection = new CommentCollection();

        // 滚动条位置变量
        double scrlocation = 0;
        // 导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool canMinimize = true;

        double ForebodyExtendHeight;

        bool isInternetAvailable = !NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
    }
}
