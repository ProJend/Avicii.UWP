using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using TrueLove.Lib.Models.Code;
using TrueLove.Lib.Notification;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.UWP.Spider
{
    public class RefineData
    {
        public ObservableCollection<CommentItem> UpdateComment(string src)
        {
            var currentList = new ObservableCollection<CommentItem>();
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(src);
                for (int i = 1; i <= 96; i++)
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
            catch (Exception)
            {
                return currentList;
            }
        }
        public ObservableCollection<BitmapImage> UpdateImage(string src)
        {
            var currentList = new ObservableCollection<BitmapImage>();
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(src);

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
            catch
            {
                return currentList;
            }
        }
    }
}
