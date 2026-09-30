using SmartEMR.Application.Common.Converter.Base;
using System.Globalization;
using System.Windows.Media;

namespace SmartEMR.Application.Common.Converter;

public class ImageSourceConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return default!;

        ImageSource? image;

        if (value is string newPath && !string.IsNullOrWhiteSpace(newPath))
        {
            image = newPath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) ? GlyphSvgToImage(newPath) : GlyphImage(newPath);
        }
        else if (value is byte[] arrBytes && arrBytes.Length > 0)
        {
            image = GenerateBitmapImage(arrBytes);
        }
        else
        {
            image = GlyphImage("Images/smartemr_default_profile.png");
        }

        return image ?? default!;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
