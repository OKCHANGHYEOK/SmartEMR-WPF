using System.Globalization;
using SmartEMR.Application.Common.Converter.Base;

namespace SmartEMR.Application.Common.Converter;

public class DayOfWeekToStringConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DayOfWeek dayOfWeek == false) return "";

        var strDayOfWeek = dayOfWeek switch
        {
            DayOfWeek.Monday => "월",
            DayOfWeek.Tuesday => "화",
            DayOfWeek.Wednesday => "수",
            DayOfWeek.Thursday => "목",
            DayOfWeek.Friday => "금",
            DayOfWeek.Saturday => "토",
            DayOfWeek.Sunday => "일",
            _ => ""
        };

        return $"{strDayOfWeek}요일";
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
