using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using System.Drawing;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class PAYITypeToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string PAYI_Type) return Brushes.Transparent;

        return PAYI_Type switch
        {
            "PAY" => SmartBrush.SMART_BRUSH_PAY_TYPE_PAY,
            "CUT" => SmartBrush.SMART_BRUSH_PAY_TYPE_CUT,
            "DIS" => SmartBrush.SMART_BRUSH_PAY_TYPE_DIS,
            "REF" => SmartBrush.SMART_BRUSH_PAY_TYPE_REF,
            _ => Brushes.Transparent
        };
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
