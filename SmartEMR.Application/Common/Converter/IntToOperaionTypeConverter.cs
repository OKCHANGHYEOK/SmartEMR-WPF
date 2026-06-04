using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class IntToOperaionTypeConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value == null || (int)value == 0) ? "CREATE" : "UPDATE";
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
