using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using SmartEMR.Application.Common.Validator;

namespace SmartEMR.Application.Views.MemberUsers;

/// <summary>
/// vMemberUserInfo_ChangePassword.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vMemberUserInfo_ChangePassword : ModelViewLayout<MemberUserInfoViewModel>
{
    public vMemberUserInfo_ChangePassword()
    {

    }

    public vMemberUserInfo_ChangePassword(MemberUser item) : base(item) { }

    protected override void Initialize()
    {
    }

    protected override void SetBindGrid()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        if (sender is not BindGrid) return;
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse();

        switch (request.MessageAction)
        {
            case "SetFocusToCurrentPassword":
                pwCurrentPassword.Focus();
                ValidationManager.Shake(pwCurrentPassword);
                break;

            case "SetFocusToNewPassword":
                pwNewPassword.Focus();
                ValidationManager.Shake(pwNewPassword);
                break;

            case "SetFocusToNewPasswordCheck":
                pwNewPasswordCheck.Focus();
                ValidationManager.Shake(pwNewPasswordCheck);
                break;
        }

        response.IsSuccess = true;
        return response;
    }

    private void OnEditValueChanged_PasswordEdit(object sender, EditValueChangedEventArgs e)
    {
        if (sender is not PasswordBoxEdit element) return;

        switch (element.Name)
        {
            case nameof(pwNewPassword):
                vm.UpdateSatisfiedFields();
                vm.UpdateIsMatchNewPassword();
                break;

            case nameof(pwNewPasswordCheck):
                vm.UpdateIsMatchNewPassword();
                break;
        }

        if (!string.IsNullOrWhiteSpace(vm.NewPassword) && !string.IsNullOrWhiteSpace(vm.NewPasswordCheck))
        {
            lblUnMatchPassword.Visibility = vm.IsMatchNewPassword ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    private void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Name)
        {
            case nameof(btnCancel):
                SmartUI.CloseView();
                break;
        }
    }
}
