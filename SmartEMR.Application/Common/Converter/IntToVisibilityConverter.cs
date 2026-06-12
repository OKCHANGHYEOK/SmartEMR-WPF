using System.Globalization;
using System.Windows;

namespace SmartEMR.Application.Common.Converter
{
    public class IntToVisibilityConverter : BaseConverter
    {
        public bool invert { get; set; } = false;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bFlag = value is int intValue && intValue > 0;
            bool isVisible = invert ? !bFlag : bFlag;

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
