using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public partial class CalendarViewModel : ReservationViewModel
{
    [ObservableProperty]
    private DateTime startDay = DateTime.Today;
    [ObservableProperty]
    private int displayDays = 7;
    [ObservableProperty]
    private TimeSpan startTime = TimeSpan.FromHours(7);
    [ObservableProperty]
    private TimeSpan endTime = TimeSpan.FromHours(24);
    [ObservableProperty]
    private ObservableCollection<CalendarRowItem> calendarItems = new();

    [ObservableProperty]
    private int pendingCount;
    [ObservableProperty]
    private int confirmedCount;
    [ObservableProperty]
    private int visitCount;
    [ObservableProperty]
    private int canceledCount;

    private List<DateTime> days = new();

    public CalendarViewModel(IPatientService patientService, IReservationService reservationService) : base(patientService, reservationService)
    {
    }

    public CalendarViewModel(IPatientService patientService, IReservationService reservationService, Reservation item) : base(patientService, reservationService, item)
    {
    }

    public override void Initialize()
    {
        SetDays();
    }

    public override async Task<bool> FetchDataAsync()
    {
        var getItem = new Reservation
        {
            sDay = StartDay.ToString("yyyy-MM-dd"),
            eDay = StartDay.AddDays(DisplayDays).ToString("yyyy-MM-dd")
        };

        var ret = await _reservationService.GetReservations(getItem);
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification("예약현황을 불러오지 못했습니다.", NotificationType.Error);
            return false;
        }

        var reservations = ret.Items;

        PendingCount = reservations.Count(x => x.RES_Status == "PND");
        ConfirmedCount = reservations.Count(x => x.RES_Status == "CNF");
        VisitCount = reservations.Count(x => x.RES_Status == "VIS");
        CanceledCount = reservations.Count(x => x.RES_Status == "CNL");

        var resMap = reservations.ToDictionary(x => $"{x.RES_ReservationDate}_{x.RES_ReservationTime}");

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

        var ret = await _reservationService.SetReservationByStatus(setRES);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification($"{msg}에 실패했습니다.", NotificationType.Error);
            return;
        }

        await SmartUI.SendMessage("UpdateCalendar", viewType: TargetViewType.PageView);

        SmartUI.SetNotification($"{msg} 되었습니다.", NotificationType.Success);
    }

    public async Task DeleteRES(Reservation item)
    {
        if (SmartUI.MsgYesNo("예약삭제하시겠습니까? 삭제이후에는 복구할 수 없습니다.") is MessageBoxResult.No) return;

        var ret = await _reservationService.SetReservation(new Reservation { RES_Idx = item.RES_Idx, RES_IsValid = false });

        if (!ret.IsSuccess || !string.IsNullOrWhiteSpace(ret.Message))
        {
            SmartUI.SetNotification($"예약삭제하지 못했습니다.\n{ret.Message}", NotificationType.Error);
            return;
        }

        await SmartUI.SendMessage("UpdateCalendar", viewType: TargetViewType.PageView);

        SmartUI.SetNotification($"삭제되었습니다.", NotificationType.Success);
    }

    public async Task MoveReservation(Reservation source, Reservation destination)
    {
        var item = new Reservation
        {
            RES_Idx = source.RES_Idx,
            RES_ReservationDate = destination.RES_ReservationDate,
            RES_ReservationTime = destination.RES_ReservationTime
        };

        var ret = await _reservationService.MoveReservationDate(item);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        await UpdateCalendar();

        SmartUI.SetNotification("예약일시가 변경되었습니다.", NotificationType.Success);
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
