using DevExpress.Xpf.Grid;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Xpf;

public class DataGrid : ContentControl
{
    public event EventHandler<DataItemChangedEventArgs>? DataGrid_DataItemChangedEvent;

    public DevExpress.Xpf.Grid.GridControl GridControl { get; private set; }
    public DevExpress.Xpf.Grid.TableView TableView { get; private set; }

    public static readonly DependencyProperty DataItemProperty =
        DependencyProperty.Register(
            nameof(DataItem), 
            typeof(object), 
            typeof(DataGrid), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDataItemChanged));

    public object? DataItem
    {
        get => GetValue(DataItemProperty);
        set => SetValue(DataItemProperty, value);
    }

    public DataGrid()
    {
        GridControl = new GridControl();
        TableView = new TableView();

        GridControl.View = TableView;

        TableView.ShowGroupPanel = false;
        TableView.AllowEditing = false;
        TableView.AutoWidth = true;
        TableView.ShowAutoFilterRow = true;
        TableView.AllowHorizontalScrollingVirtualization = false;
    
        this.Content = GridControl;

        GridControl.CurrentItemChanged += (s, e) =>
        {
            this.DataItem = GridControl.CurrentItem;
        };

        GridControl.CurrentColumnChanged += (s, e) =>
        {
            this.InvokeDataItemChanged(this.DataItem);
        };
    }

    private static void OnDataItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid)
        {
            dataGrid.InvokeDataItemChanged(e.NewValue);
        }
    }

    private void InvokeDataItemChanged(object? item)
    {
        var args = new DataItemChangedEventArgs(item, this.GridControl.CurrentColumn);
        this.DataGrid_DataItemChangedEvent?.Invoke(this, args);
    }
}

public class DataItemChangedEventArgs : EventArgs
{
    public object? DataItem { get; }
    public ColumnBase Column { get; }

    public DataItemChangedEventArgs(object? item, ColumnBase column)
    {
        DataItem = item;
        Column = column;
    }
}