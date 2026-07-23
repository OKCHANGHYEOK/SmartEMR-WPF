using SmartEMR.Application.Common.Converter.Base;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class IntToBooleanConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue == false) return false;

        return intValue == 0;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
