using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Utils.CommonDialogs;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using System.Globalization;

namespace SmartEMR.Application.ViewModels;

public partial class MemberUserInfoViewModel : MemberUserViewModel
{
    public MemberUserInfoViewModel(IMemberUserService memberUserService) : base(memberUserService) {}

    public MemberUserInfoViewModel(IMemberUserService memberUserService, MemberUser item) : base(memberUserService, item) { }

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


#region "Password"

public partial class MemberUserInfoViewModel
{
    [ObservableProperty]
    private string currentPassword = "";

    [ObservableProperty]
    private bool isCorrecCurrentPassword = false;

    [ObservableProperty]
    private string newPassword = "";
    [ObservableProperty]
    private string newPasswordCheck = "";

    [ObservableProperty]
    private bool isLengthSatisfied = false;
    [ObservableProperty]
    private bool isNumberSatisfied = false;
    [ObservableProperty]
    private bool isEnglishSatisfied = false;
    [ObservableProperty]
    private bool isSpecialSatisfied = false;


    [ObservableProperty]
    private bool isMatchNewPassword = false;

    private readonly char[] _specialCharacters = { '!', '@', '#', '$', '%' };

    public void UpdateSatisfiedFields()
    {
        SmartUI.BeginInvoke(() =>
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ClearSatisfiedFields();
                return;
            }

            IsLengthSatisfied = NewPassword.Length >= 8 && NewPassword.Length <= 20;
            IsNumberSatisfied = NewPassword.Any(Char.IsDigit);
            IsEnglishSatisfied = NewPassword.Any(c => Char.IsLetter(c) && c < 128);
            IsSpecialSatisfied = NewPassword.Any(c => _specialCharacters.Contains(c));

        }, System.Windows.Threading.DispatcherPriority.Input);
    }

    public void UpdateIsMatchNewPassword()
    {
        if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(NewPasswordCheck)) return;

        IsMatchNewPassword = string.Equals(NewPassword, NewPasswordCheck, StringComparison.InvariantCulture);
    }

    [RelayCommand]
    private async Task ChangePassword()
    {
        if (!await CanChangePassword()) return;

        var ret = await _memberUserService.ChangePassword(new MemberUser { MUR_PassWord = NewPassword, MUR_PasswordCurrent = CurrentPassword });
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        SmartUI.SetNotification("비밀번호 변경이 완료되었습니다. 다시 로그인해주세요.", NotificationType.Success);
        SmartUI.CloseWindow(TargetWindowType.AllWindows);
    }

    private async Task<bool> CanChangePassword()
    {
        if (string.IsNullOrWhiteSpace(CurrentPassword))
        {
            SmartUI.SetNotification("현재 비밀번호를 입력해주세요.", NotificationType.Warning);

            await SmartUI.SendMessage("SetFocusToCurrentPassword");

            return false;
        }

        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            SmartUI.SetNotification("새 비밀번호를 입력해주세요.", NotificationType.Warning);

            await SmartUI.SendMessage("SetFocusToNewPassword");

            return false;
        }

        if (string.IsNullOrWhiteSpace(NewPasswordCheck))
        {
            SmartUI.SetNotification("새 비밀번호 확인을 입력해주세요.", NotificationType.Warning);

            await SmartUI.SendMessage("SetFocusToNewPasswordCheck");

            return false;
        }

        if (!IsMatchNewPassword)
        {
            SmartUI.SetNotification("새 비밀번호와 새 비밀번호 확인값이 일치하지 않습니다.", NotificationType.Warning);
            return false;
        }

        return true;
    }

    private void ClearSatisfiedFields()
    {
        IsLengthSatisfied = false;
        IsNumberSatisfied = false;
        IsEnglishSatisfied = false;
        IsSpecialSatisfied = false;
    }
}

#endregion
