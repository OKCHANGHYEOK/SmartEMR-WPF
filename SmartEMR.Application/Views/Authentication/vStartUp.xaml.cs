using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vStartUp.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vStartUp : ModelViewLayout<StartUpViewModel>
{
    public event EventHandler? SuccessLogin;

    public vStartUp() {}

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse();

        switch (request.MessageAction)
        {
            case "ShowSignUp":
                vm.SetCurrentViewType(StartUpViewType.SignUp);
                break;

            case "ShowLogin":
                var paramItem = request.MessageParameter as MemberUser;
                if (paramItem is not null)
                {
                    LoginView.SetMemberUserById(paramItem);
                }
                
                vm.SetCurrentViewType(StartUpViewType.Login);
                
                break;

            case "ShowFindMemberUser":
                vm.SetCurrentViewType(StartUpViewType.FindMemberUser);
                break;

            case "SuccessLogin":
                SuccessLogin?.Invoke(this, EventArgs.Empty);
                break;
        }

        response.IsSuccess = true;
        return response;
    }
}