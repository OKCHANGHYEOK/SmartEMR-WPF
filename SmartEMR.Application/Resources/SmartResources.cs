using SmartEMR.Application.Core;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Resources;

public class SmartResourceDictionary
{
    private static readonly ResourceDictionary _dictGeneric = new ResourceDictionary { Source = new Uri("../Themes/Generic.xaml", UriKind.RelativeOrAbsolute) };
    private static readonly ResourceDictionary _dictDataGridCell = new ResourceDictionary { Source = new Uri("../Template/DataGridCellTemplates.xaml", UriKind.RelativeOrAbsolute) };
    private static readonly ResourceDictionary _dictCalendar = new ResourceDictionary { Source = new Uri("../Template/CalendarTemplates.xaml", UriKind.RelativeOrAbsolute) };

    public static T? GetStaticResource<T>(TargetResource targetResource, string resourceKey) where T : class
    {
        switch (targetResource)
        {
            case TargetResource.Generic:
                return _dictGeneric.Contains(resourceKey) ? _dictGeneric[resourceKey] as T : null;

            case TargetResource.DataGridCell:
                return _dictDataGridCell.Contains(resourceKey) ? _dictDataGridCell[resourceKey] as T : null;

            case TargetResource.Calendar:
                return _dictCalendar.Contains(resourceKey) ? _dictCalendar[resourceKey] as T : null;
        }

        return null;
    }
}

public static class SmartBrush
{
    public static readonly Brush SMART_BRUSH_SECTION_BACKGROUND = CreateBrushFromHex("#F5F6F8");
    public static readonly Brush SMART_BRUSH_SECTION_BORDER = CreateBrushFromHex("#D8DDE3");

    public static readonly Brush SMART_BRUSH_INFO = CreateBrushFromHex("#00BCD4");
    public static readonly Brush SMART_BRUSH_SUCCESS = CreateBrushFromHex("#009688");
    public static readonly Brush SMART_BRUSH_WARNING = CreateBrushFromHex("#ED7D31");
    public static readonly Brush SMART_BRUSH_ERROR = CreateBrushFromHex("#E91E63");

    public static readonly Brush SMART_BRUSH_PERSONAL_INFO_AGREE = CreateBrushFromHex("#009688");
    public static readonly Brush SMART_BRUSH_PERSONAL_INFO_NOTAGREE = CreateBrushFromHex("#D1D5D8");

    public static readonly Brush SMART_BRUSH_RES = CreateBrushFromRGB(59, 130, 246);
    public static readonly Brush SMART_BRUSH_RCP = CreateBrushFromRGB(16, 185, 129);

    public static readonly Brush SMART_BRUSH_STATUS_PENDING = CreateBrushFromHex("#D4A017");
    public static readonly Brush SMART_BRUSH_STATUS_CONFIRMED = CreateBrushFromRGB(37, 99, 235);
    public static readonly Brush SMART_BRUSH_STATUS_WAIT = CreateBrushFromRGB(255, 159, 67);
    public static readonly Brush SMART_BRUSH_STATUS_PROGRESS = CreateBrushFromRGB(59, 130, 246);
    public static readonly Brush SMART_BRUSH_STATUS_COMPLETE = CreateBrushFromRGB(22, 163, 74);
    public static readonly Brush SMART_BRUSH_STATUS_CANCEL = CreateBrushFromRGB(239, 68, 68);

    public static readonly Brush SMART_BRUSH_INSURANCE_GUN = CreateBrushFromHex("#3478F6"); // 건보
    public static readonly Brush SMART_BRUSH_INSURANCE_MED = CreateBrushFromHex("#7CA4E8"); // 의보
    public static readonly Brush SMART_BRUSH_INSURANCE_CAR = CreateBrushFromHex("#F5A623"); // 자보
    public static readonly Brush SMART_BRUSH_INSURANCE_SAN = CreateBrushFromHex("#8E6CCF"); // 산재
    public static readonly Brush SMART_BRUSH_INSURANCE_NON = CreateBrushFromHex("#9E9E9E"); // 비보험

    public static readonly Brush SMART_BRUSH_VISIT_FIR = CreateBrushFromRGB(67, 160, 71);
    public static readonly Brush SMART_BRUSH_VISIT_REP = CreateBrushFromRGB(120, 144, 156);

    public static readonly Brush SMART_BRUSH_DAY_WEEKDAY = CreateBrushFromHex("#4B5563");
    public static readonly Brush SMART_BRUSH_DAY_SAT = CreateBrushFromHex("#2563EB");
    public static readonly Brush SMART_BRUSH_DAY_SUN = CreateBrushFromHex("#DC2626");

    public static readonly Brush SMART_BRUSH_ORD_INSURANCE_INS = CreateBrushFromHex("#3B82C4");
    public static readonly Brush SMART_BRUSH_ORD_INSURANCE_NON = CreateBrushFromHex("#D9822B");

    private static Brush CreateBrushFromHex(string hex)
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