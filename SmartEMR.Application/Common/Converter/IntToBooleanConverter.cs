using SmartEMR.Application.Common.Converter.Base;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class IntToBooleanConverter : BaseConverter
{
    public bool Invert { get; set; } = false;

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var intValue = value as int?;
        bool result = intValue.GetValueOrDefault(0) == 0;
        
        if (Invert)
        {
            result = !result;
        }

        return result;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
