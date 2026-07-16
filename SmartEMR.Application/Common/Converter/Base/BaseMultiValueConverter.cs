using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SmartEMR.Application.Common.Converter.Base;

public abstract class BaseMultiValueConverter : MarkupExtension, IMultiValueConverter
{

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
    public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
}
