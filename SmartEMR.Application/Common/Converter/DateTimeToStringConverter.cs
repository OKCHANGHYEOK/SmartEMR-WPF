using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class DateTimeToStringConverter : BaseConverter
{
    public string? ValueFormat { get; set; }
    public string? DisplayFormat { get; set; }

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var strValue = value?.ToString();
        if (string.IsNullOrWhiteSpace(strValue)) return "";

        if (DateTime.TryParse(strValue, out var dt) == false) return "";

        return dt.ToString(DisplayFormat);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(ValueFormat) || value is DateTime dt == false) return "";

        return dt.ToString(ValueFormat);
    }
}
