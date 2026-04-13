using SmartEMR.Application.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace SmartEMR.Application.Controls;

public class TextBox : System.Windows.Controls.TextBox
{
    public static readonly DependencyProperty FieldNameProperty =
         DependencyProperty.Register("FieldName", typeof(string), typeof(TextBox),
             new PropertyMetadata(null, (d, e) =>
             {
                 if (d is TextBox tb && e.NewValue is string fieldName)
                 {
                     BindingExtensions.SetBinding(tb, fieldName);
                 }
             }));

    public string FieldName
    {
        get => (string)GetValue(FieldNameProperty);
        set => SetValue(FieldNameProperty, value);
    }
}
