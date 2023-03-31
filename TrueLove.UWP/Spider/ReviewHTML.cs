using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using TrueLove.Lib.Notification;

namespace TrueLove.UWP.Spider
{
    public class ReviewHTML
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        /// <param name="isLoadedDown">switch to use website address or local document address</param>
        public ReviewHTML(string path, [Optional] bool type, bool isLoadedDown = true)
        {
            try
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
            catch (AggregateException)
            {
                Assembly.Toast();
                isNetkWorkAvilable = false;
            }
        }
        private string strHTML;
        internal string StrHTML => strHTML;
        bool isNetkWorkAvilable;
    }
}
