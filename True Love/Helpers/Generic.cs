using System;
using Windows.ApplicationModel.Resources;
using Windows.System.Profile;

namespace True_Love.Helpers
{
    public static class Generic
    {
        /// <summary>
        /// 获取字符串资源。
        /// </summary>
        /// <param name="UID">唯一识别码</param>
        /// <returns><returns>
        public static string GetResourceString(string UID)
        {
            var resourceLoader = ResourceLoader.GetForCurrentView().GetString(UID);
            if (!string.IsNullOrEmpty(resourceLoader)) return resourceLoader;
            else throw new ArgumentException($"There're blank output on this uid : {UID}.\nTap Ctrl + F to search and find it out in current project.");
        }

        /// <summary>
        /// 识别设备。
        /// </summary>
        /// <param name="DeviceOS">Desktop, Mobile, Team, IoT, Holographic or Xbox</param>
        /// <returns></returns>
        public static bool IdentifyDeviceFamily(string DeviceOS)
        {
            var DeviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            var CurrentDevice = "Windows." + DeviceOS.Substring(0, 1).ToUpper() + DeviceOS.Substring(1).ToLower(); // 首字母大写
            switch (CurrentDevice)
            {   // 确认设备输入正确
                case "Windows.Desktop":
                case "Windows.Mobile":
                case "Windows.Team":
                case "Windows.IoT":
                case "Windows.Holographic":
                case "Windows.Xbox":
                    if (DeviceFamily == CurrentDevice) return true;
                    else return false;
                default:
                    throw new ArgumentException($"The parameter is incorrect : {DeviceOS}.\nTap Ctrl + F to search and find it out in current project.");
            }          
        }
    }
}
