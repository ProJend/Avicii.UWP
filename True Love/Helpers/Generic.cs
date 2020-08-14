using Windows.ApplicationModel.Resources;

namespace True_Love.Helpers
{
    public static class Generic
    {
        /// <summary>
        /// 获取字符串资源。
        /// </summary>
        /// <param name="UID">唯一识别码</param>
        public static string GetResourceString(string UID)
        {
            var resourceLoader = ResourceLoader.GetForCurrentView();
            return resourceLoader.GetString(UID);
        }
    }
}
