using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace True_Love
{
    /// <summary>
    /// 动态磁贴 参考了 https://blog.csdn.net/qiuxy23/article/details/81252402
    /// </summary>
    public class LiveTileService
    {
        //添加动态磁贴，title为磁贴的标题，detail为磁贴的内容，source为背景图片
        public void AddTile(string title, string detail, string source)
        {
            //得到磁贴的对象
            TileContent content = CreateTileContent(title, detail, source);
            var notification = new TileNotification(content.GetXml());
            //添加到磁贴的队列
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        //创建磁贴对象并返回
        private TileContent CreateTileContent(string title, string detail, string source)
        {
            return new TileContent()
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
                                    Text="有什么话想要对 Tim Bergling 说？",
                                    HintAlign=AdaptiveTextAlign.Center,
                                    HintWrap = true,
                                    
                                    //HintStyle=AdaptiveTextStyle.Body
                                }
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
                                HintAlign = AdaptiveTextAlign.Right ,
                            },
                            new AdaptiveText()
                            {
                                Text = "Memories",//设置内容
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

        internal void AddTile(object text1, object text2, string source)
        {
            throw new NotImplementedException();
        }
    }
}
