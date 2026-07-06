using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class PopupMenuItem : DevExpress.Xpf.Bars.BarButtonItem
{
    public string? MenuAction { get; set; }

    public PopupMenuItem()
    {
        this.SetValue(TextElement.FontSizeProperty, 11.0);
        this.SetValue(TextElement.ForegroundProperty, Brushes.White);
    }
}

public class PopupMenuItemClickEventArgs : RoutedEventArgs
{
    public object DataItem { get; }
    public string? MenuAction { get; set; }

    public PopupMenuItemClickEventArgs(RoutedEvent routedEvent, object? source, object dataItem, string action) 
        : base(routedEvent, source)
    {
        this.DataItem = dataItem;
        this.MenuAction = action;
    }
}