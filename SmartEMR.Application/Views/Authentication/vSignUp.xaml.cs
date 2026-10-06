using System.Windows;
using DevExpress.Xpf.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vSignUp.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSignUp : ModelViewLayout<SignUpViewModel>
{
    public vSignUp() { }

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    private void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Name)
        {
            case "btnSignUpExist":
                vm.SetSignUpType(SignUpType.EXIST);
                break;

            case "btnSignUpNew":
                vm.SetSignUpType(SignUpType.NEW);
                break;
        }

        SelectSignUpTypePanel.Visibility = Visibility.Collapsed;
        SignUpContent.Visibility = Visibility.Visible;
    }
}