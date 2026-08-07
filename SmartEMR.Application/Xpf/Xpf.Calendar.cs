using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Collections;
using SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;
using SmartEMR.Application.Core;
using SmartEMR.Application.Resources;
using SmartEMR.Domain.Entities;
using DevExpress.Xpf.Grid;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartEMR.Application.Xpf;

public enum CalendarMode
{
    Week,
    Month
}

public class Calendar : CustomControl
{
    public static readonly DependencyProperty StartDayProperty =
        DependencyProperty.Register(nameof(StartDay), typeof(DateTime), typeof(Calendar), new PropertyMetadata(DateTime.Today, OnStartDayChanged));

    private static void OnStartDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Calendar calendar && e.NewValue is not null)
        {
            calendar.SetCalendar();
        }
    }

    public DateTime StartDay
    {
        get => (DateTime)GetValue(StartDayProperty);
        set => SetValue(StartDayProperty, value);
    }

    public static readonly DependencyProperty DisplayDaysProperty =
        DependencyProperty.Register(nameof(DisplayDays), typeof(int), typeof(Calendar), new PropertyMetadata(7));

    public int DisplayDays
    {
        get => (int)GetValue(DisplayDaysProperty);
        set => SetValue(DisplayDaysProperty, value);
    }

    public static readonly DependencyProperty StartTimeProperty =
        DependencyProperty.Register(nameof(StartTime), typeof(TimeSpan), typeof(Calendar), new PropertyMetadata(new TimeSpan(0, 0, 0)));

    public TimeSpan StartTime
    {
        get => (TimeSpan)GetValue(StartTimeProperty);
        set => SetValue(StartTimeProperty, value);
    }


    public static readonly DependencyProperty EndTimeProperty =
        DependencyProperty.Register(nameof(EndTime), typeof(TimeSpan), typeof(Calendar), new PropertyMetadata(new TimeSpan(23, 30, 0)));

    public TimeSpan EndTime
    {
        get => (TimeSpan)GetValue(EndTimeProperty);
        set => SetValue(EndTimeProperty, value);
    }

    public static readonly DependencyProperty SlotIntervalProperty =
        DependencyProperty.Register(nameof(SlotInterval), typeof(int), typeof(Calendar), new PropertyMetadata(30));

    public int SlotInterval
    {
        get => (int)GetValue(SlotIntervalProperty);
        set => SetValue(SlotIntervalProperty, value);
    }


    public static readonly DependencyProperty CalendarModeProperty =
        DependencyProperty.Register(nameof(CalendarMode), typeof(CalendarMode), typeof(Calendar), new PropertyMetadata(CalendarMode.Week, OnCalendarModeChanged));

    private static void OnCalendarModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Calendar calendar)
        {
            //calendar.RefreshCalendar();
        }
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(Calendar), new PropertyMetadata(null));

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public CalendarMode CalendarMode
    {
        get => (CalendarMode)GetValue(CalendarModeProperty);
        set => SetValue(CalendarModeProperty, value);
    }

    public event EventHandler<CalendarDropEventArgs>? Calendar_Drop;

    public event EventHandler<PopupMenuOpeningEventArgs>? Calendar_PopupMenuOpening;
    public event EventHandler<PopupMenuItemClickEventArgs>? Calendar_PopupMenuItemClick;

    public GridControl GridControl { get; set; } = new();
    public TableView TableView { get; set; } = new();
    public Canvas DragOverlay { get; set; } = new();

    private Reservation? _dragReservation;
    private ReservationCalendarCellPreviewItem? _dragPreview;

    private bool _isDragging;

    private DataTemplate? _headerItemTemplate = null;
    private bool _initialized = false;

    public Calendar()
    {
        InitializeTemplate();

        var layoutRoot = new StyleGrid();

        layoutRoot.AddElement(GridControl, 0, 0);
        layoutRoot.AddElement(DragOverlay, 0, 0);

        GridControl.View = TableView;
        GridControl.AllowDrop = true;
        GridControl.DragOver += GridControl_OnDragOver;
        GridControl.Drop += GridControl_OnDrop;
        GridControl.SetBinding(GridControl.ItemsSourceProperty, new Binding("ItemsSource") { Source = this });

        TableView.HeaderPanelMinHeight = 45;
        TableView.RowMinHeight = 48;
        TableView.ShowGroupPanel = false;
        TableView.ShowIndicator = false;
        TableView.AllowEditing = false;
        TableView.AllowHorizontalScrollingVirtualization = false;
        TableView.AllowColumnFiltering = false;
        TableView.AllowColumnMoving = false;
        TableView.AllowSorting = false;
        TableView.AllowCellMerge = false;
        TableView.EnableImmediatePosting = true;
        TableView.ShowDragDropHint = false;
        TableView.IsColumnMenuEnabled = false;
        TableView.PreviewMouseRightButtonDown += TableView_OnPreviewMouseRightButtonDown;

        DragOverlay.HorizontalAlignment = HorizontalAlignment.Stretch;
        DragOverlay.VerticalAlignment = VerticalAlignment.Stretch;
        DragOverlay.IsHitTestVisible = false;

        VirtualizingStackPanel.SetIsVirtualizing(GridControl, false);
        VirtualizingStackPanel.SetIsVirtualizing(TableView, false);

        this.Content = layoutRoot;

        SetCalendar();
    }

    private void InitializeTemplate()
    {
        _headerItemTemplate = SmartResourceDictionary.GetStaticResource<DataTemplate>(TargetResource.Calendar, "CalendarHeaderItemTemplate");
    }

    private void SetCalendar() 
    {
        GridControl.BeginDataUpdate();

        if (!_initialized)
        {
            GridControl.Columns.Clear();
            GridControl.Columns.Add(GridColumnFactory.Create(new ColumnItem { FieldName = "Time", Header = "", ColumnType = ColumnType.Label, ColumnWidth = 80, FontSize = 15, FontWeight = FontWeights.SemiBold, Foreground = Brushes.DimGray, HorizontalAlignment = HorizontalAlignment.Center }));

            SetColumns();
        }
        else
        {
            SetColumns();
        }

        GridControl.EndDataUpdate();
        GridControl.RefreshData();

        _initialized = true;
    } 

    private void SetColumns()
    {
        // 주별 캘린더
        if (CalendarMode == CalendarMode.Week)
        {
            if (!_initialized)
            {
                for (int i = 0; i < DisplayDays; i++)
                {
                    DateTime dt = StartDay.AddDays(i);

                    var column = new GridColumn
                    {
                        FieldName = dt.ToString("yyyy-MM-dd"),
                        Header = new CalendarHeaderItem { Date = dt, DayOfWeek = dt.DayOfWeek },
                        HeaderTemplate = _headerItemTemplate,
                        HorizontalHeaderContentAlignment = HorizontalAlignment.Stretch,
                        Width = new GridColumnWidth(1, GridColumnUnitType.Star),
                        CellTemplate = CreateCalendarItemTemplate(dt),
                    };

                    GridControl.Columns.Add(column);
                }
            }
            else
            {
                var dateHeaders = GridControl.Columns.Where(x => x.FieldName != "Time").ToList();

                for (int i = 0; i < DisplayDays; i++)
                {
                    DateTime dt = StartDay.AddDays(i);

                    dateHeaders[i].FieldName = dt.ToString("yyyy-MM-dd");
                    dateHeaders[i].Header = new CalendarHeaderItem { Date = dt, DayOfWeek = dt.DayOfWeek };
                }
            }
        }
    }

    private DataTemplate CreateCalendarItemTemplate(DateTime day)
    {
        return new DataTemplate
        {
            VisualTree = new FrameworkElementFactory(typeof(ReservationCalendarCellItem))
        };
    }

    private void GridControl_OnDragOver(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(typeof(Reservation)))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        var dateInfo = GetDropCellDateInfo(e);
        if (dateInfo is null)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        if (SmartMVVM.Common.IsPast(dateInfo.GetValueOrDefault()))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void GridControl_OnDrop(object sender, DragEventArgs e)
    {
        if (sender is not GridControl element) return;

        Point point = e.GetPosition(element);
        var hitInfo = TableView.CalcHitInfo(point);

        if (hitInfo.InRowCell)
        {
            var row = element.GetRow(hitInfo.RowHandle) as CalendarRowItem;
            if (row is null) return;

            var source = e.Data.GetData(typeof(Reservation)) as Reservation;
            if (source is null) return;

            var dateInfo = GetDropCellDateInfo(e).GetValueOrDefault();
            if (dateInfo == default) return;

            if (SmartMVVM.Common.IsPast(dateInfo)) return;

            if (row.Reservations.TryGetValue(hitInfo.Column.FieldName, out var destination))
            {
                EndDrag();

                Calendar_Drop?.Invoke(this, new CalendarDropEventArgs(source, destination));
            }
        }
    }

    private DateTime? GetDropCellDateInfo(DragEventArgs e)
    {
        Point point = e.GetPosition(GridControl);

        var hitInfo = TableView.CalcHitInfo(point);
        if (!hitInfo.InRowCell) return null;

        var row = GridControl.GetRow(hitInfo.RowHandle) as CalendarRowItem;
        if (row is null) return null;

        var dt = hitInfo.Column.FieldName + " " + row.Time;
        if (!DateTime.TryParse(dt, out var result)) return null;

        return result;
    }

    private void TableView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (Calendar_PopupMenuOpening is null) return;

        var view = sender as TableView;
        if (view is null) return;

        var source = e.OriginalSource as DependencyObject;
        if (source is null) return;

        var row = GridRowHelper.GetRowData(source, TableView, GridControl) as CalendarRowItem;
        if (row is null) return;

        var column = GridRowHelper.GetColumn(source, TableView);
        var dataItem = row.Reservations[column.FieldName] as Reservation;
        if (dataItem is null) return;

        var popupMenu = new PopupMenu
        {
            PlacementTarget = GridControl,
            Placement = PlacementMode.MousePoint,
            DataContext = dataItem
        };

        Calendar_PopupMenuOpening.Invoke(this, new PopupMenuOpeningEventArgs(popupMenu, dataItem, GridControl.CurrentColumn));

        popupMenu.PopupMenuClick += Calendar_PopupMenuItemClick;
        popupMenu.IsOpen = true;
    }

    public void StartDrag(Reservation reservation, UIElement? dragElement)
    {
        if (dragElement is null) return;
        if (_isDragging) return;

        _isDragging = true;

        _dragReservation = reservation;

        _dragPreview = new ReservationCalendarCellPreviewItem
        {
            Width = dragElement.RenderSize.Width,
            Height = dragElement.RenderSize.Height,
            DataContext = reservation
        };

        DragOverlay.Children.Add(_dragPreview);

        Canvas.SetLeft(_dragPreview, 0);
        Canvas.SetTop(_dragPreview, 0);
    }

    public void MoveDrag(Point point)
    {
        if (_dragPreview is null) return;

        Canvas.SetLeft(_dragPreview, point.X - 60);
        Canvas.SetTop(_dragPreview, point.Y - 25);
    }

    public Reservation? EndDrag()
    {
        Reservation? result = _dragReservation;

        if (_dragPreview is not null)
        {
            DragOverlay.Children.Remove(_dragPreview);
            _dragPreview = null;
        }

        _dragReservation = null;
        _isDragging = false;

        return result;
    }
}

public class CalendarHeaderItem
{
    public DateTime Date { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}

public partial class CalendarRowItem : ObservableObject
{
    [ObservableProperty]
    private string time = "";
    [ObservableProperty]
    private Dictionary<string, Reservation> reservations = new();
}

public class CalendarDropEventArgs : EventArgs
{
    public Reservation SourceCellData { get; }
    public Reservation DestinationCellData { get; }

    public CalendarDropEventArgs(Reservation source, Reservation destination)
    {
        SourceCellData = source;
        DestinationCellData = destination;
    }
}