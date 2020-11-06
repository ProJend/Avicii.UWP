using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePageforWP : Page
    {
        public HomePageforWP()
        {
            this.InitializeComponent();
            GetFiles();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CommentsPage));
        }

        private async void GetFiles()
        {
            try
            {
                string path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"\Assets\Instagram";
                var storageFolder = await StorageFolder.GetFolderFromPathAsync(path);
                IReadOnlyList<StorageFile> sortedItems = await storageFolder.GetFilesAsync();
                var images = new List<BitmapImage>();
                if (sortedItems.Any())
                {
                    foreach (StorageFile file in sortedItems)
                    {
                        if (file.FileType.ToUpper() == ".JPG")
                        {
                            using (Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                            {
                                BitmapImage bitmapImage = new BitmapImage();
                                await bitmapImage.SetSourceAsync(fileStream);
                                images.Add(bitmapImage);
                            }
                        }
                    }
                }
                else
                {
                    var message = new MessageDialog("There are no images in the Instagram's Pictures Library.");
                    await message.ShowAsync();
                }
                ImageGridView.ItemsSource = images;
            }
            catch (UnauthorizedAccessException)
            {
                var message = new MessageDialog("The app does not have access to the Instagram's Pictures Library on this device.");
                await message.ShowAsync();
            }
        }
    }
}
