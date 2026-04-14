using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Common;

public class BindingExtensions
{
    public static void SetBinding(FrameworkElement element, string fieldName)
    {
        var dataContext = element.DataContext;
        if (dataContext == null) return;

        var targetProp = dataContext.GetType().GetProperty(fieldName);
        if (targetProp == null)
        {
            System.Diagnostics.Debug.WriteLine($"[BindingExtensions] '{fieldName}' 속성을 Model에서 찾을 수 없습니다.");
            return;
        }

        DependencyProperty? dp = element switch
        {
            _ when element is StyleTextBox => StyleTextBox.TextProperty,
            _ when element is Xpf.TextBox => Xpf.TextBox.TextProperty,
            _ when element is CheckBox => CheckBox.IsCheckedProperty,
            _ when element is ComboBox => ComboBox.SelectedValueProperty,
            _ => null
        };

        if (dp != null)
        {
            Binding binding = new Binding($"{fieldName}")
            {
                Source = dataContext,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            element.SetBinding(dp, binding);
        }
    }
}
