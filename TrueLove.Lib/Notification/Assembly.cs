using Microsoft.Toolkit.Uwp.Notifications;
using System;
using TrueLove.Lib.Helpers;
using TrueLove.Lib.Models.Enum;
using TrueLove.Lib.Notification.Template;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TrueLove.Lib.Notification
{
    public class Assembly
    {
        public static async void Dialog(DialogType name)
        {
            var dialogCreate = new ContentDialog();
            var commentCreate = new Template.ContentDialog.CommentCreate();
            switch (name)
            {
                case DialogType.CommentCreate:
                    dialogCreate.Title = "Write your story of love here:";
                    dialogCreate.CloseButtonText = "Cancel";
                    dialogCreate.PrimaryButtonText = "Send";
                    dialogCreate.PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"];
                    dialogCreate.SecondaryButtonText = "Save";
                    //dialogCreate.DefaultButton = ContentDialogButton.Primary;
                    dialogCreate.Content = commentCreate;
                    dialogCreate.Background = new SolidColorBrush(Colors.Black);
                    if (!Generic.DeviceFamilyMatch(DeviceFamilyType.Mobile))
                    {
                        dialogCreate.CloseButtonStyle = (Style)Application.Current.Resources["ButtonRevealStyle"];
                        dialogCreate.SecondaryButtonStyle = (Style)Application.Current.Resources["ButtonRevealStyle"];
                        dialogCreate.BorderBrush = (Brush)Application.Current.Resources["SystemControlBackgroundListMediumRevealBorderBrush"];
                    }
                    break;

                case DialogType.ReleaseNotes:
                    dialogCreate.Title = "Release Notes";
                    dialogCreate.CloseButtonText = "Okay";
                    dialogCreate.Content = new Template.ContentDialog.ReleaseNotes();
                    dialogCreate.Background = new SolidColorBrush(Colors.Black);
                    //if (!Generic.DeviceFamilyMatch(DeviceFamilyType.Mobile))
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

        public static void Tile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear(); // 清空队列
            TileContent content = LiveTile.StaticTemplate(); // 得到磁贴的对象
            var notification = new TileNotification(content.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification); // 添加到磁贴的队列
        }

        public static void Toast()
        {
            var content = Template.Toast.Network();
             
            // Create the notification
            var notif = new ToastNotification(content.GetXml())
            {
                ExpirationTime = DateTime.Now.AddMinutes(5)
            };

            // And show it!
            ToastNotificationManager.CreateToastNotifier().Show(notif);
        }
    }
}
