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
using System.Diagnostics;
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

    public event EventHandler<PopupMenuOpeningEventArgs>? Calendar_PopupMenuOpening;
    public event EventHandler<PopupMenuItemClickEventArgs>? Calendar_PopupMenuItemClick;

    public GridControl GridControl { get; set; } = new();
    public TableView TableView { get; set; } = new();
    
    private DataTemplate? _headerItemTemplate = null;
    private bool _initialized = false;

    public Calendar()
    {
        InitializeTemplate();

        GridControl.View = TableView;
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
        TableView.AllowDragDrop = true;
        TableView.AllowCellMerge = false;
        TableView.EnableImmediatePosting = true;
        TableView.ShowDragDropHint = false;
        TableView.IsColumnMenuEnabled = false;
        TableView.PreviewMouseRightButtonDown += TableView_OnPreviewMouseRightButtonDown;

        VirtualizingStackPanel.SetIsVirtualizing(GridControl, false);
        VirtualizingStackPanel.SetIsVirtualizing(TableView, false);

        this.Content = GridControl;

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
}

public class CalendarHeaderItem
{
    public DateTime Date { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}

public partial class CalendarRowItem : ObservableObject
{
    [ObservableProperty]
    private string? time;
    [ObservableProperty]
    private Dictionary<string, Reservation> reservations = new();
}