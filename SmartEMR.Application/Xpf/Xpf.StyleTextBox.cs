using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Xpf;

public enum StyleTextBoxType
{
    Text,
    Password
}

public class StyleTextBox : Control
{
    // DependencyProperty들은 유지하되, UI 생성 로직은 제거합니다.
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(StyleTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(StyleTextBox),
            new PropertyMetadata(string.Empty));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(StyleTextBox),
            new PropertyMetadata(new CornerRadius(5)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty TextBoxTypeProperty =
        DependencyProperty.Register(nameof(TextBoxType), typeof(StyleTextBoxType), typeof(StyleTextBox),
            new PropertyMetadata(StyleTextBoxType.Text));

    public StyleTextBoxType TextBoxType
    {
        get => (StyleTextBoxType)GetValue(TextBoxTypeProperty);
        set => SetValue(TextBoxTypeProperty, value);
    }

    static StyleTextBox()
    {
        // ⭐ XAML의 스타일을 찾아가도록 설정합니다.
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StyleTextBox),
            new FrameworkPropertyMetadata(typeof(StyleTextBox)));
    }
}