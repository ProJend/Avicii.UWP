using Microsoft.Toolkit.Uwp.Notifications;
using System;
using TrueLove.Lib.Helpers;
using TrueLove.Lib.Models.Code.Page;
using TrueLove.Lib.Models.Enum;
using TrueLove.Lib.Notification.Template;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TrueLove.Lib.Notification
{
    public class Show
    {
        public static void Tile()
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();
            updater.EnableNotificationQueue(true);

            CommentViewModel commentCollection = [];
            commentCollection.Load5ItemsRandomly();
            ImageViewModel imageCollection = [];
            imageCollection.Load9ItemsRandomly();
            TileContent content = TileTemplate.ImageTemplate(imageCollection); // 得到磁贴的对象
            TileNotification notification = new(content.GetXml());
            updater.Update(notification); // 添加到磁贴的队列
            for (var i = 0; i < 4; i++)
            {
                content = TileTemplate.CommentTemplate(commentCollection[i]);
                notification = new(content.GetXml());
                updater.Update(notification);
            }
        }

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
            }
            try
            {
                var loaded = await dialogCreate.ShowAsync();
                if (loaded == ContentDialogResult.Secondary) commentCreate.SavingDate();
            }
            catch (Exception) { } // Nothing todo.
        }

        public static void Toast()
        {
            var content = Template.Toast.CheckNetwork();

            // Create the notification
            var notif = new ToastNotification(content.GetXml())
            {
                ExpirationTime = DateTime.Now.AddMinutes(1)
            };

            // And show it!
            ToastNotificationManager.CreateToastNotifier().Show(notif);
        }
    }
}
