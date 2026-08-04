using DevExpress.Xpf.Core;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;

/// <summary>
/// ReservationCalendarCellItem.xaml에 대한 상호 작용 논리
/// </summary>
public partial class ReservationCalendarCellItem : CustomControl
{
    public static readonly DependencyProperty ReservationProperty =
        DependencyProperty.Register(nameof(Reservation), typeof(Reservation), typeof(ReservationCalendarCellItem), new PropertyMetadata(null));

    public Reservation Reservation
    {
        get => (Reservation)GetValue(ReservationProperty);
        set => SetValue(ReservationProperty, value);
    }
}

public class DictionaryValueConverter : MarkupExtension, IMultiValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2) return default!;

        if (values[0] is IDictionary<string, Reservation> dict && values[1] is string key)
        {
            if (dict.TryGetValue(key, out var value) && value is not null) 
            { 
                return value;
            }
        }

        return default!;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
