using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ReservationViewModel : BaseViewModel<Reservation>
{
    protected readonly IPatientService _patientService;
    protected readonly IReservationService _reservationService;

    public ReservationViewModel(IPatientService patientService, IReservationService reservationService) 
    {
        _patientService = patientService;
        _reservationService = reservationService;
    }

    public ReservationViewModel(IPatientService patientService, IReservationService reservationService, Reservation item) : base(item) 
    {
        _patientService = patientService;
        _reservationService = reservationService;
    }

    public override void Initialize()
    {
    }

    protected override Reservation GetModel(Reservation item)
    {
        return item;
    }
}
