using System;
using Windows.ApplicationModel.Resources;
using Windows.System.Profile;

namespace TrueLove.Lib.Helpers
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
        /// <param name="deviceOS">Desktop, Mobile, Team, IoT, Holographic or Xbox</param>
        /// <returns></returns>
        public static bool DeviceFamilyMatch(string deviceOS)
        {
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            var currentDevice = deviceOS; // 首字母大写
            switch (currentDevice)
            {   // 确认设备输入正确
                case "Windows.Desktop":
                case "Windows.Mobile":
                case "Windows.Team":
                case "Windows.IoT":
                case "Windows.Holographic":
                case "Windows.Xbox":
                    if (deviceFamily == currentDevice) return true;
                    else return false;
                default:
                    throw new ArgumentException($"The parameter is incorrect : {deviceOS}.\nTap Ctrl + F to search and find it out in current project.");
            }          
        }
    }

    public class DeviceFamilyList
    {
        public static string Desktop = "Windows.Desktop";
        public static string Mobile = "Windows.Mobile";
        public static string Team = "Windows.Team";
        public static string IoT = "Windows.IoT";
        public static string Holographic = "Windows.Holographic";
        public static string Xbox = "Windows.Xbox";
        public static string UnknowDevice = "UnknowDevice";
    }
}
