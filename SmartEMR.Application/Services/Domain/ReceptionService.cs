using SmartEMR.Application.Common;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class ReceptionService : BaseService, IReceptionService
{
    public ReceptionService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult<Reception>> GetReception(Reception item)
    {
        var result = new ServiceResult<Reception>();
        var ret = await _dataStore.GetItem<Reception>(eAPI.Reception_GetReception, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "접수 데이터를 불러오는데 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Reception>> GetReceptions(Reception item)
    {
        var result = new ServiceResult<Reception>();
        var ret = await _dataStore.GetItems<Reception>(eAPI.Reception_GetReception, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "접수내역을 불러오는데 실패했습니다.";
            return result;
        }

        DisplayDataMappers.ReceptionDisplayDataMapper.Map(ret);

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<ReceptionBoard>> GetReeptionBoards(ReceptionBoard item)
    {
        var result = new ServiceResult<ReceptionBoard>();
        var ret = await _dataStore.GetItems<ReceptionBoard>(eAPI.Reception_GetReceptionBoard, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "접수현황을 불러오는데 실패했습니다.";
            return result;
        }

        DisplayDataMappers.ReceptionBoardDisplayDataMapper.Map(ret);

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Reception>> SetReception(Reception item)
    {
        var result = new ServiceResult<Reception>();
        var ret = await _dataStore.GetItem<Reception>(eAPI.Reception_SetReception, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "접수 저장에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Reception>> CancelReception(Reception item)
    {
        var result = new ServiceResult<Reception>();
        var ret = await _dataStore.GetItem<Reception>(eAPI.Reception_CancelReception, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "접수 취소에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
