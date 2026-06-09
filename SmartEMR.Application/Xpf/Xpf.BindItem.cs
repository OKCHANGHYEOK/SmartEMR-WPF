using DevExpress.Xpf.Editors;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public enum BindType
{
    None,
    Label,
    TextBox,
    PasswordBox,
    ComboBox,
    CheckBox,
    Button,
    Image
}

[ContentProperty(nameof(Content))]
public class BindItem
{
    // 공통
    public string FieldName { get; set; } = string.Empty;
    public BindType BindType { get; set; } = BindType.TextBox;
    public int Col { get; set; }
    public int ColSpan { get; set; } = 1;
    public int Row { get; set; }
    public int RowSpan { get; set; } = 1;
    public string? TextValue { get; set; }
    public int MaxLength { get; set; } 
    public string Placeholder { get; set; } = "";
    public UIElement? Content { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public Thickness? Margin { get; set; }
    public Thickness Padding { get; set; } = new Thickness(0);
    public string? BorderBrush { get; set; }
    public Thickness BorderThickness { get; set; } = new Thickness(1);
    public string? BackGround { get; set; } = "TransParent";
    public Brush Foreground { get; set; } = Brushes.Black;
    public double FontSize { get; set; } = 13;
    public FontWeight FontWeight { get; set; } = FontWeights.Normal;
    public CornerRadius CornerRadius { get; set; } = new CornerRadius(0);

    public HorizontalAlignment HAlignment { get; set; } = HorizontalAlignment.Stretch;
    public VerticalAlignment VAlignment { get; set; } = VerticalAlignment.Stretch;
    public ContentAlignment ContentAlignment { get; set; } = ContentAlignment.LeftCenter;
    
    public IQueryable? ItemsSource { get; set; }
    public string? DisplayMember { get; set; }
    public string? ValueMember { get; set; }

    public MaskType MaskType { get; set; }
    public string Mask { get; set; } = "";

    // 헤더
    public string? Header { get; set; }
    public double? HeaderWidth { get; set; }
    public int HeaderFontSize { get; set; } = 11;
    public FontWeight HeaderFontWeight { get; set; } = FontWeights.Normal;
    public Brush HeaderForeground { get; set; } = Brushes.Black;

    // 플래그
    public bool IsNumericOnly { get; set; } = false;
    public bool IsReadOnly { get; set; } = false;
    public bool IsBottomLine { get; set; } = true;
    public bool IsHeader { get; set; } = true;
    public bool IsEnabled { get; set; } = true;
    public bool IsRequired { get; set; } = false;
    public bool IsChecked { get; set; } = false;
    public bool IsBinding { get; set; } = true;
    public bool IsBindClickEvent { get; set; } = false;
    public bool IsExpandingWhenClick { get; set; } = false;
    public bool IsApplyYNToBoolean { get; set; } = false;
}
