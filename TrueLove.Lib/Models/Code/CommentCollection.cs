using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using TrueLove.Lib.Spider;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace TrueLove.Lib.Models.Code
{
    public class CommentItem
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
    }

    public class CommentCollection : ObservableCollection<CommentItem>, ISupportIncrementalLoading
    {
        public ObservableCollection<CommentItem> CommentColl;

        public async void LoadMoreItemsManuallyAsync(bool isRefreshing = false)
        {
            if (isRefreshing)
            {
                _pageNumber = -1;
            }
            var reviewStream = new ReviewStream();
            string _src = "null";
            if (_pageNumber++ == -1)
            {
                _src = await reviewStream.GetStreamAsync(ApplicationData.Current.LocalFolder.Path + $"/OfflineData.txt", true);
            }
            else if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                _src = await reviewStream.GetStreamAsync($"https://avicii.com/page/{_pageNumber++}");
            }
            var refineStream = new RefineStream();
            try
            {
                for (int i = 1; i <= 95; i++)
                {
                    var singleItme = await refineStream.RefineComment(_src, i);
                    Add(singleItme);
                }
            }
            catch (Exception) { };
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
                    var reviewStream = new ReviewStream();
                    string _src = "null";
                    if (_pageNumber == 0)
                    {
                        _src = await reviewStream.GetStreamAsync(ApplicationData.Current.LocalFolder.Path + $"/OfflineData.txt");
                    }
                    else if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                    {
                        _src = await reviewStream.GetStreamAsync($"https://avicii.com/page/{_pageNumber}");
                    }
                    var refineStream = new RefineStream();
                    try
                    {
                        var singleItme = await refineStream.RefineComment(_src, _times++);
                        Add(singleItme);
                    }
                    catch (Exception)
                    {
                        _pageNumber++;
                        _times = 0;
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

        int _pageNumber = -1;
        int _times = 0;
    }
}
