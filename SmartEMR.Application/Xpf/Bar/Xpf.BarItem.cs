namespace SmartEMR.Application.Xpf.Bar;

public class BarItem : DevExpress.Xpf.Bars.Bar
{
    public BarItem()
    {
        this.UseWholeRow = DevExpress.Utils.DefaultBoolean.True;
        this.AllowCustomizationMenu = false;
        this.AllowQuickCustomization = DevExpress.Utils.DefaultBoolean.False;
    }
}
