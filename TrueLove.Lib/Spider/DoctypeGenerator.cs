using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace TrueLove.Lib.Spider
{
    public class DoctypeGenerator
    {
        public string GetSourceCode(string path)
        {
            sourceCode = File.ReadAllText(path);
            //TimeToNow();
            return sourceCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        public async Task<string> SaveSourceCodeAsync(string path, string page)
        {
            using var httpClient = new HttpClient();
            try
            {
                sourceCode = await httpClient.GetStringAsync(new Uri(path));
            }
            catch (HttpRequestException)
            {
                sourceCode = await httpClient.GetStringAsync(new Uri(path));
            }
            var localFolder = ApplicationData.Current.LocalFolder;
            if (page == "comment")
            {
                var file = await localFolder.CreateFileAsync("Comment.html",
                    CreationCollisionOption.ReplaceExisting);
                await FileIO.AppendTextAsync(file, sourceCode);
            }
            if (page == "image")
            {
                var file = await localFolder.CreateFileAsync("Image.html",
                    CreationCollisionOption.ReplaceExisting);
                await FileIO.AppendTextAsync(file, sourceCode);
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
