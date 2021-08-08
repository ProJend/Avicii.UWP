using System;
using TrueLove.Lib.Models.Enum;
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
            return !string.IsNullOrEmpty(resourceLoader)
                ? resourceLoader
                : throw new ArgumentException($"There're blank output on this uid : {UID}.\nTap Ctrl + F to search and find it out in current project.");
        }

        /// <summary>
        /// 识别设备。
        /// </summary>
        /// <param name="deviceOS">Desktop, Mobile, Team, IoT, Holographic or Xbox</param>
        /// <returns></returns>
        public static bool DeviceFamilyMatch(DeviceFamilyType deviceOS)
        {
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            var currentDevice = $"Windows.{deviceOS}";
            switch (deviceOS)
            {   // 确认设备输入正确
                case DeviceFamilyType.Desktop:
                case DeviceFamilyType.Mobile:
                case DeviceFamilyType.Team:
                case DeviceFamilyType.IoT:
                case DeviceFamilyType.Holographic:
                case DeviceFamilyType.Xbox:
                    return deviceFamily == currentDevice;
                default:
                    return false;
            }
        }
    }
}
