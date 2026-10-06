using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Text.RegularExpressions;
using SmartEMR.Application.Common;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Common.Validator;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using DevExpress.Xpf.Editors;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vSignUp.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSignUp : ModelViewLayout<SignUpViewModel>
{
    public static readonly DependencyProperty ValidationRequestIdProperty =
        DependencyProperty.Register(nameof(ValidationRequestId), typeof(int), typeof(vSignUp), new PropertyMetadata(0, OnValidationIdChanged));

    private static void OnValidationIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is vSignUp view)
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

    public vSignUp() { }

    protected override void Initialize()
    {
        _validationManager.SetValidationElements(new Dictionary<SignUpField, Control>()
        {
            [SignUpField.None] = txtMUR_Name,
            [SignUpField.UserName] = txtMUR_Name,
            [SignUpField.Id] = txtMUR_Id,
            [SignUpField.Password] = pwMUR_Password,
            [SignUpField.MemberName] = cmbMEM_Idx,
            [SignUpField.BizType] = cmbMEM_BizType,
            [SignUpField.MediNo] = txtMEM_MediNo,
            [SignUpField.BizNum] = txtMEM_BizNum,
            [SignUpField.Department] = cmbMUR_Department,
            [SignUpField.JobCode] = cmbMUR_JobCode,
            [SignUpField.LicenseNo] = txtMUR_LicenseNo
        });

        this.SetBinding(vSignUp.ValidationRequestIdProperty, new Binding("ValidationRequestId") { Source = vm });
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
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

    private async Task CheckDuplicateMediNo()
    {
        var result = await vm.CheckDuplicateMediNo();
        if (result.resultCode == DuplicateResultCode.EmptyInput || result.resultCode == DuplicateResultCode.UnValidInput)
        {
            lblRequiredMediNo.Content = result.Message;
            lblRequiredMediNo.Visibility = Visibility.Visible;
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

                break;

            case "txtMEM_MediNo":
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    lblRequiredMediNo.Visibility = Visibility.Collapsed;
                }

                break;
        }
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        if (sender is not Xpf.Button element) return;

        switch (element.Name)
        {
            case "btnCheckDuplicateId":
                await CheckDuplicateId();
                break;

            case "btnCheckDuplicateMediNo":
                await CheckDuplicateMediNo();
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

                break;

            case "pwMUR_PasswordCheck":
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    vm.UpdateIsCorrectPassword(newValue);
                }

                break;
        }
    }

    private void OnPreviewTextInput_TextEdit(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (sender is not Xpf.TextEdit element) return;

        var input = e.Text;
        var regex = new Regex(@"^[0-9]$");

        if (!regex.IsMatch(input))
        {
            e.Handled = true;
            return;
        }
    }
}

public class PasswordToVisibilityConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is null || values.Length != 2) return Visibility.Collapsed;
        if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue) return Visibility.Collapsed;

        var password = values[0]?.ToString();
        var passwordCheck = values[1]?.ToString();

        if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(passwordCheck))
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

public class BizTypeToItemsSourceConverter : BaseConverter
{
    private readonly List<Member> _defaultItems = [new Member { MEM_Idx = 0, MEM_Name = "기관종구분을 선택하세요." }];

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string bizType) return _defaultItems;

        return SmartMVVM.Master.GetMembers(bizType);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class DepartmentToIsEnableConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string department) return false;

        return department == Master.MUR_DEPARTMENT_MED;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class DepartmentToJobCodeItemsSourceConverter : BaseConverter
{
    private readonly List<MemberUser> _defaultItems = [new MemberUser { MUR_JobCode = "NON", vMUR_JobCode = "부서를 선택하세요." }]; 

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string department) return _defaultItems;

        if (department == Master.MUR_DEPARTMENT_ADM)
        {
            return SmartMVVM.Master.Query<MemberUser>("MUR_JobCode").Where(x => x.MUR_Department == Master.MUR_DEPARTMENT_ADM);
        }
        else if (department == Master.MUR_DEPARTMENT_MED)
        {
            return SmartMVVM.Master.Query<MemberUser>("MUR_JobCode").Where(x => x.MUR_Department == Master.MUR_DEPARTMENT_MED);
        }

        return _defaultItems;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}