using System.Globalization;
using System.Windows.Media;
using SmartEMR.Application.Common.Converter.Base;
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

        return RCP_Status == "RDY" ? SmartBrush.SMART_BRUSH_STATUS_WAIT : RCP_Status == "END" ? SmartBrush.SMART_BRUSH_STATUS_COMPLETE : SmartBrush.SMART_BRUSH_STATUS_CANCEL;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}