using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartEMR.Application.Views.Patients;

[ContentProperty(nameof(Children))]
public class vPatientViewSummary_TextItem : UserControl
{
    private StackPanel ContentPanel = new();

    public UIElementCollection Children => ContentPanel.Children;

    public vPatientViewSummary_TextItem()
    {
        this.FontSize = 13;
        this.Content = ContentPanel;

        ContentPanel.Height = 25;
        ContentPanel.VerticalAlignment = VerticalAlignment.Center;
        ContentPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
        ContentPanel.Margin = new Thickness(10, 5, 10, 5);

        ContentPanel.Orientation = Orientation.Horizontal;
    }
}
