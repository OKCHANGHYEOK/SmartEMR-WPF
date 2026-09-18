using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using System.Drawing;
using System.Globalization;

namespace SmartEMR.Application.Common.Converter;

public class PAYIMethodToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string PAYI_Method) return Brushes.Transparent;

        return PAYI_Method switch
        {
            "CAS" => SmartBrush.SMART_BRUSH_PAY_METHOD_CASH,
            "CRD" => SmartBrush.SMART_BRUSH_PAY_METHOD_CARD,
            "NAV" => SmartBrush.SMART_BRUSH_PAY_METHOD_NAVERPAY,
            _ => Brushes.Transparent
        };
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
