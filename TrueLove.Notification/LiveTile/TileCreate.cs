using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using static TrueLove.Notification.LiveTile.TileTemplate;

namespace TrueLove.Notification.LiveTile
{
    /// <summary>
    /// 动态磁贴
    /// </summary>
    public class TileCreate
    {
        public static void AddTile()
        {   
            TileUpdateManager.CreateTileUpdaterForApplication().Clear(); // 清空队列
            TileContent content = StaticTemplate(); // 得到磁贴的对象
            var notification = new TileNotification(content.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification); // 添加到磁贴的队列
        }
    }
}
