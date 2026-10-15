using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public enum StartUpViewType
{
    Login,
    SignUp,
    FindMemberUser
}

public partial class StartUpViewModel : BaseViewModel<Member>
{
    [ObservableProperty]
    private StartUpViewType currentViewType;

    public override void Initialize()
    {

    }

    protected override Member GetModel(Member item)
    {
        return item;
    }

    public void SetCurrentViewType(StartUpViewType targetViewType)
    {
        CurrentViewType = targetViewType;
    }
}
