using System.Globalization;
using System.Windows.Media;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;

namespace SmartEMR.Application.Common.Converter;

public class PAYStatusToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var PAY_Status = value?.ToString();
        if (string.IsNullOrWhiteSpace(PAY_Status))
        {
            return Brushes.Transparent;
        }

        Brush brush = PAY_Status switch
        {
            "RDY" => SmartBrush.SMART_BRUSH_STATUS_WAIT,
            "PAR" => SmartBrush.SMART_BRUSH_STATUS_PROGRESS,
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