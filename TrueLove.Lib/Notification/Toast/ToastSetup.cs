using System;
using TrueLove.Lib.Models.UI;
using Windows.UI.Notifications;

namespace TrueLove.Lib.Notification.Toast
{
    public class ToastSetup
    {
        public static void SetupToast()
        {
            if (!OtherVariable.isNetworkToastPush)
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
}
