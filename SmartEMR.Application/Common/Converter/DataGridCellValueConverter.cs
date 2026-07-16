using SmartEMR.Application.Common.Converter.Base;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class DataGridCellValueConverter : BaseMultiValueConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || !values.Any()) return default!;

        var value = values[0];
        var format = values[1] as string;
        var prefix = values[2] as string;
        var suffix = values[3] as string;

        string text;

        if (!string.IsNullOrEmpty(format))
            text = string.Format("{0:" + format + "}", value);
        else
            text = value?.ToString() ?? "";

        return $"{prefix}{text}{suffix}";
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
