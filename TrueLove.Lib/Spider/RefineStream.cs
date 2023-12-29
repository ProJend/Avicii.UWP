using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TrueLove.Lib.Models.Code;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.Lib.Spider
{


    public class RefineStream
    {
        public Task<CommentItem> RefineComment(string src, int times)
        {
            var singleComment = new CommentItem();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(src);
            string namePath = $"//*[@id=\"comments\"]/ul[2]/li[{times}]/div/strong";
            string comPath = $"//*[@id=\"comments\"]/ul[2]/li[{times}]/div/p";
            string datePath = $"//*[@id=\"comments\"]/ul[2]/li[{times}]/div/time";

            var nameText = htmlDocument.DocumentNode.SelectSingleNode(namePath).InnerText;
            nameText = nameText.Substring(5);
            var comText = htmlDocument.DocumentNode.SelectSingleNode(comPath).InnerText;
            var dateText = htmlDocument.DocumentNode.SelectSingleNode(datePath).InnerText;
            var parsedDate = DateTime.Parse(dateText);

            if (new[] { nameText, comText, dateText } != null)
            {
                singleComment.Name = nameText;
                singleComment.Comment = comText;
                singleComment.Date = parsedDate.ToString("d");
            }
            return Task.Run(() => singleComment);
        }

        public void RefineImage(string src, ObservableCollection<BitmapImage> currentList)
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
                        var path = imageNode.Attributes["src"].Value;
                        var uri = new Uri(path);
                        currentList.Add(new BitmapImage(uri));
                    }
                }
            }
            catch { }
        }
    }
}
