using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace TrueLove.Lib.Helpers
{
    public class Register
    {   //注册后台任务方法封装
        public static async Task<ApplicationTrigger> RegisterBackgroundTask(string taskEntryPoint,
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

            BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = taskName;
            taskBuilder.TaskEntryPoint = taskEntryPoint;
            taskBuilder.SetTrigger(trigger);
            if(condition is SystemCondition condition1)
            taskBuilder.AddCondition(condition1);

            var trigger2 = new ApplicationTrigger();
            taskBuilder.SetTrigger(trigger2);
            taskBuilder.Register();
            await trigger2.RequestAsync();
            return trigger2;
        }      
    }
}