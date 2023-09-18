using TrueLove.Lib.Models.Code;
using TrueLove.Lib.Spider;
using Windows.Storage;
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
        public CommentsPage()
        {
            this.InitializeComponent();
            Window.Current.Activated += OnWindowActivated; // 订阅窗口活动事件
            GetSourceCode();
        }

        async void GetSourceCode()
        {
            RefreshButton.IsEnabled = false;
            var reviewWeb = new ReviewWeb();
            _src = await reviewWeb.GetSourceCodeAsync(ApplicationData.Current.LocalFolder.Path + $"/OfflineData.txt");
            DataLoad();
        }

        /// <summary>
        /// 返回顶部按钮。
        /// </summary>
        private void BackToTop_Click(object sender, RoutedEventArgs e) => Scroller.ChangeView(null, 0, null);

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshButton.IsEnabled = false;
            commentDataCollection.Clear();
            Scroller.ChangeView(null, 0, null);
            DataLoad();
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
                BottomPartSlideDownStoryboard.Begin();
                canMinimize = false;
            }
            else if (Scroller.VerticalOffset < scrlocation - 18 && !canMinimize)
            {
                BottomPartSlideUpStoryboard.Begin();
                canMinimize = true;
            }
            else if (Scroller.VerticalOffset == 0 && !canMinimize)
            {
                BottomPartSlideUpStoryboard.Begin();
                canMinimize = true;
            }

            scrlocation = Scroller.VerticalOffset;

            if (TopPartExtendHeight == 0)
            {
                TopPartExtendHeight = TopPart.ActualHeight;
            }
            if (TopPartExtendHeight - scrlocation <= 85)
                TopPart.Height = 85;
            else
            {
                BackSubTitle.Opacity = 0;
                TopPart.Height = TopPartExtendHeight - scrlocation;
            }
            if (Scroller.VerticalOffset == 0)
            {
                BackSubTitle.Opacity = 1;
            }

            if (Scroller.ExtentHeight - scrlocation <= 1200)
            {
                DataLoad();
            }
        }

        private async void DataLoad()
        {
            progressRing.IsActive = true;
            var refineData = new RefineData();
            refineData.UpdateComment(_src, commentDataCollection);

            progressRing.IsActive = false;
            _pageNumber++;
            var reviewWeb = new ReviewWeb();
            _src = await reviewWeb.GetSourceCodeAsync($"https://avicii.com/page/{_pageNumber}", false);

            RefreshButton.IsEnabled = true;
        }

        CommentDataCollection commentDataCollection = new CommentDataCollection();
        double _pageNumber;
        string _src;

        // 滚动条位置变量
        double scrlocation = 0;
        // 导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool canMinimize = true;

        double TopPartExtendHeight;
    }
}
