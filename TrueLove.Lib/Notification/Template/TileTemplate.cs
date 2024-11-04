using Microsoft.Toolkit.Uwp.Notifications;
using TrueLove.Lib.Models.Code;
using TrueLove.Lib.Models.Code.Page;

namespace TrueLove.Lib.Notification.Template
{
    public class TileTemplate
    {
        public static TileContent CommentTemplate(CommentModel commentItem) => new()
        {
            // 创建静态磁贴对象并返回
            Visual = new TileVisual()
            {
                Branding = TileBranding.NameAndLogo,
                TileMedium = new TileBinding()
                {
                    DisplayName = commentItem.Name,
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = commentItem.Comment,
                                HintWrap = true,
                            }
                        }
                    }
                },
                TileWide = new TileBinding()
                {
                    DisplayName = "from " + commentItem.Name,
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = commentItem.Comment,
                                HintWrap = true,
                            }
                        }
                    }
                },
            }
        };
        public static TileContent ImageTemplate(ImageViewModel source) => new()
        {
            // 创建静态磁贴对象并返回
            Visual = new TileVisual()
            {
                Branding = TileBranding.NameAndLogo,
                TileLarge = new TileBinding()
                {
                    Content = new TileBindingContentPhotos()
                    {
                        Images =
                        {
                            new TileBasicImage() { Source = source[0] },
                            new TileBasicImage() { Source = source[1] },
                            new TileBasicImage() { Source = source[2] },
                            new TileBasicImage() { Source = source[3] },
                            new TileBasicImage() { Source = source[4] },
                            new TileBasicImage() { Source = source[5] },
                            new TileBasicImage() { Source = source[6] },
                            new TileBasicImage() { Source = source[7] },
                            new TileBasicImage() { Source = source[8] },
 
                            // TODO: Can have 9 images total
                        }
                    }
                }
            }
        };
    }
}