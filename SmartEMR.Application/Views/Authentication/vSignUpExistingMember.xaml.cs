using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Text.RegularExpressions;
using SmartEMR.Application.Common;
using SmartEMR.Application.Common.Validator;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using DevExpress.Xpf.Editors;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vSignUpExisitingMember.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSignUpExisitingMember : ModelViewLayout<SignUpViewModel>
{
    public static readonly DependencyProperty ValidationRequestIdProperty =
        DependencyProperty.Register(nameof(ValidationRequestId), typeof(int), typeof(vSignUpExisitingMember), new PropertyMetadata(0, OnValidationIdChanged));

    private static void OnValidationIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is vSignUpExisitingMember view)
        {
            view.ShowValidate();
        }
    }

    public int ValidationRequestId
    {
        get => (int)GetValue(ValidationRequestIdProperty);
        set => SetValue(ValidationRequestIdProperty, value);
    }

    private readonly ValidationManager _validationManager = new();

    public vSignUpExisitingMember() { }

    protected override void Initialize()
    {
        _validationManager.SetValidationElements(new Dictionary<SignUpField, Control>()
        {
            [SignUpField.None] = txtMUR_Name,
            [SignUpField.UserName] = txtMUR_Name,
            [SignUpField.Id] = txtMUR_Id,
            [SignUpField.Password] = pwMUR_Password,
            [SignUpField.MemberName] = cmbMEM_Idx,
            [SignUpField.Department] = cmbMUR_Department,
            [SignUpField.JobCode] = cmbMUR_JobCode,
            [SignUpField.LicenseNo] = txtMUR_LicenseNo
        });

        this.SetBinding(vSignUpExisitingMember.ValidationRequestIdProperty, new Binding("ValidationRequestId") { Source = vm });

        vm.SetSignUpType(SignUpType.EXIST);
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public async Task SignUp()
    {
        await vm.SignUp();
    }

    public void ClearData()
    {
        vm.ClearData();
    }

    private void ShowValidate()
    {
        var targetField = vm.ValidationTarget;
        if (targetField == SignUpField.None)
            return;

        _validationManager.ShowValidation(targetField);
    }

    private async Task CheckDuplicateId()
    {
        var result = await vm.CheckDuplicateId();
        if (result.resultCode == DuplicateResultCode.EmptyInput)
        {
            txtMUR_Id.Focus();

            lblRequiredId.Visibility = Visibility.Visible;
        }
    }

    private void OnEditValueChanged_TextEdit(object sender, EditValueChangedEventArgs e)
    {
        if (sender is not Xpf.TextEdit element) return;

        if (e.NewValue is not string newValue) return;

        switch (element.Name)
        {
            case "txtMUR_Id":
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    lblRequiredId.Visibility = Visibility.Collapsed;
                }
                
                vm.UpdateIsCheckedDuplicateId();
                break;
        }
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        if (sender is not Xpf.Button element) return;

        switch (element.Name)
        {
            case nameof(btnCheckDuplicateId):
                await CheckDuplicateId();
                break;
        }
    }

    private void OnPreviewTextInput_PasswordEdit(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (sender is not PasswordBoxEdit element) return;

        switch (element.Name)
        {
            case "pwMUR_Password":
                if (!vm.CanInputPassword(e.Text))
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
            case "pwMUR_Password":
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                   if (vm.CanUsePassword(newValue))
                   {
                        UsablePasswordPanel.Visibility = Visibility.Collapsed;
                   }
                   else
                   {
                        UsablePasswordPanel.Visibility = Visibility.Visible;
                   }
                }

                vm.UpdateIsPasswordMatch();
                break;

            case "pwMUR_PasswordCheck":
                vm.UpdateIsPasswordMatch();
                break;
        }
    }


    private void OnPreviewTextInput_TextEdit(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (sender is not Xpf.TextEdit element) return;

        switch (element.Name)
        {
            case nameof(txtMEM_MediNo):
                if (!CanInputTextByNum(e.Text))
                {
                    e.Handled = true;
                    return;
                }
                break;

            case nameof(txtMUR_LicenseNo):
                if (!CanInputTextByNum(e.Text))
                {
                    e.Handled = true;
                    return;
                }

                break;
        }
    }

    private void OnEditValueChanged_ComboBoxEdit(object sender, EditValueChangedEventArgs e)
    {
        if (sender is not Xpf.ComboBoxEdit element) return;

        switch (element.Name)
        {
            case "cmbMEM_Idx":
                var selectedItem = element.SelectedItem as Member;
                if (selectedItem != null)
                {
                    vm.SetDataBySelectedItem(selectedItem);
                }

                break;
        }
    }

    private bool CanInputTextByNum(string text)
    {
        return new Regex(@"^[0-9]$").IsMatch(text);
    }
}