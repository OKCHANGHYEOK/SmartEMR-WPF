using SmartEMR.Application.Common;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class ConsultationOrderService : BaseService, IConsultationOrderService
{
    public ConsultationOrderService(IDataStore dataStore) : base(dataStore) {}

    public async Task<ServiceResult<ConsultationOrder>> GetConsultationOrders(ConsultationOrder item)
    {
        var result = new ServiceResult<ConsultationOrder>();
        var ret = await _dataStore.GetItems<ConsultationOrder>(eAPI.ConsultationOrder_GetConsultationOrder, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "처방내역을 불러오지 못했습니다.";
            return result;
        }

        DisplayDataMappers.ConsultationOrderDisplayDataMapper.Map(ret);

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }
}
