using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Storage;
using Windows.UI.Notifications;

namespace True_Love.Helpers
{
    /// <summary>
    /// 动态磁贴 参考了 https://blog.csdn.net/qiuxy23/article/details/81252402
    /// </summary>
    public class LiveTileService
    {
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        //添加动态磁贴，title为磁贴的标题，detail为磁贴的内容，source为背景图片
        public void AddTile()
        {
            if ((bool)localSettings.Values["OnlyLiveTiles"])
            {
                //得到磁贴的对象
                TileContent content = CreateTileContent();
                var notification = new TileNotification(content.GetXml());
                //添加到磁贴的队列
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
                localSettings.Values["OnlyLiveTiles"] = false;
            }            
        }

        //创建磁贴对象并返回
        private TileContent CreateTileContent() => new TileContent()
        {
            Visual = new TileVisual()
            {
                //TileSmall = new TileBinding()
                //{
                //    Content = new TileBindingContentAdaptive()
                //    {
                //        TextStacking = TileTextStacking.Center,
                //        Children =
                //        {
                //            new AdaptiveText()
                //            {
                //                Text="◢◤",
                //                HintAlign=AdaptiveTextAlign.Center,
                //                HintStyle=AdaptiveTextStyle.Title
                //            }                                        
                //        }
                //    }
                //},

                TileMedium = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        TextStacking = TileTextStacking.Center,
                        Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Something want to talk to Tim Bergling?",
                                    HintAlign = AdaptiveTextAlign.Center,
                                    HintWrap = true,
                                    //HintStyle=(Style)Resources["AccentButtonStyle"],
                                    //HintStyle=AdaptiveTextStyle.CaptionSubtle,
                                },
                            },
                        PeekImage = new TilePeekImage()
                        {
                            Source = "Assets/Wide310x150Logo.png",
                            //HintOverlay = 20
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
                                    Text = "Tim Bergling",//设置标题
                                    HintStyle = AdaptiveTextStyle.Base,
                                    HintAlign = AdaptiveTextAlign.Right,
                                },
                                new AdaptiveText()
                                {
                                    Text = "Foundation",//设置内容
                                    HintStyle = AdaptiveTextStyle.Body,
                                    HintAlign = AdaptiveTextAlign.Right
                                }
                            },
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = "Assets/Background/BG1.jpg"
                        },
                        PeekImage = new TilePeekImage()
                        {
                            Source = "Assets/Wide310x150Logo.png",
                            //HintOverlay = 20
                        }
                    }
                },

                //TileLarge = new TileBinding()
                //{
                //    Content = new TileBindingContentAdaptive()
                //    {
                //        TextStacking = TileTextStacking.Center,
                //        Children =
                //        {
                //            new AdaptiveText()
                //            {
                //                Text="◢◤",
                //                HintAlign=AdaptiveTextAlign.Center,
                //                HintStyle=AdaptiveTextStyle.Title
                //            }
                //        }
                //    }
                //},
            }
        };
    }
}
