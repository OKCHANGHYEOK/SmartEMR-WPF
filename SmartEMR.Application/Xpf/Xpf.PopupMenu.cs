using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class PopupMenu : DevExpress.Xpf.Bars.PopupMenu
{
    public PopupMenu()
    {
        this.MinWidth = 200;
        this.Background = new SolidColorBrush(Color.FromRgb(24, 76, 136));
        this.BorderBrush = new SolidColorBrush(Color.FromRgb(25, 25, 112));
        this.Padding = new Thickness(2);
    }
}
