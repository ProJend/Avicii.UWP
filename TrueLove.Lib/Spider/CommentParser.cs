using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using TrueLove.Lib.Models.Code;
using Windows.Storage;

namespace TrueLove.Lib.Spider
{
    public class CommentParser
    {
        string _src;

        public CommentParser(int pageNumber) => ParseCommentAsync(pageNumber);

        private async void ParseCommentAsync(int _pageNumber)
        {
            var doctypeGenerator = new DoctypeGenerator();
            _src = doctypeGenerator.GetSourceCode(ApplicationData.Current.LocalFolder.Path + @"\Comment.html");
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path + @"\Comment.html");
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                await doctypeGenerator.SaveSourceCodeAsync($"https://avicii.com/page/{_pageNumber}", "comment");
        }

        public Task<CommentItem> Append(int ID)
        {
            var latestComment = new CommentItem();
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

            nameText = WebUtility.HtmlDecode(nameText);
            comText = WebUtility.HtmlDecode(comText);
            if (new[] { nameText, comText, dateText } != null)
            {
                latestComment.Name = nameText;
                latestComment.Comment = comText;
                latestComment.Date = parsedDate.ToString("d");
            }
            return Task.Run(() => latestComment);
        }
    }
}
