using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IReceptionService
{
    Task<ServiceResult<Reception>> GetReception(Reception item);
    Task<ServiceResult<Reception>> GetReceptions(Reception item);
    Task<ServiceResult<ReceptionBoard>> GetReeptionBoards(ReceptionBoard item);
    Task<ServiceResult<Reception>> SetReception(Reception item);
    Task<ServiceResult<Reception>> CancelReception(Reception item);
}
