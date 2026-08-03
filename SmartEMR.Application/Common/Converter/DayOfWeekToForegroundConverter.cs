using System.Globalization;
using System.Windows.Media;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Resources;

namespace SmartEMR.Application.Common.Converter
{
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
}
