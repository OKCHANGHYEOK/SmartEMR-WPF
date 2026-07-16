using System.Globalization;
using System.Windows.Media;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;

namespace SmartEMR.Application.Common.Converter;

public class RCBVisitTypeToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return Brushes.Transparent;

        var RCB_VisitType = value.ToString();
        if (string.IsNullOrWhiteSpace(RCB_VisitType)) return Brushes.Transparent;

        return RCB_VisitType == "FIR" ? SmartBrush.SMART_BRUSH_VISIT_FIR : SmartBrush.SMART_BRUSH_VISIT_REP;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}