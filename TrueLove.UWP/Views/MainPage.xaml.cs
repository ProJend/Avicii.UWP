﻿using System;
using System.Collections.Generic;
using System.Linq;
using TrueLove.Lib.Helpers;
using TrueLove.Lib.Models.Enum;
using TrueLove.Lib.Models.UI;
using TrueLove.Lib.Notification;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            ManipulationCompleted += The_ManipulationCompleted; // 订阅手势滑动结束后的事件
            SystemNavigationManager.GetForCurrentView().BackRequested += System_BackRequested; // 订阅系统返回事件
            Window.Current.CoreWindow.PointerPressed += Mouse_BackRequested; // 订阅鼠标返回事件
            Window.Current.Activated += OnWindowActivated; // 订阅窗口活动事件
            Window.Current.SetTitleBar(AppTitleBar); // 设置新的标题栏

            if (Generic.DeviceFamilyMatch(DeviceFamilyType.Mobile)) // = WP
            {
                if (LocalSettings.isPageBackgroundColorSwitched) TopBar.Background = new SolidColorBrush(Colors.Black);
                else TopBar.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
                ToolBar.Background = new SolidColorBrush { Color = Colors.Black, Opacity = 0.7 };

                // 调整界面，默认抬高底部工具栏以免重叠
                var applicationView = ApplicationView.GetForCurrentView();
                applicationView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
                VisualStateManager.GoToState(this, WPNavBarVisible.Name, true);
            }
            else if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5))// > OS15063
            {
                // Add keyboard accelerators for backwards navigation.
                var goBack = new KeyboardAccelerator { Key = VirtualKey.Escape };
                goBack.Invoked += Keyboard_BackRequested;
                KeyboardAccelerators.Add(goBack);
            }
        }

        #region NavigationView
        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {   // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += ContentFrame_Navigated;

            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];

            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            NavView_Navigate("home", new EntranceNavigationTransitionInfo());
        }

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("home", typeof(HomePage)),
            ("comment", typeof(CommentsPage)),
            ("image", typeof(ImagesPage)),
        };

        /// <summary>
        /// 应用内返回页面
        /// </summary>
        private void NavView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args) => TryGoBack();

        /// <summary>
        /// 键盘外返回页面
        /// </summary>
        private void Keyboard_BackRequested(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) => TryGoBack();

        /// <summary>
        /// 系统外返回页面
        /// </summary>
        private void System_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!ContentFrame.CanGoBack) return;
            ContentFrame.GoBack();
            e.Handled = true;
        }

        /// <summary>
        /// 鼠标外返回页面
        /// Invoked on every mouse click, touch screen tap, or equivalent interaction.
        /// Used to detect browser-style next and previous mouse button clicks
        /// to navigate between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void Mouse_BackRequested(CoreWindow sender, PointerEventArgs e)
        {
            var properties = e.CurrentPoint.Properties;
            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed) return;

            // If back or forward are pressed (but not both) navigate appropriately
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                e.Handled = true;
                if (backPressed) TryGoBack();
                //if (forwardPressed) this.TryGoForward();
            }
        }

        /// <summary>
        /// 尝试执行返回页面操作
        /// </summary>
        private bool TryGoBack()
        {
            if (!ContentFrame.CanGoBack) return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
                return false;
            ContentFrame.GoBack();
            return true;
        }

        private void NavView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            var navItemTag = args.InvokedItemContainer.Tag.ToString();
            if (args.IsSettingsInvoked) navItemTag = "settings";
            if (args.InvokedItemContainer != null)
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
        }

        private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings")
                _page = typeof(SettingsPage);
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }

            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Equals(preNavPageType, _page)) ContentFrame.Navigate(_page, null, transitionInfo);
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;
            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (muxc.NavigationViewItem)NavView.SettingsItem;
                NavView.Header = "SETTINGS";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);
                NavView.SelectedItem = NavView.MenuItems.OfType<muxc.NavigationViewItem>().First(n => n.Tag.Equals(item.Tag));
                NavView.Header = ((muxc.NavigationViewItem)NavView.SelectedItem)?.Content?.ToString().ToUpper();
            }
        }

        /// <summary>
        /// 获取滑动数据
        /// </summary>
        private void The_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {   // 判断滑动的距离
            if (e.Cumulative.Translation.X > 100 && BottomBar.IsTapEnabled) NavView.IsPaneOpen = true; // 打开汉堡菜单            
            else if (e.Cumulative.Translation.X < -100) NavView.IsPaneOpen = false; // 关闭汉堡菜单
        }
        #endregion
        #region 底部工具栏
        /// <summary>
        /// 鼠标右击工具栏活动。
        /// </summary>
        private void CommandBar_RightTapped(object sender, RightTappedRoutedEventArgs e) => ToolBar.IsOpen = !ToolBar.IsOpen;

        /// <summary>
        /// 检查工具栏相关的按钮可用状态。
        /// 下滑隐藏工具栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private void Sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {   // 只有当设置特定选项为 True 方可隐藏显示 ToolBar，True 值等同于下滑值
            if (LocalSettings.isBottomBarHidden is bool canSlideDown)
            {   // 情况一：滑动数据无变化时直接退出函数
                // 情况二：ToolBar 置顶时直接退出函数
                if (sv.VerticalOffset == scrlocation)
                    return;
                if (!BottomBar.IsTapEnabled)
                {
                    BottomBar_Storyboard_SlideDown.Stop();
                    return;
                }

                // 滑动方向
                // 滚动条当前位置小于存储的变量值时代表往上滑，即不可以下滑隐藏
                if (sv.VerticalOffset < scrlocation)
                    canSlideDown = false;

                if (canSlideDown && isShowBar)
                {   // 通过动画来隐藏
                    // bar.Translation = new Vector3(0, 40, 0);
                    BottomBar_Storyboard_SlideDown.Begin();
                    isShowBar = false;

                }
                else if (!canSlideDown && !isShowBar)
                {   // 通过动画来显示
                    // bar.Translation = new Vector3(0, 0, 0);
                    BottomBar_Storyboard_SlideUp.Begin();
                    isShowBar = true;
                }
                scrlocation = sv.VerticalOffset;
            }
        }

        // 滚动条位置变量
        double scrlocation = 0;

        // 导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool isShowBar = true;

        /// <summary>
        /// 返回顶部按钮。
        /// </summary>
        private void BackToTop_Click(object sender, RoutedEventArgs e) => sv.ChangeView(null, 0, null);

        /// <summary>
        /// 刷新按钮。
        /// </summary>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// 新建评论按钮。
        /// </summary>
        private void CreatComment_Click(object sender, RoutedEventArgs e) => Assembly.Dialog(DialogType.CommentCreate);
        #endregion
        #region UI
        private void OnWindowActivated(object sender, WindowActivatedEventArgs e) => VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);

        public void VisibleBounds_Changed(ApplicationView e, object sender)
        {
            var applicationView = ApplicationView.GetForCurrentView();

            var currentHeight = e.VisibleBounds.Height;

            switch (applicationView.Orientation)
            {   // 横向
                case ApplicationViewOrientation.Landscape:
                    // 控制 bar 宽度
                    if (Window.Current.Bounds.Width < 641)
                        VisualStateManager.GoToState(this, MinLandscape.Name, true);
                    else VisualStateManager.GoToState(this, MediumLandscape.Name, true);
                    break;

                // 纵向
                case ApplicationViewOrientation.Portrait:
                    // 控制底部导航栏高度
                    if (currentHeight < oldHeight)
                        VisualStateManager.GoToState(this, WPNavBarVisible.Name, true);
                    else VisualStateManager.GoToState(this, WPNavBarCollapsed.Name, true);
                    break;
            }
            oldHeight = e.VisibleBounds.Height;
        }
        double oldHeight;

        double OpaqueIfEnabled(bool isEnabled) => isEnabled ? 1.0 : 0.7;
        #endregion

        public static MainPage Current;
    }
}