using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public enum ButtonTheme
{
    White = 0,
    Black = 1,
    Blue = 2,
    Orange = 3,
}

public class Button : System.Windows.Controls.Button
{
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(Button),
        new PropertyMetadata(new CornerRadius(0)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
        nameof(Glyph),
        typeof(ImageSource),
        typeof(Button),
        new PropertyMetadata(null));

    public ImageSource Glyph     
    {
        get => (ImageSource)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    public static readonly DependencyProperty ButtonThemeProperty = DependencyProperty.Register(
        nameof(ButtonTheme),
        typeof(ButtonTheme),
        typeof(Button),
        new PropertyMetadata(ButtonTheme.White));

    public ButtonTheme ButtonTheme
    {
        get => (ButtonTheme)GetValue(ButtonThemeProperty);
        set => SetValue(ButtonThemeProperty, value);
    }

    public static readonly DependencyProperty IsExpandingWhenClickProperty = DependencyProperty.Register(
        nameof(IsExpandingWhenClick),
        typeof(bool),
        typeof(Button),
        new PropertyMetadata(false));

    public bool IsExpandingWhenClick
    {
        get => (bool)GetValue(IsExpandingWhenClickProperty);
        set => SetValue(IsExpandingWhenClickProperty, value);
    }

    static Button()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
    }

    public Button() : base() { }
}