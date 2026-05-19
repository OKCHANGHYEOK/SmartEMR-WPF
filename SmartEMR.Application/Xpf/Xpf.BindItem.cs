using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public enum BindType
{
    None,
    TextBox,
    PasswordBox,
    ComboBox,
    CheckBox,
    Button
}

public class BindItem
{
    // 공통
    public string FieldName { get; set; } = string.Empty;
    public BindType BindType { get; set; } = BindType.TextBox;
    public int Col { get; set; }
    public int ColSpan { get; set; } = 1;
    public int Row { get; set; }
    public int RowSpan { get; set; } = 1;
    public string? Placeholder { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public Thickness? Margin { get; set; } = new Thickness(0);
    public Thickness Padding { get; set; } = new Thickness(0);
    public string? BorderBrush { get; set; } = "TransParent";
    public Thickness BorderThickness { get; set; } = new Thickness(1);
    public string? BackGround { get; set; } = "TransParent";
    public Brush Foreground { get; set; } = Brushes.Black;
    public double? FontSize { get; set; } = 13;
    public FontWeight FontWeight { get; set; } = FontWeights.Normal;
    public CornerRadius CornerRadius { get; set; } = new CornerRadius(0);

    // 버튼 관련
    public string? ButtonText { get; set; } = "버튼";

    // 플래그
    public bool IsEnabled { get; set; } = true;
    public bool IsRequired { get; set; } = false;
    public bool IsChecked { get; set; } = false;
    public bool IsBinding { get; set; } = true;
    public bool IsBindClickEvent { get; set; } = false;
    public bool IsExpandingWhenClick { get; set; } = false;
}
