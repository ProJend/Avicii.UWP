using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.Storage;
using System.Numerics;
using Microsoft.Toolkit.Uwp.Connectivity;
using Windows.UI.Xaml.Media;
using System.Xml.Serialization;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using Microsoft.QueryStringDotNET;
using True_Love.Pages.XAML_ContentDialog;
using static True_Love.Helpers.Generic;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love.Pages
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
            #region 兼容低版本号系统
            if (IdentifyDeviceFamily("mobile")) // = WP
            {
                if ((bool)localSettings.Values["SetBackgroundColor"]) BackgroundOfBar.Background = new SolidColorBrush(Colors.Black);
                else BackgroundOfBar.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
                CommandBar.Background = new SolidColorBrush { Color = Colors.Black, Opacity = 0.7 };
            }
            else // = PC
            {   // Listen to the window directly so we will respond to hotkeys regardless
                // of which element has focus.
                Window.Current.CoreWindow.PointerPressed += this.CoreWindow_PointerPressed;
                Window.Current.SetTitleBar(AppTitleBar);
                BackTopButton.Style = (Style)Resources["AppBarButtonRevealStyle"];
                RefreshButton.Style = (Style)Resources["AppBarButtonRevealStyle"];
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5))// > OS15063
                {   // 键盘快捷键
                    //MemoryPage.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F1 });
                    //HomePage.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F2 });
                    //ImagesPage.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F3 });
                    BackTopButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F6 });
                    RefreshButton.KeyboardAccelerators.Add(new KeyboardAccelerator { Key = VirtualKey.F5 });

                    // Add keyboard accelerators for backwards navigation.
                    var goBack = new KeyboardAccelerator { Key = VirtualKey.Escape };
                    goBack.Invoked += BackInvoked;
                    this.KeyboardAccelerators.Add(goBack);
                }
            }
            #endregion
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
#if !DEBUG
            ImagesPage.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Collapsed;
#endif
            Main.Loaded -= Main_Loaded;
        }

        #region NavigationView
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("home", typeof(HomePage)),
            ("comment", typeof(CommentsPage)),
            ("image", typeof(ImagesPage)),
        };

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {   // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];

            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            NavView_Navigate("home", new EntranceNavigationTransitionInfo());
            NavView.Loaded -= NavView_Loaded;
        }

        /// <summary>
        /// 手势滑动结束
        /// </summary>
        private void The_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {   // 判断滑动的距离
            if (e.Cumulative.Translation.X > 100 && bar.IsTapEnabled) NavView.IsPaneOpen = true; // 打开汉堡菜单            
            if (e.Cumulative.Translation.X < -100) NavView.IsPaneOpen = false; // 关闭汉堡菜单
        }

        private void NavView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true) NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
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

        private void NavView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args) => On_BackRequested();

        private void BackInvoked(KeyboardAccelerator sender,KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;     
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!ContentFrame.CanGoBack) return;
            ContentFrame.GoBack();
            e.Handled = true;
        }

        private bool On_BackRequested()
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

        /// <summary>
        /// Invoked on every mouse click, touch screen tap, or equivalent interaction.
        /// Used to detect browser-style next and previous mouse button clicks
        /// to navigate between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
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
                if (backPressed) this.On_BackRequested();
                //if (forwardPressed) this.TryGoForward();
            }
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;
            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (muxc.NavigationViewItem)NavView.SettingsItem;
                NavView.Header = "SETTINGS";
                ControlsChanged("settings");
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);
                NavView.SelectedItem = NavView.MenuItems.OfType<muxc.NavigationViewItem>().First(n => n.Tag.Equals(item.Tag));
                NavView.Header = ((muxc.NavigationViewItem)NavView.SelectedItem)?.Content?.ToString().ToUpper();
                ControlsChanged(item.Tag.ToString());
            }
            GC.Collect();
        }
        #endregion
        #region 底部工具栏
        /// <summary>
        /// 鼠标右击工具栏活动。
        /// </summary>
        private void CommandBar_RightTapped(object sender, RightTappedRoutedEventArgs e) => CommandBar.IsOpen = !CommandBar.IsOpen == true;

        /// <summary>
        /// 检查工具栏相关的按钮可用状态。
        /// 下滑隐藏命令栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private void Sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset != scrlocation)
            {
                bool IsWide;
                if (!bar.IsTapEnabled) IsWide = false;
                else if ((bool)localSettings.Values["SetHideCommandBar"]) IsWide = true;
                else IsWide = false;
                if (sv.VerticalOffset > scrlocation && IsWide)
                {   // 滚动条当前位置大于存储的变量值时代表往下滑，隐藏底部栏
                    if (IsShowBar)
                    {   // 通过动画来隐藏
                        // bar.Translation = new Vector3(0, 40, 0);
                        Close.Begin();
                        IsShowBar = false;
                    }
                }
                else if(!IsShowBar)
                {   // 通过动画来隐藏
                    // bar.Translation = new Vector3(0, 0, 0);
                    Open.Begin();
                    IsShowBar = true;
                }
                if (sv.VerticalOffset > 1) BackTopButton.IsEnabled = true; // 当滚动条高度大于 1 时，返回顶部按钮维持使用状态
                else if (sv.VerticalOffset < sv.ViewportHeight) BackTopButton.IsEnabled = false; // 反之停用此按钮
            }
            scrlocation = sv.VerticalOffset;  
        }

        /// <summary>
        /// 返回滑动条参数。
        /// </summary>
        /// <param name="horizontalOffset">X：横度。</param>
        /// <param name="verticalOffset">Y：高度。</param>
        /// <param name="zoomFactor">Z：斜度。</param>
        public void ChangeView(double? horizontalOffset, double? verticalOffset, float? zoomFactor) { }

        /// <summary>
        /// 返回顶部按钮。
        /// </summary>
        private void BackToTop_Click(object sender, RoutedEventArgs e) => sv.ChangeView(null, 0, null);

        /// <summary>
        /// 刷新按钮。
        /// </summary>
        private void Refresh_Click(object sender, RoutedEventArgs e) => CommentsPage.Current.webview.Refresh();

        /// <summary>
        /// 新建评论按钮。
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newComment = new NewComment();
            if (!string.IsNullOrEmpty(comment) || !string.IsNullOrEmpty(nickName)) newComment.Save();
            var Comment = new ContentDialog()
            {
                Title = "Write your story of love here:",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Send",
                PrimaryButtonStyle = (Style)Resources["AccentButtonStyle"],
                SecondaryButtonText = "Save",
                //DefaultButton = ContentDialogButton.Primary,
                Content = newComment,
                Background = new SolidColorBrush(Colors.Black),
            };
            if (!IdentifyDeviceFamily("mobile"))
            {
                Comment.CloseButtonStyle = (Style)Resources["ButtonRevealStyle"];
                Comment.SecondaryButtonStyle = (Style)Resources["ButtonRevealStyle"];
                Comment.BorderBrush = (Brush)Resources["SystemControlBackgroundListMediumRevealBorderBrush"];
            }
            try
            {
                var a = await Comment.ShowAsync();
                if (a == ContentDialogResult.Secondary)
                {
                    comment = newComment.commentPlain;
                    nickName = newComment.nicknamePlain;
                }
            }
            catch (System.Runtime.InteropServices.COMException) { } // Nothing todo.
        }

        /// <summary>
        /// 检查控件可用状态
        /// </summary>
        /// <param name="tag">NavViewItem.Tag</param>
        private void ControlsChanged(string tag)
        {
            switch (tag)
            {
                case "home":
#if !DEBUG
                    CommandBarCollapsed();
#endif
                    RefreshButton.IsEnabled = false;
                    if (Notification.Visibility == Visibility.Visible && Notification.Opacity == 1) NotificationCollapsed();
                    break;

                case "comment":
                    CommandBarVisible();
                    RefreshButton.IsEnabled = true;
                    if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable && (bool)localSettings.Values["ToastIsPush"] == false)
                    {
                        Icon.Text = "⚠";
                        NameInfo.Text = "There's no network available.";
                        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7)) BackgroundOfBar.Translation = new Vector3(0, 0, 0);
                        else Notification.Visibility = Visibility.Visible;
                        Notification.Opacity = 1;
                        if ((bool)localSettings.Values["ToastIsPush"] == false)
                        {
                            int conversationId = 384928;
                            var content = new ToastContentBuilder()
                            .AddToastActivationInfo(new QueryString()
                            {
                                { "action", "checkNetwork" },
                                { "conversationId", conversationId.ToString() }
                            }.ToString(), ToastActivationType.Foreground)
                            .AddText("Time Out")
                            .AddText("There's no network available.")
                            .AddButton("Settings", ToastActivationType.Foreground, new QueryString()
                            {
                                { "action", "Settings" },
                                { "conversationId", conversationId.ToString() }
                            }.ToString())
                            .AddButton("Close", ToastActivationType.Foreground, new QueryString()
                            {
                                { "action", "Close" },
                                { "conversationId", conversationId.ToString() }
                            }.ToString())
                            .GetToastContent();

                            // Create the notification
                            var notif = new ToastNotification(content.GetXml())
                            {
                                ExpirationTime = DateTime.Now.AddSeconds(10)
                            };

                            // And show it!
                            ToastNotificationManager.CreateToastNotifier().Show(notif);
                        }
                    }
                    else NotificationCollapsed();
                    break;

                default:
                    CommandBarVisible();
                    RefreshButton.IsEnabled = false;
                    if (Notification.Visibility == Visibility.Visible && Notification.Opacity == 1) NotificationCollapsed();
                    break;
            }
            void CommandBarCollapsed()
            {
                if (IdentifyDeviceFamily("mobile")) CommandBar.Visibility = Visibility.Collapsed;
                else
                {
                    bar.Opacity = 0;
                    CommandBar.IsEnabled = false;
                }
            }
            void CommandBarVisible()
            {
                if (IdentifyDeviceFamily("mobile")) CommandBar.Visibility = Visibility.Visible;
                else
                {
                    bar.Opacity = 1;
                    CommandBar.IsEnabled = true;
                }
            }
            void NotificationCollapsed()
            {
                Notification.Opacity = 0;
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7)) BackgroundOfBar.Translation = new Vector3(0, -40, 0);
                else Notification.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        /// <summary>
        /// 更改主题颜色
        /// 方法
        /// 即时变更
        /// </summary>
        public void PageBackgroundChange()
        {
            if ((bool)localSettings.Values["SetBackgroundColor"]) Main.Background = new SolidColorBrush(Colors.Black);
            else Main.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
            if (IdentifyDeviceFamily("mobile") && (bool)localSettings.Values["SetBackgroundColor"]) BackgroundOfBar.Background = new SolidColorBrush(Colors.Black);
            else if (IdentifyDeviceFamily("mobile")) BackgroundOfBar.Background = new SolidColorBrush((Color)Resources["SystemChromeMediumColor"]);
        }

        /// <summary>
        /// 更改主题颜色
        /// 数据绑定
        /// 需要重启应用
        /// </summary>
        [XmlIgnore]
        public string Color => (bool)localSettings.Values["SetBackgroundColor"] ? "Black" : "#FF1F1F1F";

        // 滚动条位置变量
        public double scrlocation = 0;
        // 导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool IsShowBar = true;
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public double OpaqueIfEnabled(bool IsEnabled) => IsEnabled ? 1.0 : 0.6;
        public static MainPage Current;
        public string comment;
        public string nickName;
    }
}
