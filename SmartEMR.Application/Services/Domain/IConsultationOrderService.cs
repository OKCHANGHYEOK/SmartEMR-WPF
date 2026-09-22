using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IConsultationOrderService
{
    Task<ServiceResult<ConsultationOrder>> GetConsultationOrders(ConsultationOrder item);
}
