using System.Windows;
using System.Windows.Data;
using SmartEMR.Application.Xpf;
using SmartEMR.Application.ViewModels;
using DevExpress.Xpf.Editors;

namespace SmartEMR.Application.Common;

public class BindingExtensions
{
    public static void SetBinding(FrameworkElement element, BindItem bindItem)
    {
        DependencyProperty? dp = GetBindingProperty(element);
        if (dp == null) return;

        string fieldName = bindItem.FieldName;
        string bindingPath = element.DataContext is IViewModel ? $"Model.{fieldName}" : $"{fieldName}";

        Binding binding = new Binding(bindingPath)
        {
            Source = element.DataContext,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        if (element is Xpf.CheckEdit && bindItem.IsApplyYNToBoolean)
        {
            binding.Converter = new YNToBooleanConverter();
        }

        element.SetBinding(dp, binding);
    }

    public static DependencyProperty? GetBindingProperty(FrameworkElement element)
    {
        return element switch
        {
            StyleTextBox => StyleTextBox.TextProperty,
            Label => Label.ContentProperty,
            TextBox => TextBox.TextProperty,
            Image => Image.SourceProperty,
            BaseEdit => BaseEdit.EditValueProperty,
            _ => null
        };
    }
}
