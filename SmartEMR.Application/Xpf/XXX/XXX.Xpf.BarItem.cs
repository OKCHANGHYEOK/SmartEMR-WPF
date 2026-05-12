using System.Windows;

namespace SmartEMR.Application.Xpf.XXX;

public class BarItem : DevExpress.Xpf.Bars.Bar
{
    public BarItem()
    {
        this.UseWholeRow = DevExpress.Utils.DefaultBoolean.True;
        this.AllowCustomizationMenu = false;
        this.AllowQuickCustomization = DevExpress.Utils.DefaultBoolean.False;
    }
}
