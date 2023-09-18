using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
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
                    // 延迟等待数据异步加载完毕
                    if (string.IsNullOrEmpty(File.ReadAllText(path)))
                    {
                        await Task.Delay(3000);
                    }
                    SourceCode = File.ReadAllText(path);
                    TimeToNow();
                }
                else
                {
                    var httpClient = new HttpClient();
                    SourceCode = await httpClient.GetStringAsync(new Uri(path));
                }
            }
            catch (Exception exceptiion) when (exceptiion is AggregateException || exceptiion is HttpRequestException)
            {
                Assembly.Toast();
            }
            return SourceCode;
        }

        public void TimeToNow()
        {
            var pattern = @"\d+-\d+-\d+";
            var now = DateTime.Now;
            var input = now.ToString("yyyy-MM-dd");
            Regex regex = new Regex(pattern);
            SourceCode = regex.Replace(SourceCode, input);
        }

        internal string SourceCode;
    }
}
