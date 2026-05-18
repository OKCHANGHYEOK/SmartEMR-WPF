using System.Windows.Media;

namespace SmartEMR.Application.Resources;

public static class SmartBrush
{
    public static readonly Brush BRUSH_INFO = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00BCD4"));
    public static readonly Brush BRUSH_SUCCESS = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009688"));
    public static readonly Brush BRUSH_WARNING = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31"));
    public static readonly Brush BRUSH_ERROR = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E91E63"));

    public static readonly Brush BRUSH_PESRONAL_INFO_AGREE = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009688"));
    public static readonly Brush BRUSH_PESRONAL_INFO_NOTAGREE = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D1D5D8"));
}

public static class SmartImage
{
    public static readonly ImageSource IMAGE_INFO = GlyphImage("Images/smartemr_info.png");
    public static readonly ImageSource IMAGE_SUCCESS = GlyphImage("Images/smartemr_check.png");
    public static readonly ImageSource IMAGE_WARNING = GlyphImage("Images/smartemr_warning.png");
    public static readonly ImageSource IMAGE_ERROR = GlyphImage("Images/smartemr_error.png");
}