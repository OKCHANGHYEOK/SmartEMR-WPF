using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Authentication;

public interface IAuthService
{
    Task<ServiceResult> RequestVerifyCode(string MUR_Email);
    Task<ServiceResult<EmailVerification>> RequestVerifyCheck(EmailVerification item);
}
