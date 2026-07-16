using System.Globalization;
using System.Windows.Media;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;

namespace SmartEMR.Application.Common.Converter;

public class ReceptionBoardTypeToBrushConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var RCB_Type = value.ToString();
        if (string.IsNullOrWhiteSpace(RCB_Type))
        {
            return Brushes.Transparent;
        }

        return RCB_Type == "RES" ? SmartBrush.SMART_BRUSH_RES : SmartBrush.SMART_BRUSH_RCP;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}