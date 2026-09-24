using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IConsultationService
{
    Task<ServiceResult<Consultation>> GetConsultation(Consultation item);
    Task<ServiceResult<Consultation>> GetConsultationByRCP(Consultation item);
    Task<ServiceResult<Consultation>> GetConsultations(Consultation item);
    Task<ServiceResult<Consultation>> SetConsultation(Consultation item);
    Task<ServiceResult<Consultation>> SetConsultationByCST(Consultation item);
    Task<ServiceResult<Consultation>> CancelConsultation(Consultation item);
}
