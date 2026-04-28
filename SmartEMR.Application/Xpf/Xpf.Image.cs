using SmartEMR.Application.Core;
using System.Windows;

namespace SmartEMR.Application.Xpf;

public class Image : System.Windows.Controls.Image
{  
    public static readonly new DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source), typeof(string), typeof(Image), new PropertyMetadata(string.Empty, OnSourceChanged));

    public new string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    
    public Image()
    {
        this.Width = 40;
        this.Height = 40;
    }

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Image image && e.NewValue is string newPath)
        {
            if (newPath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            {
                image.SetValue(System.Windows.Controls.Image.SourceProperty, SmartMVVM.Common.GlyphSvgToImage(newPath));
            }
            else
            { 
                image.SetValue(System.Windows.Controls.Image.SourceProperty, SmartMVVM.Common.GlyphImage(newPath));
            }
        }
    }
}
