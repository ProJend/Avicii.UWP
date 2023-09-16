using System;
using Windows.UI.Xaml.Data;

namespace TrueLove.Lib.Models.UI.Converter
{
    public class InverseOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (double)value == 1 ? 0 : 1;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
