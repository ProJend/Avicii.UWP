﻿using System;
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

        private async void SelectFiles_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
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

        private void Comment_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i = 500 - comment.Text.Length;
            textLength.Text = i + " of 500 Character(s) left";
            text.Opacity = 1;
        }

        public void LoadingDate()
        {
            comment.Text = strComment;
            nickName.Text = strNickname;
        }

        public void SavingDate()
        {
            strComment = comment.Text;
            strNickname = nickName.Text;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(strComment) || !string.IsNullOrEmpty(strNickname)) LoadingDate();
        }

        string strComment;
        string strNickname;
    }
}
