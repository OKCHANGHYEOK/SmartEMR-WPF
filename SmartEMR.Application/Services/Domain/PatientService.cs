using SmartEMR.Application.Common;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class PatientService : BaseService, IPatientService
{
    public PatientService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult<Patient>> GetPatient(Patient item)
    {
        var result = new ServiceResult<Patient>();
        var ret = await _dataStore.GetItem<Patient>(eAPI.Patient_GetPatient, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "환자 데이터 조회에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Patient>> GetPatients(Patient item)
    {
        var result = new ServiceResult<Patient>();
        var ret = await _dataStore.GetItems<Patient>(eAPI.Patient_GetPatient, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "환자내역을 불러오는데 실패했습니다.";
            return result;
        }

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Patient>> SetPatient(Patient item)
    {
        var result = new ServiceResult<Patient>();
        var ret = await _dataStore.GetItem<Patient>(eAPI.Patient_SetPatient, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = $"환자정보 저장에 실패했습니다.\n{_dataStore.retMessage}";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
