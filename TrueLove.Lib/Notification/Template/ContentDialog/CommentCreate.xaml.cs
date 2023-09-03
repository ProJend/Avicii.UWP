using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TrueLove.Lib.Notification.Template.ContentDialog
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

        private async void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                imageInfo.Text = "Picked photo : " + file.Name;
                imageInfo.Opacity = 1;
            }
            else
            {
                imageInfo.Text = "Operation cancelled.";
                imageInfo.Opacity = 1;
            }
        }

        private void CommentInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i = 500 - CommentInput.Text.Length;
            LengthOverShow.Text = i + " of 500 Character(s) left";
            text.Opacity = 1;
        }

        public void LoadingDate()
        {
            CommentInput.Text = _Comment;
            NicknameInput.Text = _Nickname;
        }

        public void SavingDate()
        {
            _Comment = CommentInput.Text;
            _Nickname = NicknameInput.Text;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_Comment) || !string.IsNullOrEmpty(_Nickname)) LoadingDate();
        }

        string _Comment;
        string _Nickname;
    }
}
