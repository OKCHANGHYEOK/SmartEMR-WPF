using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using System.Windows.Media;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class RESStatusToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var RES_Status = value?.ToString();
        if (string.IsNullOrWhiteSpace(RES_Status))
        {
            return Brushes.Transparent;
        }

        Brush? brush = RES_Status switch
        {
            "HLD" => SmartBrush.SMART_BRUSH_STATUS_PENDING,
            "CNF" => SmartBrush.SMART_BRUSH_STATUS_CONFIRMED,
            "CNL" => SmartBrush.SMART_BRUSH_STATUS_CANCEL,
            _ => Brushes.Transparent
        };

        return brush;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
