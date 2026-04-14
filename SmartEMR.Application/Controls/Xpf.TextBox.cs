using SmartEMR.Application.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;

namespace SmartEMR.Application.Controls;

public class TextBox : System.Windows.Controls.TextBox
{
    public TextBox()
    {
        this.MinWidth = 24;
        this.MinHeight = 20;
        this.BorderThickness = new Thickness(0);
        this.Background = Brushes.Transparent;
        this.VerticalContentAlignment = VerticalAlignment.Center;
        this.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
    }
}
