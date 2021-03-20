    using Microsoft.Toolkit.Uwp.Notifications;

namespace TrueLove.Lib.Notification.LiveTile
{
    class TileTemplate
    {
        public static TileContent StaticTemplate() => new TileContent()
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
                                Text = "Something want to talk with Tim Bergling?",
                                HintAlign = AdaptiveTextAlign.Center,
                                HintWrap = true,
                            },
                        },
                        PeekImage = new TilePeekImage()
                        {
                            Source = "Assets/Wide310x150Logo.png",
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
                                Text = "Tim Bergling",
                                HintStyle = AdaptiveTextStyle.Body,
                                HintAlign = AdaptiveTextAlign.Right,
                            },
                            new AdaptiveText()
                            {
                                Text = "Foundation",
                                HintStyle = AdaptiveTextStyle.Body,
                                HintAlign = AdaptiveTextAlign.Right
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
