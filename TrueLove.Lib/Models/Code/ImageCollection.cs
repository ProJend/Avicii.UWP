﻿using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using TrueLove.Lib.Spider;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace TrueLove.Lib.Models.Code
{
    public class ImageCollection : ObservableCollection<BitmapImage>, ISupportIncrementalLoading
    {
        private int _pageNumber = 1;
        private int _countRepeated;
        private bool _isRepeated;

        public async void LoadMoreItemsManually()
        {
            var imageParser = new ImageParser(_pageNumber++);
            for (int element = 1; element <= 50; element++)
            {
                var latestItem = await imageParser.Append(element);
                Add(latestItem);
            }
        }

        public async Task<bool> LoadMoreItemsManuallyAsync()
        {
            try
            {
                var imageParser = new ImageParser(_pageNumber++);
                for (int element = 1; element <= 99; element++)
                {
                    var latestItem = await imageParser.Append(element);

                    if (!_isRepeated)
                    {
                        var _isRepeating = false;
                        foreach (var item in this)
                        {
                            if (item.UriSource.AbsoluteUri == latestItem.UriSource.AbsoluteUri)
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
                var imageParser = new ImageParser(_pageNumber++);
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