using TrueLove.Lib.Notification;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class LiveTileActionBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //Debug.Write("================ debug to show is working  ================");
            var deferral = taskInstance.GetDeferral();
            Assembly.Tile(); // 组装动态磁贴
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            deferral.Complete(); // 实现通知循环
        }
    }
}