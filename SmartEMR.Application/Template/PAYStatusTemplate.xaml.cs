using SmartEMR.Application.Resources;
using SmartEMR.Application.Xpf;
using System.Windows.Media;
using System.Globalization;
using SmartEMR.Application.Common.Converter.Base;

namespace SmartEMR.Application.Template;

/// <summary>
/// PAYStatusTemplate.xaml에 대한 상호 작용 논리
/// </summary>
public partial class PAYStatusTemplate : GridTemplate
{
    public PAYStatusTemplate() { }

    public override void Initialize()
    {
    }
}

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