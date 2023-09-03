using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel;

namespace TrueLove.Lib.Notification.Template
{
    internal class LiveTile
    {
        public static TileContent TitleTemplate(TileBindingContentPhotos photosContent) => new TileContent()
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
                            Source = "Assets/Square150x150Logo.png",
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
                                Text = "Tim Bergling\nMemories",
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
                TileLarge = new TileBinding()
                {
                    //Content = photosContent
                    Content = new TileBindingContentPhotos()
                    {
                        Images =
                        {
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\1.jpg"
                            },
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\2.jpg"
                            },
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\3.jpg"
                            },
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\4.jpg"
                            },
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\5.jpg"
                            },
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\6.jpg"
                            },
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\7.jpg"
                            },
                            new TileBasicImage
                            {
                                Source = Package.Current.InstalledLocation.Path + @"\Assets\Instagram\8.jpg"
                            },
                        }
                    }
                }
            }
        };
    }
}
