using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class Label : System.Windows.Controls.Label
{
    public Label()
    {
        this.Foreground = Brushes.Black;
        this.HorizontalAlignment = HorizontalAlignment.Stretch;
        this.HorizontalContentAlignment = HorizontalAlignment.Center;
        this.VerticalAlignment = VerticalAlignment.Stretch;
        this.VerticalContentAlignment = VerticalAlignment.Center;
        this.MinWidth = 10;
        this.MinHeight = 10;
        this.Margin = new Thickness(0);
        this.Padding = new Thickness(0);
    }
}
