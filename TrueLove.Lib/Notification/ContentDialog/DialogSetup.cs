using System;
using TrueLove.Lib.Helpers;
using TrueLove.Lib.Models.Enum;
using TrueLove.Lib.Notification.ContentDialog.DialogTemplate;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TrueLove.Lib.Notification.ContentDialog
{
    public static class DialogSetup
    {
        public static async void SetupDialog(GetDialogInfo name)
        {
            var dialogCreate = new Windows.UI.Xaml.Controls.ContentDialog();
            var commentCreate = new CommentCreate();
            switch (name)
            {
                case 0:
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

                case (GetDialogInfo)1:
                    dialogCreate.Title = "Release Notes";
                    dialogCreate.CloseButtonText = "Okay";
                    dialogCreate.Content = new ReleaseNotes();
                    dialogCreate.Background = new SolidColorBrush(Colors.Black);
                    //if (!Generic.DeviceFamilyMatch(DeviceFamilyList.Mobile))
                    //{
                    //    dialogCreate.CloseButtonStyle = (Style)Application.Current.Resources["ButtonRevealStyle"];
                    //    dialogCreate.BorderBrush = (Brush)Application.Current.Resources["SystemControlBackgroundListMediumRevealBorderBrush"];
                    //}
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
