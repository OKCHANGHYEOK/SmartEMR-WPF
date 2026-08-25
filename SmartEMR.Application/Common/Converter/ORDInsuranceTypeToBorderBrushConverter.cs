using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using System.Globalization;
using System.Windows.Media;

namespace SmartEMR.Application.Common.Converter;

public class ORDInsuranceTypeToBorderBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var ORD_InsuranceType = value.ToString();
        if (string.IsNullOrWhiteSpace(ORD_InsuranceType))
        {
            return Brushes.Transparent;
        }

        return ORD_InsuranceType == "INS" ? SmartBrush.SMART_BRUSH_ORD_INSURANCE_INS_BORDER : SmartBrush.SMART_BRUSH_ORD_INSURANCE_NON_BORDER;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
