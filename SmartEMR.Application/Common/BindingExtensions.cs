using System.Windows.Data;
using SmartEMR.Application.Xpf;
using System.Windows;
using SmartEMR.Application.ViewModels;

namespace SmartEMR.Application.Common;

public class BindingExtensions
{
    public static void SetBinding(FrameworkElement element, string fieldName)
    {
        DependencyProperty? dp = element switch
        {
            _ when element is StyleTextBox => StyleTextBox.TextProperty,
            _ when element is Xpf.TextBox => Xpf.TextBox.TextProperty,
            _ when element is Xpf.Image => Xpf.Image.SourceProperty,
            _ when element is CheckEdit => CheckEdit.EditValueProperty,
            _ when element is ComboBoxEdit => ComboBoxEdit.EditValueProperty,
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

            if (element.DataContext is IVIewModel vm)
            {
                binding.Path = new PropertyPath($"Model.{fieldName}");
            }
            else
            {
                binding.Path = new PropertyPath($"{fieldName}");
            }
            
            element.SetBinding(dp, binding);
        }
    }
}
