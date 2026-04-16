using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
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
    public async Task<bool> AttemptLogin()
    {
        var paramItem = new MemberUser()
        {
            MUR_Id = Model.MUR_Id,
            MUR_PassWord = Model.MUR_PassWord
        };

        var ret = await AuthenticationService.AuthenticateUserByLogin(paramItem);

        if (ret.Item == null || ret.IsSuccess == false)
        {
            return false;
        }

        SmartMVVM.AppSession.SetToken(ret.Item);

        return true;
    }
}
