using DevExpress.Xpf.Editors;
using SmartEMR.Application.Common;
using SmartEMR.Application.Common.Validator;
using SmartEMR.Application.Common.Verify;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Views.Authentication.Find;

/// <summary>
/// vFindMemberUserPassword.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vFindMemberUserPassword : ModelViewLayout<FindMemberUserViewModel>
{
    protected override void Initialize()
    {
        vm.SetFindType(FindType.Password);
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
            case "RequestRequiredField":
                var parameter = request.MessageParameter as string;
                if (!string.IsNullOrWhiteSpace(parameter))
                {
                    ShowRequiredField(parameter);
                }

                break;
        }

        response.IsSuccess = true;
        return response;
    }

    private void ShowRequiredField(string targetField)
    {
        Control? targetElement = null;
        string fieldName = "";

        switch (targetField)
        {
            case nameof(MemberUser.MUR_Id):
                targetElement = txtMUR_Id;
                fieldName = "아이디";
                break;

            case nameof(MemberUser.MUR_Name):
                targetElement = txtMUR_Name;
                fieldName = "이름";
                break;

            case nameof(MemberUser.MUR_Email):
                targetElement = txtMUR_Email;
                fieldName = "이메일";
                break;

            case nameof(VerifyManager.IdentityVerificationCode):
                targetElement = txtVerifyCode;
                fieldName = "인증코드";
                break;
        }

        if (targetElement is null)
            return;

        MessageBox.Show($"{fieldName}을/를 입력해주세요.", "경고", MessageBoxButton.OK, MessageBoxImage.Warning);

        targetElement.Focus();

        ValidationManager.Shake(targetElement);
    }

    private async Task BackToLogin()
    {
        await SmartUI.SendMessage("ShowSelectFindType", viewType: TargetViewType.ParentView);

        if (!string.IsNullOrWhiteSpace(vm.FindIdResult))
        {
            await SmartUI.SendMessage("ShowLogin", vm.Model, viewType: TargetViewType.RootView);
        }
        else
        {
            await SmartUI.SendMessage("ShowLogin", viewType: TargetViewType.RootView);
        }

        vm.ClearData();
    }

    private async void OnClick_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not Xpf.Button element) return;

        switch (element.Name)
        {
            case nameof(btnBackToLogin):
                await BackToLogin();
                break;

            case nameof(btnBackToSelectFindType):
                await SmartUI.SendMessage("ShowSelectFindType", viewType: TargetViewType.ParentView);
                break;
        }
    }

    private void OnPreviewTextInput_PasswordEdit(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (sender is not PasswordBoxEdit element) return;

        switch (element.Name)
        {
            case nameof(pwMUR_Password):
                if (!PasswordInputState.CanInputPassword(e.Text))
                {
                    e.Handled = true;
                }

                break;
        }
    }

    private void OnEditValueChanged_PasswordEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
    {
        if (sender is not PasswordBoxEdit element) return;
        if (e.NewValue is not string newValue) return;

        switch (element.Name)
        {
            case nameof(pwMUR_Password):
                vm.UpdateUsablePassword(newValue);
                vm.UpdateIsPasswordMatch();
                break;

            case nameof(pwMUR_PasswordCheck):
                vm.UpdateIsPasswordMatch();
                break;
        }
    }
}
