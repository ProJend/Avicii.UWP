using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueLove.Lib.Notification;
using Windows.Storage;

namespace TrueLove.Lib.Spider
{
    public class DoctypeGenerator
    {
        public string GetSourceCode(string path)
        {
            if (File.Exists(path))
            {
                sourceCode = File.ReadAllText(path);
                //TimeToNow();
            }
            return sourceCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        public async Task<string> GetSourceCodeAsync(string path, string page)
        {
            try
            {
                if (page == "comment")
                {
                    using var httpClient = new HttpClient();
                    sourceCode = await httpClient.GetStringAsync(new Uri(path));

                    var localFolder = ApplicationData.Current.LocalFolder;
                    var file = await localFolder.CreateFileAsync("Comment.html",
                        CreationCollisionOption.ReplaceExisting);
                    await FileIO.AppendTextAsync(file, sourceCode);
                }
                if (page == "image")
                {
                    using var httpClient = new HttpClient();
                    sourceCode = await httpClient.GetStringAsync(new Uri(path));

                    if (path.Contains("1"))
                    {
                        var localFolder = ApplicationData.Current.LocalFolder;
                        var file = await localFolder.CreateFileAsync("Image.html",
                            CreationCollisionOption.ReplaceExisting);
                        await FileIO.AppendTextAsync(file, sourceCode);
                    }
                }
            }
            catch (HttpRequestException)
            {
                Assembly.Toast();
            }
            return sourceCode;
        }

        public void TimeToNow()
        {
            var pattern = @"\d+-\d+-\d+";
            var now = DateTime.Now;
            var input = now.ToString("yyyy-MM-dd");
            Regex regex = new Regex(pattern);
            sourceCode = regex.Replace(sourceCode, input);
        }

        internal string sourceCode;
    }
}
