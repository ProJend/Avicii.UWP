using System;
using Windows.ApplicationModel.Background;

namespace TrueLove.Lib.Notification
{
    public class Register
    {   //注册后台任务方法封装
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