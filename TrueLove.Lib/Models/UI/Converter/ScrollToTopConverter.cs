using System;
using Windows.UI.Xaml.Data;

namespace TrueLove.Lib.Models.UI.Converter
{
    public class ScrollToTopConverter : IValueConverter
    {   // 大于0表示有滑动过
        public object Convert(object value, Type targetType, object parameter, string language) => (double)value > 0; 

        public object ConvertBack(object value, Type targetType, object parameter, string language)=> throw new NotImplementedException();
    }
}
