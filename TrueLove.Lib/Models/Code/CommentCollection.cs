using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using TrueLove.Lib.Spider;
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
        int _pageNum = 1;

        public async Task<bool> LoadMoreItemsManuallyAsync()
        {
            try
            {
                var refineStream = new RefineStream(_pageNum++);
                for (int i = 1; i <= 99; i++)
                {
                    var singleItme = await refineStream.RefineComment(i);
                    Add(singleItme);
                }
            }
            catch { };
            return false;
        }

        public async void LoadMoreItemsManually()
        {
            try
            {
                var refineStream = new RefineStream(_pageNum++);
                for (int i = 1; i <= 99; i++)
                {
                    var singleItme = await refineStream.RefineComment(i);
                    Add(singleItme);
                }
            }
            catch { };
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) => AsyncInfo.Run(c => LoadMoreItemsAsyncCore(c, count));

        public bool HasMoreItems => Count < 100;

        async Task<LoadMoreItemsResult> LoadMoreItemsAsyncCore(CancellationToken cancel, uint count)
        {
            var res = new LoadMoreItemsResult();
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
                var refineStream = new RefineStream(_pageNumber++);
                var singleItme = await refineStream.RefineComment(1);
                Add(singleItme);
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

        int _pageNumber = 0;
    }
}
