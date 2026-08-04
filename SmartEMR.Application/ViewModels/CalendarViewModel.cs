using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;
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
    private ObservableCollection<CalendarRowItem> calenderItems;

    private List<DateTime> days = new();

    public CalendarViewModel()
    {
        StartDay = DateTime.Today;
        DisplayDays = 7;

        StartTime = TimeSpan.FromHours(7);
        EndTime = TimeSpan.FromHours(24);

        CalenderItems = new();

        for (DateTime dt = StartDay; dt < StartDay.AddDays(DisplayDays); dt = dt.AddDays(1))
        {
            days.Add(dt);
        }

        SetCalendarItems();
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

        var resMap = ret.ToDictionary(x => $"{x.RES_ReservationDate}_{x.RES_ReservationTime}");

        foreach (var row in CalenderItems)
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

    private void SetCalendarItems()
    {
        var interval = TimeSpan.FromMinutes(SmartMVVM.AppSession.ReservationTimeInterval);

        for (TimeSpan time = StartTime; time < EndTime; time += interval)
        {
            var strTime = time.ToString(@"hh\:mm");
            var row = new CalendarRowItem { Time = strTime, Reservations = new Dictionary<string, Reservation>() };
            
            foreach (var day in days)
            {
                row.Reservations[day.ToString("yyyy-MM-dd")] = new Reservation() { RES_ReservationTime = strTime };
            }

            CalenderItems.Add(row);
        }
    }
}
