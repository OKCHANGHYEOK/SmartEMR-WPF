using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class MemberUserInfoViewModel : MemberUserViewModel
{
    protected override MemberUser GetModel(MemberUser item)
    {
        SmartMVVM.ModelProperty.SetMemberUserData(item, SmartMVVM.AppSession.MemberUser ?? new MemberUser());
        return item;
    }

    [RelayCommand]
    private async Task SetMemberUser()
    {

    }

    [RelayCommand]
    private void ClearData()
    {
        if (SmartUI.MsgYesNo("기존 정보로 초기화하시겠습니까?") is System.Windows.MessageBoxResult.Yes)
        {
            SmartMVVM.ModelProperty.SetMemberUserData(Model, SmartMVVM.AppSession.MemberUser ?? new MemberUser());
        }
    }
}
