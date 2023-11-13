using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using TrueLove.Lib.Models.Code;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.Lib.Spider
{
    public class RefineData
    {
        public void UpdateComment(string src, ObservableCollection<CommentDataType> currentList)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(src);
                for (int i = 1; i <= 99; i++)
                {
                    string namePath = $"//*[@id=\"comments\"]/ul[2]/li[{i}]/div/strong";
                    string comPath = $"//*[@id=\"comments\"]/ul[2]/li[{i}]/div/p";
                    string datePath = $"//*[@id=\"comments\"]/ul[2]/li[{i}]/div/time";

                    var nameText = htmlDocument.DocumentNode.SelectSingleNode(namePath).InnerText;
                    nameText = nameText.Substring(5);
                    var comText = htmlDocument.DocumentNode.SelectSingleNode(comPath).InnerText;
                    var dateText = htmlDocument.DocumentNode.SelectSingleNode(datePath).InnerText;
                    var parsedDate = DateTime.Parse(dateText);
                    if (new[] { nameText, comText, dateText } != null)
                    {
                        currentList.Add(new CommentDataType
                        {
                            Name = nameText,
                            Comment = comText,
                            Date = parsedDate.ToString("d"),
                        });
                    }
                }
            }
            catch { }
        }

        public void UpdateImage(string src, ObservableCollection<BitmapImage> currentList)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(src);
                for (int i = 1; i <= 99; i++)
                {
                    string imagePath = $"//*[@id=\"images\"]/ul[2]/li[{i}]/img";

                    var imageNode = htmlDocument.DocumentNode.SelectSingleNode(imagePath);
                    if (imageNode != null)
                    {
                        currentList.Add(new BitmapImage(new Uri("https://avicii.com" + imageNode.Attributes["src"].Value)));
                    }
                }
            }
            catch { }
        }
    }
}
