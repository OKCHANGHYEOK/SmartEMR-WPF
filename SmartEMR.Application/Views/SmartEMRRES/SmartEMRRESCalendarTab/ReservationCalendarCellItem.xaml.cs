using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using static SmartEMR.Application.Core.MousePointHelper;

namespace SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;

/// <summary>
/// ReservationCalendarCellItem.xaml에 대한 상호 작용 논리
/// </summary>
public partial class ReservationCalendarCellItem : CustomControl
{

    public static readonly DependencyProperty ReservationProperty =
        DependencyProperty.Register(nameof(Reservation), typeof(Reservation), typeof(ReservationCalendarCellItem), new PropertyMetadata(null));

    public Reservation Reservation
    {
        get => (Reservation)GetValue(ReservationProperty);
        set => SetValue(ReservationProperty, value);
    }

    private Xpf.Calendar? ParentCalendar;
    private Reservation? _dragReservation;
    private bool _isDragging;

    public ReservationCalendarCellItem()
    {
        this.GiveFeedback += OnReservationCalendarCellItem_GiveFeedBack;
        this.Loaded += (s, e) =>
        {
            ParentCalendar = SmartUI.FindParent<Xpf.Calendar>(this);
        };
    }

    private void OnReservationCalendarCellItem_GiveFeedBack(object sender, GiveFeedbackEventArgs e)
    {
        if (ParentCalendar is null) return;

        // 수정된 부분: Win32 API를 사용해 화면 절대 좌표를 가져옵니다.
        Win32Point w32Mouse = new Win32Point();

        MousePointHelper.GetCursorPos(ref w32Mouse);

        Point screenPoint = new Point(w32Mouse.X, w32Mouse.Y);

        // 화면 절대 좌표를 DragOverlay 컨트롤 기준의 상대 좌표로 변환합니다.
        Point point = ParentCalendar.DragOverlay.PointFromScreen(screenPoint);

        ParentCalendar.MoveDrag(point);

        e.UseDefaultCursors = true;
        e.Handled = true;
    }

    private void OnReservationCalendarCellItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is Reservation reservation)
        {
            _dragReservation = reservation;
        }
    }

    private void OnReservationCalendarCellItem_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
            return;

        if (_dragReservation is null)
            return;

        Point current = e.GetPosition(ParentCalendar?.DragOverlay);

        if (!_isDragging)
        {
            _isDragging = true;

            ParentCalendar?.StartDrag(_dragReservation, sender as UIElement);

            DragDrop.DoDragDrop(this, _dragReservation, DragDropEffects.Move);

            ParentCalendar?.EndDrag();

            _isDragging = false;
        }
    }
}

public class DictionaryValueConverter : MarkupExtension, IMultiValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2) return default!;

        if (values[0] is IDictionary<string, Reservation> dict && values[1] is string key)
        {
            if (dict.TryGetValue(key, out var value) && value is not null) 
            { 
                return value;
            }
        }

        return default!;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}