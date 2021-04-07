using System.Collections.Generic;
using TrueLove.Lib.Models.Datebase;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImagesPage : Page
    {
        private List<Comment> Comments;
        public ImagesPage()
        {
            this.InitializeComponent();
            Comments = CommentManager.GetBooks();
        }
    }
}
