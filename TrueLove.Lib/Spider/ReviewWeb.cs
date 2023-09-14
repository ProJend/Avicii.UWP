﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TrueLove.Lib.Notification;

namespace TrueLove.Lib.Spider
{
    public class ReviewWeb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        /// <param name="isLoadedDown">switch to use website address or local document address</param>
        public async Task<string> GetSourceCodeAsync(string path, bool isLoadedDown = true)
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
                    strHTML = await httpClient.GetStringAsync(new Uri(path));
                }
            }
            catch (Exception exceptiion) when (exceptiion is AggregateException || exceptiion is HttpRequestException)
            {
                Assembly.Toast();
                isNetkWorkAvilable = false;
            }
            return strHTML;
        }
        private string strHTML;
        internal string StrHTML => strHTML;

        public bool isNetkWorkAvilable;
    }
}
