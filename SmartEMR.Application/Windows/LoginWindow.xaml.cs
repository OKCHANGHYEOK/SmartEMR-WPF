using SmartEMR.Application.Xpf;
using SmartEMR.Application.ViewModels;
using System.Windows;

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
                await vm.AttemptLogin();
                break;
        };
    }
}
