using SmartEMR.Application.Controls;
using SmartEMR.Application.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Windows;

/// <summary>
/// LoginWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoginWindow : UIWindow
{
    public LoginWindow() : base()
    {
        InitializeComponent();
    }

    protected override void Initialize()
    {
        this.ContentTitle = "SmartEMR - 로그인";
        this.ContentSize = new Size(500, 450);
        this.DataContext = new LoginViewModel();
    }

    private void OnClick_Button(object sender, RoutedEventArgs e) 
    { 
        if (sender is Button element == false) return;

        LoginViewModel? item = this.DataContext as LoginViewModel;

        //if (item != null && item.Model != null)
        //{
        //    MessageBox.Show(item.Model.MUR_Id);
        //}
    }
}
