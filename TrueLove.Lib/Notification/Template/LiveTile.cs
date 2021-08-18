using Microsoft.Toolkit.Uwp.Notifications;

namespace TrueLove.Lib.Notification.Template
{
    internal class LiveTile
    {
        public static TileContent OfflineTemplate() => new TileContent()
        {   // 创建静态磁贴对象并返回
            Visual = new TileVisual()
            {
                TileMedium = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        TextStacking = TileTextStacking.Center,
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Share your memories of Avicii.",
                                HintAlign = AdaptiveTextAlign.Center,
                                HintWrap = true,
                            },
                        },
                        PeekImage = new TilePeekImage()
                        {
                            Source = "Assets/Wide310x310Logo.png",
                        }
                    }
                },
                TileWide = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Tim Bergling\nFoundation",
                                HintStyle = AdaptiveTextStyle.Body,
                                HintAlign = AdaptiveTextAlign.Right,
                            }
                        },
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = "Assets/Background.jpg"
                        },
                        PeekImage = new TilePeekImage()
                        {
                            Source = "Assets/Wide310x150Logo.png",
                        }
                    }
                },
            }
        };
    }
}
