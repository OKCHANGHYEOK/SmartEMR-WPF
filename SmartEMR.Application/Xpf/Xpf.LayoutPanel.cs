namespace SmartEMR.Application.Xpf;

public class LayoutPanel : DevExpress.Xpf.Docking.LayoutPanel
{
    public LayoutPanel()
    {
        this.ShowPinButton = false;
        this.ShowCloseButton = false;
        this.AllowDrag = false;
        this.AllowDrop = false;
        this.AllowDock = false;
        this.AllowFloat = false;
    }
}
