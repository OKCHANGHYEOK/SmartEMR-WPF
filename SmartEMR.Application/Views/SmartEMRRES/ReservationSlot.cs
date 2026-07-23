using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRRES;

public partial class ReservationSlot : ObservableObject
{
    public string? RES_Time { get; set; }
    public string? vRES_Time { get; set; }
    public Reservation? RESItem { get; set; }

    [ObservableProperty]
    private bool isSelected;
    [ObservableProperty]
    private bool isReserved; 
}
