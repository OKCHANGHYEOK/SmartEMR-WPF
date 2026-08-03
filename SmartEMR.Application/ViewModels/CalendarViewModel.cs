using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartEMR.Application.ViewModels;

public partial class CalendarViewModel : ReservationViewModel
{
    [ObservableProperty]
    private string? startDay;
    [ObservableProperty]
    private int displayDays;

    public CalendarViewModel()
    {
        StartDay = DateTime.Now.ToString("yyyy-MM-dd");
        DisplayDays = 7;
    }
}
