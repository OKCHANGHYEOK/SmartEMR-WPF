using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class PopupMenu : DevExpress.Xpf.Bars.PopupMenu
{
    public event EventHandler<PopupMenuItemClickEventArgs>? PopupMenuClick;

    public PopupMenu()
    {
        this.MinWidth = 200;
        this.Background = new SolidColorBrush(Color.FromRgb(233, 236, 239));
        this.BorderBrush = new SolidColorBrush(Color.FromRgb(105, 105, 105));
        this.Padding = new Thickness(2);
    }

    public void AddMenu(PopupMenuItem item)
    {
        if (item.Content == null) return;

        Items.Add(item);
    }

    public void AddSeperator()
    {
        Items.Add(new BarItemSeparator());
    }

    protected override void OnOpened(EventArgs e)
    {
        foreach (PopupMenuItem item in Items.OfType<PopupMenuItem>())
        {
            item.ItemClick += OnPopupMenuItemClick;
        }

        base.OnOpened(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        foreach (PopupMenuItem item in Items.OfType<PopupMenuItem>())
        {
            item.ItemClick -= OnPopupMenuItemClick;
        }

        Items.Clear();

        base.OnClosed(e);
    }

    private void OnPopupMenuItemClick(object sender, RoutedEventArgs e)
    {
        var item = sender as PopupMenuItem;
        if (item == null) return;

        PopupMenuClick?.Invoke(this, new PopupMenuItemClickEventArgs(e.RoutedEvent, item, this.DataContext, item.MenuAction ?? ""));
    }
}

public class PopupMenuOpeningEventArgs : EventArgs
{
    public PopupMenu PopupMenu { get; }
    public object? DataItem { get;  }
    public ColumnBase Column { get; }

    public PopupMenuOpeningEventArgs(PopupMenu popupMenu, object? dataItem, ColumnBase column)
    {
        this.PopupMenu = popupMenu;
        this.DataItem = dataItem;
        this.Column = column;
    }
}