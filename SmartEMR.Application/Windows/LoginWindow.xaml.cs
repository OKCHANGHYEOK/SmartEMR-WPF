using SmartEMR.Application.Xpf;
using SmartEMR.Application.ViewModels;
using System.Windows;
using SmartEMR.Application.Core;

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
                    // 로그인 실패시 띄울 창 구현
                };

                break;
        };
    }
}
