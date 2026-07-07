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
            if (e.NewValue is string newPath && ! string.IsNullOrWhiteSpace(newPath))
            {
                element.SetValue(dp, newPath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) ? GlyphSvgToImage(newPath) : GlyphImage(newPath));
            }
            else if (e.NewValue is byte[] arrBytes && arrBytes.Length > 0)
            {
                element.SetValue(dp, GenerateBitmapImage(arrBytes));
            }
            else
            {
                element.SetValue(dp, GlyphImage("Images/smartemr_default_profile.png"));
            }
        }
        else
        {
            element.ClearValue(dp);
        }
    }
}
