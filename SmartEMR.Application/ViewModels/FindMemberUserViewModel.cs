using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Xpf.LayoutControl;
using SmartEMR.Application.Common.Validator;
using SmartEMR.Application.Common.Verify;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Authentication;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public enum FindType
{
    None,
    Id,
    Password
}

public partial class FindMemberUserViewModel : MemberUserViewModel
{
    [ObservableProperty]
    private FindType findType = FindType.None;

    [ObservableProperty]
    private string verificationCode = "";

    [ObservableProperty]
    private string findIdResult = "";

    private readonly IAuthService _authService;
    public VerifyManager VerifyManager { get; } = new();

    public FindMemberUserViewModel(IMemberUserService memberUserService, IAuthService authService) : base(memberUserService)
    {
        _authService = authService;
    }

    public void ClearData()
    {
        SmartMVVM.ModelProperty.ClearMURData(Model);

        VerificationCode = "";
        FindIdResult = "";

        VerifyManager.Clear();
    }

    [RelayCommand]
    public void SetFindType(FindType targetType)
    {
        FindType = targetType;
    }

    [RelayCommand]
    private async Task RequestVerifyCode()
    {
        if (string.IsNullOrWhiteSpace(Model.MUR_Name))
        {
            await SmartUI.SendMessage("RequestRequiredField", nameof(MemberUser.MUR_Name));
            return;
        }

        if (string.IsNullOrWhiteSpace(Model.MUR_Email))
        {
            await SmartUI.SendMessage("RequestRequiredField", nameof(MemberUser.MUR_Email));
            return;
        }

        if (this.FindType == FindType.Password)
        {
            await SmartUI.SendMessage("RequestRequiredField", nameof(MemberUser.MUR_Id));
            return;
        }

        var ret = await _authService.RequestVerifyCode(new EmailVerification { MUR_Name = Model.MUR_Name, EV_Email = Model.MUR_Email, EV_Purpose = "FINDID"});
        if (ret.Item is null || !ret.IsSuccess)
        {
            MessageBox.Show(ret.Message ?? "이메일 인증요청중 오류가 발생했습니다.\n잠시 후 다시 시도하세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show("인증코드 메일이 발송되었습니다.", "확인", MessageBoxButton.OK, MessageBoxImage.Information);

        VerifyManager.StartVerifyTimer();
    }

    [RelayCommand]
    private async Task RequestVerifyCheck()
    {
        if (string.IsNullOrWhiteSpace(VerifyManager.IdentityVerificationCode))
        {
            await SmartUI.SendMessage("RequestRequriedField", nameof(VerifyManager.IdentityVerificationCode));
            return;
        }

        var ret = await _authService.RequestVerifyCheck(new EmailVerification { EV_Purpose = "FINDID", EV_Email = Model.MUR_Email, EV_CodeOrigin = VerifyManager.IdentityVerificationCode });
        if (ret.Item is null || !ret.IsSuccess)
        {
            MessageBox.Show(ret.Message ?? "", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        VerifyManager.SuccessVerify();

        Model.EV_Idx_Verified = ret.Item.EV_Idx;

        MessageBox.Show("본인인증 완료되었습니다.", "확인", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private async Task FindId()
    {
        if (!VerifyManager.IsVerifySuccess)
        {
            MessageBox.Show("본인인증이 완료되지 않았습니다.\n 인증 완료 후 다시 시도하세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var ret = await _memberUserService.FindId(new MemberUser { EV_Idx_Verified = Model.EV_Idx_Verified, MUR_Name = Model.MUR_Name, MUR_Email = Model.MUR_Email });
        if (ret.Item is null || !ret.IsSuccess)
        {
            MessageBox.Show(ret.Message ?? "아이디 찾기에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        SmartMVVM.ModelProperty.SetMemberUserData(Model, ret.Item);

        FindIdResult = 
            $"""
            - 아이디 : {ret.Item.MUR_Id}
            - 가입일자 : {ret.Item.MUR_YYMMDD}

            로그인 화면으로 이동하시면 위 아이디로 바로 로그인 할 수 있습니다.

            """;

        MessageBox.Show("아이디 찾기가 완료되었습니다.", "안내", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
