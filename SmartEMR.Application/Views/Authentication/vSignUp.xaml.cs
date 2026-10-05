using DevExpress.Xpf.Editors;
using SmartEMR.Application.Common;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

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

    private void OnEditValueChanged_TextEdit(object sender, EditValueChangedEventArgs e)
    {
        if (sender is not Xpf.TextEdit element) return;

        switch (element.Name)
        {
            case "txtMUR_Id":
                if (e.NewValue is string newValue && !string.IsNullOrWhiteSpace(newValue))
                {
                    lblRequiredId.Visibility = Visibility.Collapsed;
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
                var result = await vm.CheckDuplicateId();
                if (result.resultCode == DuplicateResultCode.EmptyInput)
                {
                    txtMUR_Id.Focus();

                    lblRequiredId.Visibility = Visibility.Visible;
                }

                break;
        }
    }

    private void OnEditValueChanged_PasswordEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
    {
        if (sender is not PasswordBoxEdit element) return;

        switch (element.Name)
        {
            case "pwMUR_PasswordCheck":
                if (e.NewValue is string newValue)
                {
                    vm.UpdateIsCorrectPassword(newValue);
                }

                break;
        }
    }

    private void OnEditvalueChanged_ComboBoxEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
    {
        if (sender is not Xpf.ComboBoxEdit element) return;

        var newValue = e.NewValue as string;
        if (string.IsNullOrWhiteSpace(newValue))
        {
            txtDomain.IsEnabled = true;
        }
        else
        {
            txtDomain.IsEnabled = false;
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
