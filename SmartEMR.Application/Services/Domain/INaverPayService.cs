using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface INaverPayService
{
    Task<ServiceResult<NaverPay>> ApplyNaverPay(NaverPay item);
}
