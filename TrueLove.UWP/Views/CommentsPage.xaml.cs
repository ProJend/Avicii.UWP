using System.Collections.Generic;
using TrueLove.Lib.Models.Code;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommentsPage : Page
    {
        private List<CommentItem> Comments;
        public CommentsPage()
        {
            this.InitializeComponent();
            Comments = CommentManager.GetComment();
        }
    }
}
