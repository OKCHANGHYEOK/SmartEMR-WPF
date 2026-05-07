using DevExpress.Xpf.Bars;
using System.Windows.Markup;

namespace SmartEMR.Application.Xpf.Bar;

[ContentProperty(nameof(BarItems))]
public class BarManager : DevExpress.Xpf.Bars.BarManager
{

    public BarCollection BarItems => this.Bars;

    public BarManager()
    {

    }
}
