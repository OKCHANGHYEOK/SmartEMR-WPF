using System.Windows.Data;
using SmartEMR.Application.Xpf;
using System.Windows;
using SmartEMR.Application.ViewModels;
using DevExpress.Mvvm.UI;

namespace SmartEMR.Application.Common;

public class BindingExtensions
{
    public static void SetBinding(FrameworkElement element, BindItem bindItem)
    {
        DependencyProperty? dp = element switch
        {
            _ when element is StyleTextBox => StyleTextBox.TextProperty,
            _ when element is Label => Label.ContentProperty,
            _ when element is TextBox => TextBox.TextProperty,
            _ when element is Image => Image.SourceProperty,
            _ when element is CheckEdit => CheckEdit.IsCheckedProperty,
            _ when element is ComboBoxEdit => ComboBoxEdit.EditValueProperty,
            _ when element is DateEdit => DateEdit.EditValueProperty,
            _ => null
        };

        if (dp != null)
        {
            Binding binding = new Binding()
            {
                Source = element.DataContext,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            string fieldName = bindItem.FieldName;

            if (element.DataContext is IVIewModel vm)
            {
                binding.Path = new PropertyPath($"Model.{fieldName}");
            }
            else
            {
                binding.Path = new PropertyPath($"{fieldName}");
            }

            if (dp == CheckEdit.IsCheckedProperty && bindItem.IsApplyYNToBoolean)
            {
                binding.Converter = new YNToBooleanConverter();
            }
            
            element.SetBinding(dp, binding);
        }
    }
}
