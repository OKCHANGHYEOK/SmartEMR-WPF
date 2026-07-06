using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class PopupMenuItem : DevExpress.Xpf.Bars.BarButtonItem
{
    public PopupMenuItem()
    {
        this.SetValue(TextElement.FontSizeProperty, 11.0);
        this.SetValue(TextElement.ForegroundProperty, Brushes.White);
    }
}

public class PopupMenuItemClickEventArgs : RoutedEventArgs
{
    public string? Name { get; set; }

    public PopupMenuItemClickEventArgs(RoutedEvent routedEvent, object? source, string name) 
        : base(routedEvent, source)
    {
        this.Name = name;
    }
}