using System.Globalization;
using System.Windows.Media;
using SmartEMR.Application.Resources;

namespace SmartEMR.Application.Common.Converter;

public class RCPStatusToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var RCP_Status = value.ToString();
        if (string.IsNullOrWhiteSpace(RCP_Status))
        {
            return Brushes.Transparent;
        }

        return RCP_Status == "RDY" ? SmartBrush.SMART_BRUSH_RCP_RDY : RCP_Status == "END" ? SmartBrush.SMART_BRUSH_RCP_END : SmartBrush.SMART_BRUSH_RCP_CNL;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}