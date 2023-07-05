using System.Linq;
using Windows.ApplicationModel.Background;

namespace TrueLove.Lib.Notification
{
    public class Register
    {   //注册后台任务方法封装
        public static void RegisterBackgroundTask(string taskName)
        {   // If background task is already registered, do nothing
            if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(taskName)))
                return;

            var builder = new BackgroundTaskBuilder()
            {
                Name = taskName,
                //TaskEntryPoint = "BackgroundTask.BackgroundTask",
            };
            var trigger = new ToastNotificationActionTrigger();
            builder.SetTrigger(trigger);
            builder.Register();
        }
    }
}