using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public partial class SignUpViewModel : MemberViewModel
{
    [ObservableProperty]
    private bool isCheckedDuplicateId = false;
    [ObservableProperty]
    private bool isUseableId = false;       
    [ObservableProperty]
    private bool isCorrectPassword = false;

    [ObservableProperty]
    private bool isCheckedDuplicateBizNum = false;
    [ObservableProperty]
    private bool isUseableBizNum = false;

    protected readonly IMemberUserService _memberUserService;

    public SignUpViewModel(IMemberService memberService, IMemberUserService memberUserService) : base(memberService)
    {
        _memberUserService = memberUserService;
    }

    public MemberUser MemberUser { get; set; } = new();

    public void UpdateIsCorrectPassword(string newValue)
    {
        IsCorrectPassword = string.Equals(MemberUser.MUR_PassWord, newValue, StringComparison.CurrentCulture);
    }

    public async Task<CheckDuplicateResult> CheckDuplicateId()
    {
        if (string.IsNullOrWhiteSpace(MemberUser.MUR_Id))
        {
            return new CheckDuplicateResult(DuplicateResultCode.EmptyInput, "아이디를 입력하지 않았습니다.");
        }

        var ret = await _memberUserService.GetMemberUserByCheckDuplicateId(MemberUser.MUR_Id);
        if (!ret.IsSuccess)
        {
            MessageBox.Show($"{ret.Message}\n잠시후 다시 시도하세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return new CheckDuplicateResult(DuplicateResultCode.ErrorOccured, "");
        } 

        IsCheckedDuplicateId = true;

        if (ret.Item != null && ret.Item.MUR_Idx > 0)
        {
            IsUseableId = false;
            return new CheckDuplicateResult(DuplicateResultCode.HasDuplicate, "");
        }
        else
        {
            IsUseableId = true;
            return new CheckDuplicateResult(DuplicateResultCode.NotDuplicate, "");
        }
    }

    [RelayCommand]
    private async Task SignUp()
    {
        if (!CanSignUp()) return;
    }

    private bool CanSignUp()
    {
        var MURValidateResult = MemberUserRequiredFieldValidator.Validate(MemberUser);
        if (!MURValidateResult.IsSuccess)
        {
            MessageBox.Show(MURValidateResult.Message ?? "", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        var MEMValidateResult = MemberRequiredFieldValidator.Validate(Model);
        if (!MEMValidateResult.IsSuccess)
        {
            MessageBox.Show(MURValidateResult.Message ?? "", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!IsCheckedDuplicateId)
        {
            MessageBox.Show("아이디 중복체크를 해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!IsUseableId)
        {
            MessageBox.Show("사용할 수 없는 아이디입니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!IsCheckedDuplicateBizNum)
        {
            MessageBox.Show("요영기관번호 중복체크를 해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!IsUseableBizNum)
        {
            MessageBox.Show("사용할 수 없는 요양기관번호입니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }
}
