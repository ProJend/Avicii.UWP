using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TrueLove.Lib.Models.Code;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.Lib.Spider
{
    public class RefineStream
    {
        string _src;


        public RefineStream(int pageNumber) => RefineStreamAsync(pageNumber);

        private async void RefineStreamAsync(int _pageNumber)
        {
            var reviewStream = new ReviewStream();
            _src = reviewStream.GetStream(ApplicationData.Current.LocalFolder.Path + @"/OfflineData.txt");
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path + @"/OfflineData.txt");
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                await reviewStream.GetStreamAsync($"https://avicii.com/page/{_pageNumber++}");
        }

        public Task<CommentItem> RefineComment(int ID)
        {
            var singleComment = new CommentItem();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_src);

            string namePath = $"//*[@id=\"comments\"]/ul[2]/li[{ID}]/div/strong";
            string comPath = $"//*[@id=\"comments\"]/ul[2]/li[{ID}]/div/p";
            string datePath = $"//*[@id=\"comments\"]/ul[2]/li[{ID}]/div/time";

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

        public Task<BitmapImage> RefineImage(string src, int ID)
        {
            var singleImage = new BitmapImage();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(src);
            string imagePath = $"//*[@id=\"images\"]/ul[2]/li[{ID}]/img";

            var imageNode = htmlDocument.DocumentNode.SelectSingleNode(imagePath);
            if (imageNode != null)
            {
                var path = imageNode.Attributes["src"].Value;
                var uri = new Uri(path);

            }
            else
            {

            }

            return Task.Run(() => singleImage);
        }
    }
}
