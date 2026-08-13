using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using System.Windows.Media;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class CSTStatusToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var CST_Status = value?.ToString();
        if (string.IsNullOrWhiteSpace(CST_Status))
        {
            return Brushes.Transparent;
        }

        Brush? brush = CST_Status switch
        {
            "RDY" => SmartBrush.SMART_BRUSH_STATUS_WAIT,
            "ING" => SmartBrush.SMART_BRUSH_STATUS_PROGRESS,
            "PND" => SmartBrush.SMART_BRUSH_STATUS_PENDING,
            "END" => SmartBrush.SMART_BRUSH_STATUS_COMPLETE,
            _ => Brushes.Transparent
        };

        return brush;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
