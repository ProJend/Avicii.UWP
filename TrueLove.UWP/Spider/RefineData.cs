using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using TrueLove.Lib.Models.Code;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.UWP.Spider
{
    public class RefineData
    {
        public RefineData()
        {

        }
        public List<CommentItem> RefineComment(string src)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(src);
            List<CommentItem> currentList = new List<CommentItem>();
            for (int i = 1; i <= 99; i++)
            {
                string namePath = $"//*[@id=\"commentlist\"]/ul/li[{i}]/p[1]/span";
                string comPath = $"//*[@id=\"commentlist\"]/ul/li[{i}]/p[2]";
                string datePath = $"//*[@id=\"commentlist\"]/ul/li[{i}]/p[3]";

                var nameText = htmlDocument.DocumentNode.SelectSingleNode(namePath).InnerText;
                var comText = htmlDocument.DocumentNode.SelectSingleNode(comPath).InnerText;
                var dateText = htmlDocument.DocumentNode.SelectSingleNode(datePath).InnerText;
                var parsedDate = DateTime.Parse(dateText);

                if (new[] { nameText, comText, dateText } != null)
                {
                    currentList.Add(new CommentItem
                    {
                        name = nameText,
                        comment = comText,
                        date = parsedDate.ToString("d"),
                    });
                }
            }
            return currentList;
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
