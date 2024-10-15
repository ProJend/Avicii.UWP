using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueLove.Lib.Models.Code;
using TrueLove.Lib.Notification;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class ImagesPage : Page
    {
        public ImagesPage()
        {
            this.InitializeComponent();
            Loaded += Page_Loaded; // 订阅页面加载后事件
            Window.Current.Activated += OnWindowActivated; // 订阅窗口活动事件            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Window.Current.Activated -= OnWindowActivated;

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e) => VisualStateManager.GoToState(this,
                e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ImageCollection.LoadMoreItemsManually();
            Loaded -= Page_Loaded;
        }

        private async void Scroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (Scroller.ScrollableHeight - Scroller.VerticalOffset <= 500)
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

        private readonly ImageCollection ImageCollection = [];

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedImageSource = (sender as FrameworkElement).DataContext as dynamic;
            if (selectedImageSource != null)
            {
                // 创建 FileSavePicker 让用户选择保存图片的位置
                FileSavePicker savePicker = new()
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };

                // 定义正则表达式，匹配不带扩展名的文件名
                string pattern = @"[^/]+(?=\.\w+$)";

                // 使用正则表达式匹配
                Match match = Regex.Match(selectedImageSource, pattern);

                // 限制文件类型为图片格式
                savePicker.FileTypeChoices.Add("JPEG Image", [".jpg"]);
                savePicker.SuggestedFileName = match.Value;

                // 用户选择保存位置
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    // 下载并保存 HTTPS 图片
                    await DownloadAndSaveImageAsync(new Uri(selectedImageSource), file);
                }
            }
        }

        // 下载并保存图片
        private async Task DownloadAndSaveImageAsync(Uri imageUri, StorageFile file)
        {
            try
            {
                // 使用 HttpClient 下载图片
                HttpClient client = new();
                byte[] imageBytes = await client.GetByteArrayAsync(imageUri);

                // 将下载的字节写入到本地文件
                await FileIO.WriteBytesAsync(file, imageBytes);
            }
            catch (Exception)
            {
                // 处理可能的下载错误
                ContentDialog errorDialog = new()
                {
                    Title = "Error",
                    Content = $"Failed to save image: No Network Available",
                    CloseButtonText = "OK"
                };
                await errorDialog.ShowAsync();
            }
        }
    }
}