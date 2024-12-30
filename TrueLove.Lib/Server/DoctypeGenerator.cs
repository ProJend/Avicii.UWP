using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TrueLove.Lib.Models.Enum;
using Windows.Storage;

namespace TrueLove.Lib.Server
{
    public class DoctypeGenerator
    {
        public string GetSourceCode(string path) => File.ReadAllText(path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Use URL address</param>
        public async Task<string> SaveSourceCodeAsync(string path, PageType page)
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
            var file = await localFolder.CreateFileAsync(page + ".html",
                CreationCollisionOption.ReplaceExisting);
            await FileIO.AppendTextAsync(file, sourceCode);
            return sourceCode;
        }
    }
}