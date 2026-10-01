using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IReservationService
{
    Task<ServiceResult<Reservation>> GetReservation(Reservation item);
    Task<ServiceResult<Reservation>> GetReservations(Reservation item);
    Task<ServiceResult<Reservation>> SetReservation(Reservation item);
}
