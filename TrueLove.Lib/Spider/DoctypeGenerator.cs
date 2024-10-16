using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace TrueLove.Lib.Spider
{
    public class DoctypeGenerator
    {
        public string GetSourceCode(string path) => File.ReadAllText(path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        public async Task<string> SaveSourceCodeAsync(string path, string page)
        {
            string sourceCode = null;
            using var httpClient = new HttpClient();
            do
            {
                try
                {
                    sourceCode = await httpClient.GetStringAsync(new Uri(path));
                }
                catch (HttpRequestException) { }
            }
            while (sourceCode == null);
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
    }
}
