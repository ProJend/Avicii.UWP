using System;
using System.Linq;
using Windows.ApplicationModel.Background;

namespace TrueLove.Lib.Notification
{
    public class Register
    {
        //注册后台任务方法封装
        public static async void RegisterBackgroundTask(string taskName)
        {
            // If background task is already registered, do nothing
            if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(taskName)))
                return;

            // Otherwise request access
            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            // Create the background task
            BackgroundTaskBuilder builder = new()
            {
                Name = taskName
            };

            // Assign the toast action trigger
            builder.SetTrigger(new ToastNotificationActionTrigger());

            // And register the task
            BackgroundTaskRegistration registration = builder.Register();
        }

        public static async void RegisterBackgroundTask(string taskName, string taskEntryPoint)
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy ||
                backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new()
                {
                    Name = taskName,
                    TaskEntryPoint = taskEntryPoint
                };
                taskBuilder.SetTrigger(new TimeTrigger(30, false));
                taskBuilder.Register();
            }
        }
    }
}