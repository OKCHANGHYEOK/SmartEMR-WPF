using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Authentication;

public interface IAuthService
{
    Task<ServiceResult<EmailVerification>> RequestVerifyCode(EmailVerification item);
    Task<ServiceResult<EmailVerification>> RequestVerifyCheck(EmailVerification item);
}
