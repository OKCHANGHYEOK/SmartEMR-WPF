using System.Windows;

namespace SmartEMR.Application.Xpf;

public class Image : System.Windows.Controls.Image
{  
    public static readonly new DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source), typeof(object), typeof(Image), new PropertyMetadata(null, OnSourceChanged));

    public new object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    
    public Image()
    {
        this.MinWidth = 40;
        this.MinHeight = 40;
    }

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var element = d as Image;
        if (element == null) return;

        var dp = System.Windows.Controls.Image.SourceProperty;

        if (e.NewValue != null)
        {
            if (e.NewValue is string newPath)
            {
                if (newPath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                {
                    element.SetValue(dp, GlyphSvgToImage(newPath));
                }
                else
                {
                    element.SetValue(dp, GlyphImage(newPath));
                }
            }
            else if (e.NewValue is byte[] arrBytes)
            {
                element.SetValue(dp, GenerateBitmapImage(arrBytes));
            }
        }
        else
        {
            element.ClearValue(dp);
        }
    }
}
