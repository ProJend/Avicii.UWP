using TrueLove.Lib.Notification;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //Debug.Write("================ debug to show is working  ================");
            var deferral = taskInstance.GetDeferral();
            Assembly.Tile(); // 組裝動態磚
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            deferral.Complete(); // 实现通知循环
        }
    }
}