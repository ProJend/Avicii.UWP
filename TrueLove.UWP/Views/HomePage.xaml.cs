using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void LinkingPage_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = sender as FrameworkElement;
            switch (button.Tag as string)
            {
                case "linkingCommentsPage": Frame.Navigate(typeof(CommentsPage)); break;
                case "linkingImagesPage": Frame.Navigate(typeof(ImagesPage)); break;
            }

        }

        private async void GetFiles()
        {
            string localPath = Package.Current.InstalledLocation.Path + @"\Assets\Instagram";
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(localPath);
            IReadOnlyList<StorageFile> sortedItems = await storageFolder.GetFilesAsync();
            var photos = new List<BitmapImage>();
            if (sortedItems.Any())
            {
                foreach (StorageFile file in sortedItems)
                {
                    if (file.FileType.ToUpper() == ".JPG")
                    {
                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            await bitmapImage.SetSourceAsync(fileStream);
                            photos.Add(bitmapImage);
                        }
                    }
                }
            }
            else
            {
                var message = new MessageDialog("There are no images in the Instagram Pictures Library.");
                await message.ShowAsync();
            }
            ImageGridView.ItemsSource = photos;
            LoadingImages.IsActive = false;
            LoadingImages.Visibility = Visibility.Collapsed;
            flipview.SelectionChanged -= FlipView_SelectionChanged; // 取消订阅事件，令其 Method 不再执行
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flipview.SelectedIndex == 3) GetFiles();
        }
    }
}