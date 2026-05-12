using System.Windows;

namespace SmartEMR.Application.Xpf.XXX;

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
        this.GlyphAlignment = System.Windows.Controls.Dock.Top;
        this.RibbonStyle = DevExpress.Xpf.Bars.RibbonItemStyles.Large;
    }
}
