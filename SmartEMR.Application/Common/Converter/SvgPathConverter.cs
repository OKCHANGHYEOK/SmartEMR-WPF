using System.Globalization;
using System.Windows.Data;
using DevExpress.Xpf.Core.Internal;

namespace SmartEMR.Application.Common.Converter;

public class SvgPathConverter : IValueConverter
{
    private readonly string BasePath = "pack://application:,,,/SmartEMR.Application;component/Images/Svg/";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string fileName)
        {
            string path = $"{BasePath}{fileName.TrimStart('/')}";

            try
            {
                return new SvgImageSourceConverter().ConvertFromString(path) ?? default!;
            }
            catch
            {
                return default!;
            }
        }

        return default!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
