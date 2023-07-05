using Microsoft.Toolkit.Uwp.Notifications;

namespace TrueLove.Lib.Notification.Template
{
    class Toast
    {
        public static ToastContent CheckNetwork() => new ToastContentBuilder()
            .AddArgument("conversationId", 98143)
            .AddText("Time Out")
            .AddText("There's no network available.")
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
