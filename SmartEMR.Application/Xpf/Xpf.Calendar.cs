using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;
using System.Diagnostics;
using System.Windows;

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


    public CalendarMode CalendarMode
    {
        get => (CalendarMode)GetValue(CalendarModeProperty);
        set => SetValue(CalendarModeProperty, value);
    }


    public GridControl GridControl { get; set; } = new();
    public TableView TableView { get; set; } = new();

    private DataTemplate? _headerItemTemplate = null;

    public Calendar()
    {
        _headerItemTemplate = FindResource("CalendarHeaderItemTemplate") as DataTemplate; 

        GridControl.View = TableView;

        TableView.HeaderPanelMinHeight = 45;
        TableView.RowMinHeight = 45;
        TableView.ShowGroupPanel = false;
        TableView.ShowIndicator = false;
        TableView.AllowEditing = false;
        TableView.AllowHorizontalScrollingVirtualization = false;
        TableView.AllowColumnFiltering = false;
        TableView.AllowColumnMoving = false;
        TableView.AllowSorting = false;
        TableView.IsColumnMenuEnabled = false;

        this.Content = GridControl;

        SetCalendar();
    }

    private void SetCalendar()
    {
        GridControl.Columns.Clear();

        GridControl.Columns.Add(new GridColumn
        {
            FieldName = "RES_Time",
            Header = "",
            Width = 100
        });

        // 주별 캘린더
        if (CalendarMode == CalendarMode.Week)
        {
            for (int i = 0; i < DisplayDays; i++)
            {
                DateTime dt = StartDay.AddDays(i);

                var column = new GridColumn
                {
                    FieldName = dt.ToString("yyyyMMdd"),
                    Header = new CalendarHeaderItem { Date = dt, DayOfWeek = dt.DayOfWeek },
                    HorizontalHeaderContentAlignment = HorizontalAlignment.Stretch,
                    Width = new GridColumnWidth(1, GridColumnUnitType.Star)
                };

                column.HeaderTemplate = _headerItemTemplate;

                GridControl.Columns.Add(column);

                Debug.WriteLine(column.Header);
                Debug.WriteLine(column.HeaderTemplate);
            }
        }
    } 
}