using System.Globalization;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.Converter;

internal class OrderTypeToItemsSourceConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IEnumerable<Order> enumarable) return default!;
        if (parameter is not string type) return default!;

        IEnumerable<Order>? finalList = enumarable.Where(x => x.ORDC_Cd == type);
        if (finalList is null)
        {
            return default!;
        }

        return finalList;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
