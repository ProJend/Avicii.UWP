using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using static TrueLove.Notification.LiveTile.LVTemplate;

namespace TrueLove.Notification.LiveTile
{
    /// <summary>
    /// 动态磁贴
    /// </summary>
    public class LVAdd
    {
        public static void AddTile()
        {   // 得到磁贴的对象
            TileContent content = StaticTemplate();
            var notification = new TileNotification(content.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification); // 添加到磁贴的队列
        }
    }
}
