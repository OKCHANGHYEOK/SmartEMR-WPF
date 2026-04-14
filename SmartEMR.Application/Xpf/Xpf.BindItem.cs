using System.Windows;

namespace SmartEMR.Application.Xpf;

public enum BindType
{
    None,
    TextBox,
    PasswordBox,
    ComboBox,
    CheckBox
}

public class BindItem
{
    public string? FieldName { get; set; }
    public BindType BindType { get; set; } = BindType.TextBox;
    public int Col { get; set; }
    public int Row { get; set; }
    public string? Placeholder { get; set; }
    public Thickness Margin { get; set; } = new Thickness(0);
    public Thickness Padding { get; set; } = new Thickness(0);
    public Thickness ContentBorderThickness { get; set; } = new Thickness(1);
}
