using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections;
using DevExpress.Xpf.Grid;
using SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;
using SmartEMR.Application.Core;
using SmartEMR.Application.Resources;
using System.Windows.Input;
using System.Diagnostics;
using SmartEMR.Domain.Entities;
using System.Windows.Controls;

namespace SmartEMR.Application.Xpf;

public enum CalendarMode
{
    Week,
    Day
}

public class Calendar : CustomControl
{
    public static readonly DependencyProperty StartDayProperty =
        DependencyProperty.Register(nameof(StartDay), typeof(DateTime), typeof(Calendar), new PropertyMetadata(DateTime.Today));

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

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public CalendarMode CalendarMode
    {
        get => (CalendarMode)GetValue(CalendarModeProperty);
        set => SetValue(CalendarModeProperty, value);
    }


    public GridControl GridControl { get; set; } = new();
    public TableView TableView { get; set; } = new();
    
    private DataTemplate? _headerItemTemplate = null;
    private DataTemplate? _calenderItemTemplate = null;

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
        TableView.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown_TableView;

        this.Content = GridControl;

        SetCalendar();
    }

    private void InitializeTemplate()
    {
        _headerItemTemplate = SmartResourceDictionary.GetStaticResource<DataTemplate>(TargetResource.Calendar, "CalendarHeaderItemTemplate");
        _calenderItemTemplate = SmartResourceDictionary.GetStaticResource<DataTemplate>(TargetResource.Calendar, "CalendarItemTemplate");
    }

    private void SetCalendar() 
    {
        GridControl.Columns.Clear();
        GridControl.Columns.Add(GridColumnFactory.Create(new ColumnItem { FieldName = "Time", Header = "", ColumnType = ColumnType.Label, ColumnWidth = 80, FontSize = 15, FontWeight = FontWeights.SemiBold, Foreground = Brushes.DimGray, HorizontalAlignment = HorizontalAlignment.Center }));

        // 주별 캘린더
        if (CalendarMode == CalendarMode.Week)
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
    } 

    private DataTemplate CreateCalendarItemTemplate(DateTime day)
    {
        var factory = new FrameworkElementFactory(typeof(ReservationCalendarCellItem));
        factory.SetBinding(ReservationCalendarCellItem.ReservationProperty, new Binding($"RowData.Row.Reservations[{day:yyyy-MM-dd}]"));

        return new DataTemplate
        {
            VisualTree = factory
        };
    }

    private void OnPreviewMouseLeftButtonDown_TableView(object sender, MouseButtonEventArgs e)
    {
        var view = sender as TableView;
        if (view is null) return;

        var hitInfo = view.CalcHitInfo(e.OriginalSource as DependencyObject);
        if (!hitInfo.InRowCell) return;

        var row = GridControl.GetRow(hitInfo.RowHandle) as CalendarRowItem;
        if (row is null) return;
        
        foreach (var item in row.Reservations)
        {
            if (SmartMVVM.Common.IsToday(item.Key) && item.Value is Reservation reservation)
            {
            }
        }
    }
}