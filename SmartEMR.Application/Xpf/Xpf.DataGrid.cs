using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using SmartEMR.Application.Core;
using SmartEMR.Application.Resources;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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

    public DevExpress.Xpf.Grid.GridControl GridControl { get; private set; } = new();
    public DevExpress.Xpf.Grid.TableView TableView { get; private set; } = new();

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

    public static readonly DependencyProperty PopupMenuProperty =
        DependencyProperty.Register(nameof(RowPopupMenu), typeof(PopupMenu), typeof(DataGrid), new PropertyMetadata(null));

    public PopupMenu RowPopupMenu
    {
        get => (PopupMenu)GetValue(PopupMenuProperty);
        set => SetValue(PopupMenuProperty, value);
    }

    public static readonly DependencyProperty IsDoubleClickedProperty =
        DependencyProperty.Register(nameof(IsDoubleClicked), typeof(bool), typeof(DataGrid), new PropertyMetadata(false));

    public bool IsDoubleClicked
    {
        get => (bool)GetValue(IsDoubleClickedProperty);
        set => SetValue(IsDoubleClickedProperty, value);
    }

    public GridColumnCollection Columns => GridControl.Columns;

    public DataGrid()
    {
        this.Content = GridControl;

        GridControl.View = TableView;
        GridControl.CurrentItemChanged += (s, e) => this.DataItem = GridControl.CurrentItem;

        TableView.RowMinHeight = 24;
        TableView.HeaderPanelMinHeight = 18;
        TableView.AllowEditing = false;
        TableView.AllowHorizontalScrollingVirtualization = false;
        TableView.AutoWidth = true;
        TableView.ShowGroupPanel = false;
        TableView.ShowAutoFilterRow = false;
        TableView.ShowIndicator = false;
        TableView.NavigationStyle = GridViewNavigationStyle.Cell;
        TableView.RowDoubleClick += TableView_OnRowDoubleClick;
        TableView.MouseDown += TableView_OnMouseDown;
        TableView.PreviewMouseRightButtonDown += TableView_OnPreviewMouseRightButtonDown;
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
        // 셀 요소 기본 설정
        GridColumn element = new GridColumn();

        element.FieldName = item.FieldName;
        element.Header = item.Header;
        element.Width = item.ColumnWidth;
        element.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
        element.CellTemplate = GetCellTemplate(item);

        if (item.CellTemplateType != null)
        {
            this.Columns.Add(element);
            return;
        }

        // 동적 속성 스타일 설정
        Style cellStyle = new Style(typeof(LightweightCellEditor));

        if (item.ColumnStyle != null)
        {
            SetColumnStyle(cellStyle, (ColumnStyle)item.ColumnStyle);
        }
        else
        {
            cellStyle.Setters.Add(new Setter(HorizontalAlignmentProperty, item.HorizontalAlignment));
            cellStyle.Setters.Add(new Setter(VerticalAlignmentProperty, VerticalAlignment.Center));
            cellStyle.Setters.Add(new Setter(TextElement.FontSizeProperty, item.FontSize));
            cellStyle.Setters.Add(new Setter(TextElement.FontWeightProperty, item.FontWeight));
            cellStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, item.Foreground));
        }

        element.CellStyle = cellStyle;

        this.Columns.Add(element);
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

    private void SetColumnStyle(Style cellStyle, ColumnStyle columnStyle)
    {
        var hAlign = columnStyle switch
        {
            ColumnStyle.Name => HorizontalAlignment.Left,
            ColumnStyle.Code => HorizontalAlignment.Center,
            ColumnStyle.Sum => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Left
        };

        cellStyle.Setters.Add(new Setter(HorizontalAlignmentProperty, hAlign));
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

    private void TableView_OnMouseDown(object sender, MouseButtonEventArgs e)
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

        var hitInfo = TableView.CalcHitInfo(e.OriginalSource as DependencyObject);
        if (hitInfo == null || !hitInfo.InRowCell) return;

        var row = GridControl.GetRow(hitInfo.RowHandle);

        DataItem = row;
    }

    private void InvokeDataItemChanged(object? item)
    {
        if (!this.IsUpdatedItemsSource) return;

        var args = new DataItemChangedEventArgs(item, this.GridControl.CurrentColumn);
        this.DataGrid_DataItemChangedEvent?.Invoke(this, args);
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