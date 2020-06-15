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
using Windows.UI.Xaml.Controls.Primitives;
using System.Numerics;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //滚动条位置变量
        double scrlocation = 0;
        //导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool isshowbmbar = true;

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(AppTitleBar);
            CommandBarTransition();
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
         {
            ("home", typeof(LovePage)),
            ("comment", typeof(CommentsPage)),
            ("music", typeof(FMPage)),
        };

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];
            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            NavView_Navigate("home", new EntranceNavigationTransitionInfo());

            // Add keyboard accelerators for backwards navigation.
            var goBack = new KeyboardAccelerator { Key = VirtualKey.Escape };
            goBack.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var back = new KeyboardAccelerator
            {
                Key = VirtualKey.Back,
                //Modifiers = VirtualKeyModifiers.Menu
            };
            back.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(back);
        }

        private void NavView_ItemInvoked(muxc.NavigationView sender,
                                         muxc.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        // NavView_SelectionChanged is not used in this example, but is shown for completeness.
        // You will typically handle either ItemInvoked or SelectionChanged to perform navigation,
        // but not both.
        private void NavView_SelectionChanged(muxc.NavigationView sender,
                                              muxc.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {;
            Type _page = null;
            if (navItemTag == "settings")
            {
                _page = typeof(SettingsPage);
                //ContentFrame.Navigate(_page, null, new DrillInNavigationTransitionInfo());
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Equals(preNavPageType, _page))
            {
                ContentFrame.Navigate(_page, null, transitionInfo);
            }
        }

        private void NavView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args) => On_BackRequested();

        private void BackInvoked(KeyboardAccelerator sender,
                                 KeyboardAcceleratorInvokedEventArgs args)
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
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (muxc.NavigationViewItem)NavView.SettingsItem;
                //NavView.Header = "Settings";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

                NavView.SelectedItem = NavView.MenuItems
                    .OfType<muxc.NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));

                //NavView.Header =
                //    ((muxc.NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
            }
        }

        #region 命令栏
        /// <summary>
        /// 鼠标右击命令栏活动。
        /// </summary>
        private void CommandBar_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            CommandBar.IsOpen = !CommandBar.IsOpen == true;
        }

        /// <summary>
        /// 命令栏跟随窗口移动动画。
        /// </summary>
        private void CommandBarTransition()
        {
            EdgeUIThemeTransition inStoryoard = new EdgeUIThemeTransition { Edge = EdgeTransitionLocation.Bottom };
            var tc = new TransitionCollection { inStoryoard };
            bar.Transitions = tc;
        }

        /// <summary>
        /// 下滑隐藏命令栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private void Sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset != scrlocation)
            {
                //滚动条当前位置大于存储的变量值时代表往下滑，隐藏底部栏
                if (sv.VerticalOffset > scrlocation)
                {                 
                    //隐藏
                    if (isshowbmbar)
                    {               
                        //通过动画来隐藏
                        bar.Translation = new Vector3(0, 40, 0);

                        //直接隐藏
                        //bar.Visibility = Visibility.Collapsed;

                        //bar.Opacity = 0;
                        //CommandBar.IsEnabled = false;

                        isshowbmbar = false;
                    }
                }
                //反之展开
                else
                {
                    //显示
                    if (isshowbmbar == false)
                    {
                        //通过动画来隐藏
                        bar.Translation = new Vector3(0, 0, 0);

                        //直接隐藏
                        //bar.Visibility = Visibility.Visible;

                        //bar.Opacity = 1;
                        //CommandBar.IsEnabled = true;

                        isshowbmbar = true;
                    }
                }

                //当滚动条高度大于1时，返回顶部按钮维持使用状态
                if (sv.VerticalOffset > 1)
                {
                    BackTopButton.IsEnabled = true;
                }
                //反之停用此按钮
                else if (sv.VerticalOffset < sv.ViewportHeight)
                {
                    BackTopButton.IsEnabled = false;
                }
            }  
            scrlocation = sv.VerticalOffset;
        }

        /// <summary>
        /// 返回滑动条参数。
        /// </summary>
        /// <param name="horizontalOffset">X：横度。</param>
        /// <param name="verticalOffset">Y：高度。</param>
        /// <param name="zoomFactor">Z：斜度。</param>
        public void ChangeView(double? horizontalOffset,
                               double? verticalOffset,
                               float? zoomFactor)
        {

        }

        /// <summary>
        /// 返回顶部按钮。
        /// </summary>
        private void BackToTop_Click(object sender, RoutedEventArgs e)
        {
            sv.ChangeView(null, 0, null);
        }

        /// <summary>
        /// 刷新按钮。
        /// </summary>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {

        }

        //private void Visualizer_RefreshStateChanged(RefreshVisualizer sender, RefreshStateChangedEventArgs args)
        //{
        //    // Respond to visualizer state changes.
        //    // Disable the refresh button if the visualizer is refreshing.
        //    if (args.NewState == RefreshVisualizerState.Refreshing)
        //    {
        //        RefreshButton.IsEnabled = false;
        //    }
        //    else
        //    {
        //        RefreshButton.IsEnabled = true;
        //    }
        //}
        #endregion
    }
}
