using SmartEMR.Application.Core;
using SmartEMR.Application.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class ContextMenu : System.Windows.Controls.ContextMenu
{
    public ContextMenu()
    {
        this.MinWidth = 200;
        this.Background = new SolidColorBrush(Color.FromRgb(24, 76, 136));
        this.BorderBrush = new SolidColorBrush(Color.FromRgb(25, 25, 112));
        this.FontSize = 11;
        this.Padding = new Thickness(2);
        this.Resources.Add(typeof(MenuItem), SmartResourceDictionary.GetStaticResource<Style>(TargetResource.Generic, "SimpleMenuItemStyle"));
    }
}

public class ContextMenuItem
{
    public string Key { get; set; } = string.Empty;
    public object? Header { get; set; }
}

public class ContextMenuItemClickedEventArgs : RoutedEventArgs
{
    public object? DataItem { get; }
    public ContextMenuItem? MenuItem { get; }

    public ContextMenuItemClickedEventArgs(RoutedEvent routedEvent, object source, object? dataItem, ContextMenuItem menuItem)
        : base(routedEvent, source)
    {
        DataItem = dataItem;
        MenuItem = menuItem;
    }
}
