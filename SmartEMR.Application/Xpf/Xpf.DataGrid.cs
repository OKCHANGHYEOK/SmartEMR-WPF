using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Grid;
using SmartEMR.Application.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

[ObservableObject]
[ContentProperty(nameof(Items))]
public partial class DataGrid : ContentControl
{
    private bool IsUpdatedItemsSource { get; set; } = false;

    public event EventHandler<DataItemChangedEventArgs>? DataGrid_DataItemChangedEvent;

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), new PropertyMetadata(null, OnItemsSourceChanged));

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid && e.NewValue != null)
        {
            if (e.NewValue is IEnumerable enumerable)
            {
                dataGrid.SetItemsSource(enumerable);
            }
        }
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private ColumnItemCollection? _columns;

    public ColumnItemCollection Items
    {
        get
        {
            if (_columns == null)
            {
                _columns = new ColumnItemCollection();
                _columns.CollectionChanged += OnItems_CollectionChanged;
            }

            return _columns;
        }
    }

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

    public static readonly DependencyProperty IsDoubleClickedProperty =
        DependencyProperty.Register(nameof(IsDoubleClicked), typeof(bool), typeof(DataGrid), new PropertyMetadata(false));

    public bool IsDoubleClicked
    {
        get => (bool)GetValue(IsDoubleClickedProperty);
        set => SetValue(IsDoubleClickedProperty, value);
    }

    public bool AutoWidth
    {
        get => TableView.AutoWidth;
        set => TableView.AutoWidth = value;
    }

    private StyleGrid _layoutRoot = new();
    public DevExpress.Xpf.Grid.GridControl GridControl { get; private set; }
    public DevExpress.Xpf.Grid.TableView TableView { get; private set; }

    public GridColumnCollection Columns => GridControl.Columns;

    public event EventHandler<PopupMenuOpeningEventArgs>? DataGrid_PopupMenuOpening;
    public event EventHandler<PopupMenuItemClickEventArgs>? DataGrid_PopupMenuItemClick;

    public DataGrid()
    {
        GridControl = new();
        TableView = new();

        _layoutRoot.SetLayout(1, 3);
        _layoutRoot.AddElement(GridControl, 0, 0);
        _layoutRoot.AddElement(new Border { Background = Brushes.LightGray, Height = 1 }, 0, 1);
        _layoutRoot.AddElement(DataPager, 0, 2);
        _layoutRoot.LayoutRoot.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
        _layoutRoot.LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
        _layoutRoot.LayoutRoot.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Auto);

        GridControl.View = TableView;
        GridControl.Margin = new Thickness(2);
        GridControl.AllowInitiallyFocusedRow = false;
        //GridControl.CurrentItemChanged += (s, e) => this.DataItem = GridControl.CurrentItem;

        TableView.NavigationStyle = GridViewNavigationStyle.Cell;
        TableView.RowMinHeight = 24;
        TableView.HeaderPanelMinHeight = 18;
        TableView.AllowEditing = false;
        TableView.AllowHorizontalScrollingVirtualization = false;
        TableView.AllowColumnFiltering = false;
        TableView.AllowColumnMoving = false;
        TableView.AllowSorting = false;
        TableView.AutoWidth = true;
        TableView.ShowGroupPanel = false;
        TableView.ShowAutoFilterRow = false;
        TableView.ShowIndicator = false;
        TableView.IsColumnMenuEnabled = false;
        TableView.RowDoubleClick += TableView_OnRowDoubleClick;
        TableView.PreviewMouseLeftButtonDown += TableView_OnPreviewMouseLeftButtonDown;
        TableView.PreviewMouseRightButtonDown += TableView_OnPreviewMouseRightButtonDown;

        DataPager.Visibility = Visibility.Collapsed;

        this.Content = _layoutRoot;
    }

    public void SetItemsSource(IEnumerable enumerable)
    {
        this.GridControl.ItemsSource = null;

        IsUpdatedItemsSource = false;

        this.GridControl.BeginInit();
        this.GridControl.ItemsSource = enumerable;
        this.GridControl.EndInit();

        IsUpdatedItemsSource = true;
    }

    public void Add(ColumnItem item)
    {
        this.Columns.Add(GridColumnFactory.Create(item));
    }

    private void OnItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item is ColumnItem column)
                        {
                            this.Add(column);
                        }
                    }
                }

                break;
        }
    }

    #region Event & Functions 

    private static void OnDataItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid)
        {
            dataGrid.InvokeDataItemChanged(e.NewValue);
        }
    }

    private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
    {
        if (e.HitInfo.InRow)
        {
            var dataItem = GridControl.CurrentItem;
            if (dataItem == null) return;

            this.DataItem = dataItem;

            this.IsDoubleClicked = true;

            InvokeDataItemChanged(this.DataItem);

            this.IsDoubleClicked = false;
        }
    }

    private void TableView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var view = sender as TableView;
        if (view == null) return;

        var hitInfo = view.CalcHitInfo(e.OriginalSource as DependencyObject);
        if (!hitInfo.InRowCell)
            return;

        var row = GridControl.GetRow(hitInfo.RowHandle);
        var column = hitInfo.Column;

        this.DataItem = null;
        this.DataItem = row;
    }

    private void TableView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (DataGrid_PopupMenuOpening is null) return;

        var view = sender as TableView;
        if (view == null) return;

        var source = e.OriginalSource as DependencyObject;
        if (source == null) return;

        var row = GridRowHelper.GetRowData(source, TableView, GridControl);
        if (row == null) return;

        var popupMenu = new PopupMenu
        {
            PlacementTarget = GridControl,
            Placement = PlacementMode.MousePoint,
            DataContext = row
        };

        DataGrid_PopupMenuOpening.Invoke(this, new PopupMenuOpeningEventArgs(popupMenu, row, GridControl.CurrentColumn));

        popupMenu.PopupMenuClick += DataGrid_PopupMenuItemClick;
        popupMenu.IsOpen = true;
    }

    private void InvokeDataItemChanged(object? item)
    {
        if (item is null || !this.IsUpdatedItemsSource) return;

        var args = new DataItemChangedEventArgs(item, this.GridControl.CurrentColumn);
        this.DataGrid_DataItemChangedEvent?.Invoke(this, args);
    }

    #endregion
}

public partial class DataGrid
{
    public static readonly DependencyProperty PageSizeProperty =
        DependencyProperty.Register(nameof(PageSize), typeof(int), typeof(DataGrid), new PropertyMetadata(10));

    public int PageSize
    {
        get => (int)GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    public static readonly DependencyProperty TotalCountProperty =
        DependencyProperty.Register(nameof(TotalCount), typeof(int), typeof(DataGrid), new PropertyMetadata(0));

    public int TotalCount
    {
        get => (int)GetValue(TotalCountProperty);
        set => SetValue(TotalCountProperty, value);
    }

    public static readonly DependencyProperty ShowPagerProperty =
        DependencyProperty.Register(nameof(ShowPager), typeof(bool), typeof(DataGrid), new PropertyMetadata(false, OnShowPagerChanged));

    private static void OnShowPagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid)
        {
            dataGrid.SetDataPager();
        }
    }

    public bool ShowPager
    {
        get => (bool)GetValue(ShowPagerProperty);
        set => SetValue(ShowPagerProperty, value);
    }

    private DataPager DataPager = new DataPager();

    public event EventHandler<PageIndexChangedEventArgs>? DataGrid_PageIndexChanged;
        
    private void SetDataPager()
    {
        DataPager.Visibility = Visibility.Visible;
        DataPager.ShowTotalPageCount = true;

        DataPager.SetBinding(DataPager.PageSizeProperty, new Binding("PageSize") { Source = this });
        DataPager.SetBinding(DataPager.ItemCountProperty, new Binding("TotalCount") { Source = this });

        DataPager.PageIndexChanged += OnDataPager_PageIndexChanged;
    }

    private void OnDataPager_PageIndexChanged(object? sender, DataPagerPageIndexChangedEventArgs e)
    {
        if (e.NewValue is int pageIndex)
        {
            var args = new PageIndexChangedEventArgs(pageIndex);

            DataGrid_PageIndexChanged?.Invoke(this, args);
        }
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

public class PageIndexChangedEventArgs : EventArgs
{
    public int PageIndex { get; }

    public PageIndexChangedEventArgs(int pageIndex)
    {
        PageIndex = pageIndex;
    }
}

public class ColumnItemCollection : ObservableCollection<ColumnItem>
{

}