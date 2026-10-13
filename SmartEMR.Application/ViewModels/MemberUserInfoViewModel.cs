using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using System.Globalization;

namespace SmartEMR.Application.ViewModels;

public partial class MemberUserInfoViewModel : MemberUserViewModel
{
    public MemberUserInfoViewModel(IMemberUserService memberUserService) : base(memberUserService) {}

    protected override MemberUser GetModel(MemberUser item)
    {
        SmartMVVM.ModelProperty.SetMemberUserData(item, SmartMVVM.AppSession.MemberUser ?? new MemberUser());
        return item;
    }

    [RelayCommand]
    private async Task SetMemberUser()
    {
        if (!CanSaveData()) return;

        var ret = await _memberUserService.SetMemberUser(Model);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        SmartMVVM.AppSession.SetMemberUser(ret.Item);

        SmartUI.SetNotification("회원정보가 수정되었습니다.", NotificationType.Success);

        await SmartUI.SendMessage("CloseView");
    }

    private bool CanSaveData()
    {
        if (!string.IsNullOrWhiteSpace(Model.MUR_BirthDate) && !DateTime.TryParseExact(Model.MUR_BirthDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            SmartUI.SetNotification("생년월일 형식이 올바르지 않습니다.", NotificationType.Warning);
            return false;
        }

        return true;
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
