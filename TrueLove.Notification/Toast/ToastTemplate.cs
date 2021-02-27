using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;

namespace TrueLove.Notification.Toast
{
    class ToastTemplate
    {
        public static ToastContent Network(int conversationId = 384928) => new ToastContentBuilder()
        .AddToastActivationInfo(new QueryString()
        {
            { "action", "checkNetwork" },
            { "conversationId", conversationId.ToString() }
        }.ToString(), ToastActivationType.Foreground)
        .AddText("Time Out")
        .AddText("There's no network available.")
        .AddButton("Settings", ToastActivationType.Foreground, new QueryString()
        {
            { "action", "Settings" },
            { "conversationId", conversationId.ToString() }
        }.ToString())
        .AddButton("Close", ToastActivationType.Foreground, new QueryString()
        {
            { "action", "Close" },
            { "conversationId", conversationId.ToString() }
        }.ToString())
        .GetToastContent();
    }
}
