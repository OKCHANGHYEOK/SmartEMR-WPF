using DevExpress.Mvvm.Native;
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

    public async Task<ServiceResult<EmailVerification>> RequestVerifyCode(EmailVerification item)
    {
        var result = new ServiceResult<EmailVerification>();
        var ret = await _dataStore.GetItem<EmailVerification>(eAPI.Auth_RequestVerifyCode, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = _dataStore.retMessage ?? "인증코드 요청 중 오류가 발생했습니다.\n 잠시 후 다시 시도하세요.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<EmailVerification>> RequestVerifyCheck(EmailVerification item)
    {
        var result = new ServiceResult<EmailVerification>();
        var ret = await _dataStore.GetItem<EmailVerification>(eAPI.Auth_RequestVerifyCheck, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = _dataStore.retMessage ?? "인증확인 요청 중 오류가 발생했습니다.\n 잠시 후 다시 시도하세요.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
