using System.Diagnostics;
using System.Windows.Media;

namespace SmartEMR.Application.Resources;

public static class SmartBrush
{
    public static readonly Brush SMART_BRUSH_INFO = CreateBrushFromString("#00BCD4");
    public static readonly Brush SMART_BRUSH_SUCCESS = CreateBrushFromString("#009688");
    public static readonly Brush SMART_BRUSH_WARNING = CreateBrushFromString("#ED7D31");
    public static readonly Brush SMART_BRUSH_ERROR = CreateBrushFromString("#E91E63");

    public static readonly Brush SMART_BRUSH_PERSONAL_INFO_AGREE = CreateBrushFromString("#009688");
    public static readonly Brush SMART_BRUSH_PERSONAL_INFO_NOTAGREE = CreateBrushFromString("#D1D5D8");

    public static readonly Brush SMART_BRUSH_RES = CreateBrushFromRGB(59, 130, 246);
    public static readonly Brush SMART_BRUSH_RCP = CreateBrushFromRGB(16, 185, 129);

    public static readonly Brush SMART_BRUSH_RCP_RDY = CreateBrushFromRGB(255, 159, 67);
    public static readonly Brush SMART_BRUSH_RCP_END = CreateBrushFromRGB(16, 185, 129);
    public static readonly Brush SMART_BRUSH_RCP_CNL = CreateBrushFromRGB(239, 68, 68);

    private static Brush CreateBrushFromString(string hex)
    {
        if (!(ColorConverter.ConvertFromString(hex) is Color hexColor))
        {
            Debug.WriteLine("CreateBrush : 올바르지 않은 값이 전달되었습니다.");
            return default!;
        }

        var brush = new SolidColorBrush(hexColor);
        brush.Freeze();
        return brush;
    }

    private static Brush CreateBrushFromRGB(byte r, byte g, byte b)
    {
        var brush = new SolidColorBrush(Color.FromRgb(r, g, b));
        brush.Freeze();
        return brush;
    }
}

public static class SmartImage
{
    public static readonly ImageSource SMART_IMAGE_INFO = GlyphImage("Images/smartemr_info.png");
    public static readonly ImageSource SMART_IMAGE_SUCCESS = GlyphImage("Images/smartemr_check.png");
    public static readonly ImageSource SMART_IMAGE_WARNING = GlyphImage("Images/smartemr_warning.png");
    public static readonly ImageSource SMART_IMAGE_ERROR = GlyphImage("Images/smartemr_error.png");
}