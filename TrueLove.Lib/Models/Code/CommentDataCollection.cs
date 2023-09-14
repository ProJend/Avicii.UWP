using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using TrueLove.Lib.Spider;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;

namespace TrueLove.Lib.Models.Code
{
    public class CommentDataType
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }

    }

    public class CommentDataCollection : ObservableCollection<CommentDataType>, ISupportIncrementalLoading
    {
        public ObservableCollection<CommentDataType> CommentDataColl;

        public CommentDataCollection()
        {
            //_pageNumber = 1;
            //_loadingID = 1;
            //AsyncCommentDataCollection();
        }
        private async void AsyncCommentDataCollection()
        {
            var reviewWeb = new ReviewWeb();
            //_src = await reviewWeb.GetSourceCode($"https://avicii.com/page/{_pageNumber}", false);
            _src = await reviewWeb.GetSourceCodeAsync(Package.Current.InstalledPath + "/TrueLove.Lib/Spider/Sample/CommentData.txt");
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) => AsyncInfo.Run(c => LoadMoreItemsAsyncCore(c, count));


        public bool HasMoreItems => false;

        async Task<LoadMoreItemsResult> LoadMoreItemsAsyncCore(CancellationToken cancel, uint count)
        {
            LoadMoreItemsResult res = new LoadMoreItemsResult();
            // 开始加载
            LoadMoreStarted?.Invoke(this, EventArgs.Empty);
            // 如果操作已处于取消状态，则不再加载项
            if (cancel.IsCancellationRequested)
            {
                res.Count = 0;
            }
            else
            {
                // 向集合中添加指定项
                for (uint n = 0; n < count; n++)
                {
                    try
                    {
                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(_src);
                        string namePath = $"//*[@id=\"commentlist\"]/ul/li[{_loadingID}]/p[1]/span";
                        string comPath = $"//*[@id=\"commentlist\"]/ul/li[{_loadingID}]/p[2]";
                        string datePath = $"//*[@id=\"commentlist\"]/ul/li[{_loadingID}]/p[3]";

                        var nameText = htmlDocument.DocumentNode.SelectSingleNode(namePath).InnerText;
                        var comText = htmlDocument.DocumentNode.SelectSingleNode(comPath).InnerText;
                        var dateText = htmlDocument.DocumentNode.SelectSingleNode(datePath).InnerText;
                        var parsedDate = DateTime.Parse(dateText);

                        if (new[] { nameText, comText, dateText } != null)
                        {
                            Add(new CommentDataType
                            {
                                Name = nameText,
                                Comment = comText,
                                Date = parsedDate.ToString("d"),
                            });
                        }
                    }
                    catch (Exception)
                    {
                        _pageNumber++;
                        _loadingID = 1;
                    }
                }
            }
            // 完成加载
            LoadMoreEnd?.Invoke(this, EventArgs.Empty);
            return res;
        }

        /// <summary>
        /// 该事件在开始加载时发生。
        /// </summary>
        public event EventHandler LoadMoreStarted;
        /// <summary>
        /// 该事件在加载完成后发生。
        /// </summary>
        public event EventHandler LoadMoreEnd;

        string _src;
        int _pageNumber;
        int _loadingID;
    }
}
