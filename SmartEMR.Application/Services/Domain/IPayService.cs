using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IPayService
{
    Task<ServiceResult<Pay>> GetPay(Pay item);
    Task<ServiceResult<Pay>> GetPays(Pay item);
    Task<ServiceResult<PayItem>> GetPayItems(PayItem item);
    Task<ServiceResult<Pay>> SetPay(Pay item);
    Task<ServiceResult<Pay>> CancelPay(int PAY_Idx);
    Task<ServiceResult<PayItem>> SetPayItem(PayItem item);
}
