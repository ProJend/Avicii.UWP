using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Serialization;
using TrueLove.Lib.Helpers;
using TrueLove.Lib.Models.Enum;
using TrueLove.Lib.Models.UI;
using TrueLove.Lib.Notification.ContentDialog;
using TrueLove.Lib.Notification.Toast;
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
            this.ManipulationCompleted += The_ManipulationCompleted; // 订阅手势滑动结束后的事件
            SystemNavigationManager.GetForCurrentView().BackRequested += System_BackRequested;

            #region 兼容低版本号系统
            if (Generic.DeviceFamilyMatch(DeviceFamilyList.Mobile)) // = WP
            {
                if (LocalSettingsVariable.setPageBackgroundColor) TopBar.Background = new SolidColorBrush(Colors.Black);
                else TopBar.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
                TaskBar.Background = new SolidColorBrush { Color = Colors.Black, Opacity = 0.7 };
            }
            else // = PC
            {   // Listen to the window directly so we will respond to hotkeys regardless
                // of which element has focus.
                Window.Current.CoreWindow.PointerPressed += this.Mouse_BackRequested;
                Window.Current.Activated += OnWindowActivated;
                Window.Current.SetTitleBar(AppTitleBar);

                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5))// > OS15063
                {   // 键盘快捷键
                    //MemoryPage.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F1 });
                    //HomePage.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F2 });
                    //ImagesPage.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F3 });
                    BackTopButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F6 });
                    RefreshButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F5 });

                    // Add keyboard accelerators for backwards navigation.
                    var goBack = new KeyboardAccelerator { Key = VirtualKey.Escape };
                    goBack.Invoked += Keyboard_BackRequested;
                    this.KeyboardAccelerators.Add(goBack);
                }
            }
            #endregion
#if !DEBUG
            ImagesPage.Visibility = Visibility.Collapsed;
            CreatComment.Visibility = Visibility.Collapsed;
#endif
            Main.Loaded -= Main_Loaded;
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
            NavView.Loaded -= NavView_Loaded;
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
            TryGoBack();
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
            if (args.InvokedItemContainer != null) NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
        }

        private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings") _page = typeof(SettingsPage);
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
                StoryboardControl("settings");
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);
                NavView.SelectedItem = NavView.MenuItems.OfType<muxc.NavigationViewItem>().First(n => n.Tag.Equals(item.Tag));
                NavView.Header = ((muxc.NavigationViewItem)NavView.SelectedItem)?.Content?.ToString().ToUpper();
                StoryboardControl(item.Tag.ToString());
            }
        }

        void StoryboardControl(string navItemTag)
        {
            switch (navItemTag)
            {
                case "home":
#if !DEBUG
                    BottomBar_Storyboard_Fadeout.Begin();
#endif
                    goto case "notificationBarCollapsed";

                case "comment":
                    if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                    {
                        NotificationIcon.Text = "⚠";
                        NotificationHint.Text = "There's no network available.";

                        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                        {
                            NotificationGrid.Opacity = 1;
                            TopBar.Translation = new Vector3(0, 0, 0);
                        }
                        else NotificationGrid.Visibility = Visibility.Visible;

                        ToastSetup.ToastCharge();
                    }
                    else goto default;
                    break;

                case "notificationBarCollapsed":
                    if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                    {
                        NotificationGrid.Opacity = 0;
                        TopBar.Translation = new Vector3(0, -40, 0);
                    }
                    else NotificationGrid.Visibility = Visibility.Collapsed;
                    break;

                default:
                    BottomBar_Storyboard_Fadein.Begin();
                    goto case "notificationBarCollapsed";
            }
        }

        /// <summary>
        /// 手势滑动结束
        /// </summary>
        private void The_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {   // 判断滑动的距离
            if (e.Cumulative.Translation.X > 150 && BottomBar.IsTapEnabled) NavView.IsPaneOpen = true; // 打开汉堡菜单            
            if (e.Cumulative.Translation.X < -150) NavView.IsPaneOpen = false; // 关闭汉堡菜单
        }
        #endregion
        #region 底部工具栏
        /// <summary>
        /// 鼠标右击工具栏活动。
        /// </summary>
        private void CommandBar_RightTapped(object sender, RightTappedRoutedEventArgs e) => TaskBar.IsOpen = !TaskBar.IsOpen == true;

        /// <summary>
        /// 检查工具栏相关的按钮可用状态。
        /// 下滑隐藏命令栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private void Sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset != scrlocation)
            {
                bool isWide;
                if (!BottomBar.IsTapEnabled) isWide = false;
                else isWide = LocalSettingsVariable.setHideBottomBar;
                if (sv.VerticalOffset > scrlocation && isWide)
                {   // 滚动条当前位置大于存储的变量值时代表往下滑，隐藏底部栏
                    if (isShowBar)
                    {   // 通过动画来隐藏
                        // bar.Translation = new Vector3(0, 40, 0);
                        BottomBar_Storyboard_SlideDown.Begin();
                        isShowBar = false;
                    }
                }
                else if(!isShowBar)
                {   // 通过动画来隐藏
                    // bar.Translation = new Vector3(0, 0, 0);
                    BottomBar_Storyboard_SlideUp.Begin();
                    isShowBar = true;
                }
                if (sv.VerticalOffset > 1) BackTopButton.IsEnabled = true; // 当滚动条高度大于 1 时，返回顶部按钮维持使用状态
                else if (sv.VerticalOffset < sv.ViewportHeight) BackTopButton.IsEnabled = false; // 反之停用此按钮
            }
            scrlocation = sv.VerticalOffset;  
        }

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
        private void CreatComment_Click(object sender, RoutedEventArgs e) => DialogSetup.SetupDialog(GetDialogInfo.CommentCreate);
        #endregion

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e)
        {
            VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);
        }

        public void VisibleBounds_Changed(ApplicationView e, object sender)
        {
            var applicationView = ApplicationView.GetForCurrentView();
            applicationView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

            var currentHeight = e.VisibleBounds.Height;

            switch (applicationView.Orientation)
            {   // 横向
                case ApplicationViewOrientation.Landscape:
                    // 控制底部导航栏高度
                    VisualStateManager.GoToState(this, WPNavBarVisible.Name, true);
                    // 控制 bar 宽度
                    if (Window.Current.Bounds.Width < 876)
                    {
                        MainPage.Current.NavViewRoot.Margin = new Thickness(48, 0, 48, 0);
                        MainPage.Current.TaskBar.Margin = new Thickness(0, 0, 48, 0);
                    }
                    break;

                // 纵向
                case ApplicationViewOrientation.Portrait:
                    // 控制底部导航栏高度
                    if (currentHeight < OtherVariable.oldHeight) 
                        VisualStateManager.GoToState(this, WPNavBarVisible.Name, true);
                    else
                        VisualStateManager.GoToState(this, WPNavBarCollapsed.Name, true);

                    // 控制 bar 宽度
                    MainPage.Current.NavViewRoot.Margin = new Thickness(0);
                    MainPage.Current.TaskBar.Margin = new Thickness(0);
                    break;
            }
            OtherVariable.oldHeight = e.VisibleBounds.Height;
        }


        /// <summary>
        /// 更改主题颜色
        /// 方法
        /// 即时变更
        /// </summary>
        public void PageBackgroundChange()
        {
            if (!LocalSettingsVariable.setPageBackgroundColor) Main.Background = new SolidColorBrush(Colors.Black);
            else Main.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
            if (Generic.DeviceFamilyMatch(DeviceFamilyList.Mobile) && !LocalSettingsVariable.setPageBackgroundColor)
                TopBar.Background = new SolidColorBrush(Colors.Black);
            else if (Generic.DeviceFamilyMatch(DeviceFamilyList.Mobile))
                TopBar.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
        }

        /// <summary>
        /// 更改主题颜色
        /// 数据绑定
        /// 需要重启应用
        /// </summary>
        [XmlIgnore]
        public string PageBackgroundColor => LocalSettingsVariable.setPageBackgroundColor ? "Black" : "#FF1F1F1F";

        // 滚动条位置变量
        public double scrlocation = 0;
        // 导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool isShowBar = true;
        public double OpaqueIfEnabled(bool isEnabled) => isEnabled ? 1.0 : 0.7;
        public static MainPage Current;
    }
}
