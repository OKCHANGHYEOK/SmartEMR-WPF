using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SmartEMR.Application.Common.Converter;

public class StringAndFocusToVisiblityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        string text = values[0] as string ?? string.Empty;
        bool isFocused = values[1] is bool focused && focused;

        if (string.IsNullOrWhiteSpace(text) && !isFocused)
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
