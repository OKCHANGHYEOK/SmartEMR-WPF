using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class Spliter : Xpf.TextBlock
{
    public Spliter()
    {
        this.FontSize = 10;
        this.Text = "|";
        this.Foreground = Brushes.LightGray;
        this.MinWidth = 5;
        this.Width = 5;
        this.Margin = new Thickness(3, 0, 0, 0);
        this.VerticalAlignment = VerticalAlignment.Center;
    }
}
