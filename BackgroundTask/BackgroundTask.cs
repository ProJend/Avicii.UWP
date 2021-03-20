using TrueLove.Lib.Notification.LiveTile;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //Debug.Write("================ debug to show is working  ================");
            var deferral = taskInstance.GetDeferral();
            TileSetup.SetupTile();
            deferral.Complete();
        }
    }
}