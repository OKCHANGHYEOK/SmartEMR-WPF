using DevExpress.Xpf.Core.Internal;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class ImageButton : Control
{
    public static readonly RoutedEvent ClickEvent =
        EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImageButton));

    public event RoutedEventHandler Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    public static DependencyProperty GlyphProperty =
        DependencyProperty.Register("Glyph", typeof(ImageSource), typeof(ImageButton),
            new FrameworkPropertyMetadata(null));

    [TypeConverter(typeof(SvgImageSourceConverter))]
    public ImageSource Glyph
    {
        get => (ImageSource)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    public static readonly DependencyProperty ImageWidthProperty =
        DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageButton),
            new FrameworkPropertyMetadata(20.0));

    public double ImageWidth
    {
        get => (double)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    public static readonly DependencyProperty ImageHeightProperty =
    DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageButton),
        new FrameworkPropertyMetadata(20.0));

    public double ImageHeight
    {
        get => (double)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    static ImageButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var btn = GetTemplateChild("PART_BUTTON") as System.Windows.Controls.Button;

        if (btn != null)
        {
            btn.Click += (s, e) =>
            {
                RaiseEvent(new RoutedEventArgs(ImageButton.ClickEvent));
            };
        }
    }
}

