using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartEMR.Application.Views.SmartEMRDesk;

[ContentProperty(nameof(Children))]
public class vSmartEMRDeskPATView_TextItem : UserControl
{
    private StackPanel ContentPanel = new();

    public UIElementCollection Children => ContentPanel.Children;

    public vSmartEMRDeskPATView_TextItem()
    {
        this.Content = ContentPanel;

        ContentPanel.Height = 25;
        ContentPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
        ContentPanel.Margin = new Thickness(10, 5, 10, 5);

        ContentPanel.Orientation = Orientation.Horizontal;
    }
}
