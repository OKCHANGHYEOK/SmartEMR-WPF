using SmartEMR.Application.Common.Converter.Base;
using System.Globalization;
using System.Windows;

namespace SmartEMR.Application.Common.Converter;

public class BooleanToVisibilityConverter : BaseConverter
{
    public bool invert { get; set; } = false;

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool bFlag == false) return Visibility.Visible;
        
        if (invert)
        {
            bFlag = !bFlag;
        }

        return bFlag ? Visibility.Visible : Visibility.Collapsed;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
