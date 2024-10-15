using Microsoft.Toolkit.Uwp.Notifications;

namespace TrueLove.Lib.Notification.Template
{
    class Toast
    {
        public static ToastContent CheckNetwork() => new ToastContentBuilder()
            .AddArgument("conversationId", 98143)
            .AddText("No Internet Connection")
            .AddText("Loading local data.")
            .AddButton(new ToastButton()
                .SetContent("Settings")
                .AddArgument("action", "settings")
                .SetBackgroundActivation())
            .AddButton(new ToastButton()
                .SetContent("Close")
                .SetDismissActivation())
            .GetToastContent();
    }
}
