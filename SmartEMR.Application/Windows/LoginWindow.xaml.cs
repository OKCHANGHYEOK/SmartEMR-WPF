using System.Windows;

namespace SmartEMR.Application.Windows;

/// <summary>
/// LoginWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoginWindow : Window
{
    public LoginWindow() : base()
    {
        InitializeComponent();

        this.Title = "SmartEMR - 로그인";
    }
}
