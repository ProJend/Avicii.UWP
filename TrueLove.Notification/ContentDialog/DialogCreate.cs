using System;
using TrueLove.Lib.Helpers;
using TrueLove.Notification.ContentDialog.DialogTemplate;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TrueLove.Notification.ContentDialog
{
    public static class DialogCreate
    {
        public static async void DialogAdd(string name)
        {
            var dialogCreate = new Windows.UI.Xaml.Controls.ContentDialog();
            var commentCreate = new CommentCreate();
            switch (name)
            {
                case "ReleaseNotes":
                    dialogCreate.Title = "Release Notes";
                    dialogCreate.CloseButtonText = "Okay";
                    dialogCreate.Content = new ReleaseNotes();
                    dialogCreate.Background = new SolidColorBrush(Colors.Black);
                    if (!Generic.DeviceFamilyMatch(DeviceFamilyList.Mobile))
                    {
                        dialogCreate.CloseButtonStyle = (Style)Application.Current.Resources["ButtonRevealStyle"];
                        dialogCreate.BorderBrush = (Brush)Application.Current.Resources["SystemControlBackgroundListMediumRevealBorderBrush"];
                    }
                    break;                  

                case "CommentCreate":
                    dialogCreate.Title = "Write your story of love here:";
                    dialogCreate.CloseButtonText = "Cancel";
                    dialogCreate.PrimaryButtonText = "Send";
                    dialogCreate.PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"];
                    dialogCreate.SecondaryButtonText = "Save";
                    //dialogCreate.DefaultButton = ContentDialogButton.Primary;
                    dialogCreate.Content = commentCreate;
                    dialogCreate.Background = new SolidColorBrush(Colors.Black);
                    if (!Generic.DeviceFamilyMatch(DeviceFamilyList.Mobile))
                    {
                        dialogCreate.CloseButtonStyle = (Style)Application.Current.Resources["ButtonRevealStyle"];
                        dialogCreate.SecondaryButtonStyle = (Style)Application.Current.Resources["ButtonRevealStyle"];
                        dialogCreate.BorderBrush = (Brush)Application.Current.Resources["SystemControlBackgroundListMediumRevealBorderBrush"];
                    }
                    break;
            }

            try
            {
                var loaded = await dialogCreate.ShowAsync();
                if (loaded == ContentDialogResult.Secondary) commentCreate.SavingDate();
            }
            catch (System.Runtime.InteropServices.COMException) { } // Nothing todo.
        }
}
}
