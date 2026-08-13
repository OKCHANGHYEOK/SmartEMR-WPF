using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using System.Globalization;
using System.Windows.Media;

namespace SmartEMR.Application.Common.Converter;

public class InsuranceTypeToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return Brushes.Transparent;

        var RCB_InsuranceType = value.ToString();
        if (string.IsNullOrWhiteSpace(RCB_InsuranceType)) return Brushes.Transparent;

        var brush = RCB_InsuranceType switch
        {
            "GUN" => SmartBrush.SMART_BRUSH_INSURANCE_GUN,
            "MED" => SmartBrush.SMART_BRUSH_INSURANCE_MED,
            "CAR" => SmartBrush.SMART_BRUSH_INSURANCE_CAR,
            "SAN" => SmartBrush.SMART_BRUSH_INSURANCE_SAN,
            "NON" => SmartBrush.SMART_BRUSH_INSURANCE_NON,
            _ => Brushes.Transparent
        };

        return brush;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}