using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueLove.Lib.Notification;
using Windows.Storage;

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
                    if (string.IsNullOrEmpty(File.ReadAllText(path)))
                    {
                        var _src = await new ReviewWeb().GetSourceCodeAsync($"https://avicii.com/page/12", false);
                        StorageFile file = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.LocalFolder.Path + $"/OfflineData.txt");
                        await FileIO.AppendTextAsync(file, _src);
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
