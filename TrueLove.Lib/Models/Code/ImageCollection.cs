using Microsoft.Toolkit.Uwp.Connectivity;
using System;
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
        int _pageNumber = 1;

        public async Task<bool> LoadMoreItemsManuallyAsync()
        {
            try
            {
                var imageParser = new ImageParser(_pageNumber++);
                for (int element = 1; element <= 100; element++)
                {
                    var singleItme = await imageParser.Append(element);
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
                var imageParser = new ImageParser(_pageNumber++);
                for (int element = 1; element <= 100; element++)
                {
                    var singleItme = await imageParser.Append(element);
                    Add(singleItme);
                }
            }
            catch { };
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
                var singleItme = await imageParser.Append(1);
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
    }
}
