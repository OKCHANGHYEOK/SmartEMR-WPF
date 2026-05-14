using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SmartEMR.Application.Common.Converter
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public bool invert = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = !invert && (value is int intValue && intValue > 0) ? Visibility.Visible : Visibility.Collapsed;

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
