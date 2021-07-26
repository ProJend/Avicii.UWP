using TrueLove.Lib.Notification;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //Debug.Write("================ debug to show is working  ================");
            var deferral = taskInstance.GetDeferral();
            Assembly.Tile(); //組裝動態磚
            deferral.Complete();
        }
    }
}