using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

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

    static Button()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
    }

    public Button() : base() { }
}