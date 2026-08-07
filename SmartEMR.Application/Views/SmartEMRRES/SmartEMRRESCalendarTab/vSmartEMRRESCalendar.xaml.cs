using DevExpress.Xpf.Grid;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;

/// <summary>
/// vSmartEMRRESCalendar.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESCalendar : ModelViewLayout<CalendarViewModel>
{
    public vSmartEMRRESCalendar() {  }

    protected override async void Initialize()
    {
        await UpdateCalendar();
    }

    protected override void SetViewLayout()
    {
        Calendar.TableView.CustomCellAppearance += TableView_OnCustomCellAppearance;
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public async Task UpdateCalendar()
    {
        await vm.UpdateCalendar();
    }

    private void TableView_OnCustomCellAppearance(object? sender, CustomCellAppearanceEventArgs e)
    {
        var view = sender as TableView;
        if (view is null) return;

        var row = Calendar.GridControl.GetRow(e.RowHandle) as CalendarRowItem;
        if (row is null) return;

        if (!DateTime.TryParse(e.Column?.FieldName, out var dt)) return;

        if (!row.Reservations.TryGetValue(dt.ToString("yyyy-MM-dd"), out var reservation)) return;

        if (SmartMVVM.Common.IsPast(dt.ToString("yyyy-MM-dd"), row.Time))
        {
            e.Result = new SolidColorBrush(Color.FromArgb(80, 255, 0, 0));
            e.Handled = true;
        }
    }

    private async void OnDrop_Calendar(object sender, CalendarDropEventArgs e)
    {
        var element = sender as Xpf.Calendar;
        if (element is null) return;

        var source = e.SourceCellData;
        var destination = e.DestinationCellData;

        if (!CanMove(source, destination)) return;

        var dataItem = e.SourceCellData;
        var messages = new List<Inline>();
        messages.AddRange(MessageBuilder.CreateReservationInfo(dataItem));
        messages.Add(new LineBreak());
        messages.Add(new InlineUIContainer(new Border
        {
            Width = 215,
            Height = 1,
            Background = Brushes.Gray,
            Opacity = 0.9,
            Margin = new Thickness(10, 4, 10, 4)
        }));
        messages.Add(new LineBreak());
        messages.Add(new Run("예약일시를 변경하시겠습니까?"));
        messages.Add(new LineBreak());
        messages.Add(new Run($"변경일시 : {destination.RES_ReservationDate} {destination.RES_ReservationTime}"));

        if (SmartUI.MsgYesNo(messages) is MessageBoxResult.No) return;

        await vm.MoveReservation(source, destination);
    }

    private void OnPopupMenuOpening_Calendar(object sender, PopupMenuOpeningEventArgs e)
    {
        var element = sender as Xpf.Calendar;
        if (element is null) return;

        var popup = e.PopupMenu;
        var dataItem = e.DataItem as Reservation;
        if (dataItem is null) return;

        if (SmartMVVM.Common.IsPast(dataItem.RES_ReservationDate, dataItem.RES_ReservationTime)) return;

        if (dataItem.RES_Idx.GetValueOrDefault(0) == 0)
        {
            popup.AddMenu(new PopupMenuItem { MenuAction = "AddRES", Content = "예약등록", Glyph = GlyphImage("Images/smartemr_register_pen.png") });
        }
        else
        {
            if (dataItem.RES_Status != "CNL")
            {
                popup.AddMenu(new PopupMenuItem { MenuAction = "EditRES", Content = "예약수정", Glyph = GlyphImage("Images/smartemr_edit_paper.png") });
                popup.AddMenu(new PopupMenuItem { MenuAction = "CancelRES", Content = "예약취소", Glyph = GlyphImage("Images/smartemr_cancel_new.png") });
            }
            else
            {
                popup.AddMenu(new PopupMenuItem { MenuAction = "ReAddRES", Content = "예약재등록", Glyph = GlyphImage("Images/smartemr_calendar_refresh.png") });
                popup.AddMenu(new PopupMenuItem { MenuAction = "DeleteRES", Content = "예약삭제", Glyph = GlyphImage("Images/smartemr_delete.png") });
            }

            popup.AddSeperator();
            popup.AddMenu(new PopupMenuItem { MenuAction = "EditPAT", Content = $"{dataItem.PAT_Name}님 정보수정", Glyph = GlyphImage("Images/smartemr_edit_patient.png") });
        }
    }

    private async void OnPopupMenuItemClick_Calendar(object sender, PopupMenuItemClickEventArgs e)
    {
        var element = sender as PopupMenu;
        if (element is null) return;

        var dataItem = e.DataItem as Reservation;
        if (dataItem is null) return;

        switch (e.MenuAction)
        {
            case "AddRES":
                await SmartUI.NavigateToPage(new vSmartEMRRESInfo(new Reservation { RES_ReservationDate = dataItem.RES_ReservationDate, RES_ReservationTime = dataItem.RES_ReservationTime}), isPopup:true);
                break;

            case "EditRES":
                await SmartUI.NavigateToPage(new vSmartEMRRESInfo(new Reservation { RES_Idx = dataItem.RES_Idx, PAT_Idx = dataItem.PAT_Idx }), isPopup: true);
                break;

            case "CancelRES":
                await vm.SetReservationByStatus(dataItem, "CNL");
                break;

            case "ReAddRES":
                await vm.SetReservationByStatus(dataItem, "CNF");
                break;

            case "DeleteRES":
                await vm.DeleteRES(dataItem);
                break;
        }
    }

    private bool CanMove(Reservation source, Reservation destination)
    {
        if (source.RES_Idx == destination.RES_Idx)
        {
            return false;
        }

        if (SmartMVVM.Common.IsPast(destination.RES_ReservationDate, destination.RES_ReservationTime))
        {
            SmartUI.SetNofification("과거일시로는 변경하실 수 없습니다.", NotificationType.Warning);
            return false;
        }

        if (destination.RES_Idx.GetValueOrDefault(0) > 0)
        {
            SmartUI.SetNofification("해당 시간에는 이미 예약이 존재합니다.", NotificationType.Warning);
            return false;
        }

        return true;
    }
}