﻿using System;
using System.Collections.Generic;
using TrueLove.Lib.Models.Code;
using TrueLove.UWP.Spider;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommentsPage : Page
    {
        private List<CommentItem> Comments;
        public CommentsPage()
        {
            this.InitializeComponent();
            Window.Current.Activated += OnWindowActivated; // 订阅窗口活动事件

            var readHTML = new ReviewHTML(Package.Current.InstalledPath + "/Spider/CommentCodeSample.txt");
            var refineData = new RefineData();
            Comments = refineData.RefineComment(readHTML.StrHTML);
        }
        /// <summary>
        /// 返回顶部按钮。
        /// </summary>
        private void BackToTop_Click(object sender, RoutedEventArgs e) => hapticScrollViewer.ChangeView(null, 0, null);

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e) => VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);

        /// <summary>
        /// 检查工具栏相关的按钮可用状态。
        /// 下滑隐藏工具栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private void hapticScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            // 滚动条当前位置小于存储的变量值时代表往上滑，即不可以下滑隐藏
            // 增加额外距离以防误触
            if (hapticScrollViewer.VerticalOffset > scrlocation + 1 && canMinimize)
            {
                toolBarSlideDownStoryboard.Begin();
                canMinimize = false;
            }
            else if (hapticScrollViewer.VerticalOffset < scrlocation - 18 && !canMinimize)
            {
                toolBarSlideUpStoryboard.Begin();
                canMinimize = true;
            }
            else if (hapticScrollViewer.VerticalOffset == 0 && !canMinimize)
            {
                toolBarSlideUpStoryboard.Begin();
                canMinimize = true;
            }

            scrlocation = hapticScrollViewer.VerticalOffset;

            if (backgroundImageFixedHeight == 0)
                backgroundImageFixedHeight = backgroundSubTitle.ActualHeight;
            try
            {
                backgroundSubTitle.Height = backgroundImageFixedHeight - scrlocation;
            }
            catch (ArgumentException)
            {
                // 快速滑动引起的参数修正
                backgroundSubTitle.Height = 0;
            }
        }

        // 滚动条位置变量
        double scrlocation = 0;
        // 导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool canMinimize = true;

        double backgroundImageFixedHeight;
    }
}
