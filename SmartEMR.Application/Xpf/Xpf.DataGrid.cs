using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using DevExpress.XtraRichEdit.Import.OpenDocument;
using SmartEMR.Application.Core;
using SmartEMR.Application.Resources;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace SmartEMR.Application.Xpf;

[ObservableObject]
[ContentProperty(nameof(Items))]
public partial class DataGrid : ContentControl
{
    private bool IsUpdatedItemsSource { get; set; } = false;

    public event EventHandler<DataItemChangedEventArgs>? DataGrid_DataItemChangedEvent;

    public DevExpress.Xpf.Grid.GridControl GridControl { get; private set; }
    public DevExpress.Xpf.Grid.TableView TableView { get; private set; } 

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

    public GridColumnCollection Columns => GridControl.Columns;

    public event EventHandler<PopupMenuOpeningEventArgs>? DataGrid_PopupMenuOpening;
    public event EventHandler<PopupMenuItemClickEventArgs>? DataGrid_PopupMenuItemClick;

    public DataGrid()
    {
        GridControl = new();
        TableView = new();

        GridControl.View = TableView;
        GridControl.Margin = new Thickness(2);
        GridControl.AllowInitiallyFocusedRow = false;
        GridControl.CurrentItemChanged += (s, e) => this.DataItem = GridControl.CurrentItem;

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
        TableView.MouseLeftButtonDown += TableView_OnMouseLeftButtonDown;
        TableView.PreviewMouseRightButtonDown += TableView_OnPreviewMouseRightButtonDown;

        this.Content = GridControl;
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
        StyleGridColumn element = new StyleGridColumn();

        if (item.ColumnStyle != null)
        {
            SetHorizontalAlignment(item);
        }

        element.FieldName = item.FieldName;
        element.Header = item.Header;
        element.Width = item.ColumnWidth > 0 ? new GridColumnWidth(item.ColumnWidth, GridColumnUnitType.Pixel) : new GridColumnWidth(1, GridColumnUnitType.Star);
        element.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
        element.CellTemplate = GetCellTemplate(item);
        element.ColumnItem = item;
        element.AllowSorting = item.AllowSorting ? DefaultBoolean.True : DefaultBoolean.False;

        this.Columns.Add(element);
    }

    private void SetHorizontalAlignment(ColumnItem item)
    {
        item.HorizontalAlignment = item.ColumnStyle switch
        {
            ColumnStyle.Name => HorizontalAlignment.Left,
            ColumnStyle.Code => HorizontalAlignment.Center,
            ColumnStyle.YYMMDD => HorizontalAlignment.Center,
            ColumnStyle.Sum => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Left
        };
    }

    private DataTemplate? GetCellTemplate(ColumnItem item)
    {
        var template = new DataTemplate();

        if (item.CellTemplateType != null)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(item.CellTemplateType))
            {
                Debug.WriteLine($"{item.CellTemplateType.Name}은 FrameworkElement를 상속해야 합니다.");
                return null;
            }

            template = CreateTemplate(item.CellTemplateType);
        }
        else
        {
            string resourceKey = item.ColumnType switch
            {
                ColumnType.Label => "GridColumnLabelTemplate",
                ColumnType.TextBox => "GridColumnTextBoxTemplate",
                ColumnType.CheckBox => "GridColumnCheckBoxTemplate",
                ColumnType.TextLink => "GridColumnTextLinkTemplate",
                _ => "GridColumnLabelTemplate" // 기본값
            };

            template = SmartResourceDictionary.GetStaticResource<DataTemplate>(TargetResource.DataGridCell, resourceKey);
        }

        return template;
    }

    private DataTemplate CreateTemplate(Type templateType)
    {
        var presenter = new FrameworkElementFactory(typeof(ContentPresenter));
        var innerTemplate = new DataTemplate();
        innerTemplate.VisualTree = new FrameworkElementFactory(templateType);

        presenter.SetBinding(ContentPresenter.ContentProperty, new Binding("RowData.Row"));
        presenter.SetValue(ContentPresenter.ContentTemplateProperty, innerTemplate);

        return new DataTemplate { VisualTree = presenter };
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

    private void TableView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var view = sender as TableView;
        if (view == null) return;

        var hitInfo = view.CalcHitInfo(e.OriginalSource as DependencyObject);
        if (!hitInfo.InRowCell)
            return;

        var row = GridControl.GetRow(hitInfo.RowHandle);
        var column = hitInfo.Column;

        this.DataItem = row;
        this.InvokeDataItemChanged(row);
    }

    private void TableView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var view = sender as TableView;
        if (view == null) return;

        var source = e.OriginalSource as DependencyObject;
        if (source == null) return;

        var row = GetRowData(source);
        if (row == null) return;

        if (DataGrid_PopupMenuOpening != null)
        {
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
    }

    private void InvokeDataItemChanged(object? item)
    {
        if (!this.IsUpdatedItemsSource) return;

        var args = new DataItemChangedEventArgs(item, this.GridControl.CurrentColumn);
        this.DataGrid_DataItemChangedEvent?.Invoke(this, args);
    }

    private object? GetRowData(DependencyObject source)
    {
        var hitInfo = TableView.CalcHitInfo(source);
        if (hitInfo == null || !hitInfo.InRowCell) return null;

        return GridControl.GetRow(hitInfo.RowHandle);
    }

    #endregion
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

public class ColumnItemCollection : ObservableCollection<ColumnItem>
{

}