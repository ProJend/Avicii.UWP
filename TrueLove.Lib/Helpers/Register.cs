using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace TrueLove.Lib.Helpers
{
    public class Register
    {
        //注册后台任务方法封装
        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(string taskEntryPoint,
                                                                                    string taskName,
                                                                                    IBackgroundTrigger trigger,
                                                                                    IBackgroundCondition condition)
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedByUser)
            {
                return null;
            }
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == taskName)
                {
                    cur.Value.Unregister(true);
                }
            }
            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = taskEntryPoint,
            };
            builder.SetTrigger(trigger);
            if (condition != null)
            {
                builder.AddCondition(condition);
            }
            BackgroundTaskRegistration task = builder.Register();
            return task;
        }
    }
}
