using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;

public class CalendarRowItem
{
    public string? Time { get; set; }
    public Dictionary<string, Reservation> Reservations { get; set; } = new();
}