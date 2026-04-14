using System.Windows;

namespace SmartEMR.Application.Xpf;

public enum BindingType
{
    None,
    TextBox,
    PasswordBox,
    ComboBox,
    CheckBox
}

public class BindingElement
{
    public string? FieldName { get; set; }
    public BindingType BindingType { get; set; } = BindingType.TextBox;
    public int Col { get; set; }
    public int Row { get; set; }
    public string? Placeholder { get; set; }
    public Thickness Margin { get; set; } = new Thickness(0);
    public Thickness Padding { get; set; } = new Thickness(0);
    public Thickness ContentBorderThickness { get; set; } = new Thickness(1);
}
