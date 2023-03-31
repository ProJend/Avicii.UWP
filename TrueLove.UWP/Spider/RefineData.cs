using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.UWP.Spider
{
    public class RefineData
    {
        public RefineData()
        {

        }

        public List<BitmapImage> RefineImage(string src)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(src);
            List<BitmapImage> currentList = new List<BitmapImage>();
            for (int i = 1; i <= 50; i++)
            {
                string imagePath = $"//*[@id=\"instagramhashtag\"]/ul/li[{i}]/img";

                var imageNode = htmlDocument.DocumentNode.SelectSingleNode(imagePath);
                if (imageNode != null)
                {
                    currentList.Add(new BitmapImage(new Uri("https://avicii.com" + imageNode.Attributes["src"].Value)));
                }
            }
            return currentList;
        }
    }
}
