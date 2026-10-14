using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Common.Validator;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Authentication;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;

namespace SmartEMR.Application.ViewModels;

public enum SignUpType
{
    NONE,
    EXIST,
    NEW
}

public partial class SignUpViewModel : MemberViewModel
{
    public MemberUser MemberUser { get; set; } = new();

    [ObservableProperty]
    private SignUpType signUpType = SignUpType.NONE;

    protected readonly IAuthService _authService;
    protected readonly IMemberUserService _memberUserService;

    public SignUpViewModel(IAuthService authService,
                           IMemberService memberService, 
                           IMemberUserService memberUserService) : base(memberService)
    {
        _authService = authService;
        _memberUserService = memberUserService;
    }

    public void SetSignUpType(SignUpType type)
    {
        this.SignUpType = type;
    }

    public void SetDataBySelectedItem(Member selectedItem)
    {
        MemberUser.MEM_Idx = selectedItem.MEM_Idx;

        SmartMVVM.ModelProperty.SetMemberData(Model, selectedItem);
    }

    [RelayCommand]
    private async Task SignUpRequest()
    {
        await SmartUI.SendMessage("SignUp", viewType:TargetViewType.ParentView);
    }

    public async Task SignUp()
    {
        if (!CanSignUp()) return;

        MemberUser? loginUser = null;

        if (this.SignUpType == SignUpType.NEW)
        {
            var ret = await SignUpByNewMember();
            if (ret is null || ret.MURItem is null)
                return;

            loginUser = ret.MURItem;
        }
        else if (this.SignUpType == SignUpType.EXIST)
        {
            var ret = await SignUpByExistMember();
            if (ret is null)
                return;

            loginUser = ret;
        }

        MessageBox.Show("회원가입되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);

        await SmartUI.SendMessage("ShowLogin", loginUser, viewType:TargetViewType.RootView);

        ClearData();
    }

    public void ClearData()
    {
        IsCheckedDuplicateId = false;
        UsableId = false;
        
        IsCheckedDuplicateMediNo = false;
        UsableMediNo = false;

        IsRequestVerifyCode = false;
        IsIdentityVerified = false;
        IsPasswordMatch = false;

        IdentityVerificationCode = "";

        SmartMVVM.ModelProperty.ClearMEMData(Model);
        SmartMVVM.ModelProperty.ClearMURData(MemberUser);

        StopVerifyTimer();
    }

    private async Task<MemberUser?> SignUpByExistMember()
    {
        var ret = await _memberUserService.SignUp(MemberUser);
        if (ret.Item is null || !ret.IsSuccess)
        {
            MessageBox.Show(ret.Message ?? "", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        return ret.Item;
    }

    private async Task<Member?> SignUpByNewMember() 
    {
        var ret = await _memberService.SignUp(Model, MemberUser);
        if (ret.Item is null || !ret.IsSuccess)
        {
            MessageBox.Show(ret.Message ?? "", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        return ret.Item;
    }
}

#region "Veritication"

public partial class SignUpViewModel
{
    private DispatcherTimer? _verifyTimer;

    [ObservableProperty]
    private bool isRequestVerifyCode = false;

    [ObservableProperty]
    private TimeSpan verifylimitTime = TimeSpan.FromMinutes(10);
    [ObservableProperty]
    private string verifyLimitTimeText = "10:00";

    [ObservableProperty]
    private string identityVerificationCode = "";
    [ObservableProperty]
    private bool isIdentityVerified = false;

    public void UpdateIsIdentityVerified()
    {
        if (IsIdentityVerified)
        {
            IsIdentityVerified = false;
        }
    }

    private void StartVerifyTimer()
    {
        _verifyTimer?.Stop();

        VerifylimitTime = TimeSpan.FromMinutes(10);

        UpdateVerifyLimitTimeText();

        _verifyTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _verifyTimer.Tick += VerifyTimer_Tick;
        _verifyTimer.Start();
    }

    private void StopVerifyTimer()
    {
        _verifyTimer?.Stop();

        VerifylimitTime = TimeSpan.Zero;

        UpdateVerifyLimitTimeText();
    }

    private void UpdateVerifyLimitTimeText()
    {
        VerifyLimitTimeText = $"{(int)VerifylimitTime.TotalMinutes:D2}:{VerifylimitTime.Seconds:D2}";
    }

    private void VerifyTimer_Tick(object? sender, EventArgs e)
    {
        if (_verifyTimer is null) return;

        if (VerifylimitTime <= TimeSpan.Zero)
        {
            StopVerifyTimer();
            return;
        }

        VerifylimitTime -= TimeSpan.FromSeconds(1);

        UpdateVerifyLimitTimeText();
    }


    [RelayCommand]
    private async Task RequestVerify()
    {
        if (string.IsNullOrWhiteSpace(MemberUser.MUR_Email))
        {
            MessageBox.Show("이메일을 입력해주세요.", "안내", MessageBoxButton.OK, MessageBoxImage.Information);
            RequestValidation(SignUpField.Email);            
            return;
        }

        var ret = await _authService.RequestVerifyCode(MemberUser.MUR_Email);
        if (!ret.IsSuccess || !string.IsNullOrWhiteSpace(ret.Message))
        {
            MessageBox.Show(ret.Message ?? "", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        IsRequestVerifyCode = true;

        MessageBox.Show("인증코드 메일이 발송되었습니다.", "확인", MessageBoxButton.OK, MessageBoxImage.Information);

        IsIdentityVerified = false;

        StartVerifyTimer();
    }

    [RelayCommand]
    private async Task RequestVerifyCheck()
    {
        if (string.IsNullOrWhiteSpace(IdentityVerificationCode))
        {
            MessageBox.Show("인증코드를 입력해주세요.", "안내", MessageBoxButton.OK, MessageBoxImage.Information);
            RequestValidation(SignUpField.VerifyCode);
            return;
        }

        var ret = await _authService.RequestVerifyCheck(new EmailVerification { EV_Email = MemberUser.MUR_Email, EV_CodeOrigin = IdentityVerificationCode });
        if (ret.Item is null || !ret.IsSuccess)
        {
            MessageBox.Show(ret.Message ?? "", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MemberUser.EV_Idx_Verified = ret.Item.EV_Idx;

        StopVerifyTimer();

        IsIdentityVerified = true;

        MessageBox.Show("본인인증 완료되었습니다.", "확인", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}

#endregion


#region "Validation"

public partial class SignUpViewModel
{
    [ObservableProperty]
    private bool isCheckedDuplicateId = false;
    [ObservableProperty]
    private bool usableId = false;

    [ObservableProperty]
    private bool usablePassword = false;
    [ObservableProperty]
    private bool isPasswordMatch = false;

    [ObservableProperty]
    private bool isCheckedDuplicateMediNo = false;
    [ObservableProperty]
    private bool usableMediNo = false;

    [ObservableProperty]
    private SignUpField validationTarget = SignUpField.None;
    [ObservableProperty]
    private int validationRequestId;


    // 비밀번호에 입력 가능한 문자 체크용
    private static readonly Regex _passwordCharacterRegex = new(@"^[A-Za-z0-9!@#$%]+$");
    // 비밀번호 형식 체크용
    private static readonly Regex _passwordRegex = new(@"^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[!@#$%])[A-Za-z0-9!@#$%]{8,20}$");


    public bool CanInputPassword(string input)
    {
        return _passwordCharacterRegex.IsMatch(input);
    }

    public bool CanUsePassword(string password)
    {
        var isMatch = _passwordRegex.IsMatch(password);

        UsablePassword = isMatch;

        return isMatch;
    }

    public void UpdateIsCheckedDuplicateId()
    {
        if (IsCheckedDuplicateId)
        {
            IsCheckedDuplicateId = false;
        }
    }

    public void UpdateIsCheckedDuplicateMediNo()
    {
        if (IsCheckedDuplicateMediNo)
        {
            IsCheckedDuplicateMediNo = false;
        }
    }

    public void UpdateIsPasswordMatch()
    {
        IsPasswordMatch = string.Equals(MemberUser.MUR_PassWord, MemberUser.MUR_PassWordCheck, StringComparison.CurrentCulture);
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
            UsableId = false;
            return new CheckDuplicateResult(DuplicateResultCode.HasDuplicate, "");
        }
        else
        {
            UsableId = true;
            return new CheckDuplicateResult(DuplicateResultCode.NotDuplicate, "");
        }
    }

    public async Task<CheckDuplicateResult> CheckDuplicateMediNo()
    {
        if (string.IsNullOrWhiteSpace(Model.MEM_MediNo))
        {
            return new CheckDuplicateResult(DuplicateResultCode.EmptyInput, "요양기관번호를 입력해주세요.");
        }

        if (Model.MEM_MediNo.Length < 8)
        {
            return new CheckDuplicateResult(DuplicateResultCode.UnValidInput, "요양기관번호가 올바르지 않습니다.");
        }

        var ret = await _memberService.GetMemberByDuplicateMediNo(Model.MEM_MediNo);
        if (!ret.IsSuccess)
        {
            MessageBox.Show($"{ret.Message}\n잠시후 다시 시도하세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            return new CheckDuplicateResult(DuplicateResultCode.ErrorOccured, "");
        }

        IsCheckedDuplicateMediNo = true;

        if (ret.Item != null && ret.Item.MEM_Idx > 0)
        {
            UsableMediNo = false;
            return new CheckDuplicateResult(DuplicateResultCode.HasDuplicate, "");
        }
        else
        {
            UsableMediNo = true;
            return new CheckDuplicateResult(DuplicateResultCode.NotDuplicate, "");
        }
    }


    private bool RequestValidation(SignUpField field)
    {
        if (field == SignUpField.LicenseNo && MemberUser.MUR_Department != Master.MUR_DEPARTMENT_MED)
            return false;

        ValidationTarget = field;
        ValidationRequestId++;

        return true;
    }

    private bool CanSignUp()
    {
        if (this.SignUpType == SignUpType.NONE) return false;

        var validateResult = SignUpRequiredFieldValidator.ValidateSignUp(Model, MemberUser, this.SignUpType);
        if (!validateResult.IsSuccess)
        {
            if (RequestValidation(validateResult.SignUpMissingField))
            {
                return false;
            }
        }

        if (!IsCheckedDuplicateId)
        {
            MessageBox.Show("아이디 중복체크를 해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!UsableId)
        {
            MessageBox.Show("사용할 수 없는 아이디입니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!IsIdentityVerified)
        {
            MessageBox.Show("이메일 본인인증을 진행해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!IsPasswordMatch)
        {
            MessageBox.Show("비밀번호와 비밀번호 재입력값이 다릅니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (this.SignUpType == SignUpType.NEW)
        {
            if (!IsCheckedDuplicateMediNo)
            {
                MessageBox.Show("요양기관번호 중복체크를 해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!UsableMediNo)
            {
                MessageBox.Show("사용할 수 없는 요양기관번호입니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        return true;
    }
}

#endregion
