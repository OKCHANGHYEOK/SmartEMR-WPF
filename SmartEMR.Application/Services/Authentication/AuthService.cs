using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Authentication;

public class AuthService : BaseService, IAuthService
{
    public AuthService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult> RequestVerifyCode(string MUR_Email)
    {
        var result = new ServiceResult();
        var ret = await _dataStore.GetItem<object>(eAPI.Auth_RequestVerifyCode, new { MUR_Email = MUR_Email });

        if (!_dataStore.retIsSuccess)
        {
            result.Message = "인증코드 요청 중 오류가 발생했습니다.\n 잠시 후 다시 시도하세요.";
            return result;
        }

        result.IsSuccess = true;
        return result;
    }

    public async Task<ServiceResult<EmailVerification>> RequestVerifyCheck(EmailVerification item)
    {
        var result = new ServiceResult<EmailVerification>();
        var ret = await _dataStore.GetItem<EmailVerification>(eAPI.Auth_RequestVerifyCheck, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "인증확인 요청 중 오류가 발생했습니다.\n 잠시 후 다시 시도하세요.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
