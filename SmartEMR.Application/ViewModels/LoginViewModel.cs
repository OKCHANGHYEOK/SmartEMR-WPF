using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Domain.Entities;

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

    protected override MemberUser? GetModel(MemberUser? item)
    {
        return item;
    }
}
