using System;
using System.IO;
using System.Net.Http;

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
        public void ReadHTML(string path, bool isLoadedDown = true)
        {
            if (isLoadedDown)
            {
                strHTML = File.ReadAllText(path);
            }
            else
            {
                var httpClient = new HttpClient();
                strHTML = httpClient.GetStringAsync(new Uri(path)).Result;
            }
        }
        private string strHTML;
        internal string StrHTML { get => strHTML; }
    }
}
