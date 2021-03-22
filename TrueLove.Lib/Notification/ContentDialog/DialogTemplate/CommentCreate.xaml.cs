using System;
using TrueLove.Lib.Models;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TrueLove.Lib.Notification.ContentDialog.DialogTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommentCreate : Page
    {
        public CommentCreate()
        {
            this.InitializeComponent();
        }

        private async void SelectFiles_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                this.imageInfo.Text = "Picked photo : " + file.Name;
                imageInfo.Opacity = 1;
            }
            else
            {
                this.imageInfo.Text = "Operation cancelled.";
                imageInfo.Opacity = 1;
            }
        }

        private void comment_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i = 500 - comment.Text.Length;
            textLength.Text = i + " of 500 Character(s) left";
            text.Opacity = 1;
        }

        public void LoadingDate()
        {
            comment.Text = ProvisionalDatebase.COMMENT_TXT;
            nickName.Text = ProvisionalDatebase.NICKNAME_TXT;
        }

        public void SavingDate()
        {
            ProvisionalDatebase.COMMENT_TXT = comment.Text;
            ProvisionalDatebase.NICKNAME_TXT = nickName.Text;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datePicker.Date = DateTime.Today;
            if (!string.IsNullOrEmpty(ProvisionalDatebase.COMMENT_TXT) || !string.IsNullOrEmpty(ProvisionalDatebase.NICKNAME_TXT)) LoadingDate();
        }
    }
}
