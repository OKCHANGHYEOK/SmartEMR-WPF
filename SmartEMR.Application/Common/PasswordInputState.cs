using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Domain.Entities;
using System.Text.RegularExpressions;

namespace SmartEMR.Application.Common;

public partial class PasswordInputState : ObservableObject
{
    [ObservableProperty]
    private bool usablePassword = false;
    [ObservableProperty]
    private bool isChangedPassword = false;
    [ObservableProperty]
    private bool isPasswordMatch = false;

    // 비밀번호에 입력 가능한 문자 체크용
    private static readonly Regex _passwordCharacterRegex = new(@"^[A-Za-z0-9!@#$%]+$");
    // 비밀번호 형식 체크용
    private static readonly Regex _passwordRegex = new(@"^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[!@#$%])[A-Za-z0-9!@#$%]{8,20}$");

    public void UpdateUsablePassword(string newValue)
    {
        if (CanUsePassword(newValue))
        {
            UsablePassword = true;
        }
        else
        {
            UsablePassword = false;
        }

        if (!IsChangedPassword)
        {
            IsChangedPassword = true;
        }
    }

    public static bool CanInputPassword(string input)
    {
        return _passwordCharacterRegex.IsMatch(input);
    }

    public void UpdateIsPasswordMatch(MemberUser item)
    {
        IsPasswordMatch = string.Equals(item.MUR_PassWord, item.MUR_PassWordCheck, StringComparison.CurrentCulture);
    }

    private bool CanUsePassword(string password)
    {
        var isMatch = _passwordRegex.IsMatch(password);

        UsablePassword = isMatch;

        return isMatch;
    }
}
