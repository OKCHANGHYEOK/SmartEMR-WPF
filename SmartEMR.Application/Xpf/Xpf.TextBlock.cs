using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class TextBlock : System.Windows.Controls.TextBlock
{
    public TextBlock()
    {
        this.MinWidth = 24;
        this.MinHeight = 20;
        this.Background = Brushes.Transparent;
        this.HorizontalAlignment = HorizontalAlignment.Center;
        this.VerticalAlignment = VerticalAlignment.Center;
        this.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
    }
}
