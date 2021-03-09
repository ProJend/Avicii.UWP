using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace TrueLove.UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommentsPage : Page
    {
        public static CommentsPage Current;
        public CommentsPage()
        {
            this.InitializeComponent();
            Current = this;
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
    }
}
