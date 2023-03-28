using System;
using System.IO;
using System.Net.Http;
using Windows.Storage.Search;
using Windows.UI.Xaml.Controls;

namespace TrueLove.UWP.Spider
{
    public class URLRefining
    {
        public URLRefining()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        /// <param name="isLoadedDown">switch to use website address or local document address</param>
        public async void ReadHTML(string path, bool isLoadedDown = true)
        {
            if (isLoadedDown)
            {
                strHTML = File.ReadAllText(path);
            }
            else
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.ConnectionClose = true;
                strHTML = await httpClient.GetStringAsync(new Uri(path));
            }
        }

        void RefineDate()
        {

        }

        private string strHTML;
        internal string StrHTML { get => strHTML; }
    }
}
