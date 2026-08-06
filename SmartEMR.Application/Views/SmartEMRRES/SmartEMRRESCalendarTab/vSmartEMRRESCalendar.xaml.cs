using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

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

    private void OnPopupMenuOpening_Calendar(object sender, PopupMenuOpeningEventArgs e)
    {
        var element = sender as Calendar;
        if (element is null) return;

        var popup = e.PopupMenu;
        var dataItem = e.DataItem as Reservation;
        if (dataItem is null) return;

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
}