using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IPatientService
{
    Task<ServiceResult<Patient>> GetPatient(Patient item);
    Task<ServiceResult<Patient>> GetPatients(Patient item);
    Task<ServiceResult<Patient>> SetPatient(Patient item);
}
