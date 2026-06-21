using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Xpf.Grid;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace SmartEMR.Application.Xpf;

[ObservableObject]
[ContentProperty(nameof(Items))]
public partial class DataGrid : ContentControl
{
    private ResourceDictionary? _templateResource;

    public event EventHandler<DataItemChangedEventArgs>? DataGrid_DataItemChangedEvent;

    public DevExpress.Xpf.Grid.GridControl GridControl { get; private set; } = new();
    public DevExpress.Xpf.Grid.TableView TableView { get; private set; } = new();

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IQueryable), typeof(DataGrid), new PropertyMetadata(null, OnItemsSourceChanged));

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid && e.NewValue != null)
        {
            if (e.NewValue is IQueryable queryable)
            {
                dataGrid.SetItemsSource(queryable);
            }
        }
    }

    public IQueryable ItemsSource
    {
        get => (IQueryable)GetValue(ItemsSourceProperty);
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

    public GridColumnCollection Columns => GridControl.Columns;

    public DataGrid()
    {
        this.Content = GridControl;

        GridControl.View = TableView;
        GridControl.CurrentItemChanged += (s,e) => this.DataItem = GridControl.CurrentItem;
        GridControl.CurrentColumnChanged += (s, e) => InvokeDataItemChanged(this.DataItem);

        TableView.RowMinHeight = 24;
        TableView.HeaderPanelMinHeight = 18;
        TableView.AllowEditing = false;
        TableView.AllowHorizontalScrollingVirtualization = false;
        TableView.AutoWidth = true;
        TableView.ShowGroupPanel = false;
        TableView.ShowAutoFilterRow = false;
        TableView.ShowIndicator = false;
        TableView.RowDoubleClick += OnTableView_RowDoubleClick;

        SetTemplateResource();
    }

    public void SetItemsSource(IQueryable queryable)
    {
        this.GridControl.ItemsSource = null;

        this.GridControl.BeginInit();
        this.GridControl.ItemsSource = queryable;
        this.GridControl.EndInit();
    }

    private void SetTemplateResource()
    {
        _templateResource = new ResourceDictionary
        {
            Source = new Uri("../Template/DataGridCellTemplates.xaml", UriKind.RelativeOrAbsolute)
        };
    }

    public void Add(ColumnItem item)
    {
        // 셀 요소 기본 설정
        GridColumn element = new GridColumn();

        element.FieldName = item.FIeldName;
        element.Header = item.Header;
        element.Width = item.ColumnWidth;
        element.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;

        if (item.Template != null)
        {
            element.CellTemplate = item.Template;
        }
        else
        {
            element.CellTemplate = GetCellTemplate(item);
        }

        // 동적 속성 스타일 설정
        Style cellStyle = new Style(typeof(LightweightCellEditor));
        
        if (item.ColumnStyle == null)
        {
            cellStyle.Setters.Add(new Setter(HorizontalAlignmentProperty, item.HorizontalAlignment));
        }
        else
        {
            SetColumnStyle(cellStyle, (ColumnStyle)item.ColumnStyle);
        }

        cellStyle.Setters.Add(new Setter(VerticalAlignmentProperty, VerticalAlignment.Center));
        cellStyle.Setters.Add(new Setter(TextElement.FontSizeProperty, item.FontSize));
        cellStyle.Setters.Add(new Setter(TextElement.FontWeightProperty, item.FontWeight));
        cellStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, item.Foreground));

        element.CellStyle = cellStyle;

        this.Columns.Add(element);
    }

    private DataTemplate? GetCellTemplate(ColumnItem item)
    {
        // 템플릿 로드가 제대로 되지 않은 경우 한 번 더 로드
        if (_templateResource == null)
        {
            SetTemplateResource();
        }

        // 다시 로드해도 못 찾은 경우 반환
        if (_templateResource == null) return default!;

        string resourceKey = item.ColumnType switch
        {
            ColumnType.Label => "GridColumnLabelTemplate",
            ColumnType.TextBox => "GridColumnTextBoxTemplate",
            ColumnType.CheckBox => "GridColumnCheckBoxTemplate",
            ColumnType.TextLink => "GridColumnTextLinkTemplate",
            _ => "GridColumnLabelTemplate" // 기본값
        };

        var template = _templateResource[resourceKey] as DataTemplate;

        return template;
    }

    private void SetColumnStyle(Style cellStyle, ColumnStyle columnStyle)
    {
        var hAlign = columnStyle switch
        {
            ColumnStyle.Name => HorizontalAlignment.Left,
            ColumnStyle.Code => HorizontalAlignment.Center,
            ColumnStyle.Sum  => HorizontalAlignment.Right,
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

    private void OnTableView_RowDoubleClick(object sender, RowDoubleClickEventArgs e)
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