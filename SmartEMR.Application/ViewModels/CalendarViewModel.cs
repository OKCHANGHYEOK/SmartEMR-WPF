using System.Collections.ObjectModel;
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
                else
                {
                    SmartMVVM.ModelProperty.ClearRESData(row.Reservations[cell.Key], true);
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
                row.Reservations[day.ToString("yyyy-MM-dd")] = new Reservation() { RES_Idx = 0, RES_ReservationTime = strTime };
            }

            CalendarItems.Add(row);
        }

        await FetchDataAsync();
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
