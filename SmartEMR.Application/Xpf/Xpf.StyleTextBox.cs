using System.Windows.Media;
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

    public HorizontalAlignment PlaceHolderHorizontalAlignment
    {
        get => (HorizontalAlignment)GetValue(PlaceHolderHorizontalAlignmentProperty);
        set => SetValue(PlaceHolderHorizontalAlignmentProperty, value);
    }

    public static readonly DependencyProperty PlaceHolderHorizontalAlignmentProperty =
        DependencyProperty.Register(nameof(PlaceHolderHorizontalAlignment), typeof(HorizontalAlignment), typeof(StyleTextBox),
            new PropertyMetadata(HorizontalAlignment.Left));

    public Thickness PlaceHolderMargin
    {
        get => (Thickness)GetValue(PlaceHolderMarginProperty);
        set => SetValue(PlaceHolderMarginProperty, value);
    }

    public static readonly DependencyProperty PlaceHolderMarginProperty =
        DependencyProperty.Register(nameof(PlaceHolderMargin), typeof(Thickness), typeof(StyleTextBox),
            new PropertyMetadata(new Thickness(5,0,0,0)));

    public Thickness PlaceHolderPadding
    {
        get => (Thickness)GetValue(PlaceHolderPaddingProperty);
        set => SetValue(PlaceHolderPaddingProperty, value);
    }

    public static readonly DependencyProperty PlaceHolderPaddingProperty =
        DependencyProperty.Register(nameof(PlaceHolderPadding), typeof(Thickness), typeof(StyleTextBox),
            new PropertyMetadata(new Thickness(5, 0, 0, 0)));

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


    public static readonly DependencyProperty TextForegroundProperty =
        DependencyProperty.Register(nameof(TextForeground), typeof(Brush), typeof(StyleTextBox), new PropertyMetadata(Brushes.Black));

    public Brush TextForeground
    {
        get => (Brush)GetValue(TextForegroundProperty);
        set => SetValue(TextForegroundProperty, value);
    }

    public static readonly DependencyProperty BoxMarginProperty =
        DependencyProperty.Register(nameof(BoxMargin), typeof(Thickness), typeof(StyleTextBox), new PropertyMetadata(new Thickness(5)));

    public Thickness BoxMargin
    {
        get => (Thickness)GetValue(BoxMarginProperty);
        set => SetValue(BoxMarginProperty, value);
    }

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(StyleTextBox), null);

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public static readonly DependencyProperty IsNumericOnlyProperty =
        DependencyProperty.Register(nameof(IsNumericOnly), typeof(bool), typeof(StyleTextBox), null);

    public bool IsNumericOnly
    {
        get => (bool)GetValue(IsNumericOnlyProperty);
        set => SetValue(IsNumericOnlyProperty, value);
    }

    static StyleTextBox()
    {
        // ⭐ XAML의 스타일을 찾아가도록 설정합니다.
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StyleTextBox),
            new FrameworkPropertyMetadata(typeof(StyleTextBox)));
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);

        SetFocusToIntenalElement();
    }

    private void SetFocusToIntenalElement()
    {
        if (this.TextBoxType == StyleTextBoxType.Password)
        {
            // Password 모드일 때
            if (GetTemplateChild("PART_PasswordBox") is PasswordBox passwordBox)
            {
                passwordBox.Focus();
            }
        }
        else
        {
            // 기본적으로 Text 모드일 때
            if (GetTemplateChild("PART_TextBox") is TextBox textBox)
            {
                textBox.Focus();
                textBox.SelectAll();
            }
        }
    }
}