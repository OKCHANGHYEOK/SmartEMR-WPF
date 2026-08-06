using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class CalendarViewModel : ReservationViewModel
{
    [ObservableProperty]
    private DateTime startDay;
    [ObservableProperty]
    private int displayDays;
    [ObservableProperty]
    private TimeSpan startTime;
    [ObservableProperty]
    private TimeSpan endTime;
    [ObservableProperty]
    private ObservableCollection<CalendarRowItem> calendarItems;

    [ObservableProperty]
    private int pendingCount;
    [ObservableProperty]
    private int confirmedCount;
    [ObservableProperty]
    private int visitCount;
    [ObservableProperty]
    private int canceledCount;

    private List<DateTime> days = new();

    public CalendarViewModel()
    {
        StartDay = DateTime.Today;
        DisplayDays = 7;

        StartTime = TimeSpan.FromHours(7);
        EndTime = TimeSpan.FromHours(24);

        CalendarItems = new();

        SetDays();
    }

    public override async Task<bool> FetchDataAsync()
    {
        var getItem = new Reservation
        {
            sDay = StartDay.ToString("yyyy-MM-dd"),
            eDay = StartDay.AddDays(DisplayDays).ToString("yyyy-MM-dd")
        };

        var ret = await SmartMVVM.DataStore.GetItems<Reservation>(eAPI.Reservation_GetReservation, getItem);
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("예약현황을 불러오지 못했습니다.", NotificationType.Error);
            return false;
        }

        PendingCount = ret.Count(x => x.RES_Status == "PND");
        ConfirmedCount = ret.Count(x => x.RES_Status == "CNF");
        VisitCount = ret.Count(x => x.RES_Status == "VIS");
        CanceledCount = ret.Count(x => x.RES_Status == "CNL");

        var resMap = ret.ToDictionary(x => $"{x.RES_ReservationDate}_{x.RES_ReservationTime}");

        foreach (var row in CalendarItems)
        {
            foreach (var cell in row.Reservations)
            {
                var key = $"{cell.Key}_{row.Time}";
                
                if (resMap.TryGetValue(key, out var reservation))
                {
                    SmartMVVM.ModelProperty.SetReservationData(row.Reservations[cell.Key], reservation);
                }
            }
        }

        return true;
    }

    public async Task UpdateCalendar()
    {
        SetDays();

        var interval = TimeSpan.FromMinutes(SmartMVVM.AppSession.ReservationTimeInterval);

        CalendarItems.Clear();

        for (TimeSpan time = StartTime; time < EndTime; time += interval)
        {
            var strTime = time.ToString(@"hh\:mm");
            var row = new CalendarRowItem { Time = strTime, Reservations = new Dictionary<string, Reservation>() };

            foreach (var day in days)
            {
                var yyyyMMdd = day.ToString("yyyy-MM-dd");

                row.Reservations[yyyyMMdd] = new Reservation() { RES_ReservationDate = yyyyMMdd, RES_ReservationTime = strTime };
            }

            CalendarItems.Add(row);
        }

        await FetchDataAsync();
    }

    public async Task SetReservationByStatus(Reservation item, string targetStatus)
    {
        var msg = "예약" + (targetStatus == "CNF" ? "등록" : targetStatus == "CNL" ? "취소" : "");
        if (SmartUI.MsgYesNo($"{msg} 하시겠습니까?") is MessageBoxResult.No) return;

        var setRES = new Reservation
        {
            RES_Idx = item.RES_Idx,
            RES_Status = targetStatus
        };

        var ret = await SmartMVVM.DataStore.GetItem<Reservation>(eAPI.Reservation_SetReservationByStatus, setRES);

        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification($"{msg}에 실패했습니다.", NotificationType.Error);
            return;
        }

        await SmartUI.SendMessage("UpdateCalendar", viewType: TargetViewType.PageView);

        SmartUI.SetNofification($"{msg} 되었습니다.", NotificationType.Success);
    }

    public async Task DeleteRES(Reservation item)
    {
        if (SmartUI.MsgYesNo("예약삭제하시겠습니까? 삭제이후에는 복구할 수 없습니다.") is MessageBoxResult.No) return;

        await SmartMVVM.DataStore.GetItem<Reservation>(eAPI.Reservation_SetReservation, new Reservation { RES_Idx = item.RES_Idx, RES_IsValid = false });

        if (!SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("예약삭제하지 못했습니다.", NotificationType.Error);
            return;
        }

        await SmartUI.SendMessage("UpdateCalendar", viewType: TargetViewType.PageView);

        SmartUI.SetNofification($"삭제되었습니다.", NotificationType.Success);
    }

    private void SetDays()
    {
        days.Clear();

        for (DateTime dt = StartDay; dt < StartDay.AddDays(DisplayDays); dt = dt.AddDays(1))
        {
            days.Add(dt);
        }
    }
}
