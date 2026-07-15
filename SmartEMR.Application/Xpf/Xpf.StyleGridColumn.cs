using DevExpress.Xpf.Grid;
using System.Windows;

namespace SmartEMR.Application.Xpf;

public class StyleGridColumn : GridColumn
{
    public static DependencyProperty ColumnItemProperty =
        DependencyProperty.Register(nameof(ColumnItem), typeof(ColumnItem), typeof(StyleGridColumn), new PropertyMetadata(null));

    public ColumnItem ColumnItem
    {
        get => (ColumnItem)GetValue(ColumnItemProperty);
        set => SetValue(ColumnItemProperty, value);
    }
}
