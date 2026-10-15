using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Services.Domain;

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

    public FindMemberUserViewModel(IMemberUserService memberUserService) : base(memberUserService)
    {
    }

    [RelayCommand]
    public void SetFindType(FindType targetType)
    {
        FindType = targetType;
    }

    [RelayCommand]
    private async Task RequestVerifyCode()
    {

    }

    [RelayCommand]
    private async Task VerifyCodeCheck()
    {

    }

    [RelayCommand]
    private async Task FindId()
    {

    }
}
