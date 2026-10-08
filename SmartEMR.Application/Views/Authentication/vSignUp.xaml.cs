using DevExpress.Xpf.Core;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vSignUp.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSignUp : ModelViewLayout<SignUpViewModel>
{
    private bool _isShowSelectSignUpType = true;

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

        ToggleSignUpLayout();
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        if (sender is not Button element) return;

        switch (element.Name)
        {
            case nameof(btnSignUp):
                await SignUp();
                break;

            case nameof(btnCancel):
                ToggleSignUpLayout();
                break;

            case nameof(btnBackToLogin):
                await BackToLogin();
                break;
        }
    }

    private void ToggleSignUpLayout()
    {
        _isShowSelectSignUpType = !_isShowSelectSignUpType;

        SelectSignUpTypePanel.Visibility = _isShowSelectSignUpType ? Visibility.Visible : Visibility.Collapsed;
        SignUpContentGrid.Visibility = !_isShowSelectSignUpType ? Visibility.Visible : Visibility.Collapsed;
    }

    private async Task BackToLogin()
    {
        ToggleSignUpLayout();

        await SmartUI.SendMessage("ShowLogin", viewType: TargetViewType.ParentView);
    }

    private async Task SignUp()
    {
        if (vm.SignUpType == SignUpType.EXIST)
        {
            await SignUpExistingMember.SignUp();
        }
        else if (vm.SignUpType == SignUpType.NEW)
        {
            await SignUpNewMember.SignUp();
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
