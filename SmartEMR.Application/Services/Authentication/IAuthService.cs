using SmartEMR.Application.Services.Domain;

namespace SmartEMR.Application.Services.Authentication;

public interface IAuthService
{
    Task<ServiceResult> RequestVerifyCode(string MUR_Email);
}
