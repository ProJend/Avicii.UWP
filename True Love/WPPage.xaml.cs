using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace True_Love
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WPPage : Page
    {
        //滚动条位置变量
        double scrlocation = 0;
        //导航栏当前显示状态（这个是为了减少不必要的开销，因为我做的是动画隐藏显示效果如果不用一个变量来记录当前导航栏状态的会重复执行隐藏或显示）
        bool isshowbmbar = true;

        public WPPage()
        {
            this.InitializeComponent();
            //ContentFrame.Navigate(typeof(LovePage));
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// 下滑隐藏导航栏 https://www.cnblogs.com/lonelyxmas/p/9919869.html
        /// </summary>
        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.VerticalOffset != scrlocation)
            {
                if (sv.VerticalOffset > scrlocation)
                {
                    //滚动条当前位置大于存储的变量值时代表往下滑，隐藏底部栏
                    //隐藏
                    if (isshowbmbar)
                    {
                        //这里为了简洁易懂就不做动画隐藏效果，直接用透明度进行隐藏。
                        bar.Opacity = 0;
                        isshowbmbar = false;                         
                    }
                }
                else
                {
                    //显示
                    if (isshowbmbar == false)
                    {
                        bar.Opacity = 1;
                        isshowbmbar = true;                        
                    }
                }
            }
            scrlocation = sv.VerticalOffset;
        }

        private void SET_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
    }
}
