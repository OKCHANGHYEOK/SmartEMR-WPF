using System.Windows;

namespace SmartEMR.Application.Xpf.Bar;

public class BarButtonItem : DevExpress.Xpf.Bars.BarButtonItem
{
    static BarButtonItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BarButtonItem), new FrameworkPropertyMetadata(typeof(BarButtonItem)));
    }

    public BarButtonItem()
    {
        this.BarItemDisplayMode = DevExpress.Xpf.Bars.BarItemDisplayMode.ContentAndGlyph;
        this.Alignment = DevExpress.Xpf.Bars.BarItemAlignment.Default;     
    }
}
