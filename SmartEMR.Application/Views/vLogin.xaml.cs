using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views;

/// <summary>
/// vLogin.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vLogin : ModelViewLayout<LoginViewModel>
{
    public vLogin()
    {
        InitializeComponent();
    }

    protected override void Initialize()
    {
        this.DataContext = new LoginViewModel();
    }

    public override async void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        if (sender is BindItem bindItem == false) return;

        switch (bindItem.FieldName)
        {
            case "btnLogin":
                var retLogin = await vm.AttemptLogin();

                if (!retLogin.IsSuccess)
                {
                    SmartUI.MsgConfirm("로그인 실패", retLogin.Message ?? "");
                    return;
                }

                break;
        }
    }
}

