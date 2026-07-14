using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public enum ColumnType
{
    Label,
    TextBox,
    TextLink,
    CheckBox
}

public enum ColumnStyle
{
    Name,
    Code,
    Sum,
    YYMMDD
}

public class ColumnItem 
{
    public string FieldName { get; set; } = string.Empty;
    public string Header { get; set; } = string.Empty;
    public ColumnType ColumnType { get; set; } = ColumnType.Label;
    public ColumnStyle? ColumnStyle { get; set; }
    public double ColumnWidth { get; set; }
    public double FontSize { get; set; } = 11;
    public FontWeight FontWeight { get; set; } = FontWeights.Normal;
    public Brush? Foreground { get; set; } = Brushes.Black;
    public Type? CellTemplateType { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
    public bool AllowSorting { get; set; } = false;
}
