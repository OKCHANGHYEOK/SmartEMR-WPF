using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;
using SmartEMR.Application.Xpf;
using System.Drawing;
using System.Globalization;

namespace SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;

/// <summary>
/// CalendarHeaderItemTemplate.xaml에 대한 상호 작용 논리
/// </summary>
public partial class CalendarHeaderItemTemplate : CustomControl
{
}

public class DayOfWeekToForegroundConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DayOfWeek dayOfWeek == false) return Brushes.Transparent;

        var foreground = dayOfWeek switch
        {
            DayOfWeek.Saturday => SmartBrush.SMART_BRUSH_DAY_SAT,
            DayOfWeek.Sunday => SmartBrush.SMART_BRUSH_DAY_SUN,
            _ => SmartBrush.SMART_BRUSH_DAY_WEEKDAY
        };

        return foreground;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
