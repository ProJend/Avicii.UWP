using System.Diagnostics;
using TrueLove.Lib.Notification;
using Windows.ApplicationModel.Background;

namespace BackgroundTasks
{
    public sealed class TileFeedBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("================ debug to updating tiles  ================");

            // Get a deferral, to prevent the task from closing prematurely
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            Assembly.Tile(); // 组装动态磁贴

            // Inform the system that the task is finished.
            deferral.Complete();
        }
    }
}