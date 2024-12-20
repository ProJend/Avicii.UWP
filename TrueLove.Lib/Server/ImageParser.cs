﻿using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Connectivity;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace TrueLove.Lib.Server
{
    public class ImageParser
    {
        string _src;

        public void BackgroundParseImage(int _pageNumber)
        {
            var doctypeGenerator = new DoctypeGenerator();
            _src = doctypeGenerator.GetSourceCode(ApplicationData.Current.LocalFolder.Path + @"\Image.html");
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path + @"\Image.html");
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                _ = doctypeGenerator.SaveSourceCodeAsync($"https://avicii.com/images/page/{_pageNumber}", "image");
        }

        public async void ForegroundParseImage(int _pageNumber)
        {
            var doctypeGenerator = new DoctypeGenerator();
            _src = doctypeGenerator.GetSourceCode(ApplicationData.Current.LocalFolder.Path + @"\Image.html");
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path + @"\Image.html");
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                await doctypeGenerator.SaveSourceCodeAsync($"https://avicii.com/images/page/{_pageNumber}", "image");
        }

        public Task<string> Append(int ID)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_src);

            string imagePath = $"//*[@id=\"images\"]/ul[2]/li[{ID}]/img";

            var imageNode = htmlDocument.DocumentNode.SelectSingleNode(imagePath);
            string uri = null;
            if (imageNode != null)
            {
                uri = imageNode.Attributes["src"].Value;
            }

            return Task.Run(() => uri);
        }
    }
}
