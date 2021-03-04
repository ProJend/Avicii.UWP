using System;
using Windows.UI.Notifications;

namespace TrueLove.Notification.Toast
{
    public class ToastCreate
    {
        public static void AddToast()
        {
            var content = ToastTemplate.Network();

            // Create the notification
            var notif = new ToastNotification(content.GetXml())
            {
                ExpirationTime = DateTime.Now.AddSeconds(10)
            };

            // And show it!
            ToastNotificationManager.CreateToastNotifier().Show(notif);
        }
    }
}
