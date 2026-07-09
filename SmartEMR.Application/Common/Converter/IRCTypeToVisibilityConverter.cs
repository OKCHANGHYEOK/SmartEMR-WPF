using System.Globalization;
using System.Windows;

namespace SmartEMR.Application.Common.Converter;

public class IRCTypeToVisibilityConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var IRC_Type = value?.ToString();
        if (string.IsNullOrWhiteSpace(IRC_Type)) return Visibility.Visible!;

        return IRC_Type == "NON" ? Visibility.Visible : Visibility.Collapsed;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
