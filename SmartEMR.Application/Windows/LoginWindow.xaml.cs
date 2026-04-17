using SmartEMR.Application.Xpf;
using SmartEMR.Application.ViewModels;
using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services;

namespace SmartEMR.Application.Windows;

/// <summary>
/// LoginWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoginWindow : UIWindow
{

    private LoginViewModel vm = new LoginViewModel();

    public LoginWindow() : base()
    {
        InitializeComponent();
    }

    protected override void Initialize()
    {
        this.ContentTitle = "SmartEMR - 로그인";
        this.ContentSize = new Size(500, 450);
        this.DataContext = vm;
    }

    public async override void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        if (sender is BindItem bindItem == false) return;

        switch (bindItem.FieldName)
        {
            case "btnLogin":
                var isLogin = await vm.AttemptLogin();

                if (!isLogin)
                {
                    SmartUI.MsgConfirm("로그인 실패", "아이디 또는 비밀번호를 확인해주세요.");
                };

                this.DialogResult = true;
                this.Close();

                break;
        };
    }
}
