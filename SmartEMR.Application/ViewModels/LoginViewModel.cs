using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.DTOs;
using SmartEMR.Domain.Entities;
using SmartEMR.Infrastructure.Services;

namespace SmartEMR.Application.ViewModels;

public partial class LoginViewModel : BaseViewModel<MemberUser>
{
    public LoginViewModel() : base()
    {

    }

    public LoginViewModel(MemberUser? item) : base()
    {

    }

    public override void Initialize()
    {

    }

    protected override MemberUser GetModel(MemberUser item)
    {
        return item;
    }

    [RelayCommand]
    public async Task<DataResponse<MemberUser>> AttemptLogin()
    {
        var retResponse = new DataResponse<MemberUser>() { IsSuccess = false};

        var paramItem = new MemberUser()
        {
            MUR_Id = Model.MUR_Id,
            MUR_PassWord = Model.MUR_PassWord
        };

        var ret = await AuthenticationService.AuthenticateUserByLogin(paramItem);

        if (ret == null || !string.IsNullOrWhiteSpace(ret.FailMessage))
        {
            retResponse.Message = ret?.FailMessage ?? "로그인 중 오류가 발생했습니다.";
            return retResponse;
        }

        retResponse.IsSuccess = true;

        SmartMVVM.AppSession.SetToken(ret);
        SmartMVVM.AppSession.SetMemberUser(ret.User);

        return retResponse;
    }
}
