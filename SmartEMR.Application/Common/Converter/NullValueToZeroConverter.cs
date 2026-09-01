using SmartEMR.Application.Common.Converter.Base;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class NullValueToZeroConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return 0;
        }

        return value ?? default!;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
