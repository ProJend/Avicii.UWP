﻿using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using TrueLove.Lib.Models.Code;
using TrueLove.Lib.Models.Enum;
using Windows.Storage;

namespace TrueLove.Lib.Server
{
    public class CommentParser
    {
        string _src;

        public void BackgroundParseComment(int _pageNumber)
        {
            var doctypeGenerator = new DoctypeGenerator();
            _src = doctypeGenerator.GetSourceCode(ApplicationData.Current.LocalFolder.Path + @"\Comment.html");
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path + @"\Comment.html");
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                _ = doctypeGenerator.SaveSourceCodeAsync($"https://avicii.com/page/{_pageNumber}", PageType.Comment);
        }

        public async void ForegroundParseComment(int _pageNumber)
        {
            var doctypeGenerator = new DoctypeGenerator();
            _src = doctypeGenerator.GetSourceCode(ApplicationData.Current.LocalFolder.Path + @"\Comment.html");
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path + @"\Comment.html");
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                await doctypeGenerator.SaveSourceCodeAsync($"https://avicii.com/page/{_pageNumber}", PageType.Comment);
        }

        public Task<CommentModel> Append(int ID)
        {
            var latestComment = new CommentModel();
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
            if (string.IsNullOrEmpty(nameText))
                nameText = "Anonymity";

            comText = WebUtility.HtmlDecode(comText);
            if (string.IsNullOrEmpty(comText) || string.IsNullOrEmpty(dateText))
                return Task.Run(() => latestComment = null);

            latestComment.Name = nameText;
            latestComment.Comment = comText;
            latestComment.Date = parsedDate.ToString("d");
            return Task.Run(() => latestComment);
        }
    }
}
