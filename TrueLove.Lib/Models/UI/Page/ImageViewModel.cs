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
    public class ImageViewModel : ObservableCollection<string>, ISupportIncrementalLoading
    {
        private int _pageNumber = 1;
        private int _countRepeated;
        private bool _isRepeated;

        public async Task<bool> LoadMoreItemsAsync()
        {
            try
            {
                ImageParser imageParser = new();
                imageParser.ForegroundParseImage(++_pageNumber);
                for (int element = 1; element <= 99; element++)
                {
                    var latestItem = await imageParser.Append(element);

                    if (!_isRepeated)
                    {
                        var _isRepeating = false;
                        foreach (var item in this)
                        {
                            if (item == latestItem)
                            {
                                _isRepeating = true;
                                _countRepeated++;
                                if (element == 99 && _countRepeated > 0)
                                    _isRepeated = true;
                                break;
                            }
                        }
                        if (!_isRepeating)
                            Add(latestItem);
                        continue;
                    }

                    Add(latestItem);
                }
            }
            catch (NullReferenceException) { } //爬取溢出           
            return false;
        }

        public void Load9ItemsRandomly()
        {
            List<int> list = [];
            ImageParser imageParser = new();
            imageParser.BackgroundParseImage(++_pageNumber);
            for (int j = 0; j < 9; j++)
            {
                Random random = new();
                int element;
                do
                {
                    element = random.Next(1, 99);
                }
                while (list.Contains(element));
                list.Add(element);

                var latestItem = imageParser.Append(element).Result;
                Add(latestItem);
            }
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) => AsyncInfo.Run(c => LoadMoreItemsAsyncCore(c, count));

        public bool HasMoreItems => false;

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
                ImageParser imageParser = new();
                imageParser.BackgroundParseImage(++_pageNumber);
                var latestItem = await imageParser.Append(1);
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