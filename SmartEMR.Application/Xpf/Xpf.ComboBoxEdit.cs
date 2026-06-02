using System.Windows;

namespace SmartEMR.Application.Xpf;

public class ComboBoxEdit : DevExpress.Xpf.Editors.ComboBoxEdit
{
    public ComboBoxEdit()
    {
        this.MinHeight = 23;
        this.MinWidth = 40;
        this.VerticalAlignment = VerticalAlignment.Center;
        this.HorizontalAlignment = HorizontalAlignment.Center;
    }
}
