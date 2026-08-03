using SmartEMR.Application.Common.Converter.Base;
using System.Globalization;
using System.Windows.Media;


namespace SmartEMR.Application.Common.Converter;

public class DayOfWeekToBackgroundConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DayOfWeek dayOfWeek == false) return Brushes.Transparent;

        return dayOfWeek switch
        {
            DayOfWeek.Saturday => new SolidColorBrush(Color.FromRgb(239, 246, 255)),
            DayOfWeek.Sunday => new SolidColorBrush(Color.FromRgb(254, 242, 242)),
            DayOfWeek.Friday => new SolidColorBrush(Color.FromRgb(241, 245, 249)),
            _ => new SolidColorBrush(Color.FromRgb(248, 250, 252))
        };
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
