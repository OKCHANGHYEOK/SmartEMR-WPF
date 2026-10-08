using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vStartUp.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vStartUp : ModelViewLayout<StartUpViewModel>
{
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
                ShowSignUp();
                break;

            case "ShowLogin":
                ShowLogin();
                break;
        }

        response.IsSuccess = true;
        return response;
    }

    private void ShowSignUp()
    {
        LoginView.Visibility = Visibility.Collapsed;
        SignUpView.Visibility = Visibility.Visible;
    }

    private void ShowLogin()
    {
        LoginView.Visibility = Visibility.Visible;
        SignUpView.Visibility = Visibility.Collapsed;
    }
}