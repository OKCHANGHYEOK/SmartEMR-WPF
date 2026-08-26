using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using System.Windows.Media;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class OrderTypeToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string orderType) return Brushes.Transparent;

        Brush brush = orderType switch
        {
            "PRC" => SmartBrush.SMART_BRUSH_ORD_PRC,
            "TRT" => SmartBrush.SMART_BRUSH_ORD_TRT,
            "EXM" => SmartBrush.SMART_BRUSH_ORD_EXM,
            "DOC" => SmartBrush.SMART_BRUSH_ORD_DOC,
            "MED" => SmartBrush.SMART_BRUSH_ORD_MED,
            "ETC" => SmartBrush.SMART_BRUSH_ORD_ETC,
            _ => Brushes.Transparent
        };

        return brush;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
