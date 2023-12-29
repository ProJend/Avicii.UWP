using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueLove.Lib.Notification;
using Windows.Storage;

namespace TrueLove.Lib.Spider
{
    public class ReviewStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        /// <param name="isLocalFile">switch to use website address or local document address</param>
        public async Task<string> GetStreamAsync(string path, bool isLocalFile = false)
        {
            try
            {
                if (isLocalFile)
                {
                    stream = File.ReadAllText(path);
                    TimeToNow();
                }
                else
                {
                    using (var httpClient = new HttpClient())
                    {
                        stream = await httpClient.GetStringAsync(new Uri(path));

                        var localFolder = ApplicationData.Current.LocalFolder;
                        var file = await localFolder.CreateFileAsync("OfflineData.txt",
                            CreationCollisionOption.ReplaceExisting);
                        await FileIO.AppendTextAsync(file, stream);
                    }
                }
            }
            catch (HttpRequestException)
            {
                Assembly.Toast();
            }
            return stream;
        }

        public void TimeToNow()
        {
            var pattern = @"\d+-\d+-\d+";
            var now = DateTime.Now;
            var input = now.ToString("yyyy-MM-dd");
            Regex regex = new Regex(pattern);
            stream = regex.Replace(stream, input);
        }

        internal string stream;
    }
}
