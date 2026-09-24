using SmartEMR.Application.Common;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class ConsultationService : BaseService, IConsultationService
{
    public ConsultationService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult<Consultation>> GetConsultation(Consultation item)
    {
        var result = new ServiceResult<Consultation>();
        var ret = await _dataStore.GetItem<Consultation>(eAPI.Consultation_GetConsultation, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "진료를 불러오는데 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Consultation>> GetConsultationByRCP(Consultation item)
    {
        var result = new ServiceResult<Consultation>();
        var ret = await _dataStore.GetItems<Consultation>(eAPI.Consultation_GetConsultationByRCP, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "진료를 불러오는데 실패했습니다.";
            return result;
        }

        DisplayDataMappers.ConsultationDisplayDataMapper.Map(ret);

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Consultation>> GetConsultations(Consultation item)
    {
        var result = new ServiceResult<Consultation>();
        var ret = await _dataStore.GetItems<Consultation>(eAPI.Consultation_GetConsultation, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "진료내역을 불러오는데 실패했습니다.";
            return result;
        }

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Consultation>> SetConsultation(Consultation item)
    {
        var result = new ServiceResult<Consultation>();
        var ret = await _dataStore.GetItem<Consultation>(eAPI.Consultation_SetConsultation, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "진료저장에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Consultation>> SetConsultationByCST(Consultation item)
    {
        var result = new ServiceResult<Consultation>();
        var ret = await _dataStore.GetItem<Consultation>(eAPI.Consultation_SetConsultationByCST, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "진료저장에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Consultation>> CancelConsultation(Consultation item)
    {
        var result = new ServiceResult<Consultation>();
        var ret = await _dataStore.GetItem<Consultation>(eAPI.Consultation_CancelConsultation, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "진료취소하는데 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
