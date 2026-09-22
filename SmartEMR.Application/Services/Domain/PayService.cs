using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;
using static DevExpress.Office.Utils.HdcOriginModifier;

namespace SmartEMR.Application.Services.Domain;

internal class PayService : IPayService
{
    private IDataStore _dataStore;

    public PayService(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public async Task<ServiceResult<Pay>> GetPay(Pay item)
    {
        var result = new ServiceResult<Pay>();
        var ret = await _dataStore.GetItem<Pay>(eAPI.Pay_GetPay, new Pay { PAY_Idx = item.PAY_Idx });

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "존재하지 않거나 삭제된 수납입니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<PayItem>> GetPayItems(PayItem item)
    {
        var result = new ServiceResult<PayItem>();
        var getItem = new PayItem
        {
            PAY_Idx = item.PAY_Idx,
            PAT_Idx = item.PAT_Idx,

            PAYI_Type = item.PAYI_Type,
            PAYI_Method = item.PAYI_Method,
            PAYI_YYMMDD = item.PAYI_YYMMDD
        };
        
        var ret = await _dataStore.GetItems<PayItem>(eAPI.PayItem_GetPayItem, getItem);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "수납 내역 조회에 실패했습니다.";
            return result;
        }

        result.IsSuccess = true;
        return result;
    }

    public async Task<ServiceResult<Pay>> GetPays(Pay item)
    {
        var result = new ServiceResult<Pay>();
        var getItem = new Pay
        {
            PAY_Idx = item.PAY_Idx,

            CST_Status = item.CST_Status,
            PAY_Status = item.PAY_Status,
            PAY_YYMMDD = item.PAY_YYMMDD,

            sDay = item.sDay,
            eDay = item.eDay,

            Keyword = item.Keyword,
            SortField = item.SortField,
            SortDir = item.SortDir ?? "desc",
            PageSize = item.PageSize.GetValueOrDefault(0) == 0 ? 20 : item.PageSize,
            PageIndex = item.PageIndex
        };

        var ret = await _dataStore.GetItems<Pay>(eAPI.Pay_GetPay, getItem);
        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "수납내역 조회에 실패했습니다.";
            return result;
        }

        DisplayDataMappers.PayDisplayDataMapper.Map(ret);

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Pay>> SetPay(Pay item)
    {
        var result = new ServiceResult<Pay>();

        result.IsSuccess = true;
        return result;
    }

    public async Task<ServiceResult<PayItem>> SetPayItem(PayItem item)
    {
        var result = new ServiceResult<PayItem>();

        result.IsSuccess = true;
        return result;

    }
}
