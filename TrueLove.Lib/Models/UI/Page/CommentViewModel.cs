using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using TrueLove.Lib.Server;
using Windows.UI.Xaml.Data;

namespace TrueLove.Lib.Models.Code.Page
{
    public class CommentViewModel : ObservableCollection<CommentModel>, ISupportIncrementalLoading
    {
        int _pageNumber = 1;
        private int _countRepeated;
        private bool _isRepeated;

        public async Task<bool> LoadMoreItemsAsync()
        {
            try
            {
                CommentParser commentParser = new();
                commentParser.ForegroundParseComment(++_pageNumber);
                for (int element = 1; element <= 99; element++)
                {
                    var latestItem = await commentParser.Append(element);

                    if (!_isRepeated)
                    {
                        var _isRepeating = false;
                        foreach (var item in this)
                        {
                            if (item.Comment == latestItem.Comment &&
                                item.Name == latestItem.Name)
                            {
                                _isRepeating = true;
                                _countRepeated++;
                                if (element == 99 && _countRepeated > 0)
                                    _isRepeated = true;
                                break;
                            }
                        }
                        if (!_isRepeating && latestItem != null)
                            Add(latestItem);
                        continue;
                    }

                    if (latestItem != null)
                        Add(latestItem);
                }
            }
            catch (NullReferenceException) { } //爬取溢出
            return false;
        }

        public void Load5ItemsRandomly()
        {
            List<int> list = [];
            CommentParser commentParser = new();
            commentParser.BackgroundParseComment(++_pageNumber);
            for (int j = 1; j <= 5; j++)
            {
                Random random = new();
                int element;
                do
                {
                    element = random.Next(1, 99);
                }
                while (list.Contains(element));
                list.Add(element);

                var latestItem = commentParser.Append(element).Result;
                if (latestItem != null)
                    Add(latestItem);
            }
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
                CommentParser commentParser = new();
                commentParser.BackgroundParseComment(++_pageNumber);
                var latestItem = await commentParser.Append(1);
                if (latestItem != null)
                    Add(latestItem);
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
    }
}
