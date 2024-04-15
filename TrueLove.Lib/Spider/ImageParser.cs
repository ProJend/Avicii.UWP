using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.Lib.Spider
{
    internal class ImageParser
    {
        string _src;

        public ImageParser(int pageNumber) => ParseImageAsync(pageNumber);

        private async void ParseImageAsync(int _pageNumber)
        {
            var doctypeGenerator = new DoctypeGenerator();
            _src = doctypeGenerator.GetSourceCode(ApplicationData.Current.LocalFolder.Path + @"\Image.html");
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path + @"\Image.html");
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                _src = await doctypeGenerator.GetSourceCodeAsync($"https://avicii.com/images/page/{_pageNumber}", "image");
        }

        public Task<BitmapImage> Append(int ID)
        {
            var singleImage = new BitmapImage();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_src);

            string imagePath = $"//*[@id=\"images\"]/ul[2]/li[{ID}]/img";

            var imageNode = htmlDocument.DocumentNode.SelectSingleNode(imagePath);
            if (imageNode != null)
            {
                var path = imageNode.Attributes["src"].Value;
                var uri = new Uri(path);
                singleImage.UriSource = uri;
            }

            return Task.Run(() => singleImage);
        }
    }
}
