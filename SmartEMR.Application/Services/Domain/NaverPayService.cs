using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class NaverPayService : BaseService, INaverPayService
{
    public NaverPayService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult<NaverPay>> ApplyNaverPay(NaverPay item)
    {
        var result = new ServiceResult<NaverPay>();
        var ret = await _dataStore.GetItem<NaverPay>(eAPI.NaverPay_ApplyPayment, item);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "네이버페이 저장에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
